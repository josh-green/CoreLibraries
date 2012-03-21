﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Security;
using System.Security.Permissions;
using Microsoft.Build.Framework;
using Mono.Cecil;
using Mono.Cecil.Cil;
using Mono.Cecil.Pdb;

namespace WebApplications.Utilities.Initializer
{
    /// <summary>
    /// Used to inject a module initializer into an assembly.
    /// </summary>
    /// <remarks>
    /// <para>Creates a seperate domain in which to load and inject the assembly, to allow assembly unloading and prevent file locks.</para>
    /// </remarks>
    internal class Injector : MarshalByRefObject
    {
        /// <summary>
        /// The current domain.
        /// </summary>
        private static readonly AppDomain _currentDomain = AppDomain.CurrentDomain;

        /// <summary>
        /// The type of the injector.
        /// </summary>
        private static readonly Type _injectorType;

        /// <summary>
        /// The injector assembly.
        /// </summary>
        private static readonly string _assemblyFileName;

        /// <summary>
        /// The injector type.
        /// </summary>
        private static readonly string _typeName;

        /// <summary>
        /// Initializes a new instance of the <see cref="T:System.Object"/> class.
        /// </summary>
        /// <remarks></remarks>
        static Injector()
        {
            _injectorType = typeof(Injector);
            _assemblyFileName = _injectorType.Assembly.Location;
            _typeName = _injectorType.FullName;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="T:System.MarshalByRefObject"/> class.
        /// </summary>
        /// <remarks>This can only be done by the static <see cref="Inject"/> method.</remarks>
        private Injector()
        {
        }

        /// <summary>
        /// Injects the module initializer into the specified assembly file.
        /// </summary>
        /// <param name="assemblyFile">The assembly file.</param>
        /// <param name="typeName">Name of the type.</param>
        /// <param name="methodName">Name of the method.</param>
        /// <param name="strongNameKeyPair">The strong name key pair.</param>
        /// <param name="useIsolatedAppDomain">if set to <c>true</c> uses a new <see cref="AppDomain"/>.</param>
        /// <returns>Any errors; otherwise <see langword="null"/>.</returns>
        /// <remarks></remarks>
        public static IEnumerable<Output> Inject(string assemblyFile, string typeName, string methodName, string strongNameKeyPair, bool useIsolatedAppDomain)
        {
            Injector injector;
            if (!useIsolatedAppDomain)
            {
                injector = new Injector();
                return injector.DoInject(assemblyFile, typeName, methodName, strongNameKeyPair);
            }

            AppDomain childDomain = null;
            try
            {
                // Create a child app domain.
                childDomain = AppDomain.CreateDomain("InjectionDomain");

                // Create an instance of the injector object in the new domain.
                injector = (Injector)childDomain.CreateInstanceFromAndUnwrap(
                    _assemblyFileName,
                    _typeName,
                    false,
                    BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.CreateInstance,
                    null,
                    null,
                    null,
                    null);

                // Call the DoInject method on the injector in the child domain.
                return injector.DoInject(assemblyFile, typeName, methodName, strongNameKeyPair);
            }
            finally
            {
                if (childDomain != null)
                    AppDomain.Unload(childDomain);
            }
        }

        /// <summary>
        /// Does the injection in an isolated app domain.
        /// </summary>
        /// <param name="assemblyFile">The assembly file.</param>
        /// <param name="typeName">Name of the type.</param>
        /// <param name="methodName">Name of the method.</param>
        /// <param name="strongNameKeyPair">The strong name key pair.</param>
        /// <returns>Any errors; otherwise <see langword="null"/>.</returns>
        /// <remarks></remarks>
        [SecuritySafeCritical] 
        private IEnumerable<Output> DoInject(string assemblyFile, string typeName, string methodName, string strongNameKeyPair)
        {
            OutputCollection outputCollection = new OutputCollection();
            try
            {
                if (String.IsNullOrWhiteSpace(assemblyFile))
                {
                    outputCollection.Add(OutputImportance.Error, "Must specify a valid assembly.");
                    return outputCollection;
                }

                if (String.IsNullOrWhiteSpace(typeName))
                    typeName = "ModuleInitializer";
                if (String.IsNullOrWhiteSpace(methodName))
                    methodName = "Initialize";

                StrongNameKeyPair snkpair;
                if (!String.IsNullOrWhiteSpace(strongNameKeyPair))
                {
                    if (!File.Exists(strongNameKeyPair))
                    {
                        outputCollection.Add(OutputImportance.Error, "The '{0}' strong name keypair was not found.",
                                             strongNameKeyPair);
                        return outputCollection;
                    }

                    // Accessing public key requires UnmanagedCode security permission.
                    try
                    {
                        SecurityPermission sp = new SecurityPermission(SecurityPermissionFlag.UnmanagedCode);
                        sp.Demand();
                    }
                    catch (Exception e)
                    {
                        outputCollection.Add(OutputImportance.Error,
                                             "Could not instrument '{0}' as cannot resign assembly, UnmanagedCode security permission denied.",
                                             strongNameKeyPair,
                                             e.Message);
                        return outputCollection;
                    }

                    try
                    {
                        /*
                         * Turns out that if we get the strong name key pair directly using the StrongNameKeyPair(string filename)
                         * constructor overload, then retrieving the public key fails due to file permissions.
                         * Opening the filestream ourselves with read access, and reading the snk that way works, and allows
                         * us to successfully resign (who knew?).
                         */
                        using (FileStream fs = new FileStream(strongNameKeyPair, FileMode.Open, FileAccess.Read))
                        {
                            snkpair = new StrongNameKeyPair(fs);
                        }

                        // Ensure we can access public key - this will be done by mono later so check works now.
                        byte[] publicKey = snkpair.PublicKey;
                    }
                    catch (Exception e)
                    {
                        outputCollection.Add(OutputImportance.Error,
                                             "Error occurred whilst accessing public key from '{0}' strong name keypair. {1}",
                                             strongNameKeyPair,
                                             e.Message);
                        return outputCollection;
                    }
                }
                else
                {
                    // No resigning necessary.
                    snkpair = null;
                }

                if (!File.Exists(assemblyFile))
                {
                    outputCollection.Add(OutputImportance.Error, "The '{0}' assembly was not found.", assemblyFile);
                    return outputCollection;
                }

                // Look for PDB file.
                string pdbFile = Path.ChangeExtension(assemblyFile, ".pdb");

                // Read the assembly definition
                ReaderParameters readParams = new ReaderParameters(ReadingMode.Immediate);
                bool hasPdb = false;
                if (File.Exists(pdbFile))
                {
                    readParams.ReadSymbols = true;
                    readParams.SymbolReaderProvider = new PdbReaderProvider();
                    hasPdb = true;
                }
                AssemblyDefinition assembly = AssemblyDefinition.ReadAssembly(assemblyFile, readParams);
                if (assembly == null)
                {
                    outputCollection.Add(OutputImportance.Error, "Failed to load assembly definition for '{0}'.", assemblyFile);
                    return outputCollection;
                }

                // Find the main module.
                ModuleDefinition module = assembly.MainModule;
                if (module == null)
                {
                    outputCollection.Add(OutputImportance.Error, "Failed to load main module definition from assembly '{0}'.", assemblyFile);
                    return outputCollection;
                }

                if (module.Types == null)
                {
                    outputCollection.Add(OutputImportance.Error, "Failed to load main module types from assembly '{0}'.", assemblyFile);
                    return outputCollection;
                }

                // Find the <Module> type definition
                TypeDefinition moduleType = module.Types.SingleOrDefault(t => t.Name == "<Module>");
                if (moduleType == null)
                {
                    outputCollection.Add(OutputImportance.Error, "Could not find type '<Module>' in assembly '{0}'.", assemblyFile);
                    return outputCollection;
                }

                // Find void type
                TypeReference voidRef = module.TypeSystem.Void;
                if (voidRef == null)
                {
                    outputCollection.Add(OutputImportance.Error, "Could not find type 'void' in assembly '{0}'.", assemblyFile);
                    return outputCollection;
                }

                // Find the type definition
                TypeDefinition typeDefinition = module.Types.SingleOrDefault(t => t.Name == typeName);

                if (typeDefinition == null)
                {
                    outputCollection.Add(OutputImportance.Warning, "Could not find type '{0}' in assembly '{1}'.", typeName, assemblyFile);
                    return outputCollection;
                }

                // Find the method
                MethodDefinition callee = typeDefinition.Methods != null
                                              ? typeDefinition.Methods.FirstOrDefault(
                                                  m => m.Name == methodName && m.Parameters.Count == 0)
                                              : null;

                if (callee == null)
                {
                    outputCollection.Add(OutputImportance.Warning,
                                         "Could not find method '{0}' with no parameters in type '{1}' in assembly '{2}'.",
                                         methodName, typeName, assemblyFile);
                    return outputCollection;
                }

                if (callee.IsPrivate)
                {
                    outputCollection.Add(OutputImportance.Error,
                                         "Method '{0}' in type '{1}' in assembly '{2}' cannot be private as it can't be accessed by the Module Initializer.",
                                         methodName, typeName, assemblyFile);
                    return outputCollection;
                }

                if (!callee.IsStatic)
                {
                    outputCollection.Add(OutputImportance.Error,
                                         "Method '{0}' in type '{1}' in assembly '{2}' cannot be an instance method as it can't be instantiated by the Module Initializer.",
                                         methodName, typeName, assemblyFile);
                    return outputCollection;
                }


                outputCollection.Add(OutputImportance.MessageHigh,
                                     "Method '{0}' in type '{1}' in assembly '{2}' will be called during Module initialization.",
                                     methodName, typeName, assemblyFile);

                // Create the module initializer.
                MethodDefinition cctor = new MethodDefinition(".cctor", Mono.Cecil.MethodAttributes.Static
                                                                        | Mono.Cecil.MethodAttributes.SpecialName
                                                                        | Mono.Cecil.MethodAttributes.RTSpecialName, voidRef);
                ILProcessor il = cctor.Body.GetILProcessor();
                il.Append(il.Create(OpCodes.Call, callee));
                il.Append(il.Create(OpCodes.Ret));
                moduleType.Methods.Add(cctor);

                WriterParameters writeParams = new WriterParameters();
                if (hasPdb)
                {
                    writeParams.WriteSymbols = true;
                    writeParams.SymbolWriterProvider = new PdbWriterProvider();
                }
                if (snkpair != null)
                {
                    writeParams.StrongNameKeyPair = snkpair;
                    outputCollection.Add(OutputImportance.MessageHigh,
                                         "Assembly '{0}' is being resigned by '{1}.",
                                         assemblyFile, strongNameKeyPair);
                }
                else
                {
                    outputCollection.Add(OutputImportance.MessageHigh,
                                         "Assembly '{0}' will not be resigned.",
                                         assemblyFile);
                }

                assembly.Write(assemblyFile, writeParams);
                return outputCollection;
            }
            catch (Exception ex)
            {
                outputCollection.Add(OutputImportance.Error, "An unexpected error occurred. {0}", ex.Message);
                return outputCollection;
            }
        }
    }
}
