﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.42000
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace WebApplications.Utilities.Cryptography {
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
    internal class Resources {
        
        private static global::System.Resources.ResourceManager resourceMan;
        
        private static global::System.Globalization.CultureInfo resourceCulture;
        
        [global::System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
        internal Resources() {
        }
        
        /// <summary>
        ///   Returns the cached ResourceManager instance used by this class.
        /// </summary>
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Advanced)]
        internal static global::System.Resources.ResourceManager ResourceManager {
            get {
                if (object.ReferenceEquals(resourceMan, null)) {
                    global::System.Resources.ResourceManager temp = new global::System.Resources.ResourceManager("WebApplications.Utilities.Cryptography.Resources", typeof(Resources).Assembly);
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
        ///   Looks up a localized string similar to No enabled AESCryptographer providers were found in configuration..
        /// </summary>
        internal static string AESCryptographer_Constructor_Configuration_NoProviderFound {
            get {
                return ResourceManager.GetString("AESCryptographer_Constructor_Configuration_NoProviderFound", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to The provided input was not a Base32 encoded string: &apos;{0}&apos;.
        /// </summary>
        internal static string AESCryptographer_Decrypt_DecryptFailed_InputNotBase32String {
            get {
                return ResourceManager.GetString("AESCryptographer_Decrypt_DecryptFailed_InputNotBase32String", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to The input failed to decrypt with the current collection of keys..
        /// </summary>
        internal static string AESCryptographer_Decrypt_DecryptFailed_KeyNotFound {
            get {
                return ResourceManager.GetString("AESCryptographer_Decrypt_DecryptFailed_KeyNotFound", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Unknown, or unsupported, cryptographic provider &apos;{0}&apos;..
        /// </summary>
        internal static string AsymmetricCryptographyProvider_Create_Unknown_Cryptography_Provider {
            get {
                return ResourceManager.GetString("AsymmetricCryptographyProvider_Create_Unknown_Cryptography_Provider", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Cannot decrypt a node of type &apos;{0}&apos;..
        /// </summary>
        internal static string Cryptographer_Decrypt_CannotDecryptNode {
            get {
                return ResourceManager.GetString("Cryptographer_Decrypt_CannotDecryptNode", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Cannot encrypt a node of type &apos;{0}&apos;..
        /// </summary>
        internal static string Cryptographer_Encrypt_CannotEncryptNode {
            get {
                return ResourceManager.GetString("Cryptographer_Encrypt_CannotEncryptNode", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to No embedded key files were found..
        /// </summary>
        internal static string Cryptographer_InitializeEmbeddedKeys_NoFilesFound {
            get {
                return ResourceManager.GetString("Cryptographer_InitializeEmbeddedKeys_NoFilesFound", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to The supplied function did not return a CryptographyProvider.
        /// </summary>
        internal static string CryptographyConfiguration_GetOrAddProvider_Add_Returned_Null {
            get {
                return ResourceManager.GetString("CryptographyConfiguration_GetOrAddProvider_Add_Returned_Null", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Invalid provider ID supplied..
        /// </summary>
        internal static string CryptographyConfiguration_GetOrAddProvider_Invalid_Provider_ID {
            get {
                return ResourceManager.GetString("CryptographyConfiguration_GetOrAddProvider_Invalid_Provider_ID", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Unknown, or unsupported, cryptographic provider &apos;{0}&apos;..
        /// </summary>
        internal static string CryptographyProvider_Create_Unknown_Provider {
            get {
                return ResourceManager.GetString("CryptographyProvider_Create_Unknown_Provider", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Cannot persist the current cryptography provider to the configuration as no configuration could be found..
        /// </summary>
        internal static string CryptographyProvider_SaveToConfiguration_No_Configuration {
            get {
                return ResourceManager.GetString("CryptographyProvider_SaveToConfiguration_No_Configuration", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Cannot persist the current cryptography provider to the configuration as it doesn&apos;t have an existing id and no id was specified..
        /// </summary>
        internal static string CryptographyProvider_SaveToConfiguration_No_ID {
            get {
                return ResourceManager.GetString("CryptographyProvider_SaveToConfiguration_No_ID", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to The provider with id: &apos;{0}&apos; wasn&apos;t enabled in the configuration..
        /// </summary>
        internal static string CryptoProviderWrapper_Constructor_ProviderNotEnabled {
            get {
                return ResourceManager.GetString("CryptoProviderWrapper_Constructor_ProviderNotEnabled", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Cannot decrypt the string as the cryptography provider with ID &apos;{0}&apos; was not found..
        /// </summary>
        internal static string EncryptedString_EncryptedString_InvalidProviderId {
            get {
                return ResourceManager.GetString("EncryptedString_EncryptedString_InvalidProviderId", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Cannot serialize an encrypted string unless the cryptography provider is persisted in the configuration..
        /// </summary>
        internal static string EncryptedString_GetObjectData_No_Provider_Id {
            get {
                return ResourceManager.GetString("EncryptedString_GetObjectData_No_Provider_Id", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Password cannot be empty.
        /// </summary>
        internal static string PBKDF2_PasswordEmpty {
            get {
                return ResourceManager.GetString("PBKDF2_PasswordEmpty", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Salt cannot be empty.
        /// </summary>
        internal static string PBKDF2_SaltEmpty {
            get {
                return ResourceManager.GetString("PBKDF2_SaltEmpty", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to No enabled RSACryptographer providers were found in configuration..
        /// </summary>
        internal static string RSACryptographer_Constructor_Configuration_NoProviderFound {
            get {
                return ResourceManager.GetString("RSACryptographer_Constructor_Configuration_NoProviderFound", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Decryption failed. No key attempted was successful..
        /// </summary>
        internal static string RSACryptographer_Decrypt_DecryptionFailed {
            get {
                return ResourceManager.GetString("RSACryptographer_Decrypt_DecryptionFailed", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Length cannot be less than {0} bytes as that would be too insecure..
        /// </summary>
        internal static string SecureIdentifier_Ctor_Invalid_Length_Supplied {
            get {
                return ResourceManager.GetString("SecureIdentifier_Ctor_Invalid_Length_Supplied", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Invalid SecureIdentifier string supplied..
        /// </summary>
        internal static string SecureIdentifier_Parse_Invalid_SecureIdentifier_String_Supplied {
            get {
                return ResourceManager.GetString("SecureIdentifier_Parse_Invalid_SecureIdentifier_String_Supplied", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Invalid byte array supplied..
        /// </summary>
        internal static string SecureIdentifier_SecureIdentifier_Invalid_Byte_Array_Supplied {
            get {
                return ResourceManager.GetString("SecureIdentifier_SecureIdentifier_Invalid_Byte_Array_Supplied", resourceCulture);
            }
        }
    }
}
