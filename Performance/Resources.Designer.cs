﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.18449
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace WebApplications.Utilities.Performance {
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
                    global::System.Resources.ResourceManager temp = new global::System.Resources.ResourceManager("WebApplications.Utilities.Performance.Resources", typeof(Resources).Assembly);
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
        ///   Looks up a localized string similar to The performance counter type &apos;{0}&apos; does not have a constructor that takes a string..
        /// </summary>
        internal static string PerfCategoryType_Invalid_Constructor {
            get {
                return ResourceManager.GetString("PerfCategoryType_Invalid_Constructor", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to The performance counter type &apos;{0}&apos; does not have a single readonly static field of type CounterCreationData[]..
        /// </summary>
        internal static string PerfCategoryType_Missing_Static_Readonly_Field {
            get {
                return ResourceManager.GetString("PerfCategoryType_Missing_Static_Readonly_Field", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to The performance counter type &apos;{0}&apos; does not descend from PerfCategory..
        /// </summary>
        internal static string PerfCategoryType_Must_Descend_From_PerfCategory {
            get {
                return ResourceManager.GetString("PerfCategoryType_Must_Descend_From_PerfCategory", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to The performance counter category &apos;{0}&apos; does not exist, and must be created before it can be used..
        /// </summary>
        internal static string PerformanceCounterHelper_CategoryDoesNotExist {
            get {
                return ResourceManager.GetString("PerformanceCounterHelper_CategoryDoesNotExist", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to The performance counter category &apos;{0}&apos; does not contain a counter &apos;{1}&apos;, that must be created before it can be used..
        /// </summary>
        internal static string PerformanceCounterHelper_CounterDoesNotExist {
            get {
                return ResourceManager.GetString("PerformanceCounterHelper_CounterDoesNotExist", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Could not create the performance counter category &apos;{0}&apos;..
        /// </summary>
        internal static string PerformanceCounterHelper_Create_Failed {
            get {
                return ResourceManager.GetString("PerformanceCounterHelper_Create_Failed", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Could not delete the performance counter category &apos;{0}&apos;..
        /// </summary>
        internal static string PerformanceCounterHelper_Delete_Failed {
            get {
                return ResourceManager.GetString("PerformanceCounterHelper_Delete_Failed", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Performance counters are enabled - instance GUID &apos;{0}&apos;..
        /// </summary>
        internal static string PerformanceCounterHelper_Enabled {
            get {
                return ResourceManager.GetString("PerformanceCounterHelper_Enabled", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to The current process identity does not have access to performance counters, disabling counters..
        /// </summary>
        internal static string PerformanceCounterHelper_ProcessDoesNotHaveAccess {
            get {
                return ResourceManager.GetString("PerformanceCounterHelper_ProcessDoesNotHaveAccess", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to An unhandled error occurred whilst initialising the counter category &apos;{0}&apos;..
        /// </summary>
        internal static string PerformanceCounterHelper_UnhandledExceptionOccurred {
            get {
                return ResourceManager.GetString("PerformanceCounterHelper_UnhandledExceptionOccurred", resourceCulture);
            }
        }
    }
}
