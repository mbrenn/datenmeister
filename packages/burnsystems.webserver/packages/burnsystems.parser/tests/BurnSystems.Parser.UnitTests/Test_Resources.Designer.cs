﻿//------------------------------------------------------------------------------
// <auto-generated>
//     Dieser Code wurde von einem Tool generiert.
//     Laufzeitversion:4.0.30319.18010
//
//     Änderungen an dieser Datei können falsches Verhalten verursachen und gehen verloren, wenn
//     der Code erneut generiert wird.
// </auto-generated>
//------------------------------------------------------------------------------

namespace BurnSystems.Parser.UnitTests {
    using System;
    
    
    /// <summary>
    ///   Eine stark typisierte Ressourcenklasse zum Suchen von lokalisierten Zeichenfolgen usw.
    /// </summary>
    // Diese Klasse wurde von der StronglyTypedResourceBuilder automatisch generiert
    // -Klasse über ein Tool wie ResGen oder Visual Studio automatisch generiert.
    // Um einen Member hinzuzufügen oder zu entfernen, bearbeiten Sie die .ResX-Datei und führen dann ResGen
    // mit der /str-Option erneut aus, oder Sie erstellen Ihr VS-Projekt neu.
    [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Resources.Tools.StronglyTypedResourceBuilder", "4.0.0.0")]
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
    [global::System.Runtime.CompilerServices.CompilerGeneratedAttribute()]
    internal class Test_Resources {
        
        private static global::System.Resources.ResourceManager resourceMan;
        
        private static global::System.Globalization.CultureInfo resourceCulture;
        
        [global::System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
        internal Test_Resources() {
        }
        
        /// <summary>
        ///   Gibt die zwischengespeicherte ResourceManager-Instanz zurück, die von dieser Klasse verwendet wird.
        /// </summary>
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Advanced)]
        internal static global::System.Resources.ResourceManager ResourceManager {
            get {
                if (object.ReferenceEquals(resourceMan, null)) {
                    global::System.Resources.ResourceManager temp = new global::System.Resources.ResourceManager("BurnSystems.Parser.UnitTests.Test_Resources", typeof(Test_Resources).Assembly);
                    resourceMan = temp;
                }
                return resourceMan;
            }
        }
        
        /// <summary>
        ///   Überschreibt die CurrentUICulture-Eigenschaft des aktuellen Threads für alle
        ///   Ressourcenzuordnungen, die diese stark typisierte Ressourcenklasse verwenden.
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
        ///   Sucht eine lokalisierte Zeichenfolge, die &lt;?xml version=&quot;1.0&quot;?&gt;
        ///&lt;tests&gt;
        ///  &lt;case&gt;
        ///    &lt;input&gt;@[=&quot;affe&quot;]&lt;/input&gt;
        ///    &lt;output&gt;affe&lt;/output&gt;
        ///  &lt;/case&gt;
        ///  &lt;case&gt;
        ///    &lt;input&gt;@[=123]&lt;/input&gt;
        ///    &lt;output&gt;123&lt;/output&gt;
        ///  &lt;/case&gt;
        ///  &lt;case&gt;
        ///    &lt;input&gt;@[=&quot;affe&quot;]nhausen&lt;/input&gt;
        ///    &lt;output&gt;affenhausen&lt;/output&gt;
        ///  &lt;/case&gt;
        ///  &lt;case&gt;
        ///    &lt;input&gt;affe@[=&quot;nhausen&quot;]&lt;/input&gt;
        ///    &lt;output&gt;affenhausen&lt;/output&gt;
        ///  &lt;/case&gt;
        ///  &lt;case&gt;
        ///    &lt;input&gt;affe@[=&quot;n&quot;]hausen&lt;/input&gt;
        ///    &lt;output&gt;affenhausen&lt;/output&gt;
        ///  &lt;/case&gt;
        ///  &lt;case&gt;
        ///    &lt;input&gt;@&lt;/input&gt;
        ///    &lt;output&gt;@&lt;/ [Rest der Zeichenfolge wurde abgeschnitten]&quot;; ähnelt.
        /// </summary>
        internal static string ParserTest_Case {
            get {
                return ResourceManager.GetString("ParserTest_Case", resourceCulture);
            }
        }
    }
}
