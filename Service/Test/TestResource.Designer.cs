﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.34209
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace WebApplications.Utilities.Service.Test {
    using System;
    
    
    /// <summary>
    ///   A strongly-typed resource class, for looking up localized strings, etc.
    /// </summary>
    // This class was auto-generated by the StronglyTypedResourceBuilder
    // class via a tool like ResGen or Visual Studio.
    // To add or remove a member, edit your .ResX file then rerun ResGen
    // with the /str option, or rebuild your VS project.
    [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Resources.Tools.StronglyTypedResourceBuilder", "4.0.0.0")]
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
    [global::System.Runtime.CompilerServices.CompilerGeneratedAttribute()]
    internal class TestResource {
        
        private static global::System.Resources.ResourceManager resourceMan;
        
        private static global::System.Globalization.CultureInfo resourceCulture;
        
        [global::System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
        internal TestResource() {
        }
        
        /// <summary>
        ///   Returns the cached ResourceManager instance used by this class.
        /// </summary>
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Advanced)]
        internal static global::System.Resources.ResourceManager ResourceManager {
            get {
                if (object.ReferenceEquals(resourceMan, null)) {
                    global::System.Resources.ResourceManager temp = new global::System.Resources.ResourceManager("WebApplications.Utilities.Service.Test.TestResource", typeof(TestResource).Assembly);
                    resourceMan = temp;
                }
                return resourceMan;
            }
        }
        
        /// <summary>
        ///   Overrides the current thread's CurrentUICulture property for all
        ///   resource lookups using this strongly typed resource class.
        /// </summary>
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Advanced)]
        internal static global::System.Globalization.CultureInfo Culture {
            get {
                return resourceCulture;
            }
            set {
                resourceCulture = value;
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to LongRun.
        /// </summary>
        internal static string Cmd_LongRun {
            get {
                return ResourceManager.GetString("Cmd_LongRun", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to A long running command that loops and writers once a second..
        /// </summary>
        internal static string Cmd_LongRun_Description {
            get {
                return ResourceManager.GetString("Cmd_LongRun_Description", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to The number of loops (defaults to 10)..
        /// </summary>
        internal static string Cmd_LongRun_Loops_Description {
            get {
                return ResourceManager.GetString("Cmd_LongRun_Loops_Description", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to If set to true, throws an error at the end of the loops (defaults to false)..
        /// </summary>
        internal static string Cmd_LongRun_ThrowError_Description {
            get {
                return ResourceManager.GetString("Cmd_LongRun_ThrowError_Description", resourceCulture);
            }
        }
    }
}
