﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.239
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Brainiac.Design.Properties {
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
                    global::System.Resources.ResourceManager temp = new global::System.Resources.ResourceManager("Brainiac.Design.Properties.Resources", typeof(Resources).Assembly);
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
        ///   Looks up a localized string similar to Behaviors.
        /// </summary>
        internal static string BehaviorGroupName {
            get {
                return ResourceManager.GetString("BehaviorGroupName", resourceCulture);
            }
        }
        
        internal static System.Drawing.Bitmap brain_logo_clean {
            get {
                object obj = ResourceManager.GetObject("brain_logo_clean", resourceCulture);
                return ((System.Drawing.Bitmap)(obj));
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Design.
        /// </summary>
        internal static string DesignerTabName {
            get {
                return ResourceManager.GetString("DesignerTabName", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Plugin could not be loaded as it does not contain the type {0}..
        /// </summary>
        internal static string ExceptionCouldNotLoadPlugin {
            get {
                return ResourceManager.GetString("ExceptionCouldNotLoadPlugin", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to The node could not be extracted. Please check with Node.ParentCanAdoptChildren.
        /// </summary>
        internal static string ExceptionNodeCouldNotBeExtracted {
            get {
                return ResourceManager.GetString("ExceptionNodeCouldNotBeExtracted", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to The file {0} does not exist..
        /// </summary>
        internal static string ExceptionNoSuchFile {
            get {
                return ResourceManager.GetString("ExceptionNoSuchFile", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to New Behavior.
        /// </summary>
        internal static string NewBehavior {
            get {
                return ResourceManager.GetString("NewBehavior", resourceCulture);
            }
        }
    }
}
