/**      1         2         3         4         5         6         7         8
 * 45678901234567890123456789012345678901234567890123456789012345678901234567890
 *
 * Common.TypeExtensions: Common library include extending types, v.0.2.3
 *    Johnathan Graham McKnight <akoimeexx@gmail.com>
 *
 *
 * Copyright (c) 2015-2016, Johnathan Graham McKnight
 * All rights reserved.
 *
 * Redistribution and use in source and binary forms, with or without
 * modification, are permitted provided that the following conditions are met:
 *
 * 1. Redistributions of source code must retain the above copyright notice,
 * this list of conditions and the following disclaimer.
 *
 * 2. Redistributions in binary form must reproduce the above copyright notice,
 * this list of conditions and the following disclaimer in the documentation
 * and/or other materials provided with the distribution.
 *
 * 3. Neither the name of the copyright holder nor the names of its contributors
 * may be used to endorse or promote products derived from this software without
 * specific prior written permission.
 *
 * THIS SOFTWARE IS PROVIDED BY THE COPYRIGHT HOLDERS AND CONTRIBUTORS "AS IS"
 * AND ANY EXPRESS OR IMPLIED WARRANTIES, INCLUDING, BUT NOT LIMITED TO, THE
 * IMPLIED WARRANTIES OF MERCHANTABILITY AND FITNESS FOR A PARTICULAR PURPOSE
 * ARE DISCLAIMED. IN NO EVENT SHALL THE COPYRIGHT HOLDER OR CONTRIBUTORS BE
 * LIABLE FOR ANY DIRECT, INDIRECT, INCIDENTAL, SPECIAL, EXEMPLARY, OR
 * CONSEQUENTIAL DAMAGES (INCLUDING, BUT NOT LIMITED TO, PROCUREMENT OF
 * SUBSTITUTE GOODS OR SERVICES; LOSS OF USE, DATA, OR PROFITS; OR BUSINESS
 * INTERRUPTION) HOWEVER CAUSED AND ON ANY THEORY OF LIABILITY, WHETHER IN
 * CONTRACT, STRICT LIABILITY, OR TORT (INCLUDING NEGLIGENCE OR OTHERWISE)
 * ARISING IN ANY WAY OUT OF THE USE OF THIS SOFTWARE, EVEN IF ADVISED OF THE
 * POSSIBILITY OF SUCH DAMAGE.
 */
namespace Common.Extensions {
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Globalization;
    using System.IO;
    using System.Linq;
    using System.Reflection;
    using System.Runtime.InteropServices;
    using System.Runtime.Serialization;
    // Needs Assembly 'System.Runtime.Serialization' in Project References
    using System.Runtime.Serialization.Json;
    using System.Text;
    using System.Windows.Interop;
    using System.Windows.Media;
    using System.Windows.Media.Imaging;

    /**
     * Beware, here be Monsters.
     *   usage:
     *   (Assembly)asm.GetAttribute<AssemblyFIELDAttribute>().FIELD [as string]
     */
    public static partial class AssemblyExtensions {
        public static IEnumerable<T> GetAttributes<T>(
        this ICustomAttributeProvider asm, bool inherit=false
        ) where T : Attribute {
            return asm.GetCustomAttributes(typeof(T), inherit).OfType<T>();
        }
        public static T GetAttribute<T>(this ICustomAttributeProvider asm, 
        bool inherit=false) where T : Attribute {
            return AssemblyExtensions.GetAttributes<T>(asm, inherit)
                .FirstOrDefault();
        }
        public static IEnumerable<string> GetClasses(this Assembly a) {
            List<string> ls = new List<string>();
            foreach (Type t in a.GetTypes()) {
                ls.Add(t.Name);
            }
            return ls;
        }
    }

    /**
     * Enum DescriptionAttribute reader utilizing Object Reflection.
     * Dark Voodoo Majicks. Also provides an easy way to get an Enum's 
     * integer value. Refactored to use TryParse instead of parse, take
     * results for a test drive.
     * DONE: Refactor Value.
     * TODO: Test refactored Value code For Great Justice.
     * FIXME: Value bork bork. De cheekin in der pot; oh ho ho ho! Bork bork!
     */
    public static partial class EnumExtensions {
        public static string Title(this Enum e) {
            throw new NotImplementedException("DisplayValueAttribute");
        }
        public static string Description(this Enum e) {
            FieldInfo f = e.GetType().GetField(e.ToString());
            DescriptionAttribute[] a = (DescriptionAttribute[])f.
                GetCustomAttributes(typeof(DescriptionAttribute), false);
            if (a.Length > 0) {
                return a[0].Description;
            }
            return e.ToString();
        }
// FIXME: Multiple issues, bork bork
//        public static int Value(this Enum e) {
//            Enum d = new e.GetType();
//            if (Enum.TryParse(e.ToString(), out d)) {
//                return (int)d;
////                return (int)Enum.Parse(e.GetType(), e.ToString());
////  new Type[] { typeof(string) },
//            }
//            return false;
//        }
    }
    
    /**
     * Grabs the index for each max value in the list, and returns a list of 
     * indices
     */
    public static partial class EnumerableExtensions {
        public static List<int> MaxIndices<T>(this IEnumerable<T> o) {
            List<int> indices = new List<int>();
            int index = 0;
            foreach (var t in o.ToList<T>()) {
                if (object.Equals(t, o.Max()))
                    indices.Add(index);
                index++;
            }
            return indices;
        }
    }
    
    public static partial class FileExtensions {
        [StructLayout(LayoutKind.Sequential)]
        private struct ShFileInfo {
            public IntPtr hIcon;
            public int iIcon;
            public uint dwAttributes;
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 260)]
            public string szDisplayName;
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 80)]
            public string szTypeName;
        };
        private const uint SHGFI_ICON = 0x100;
        private const uint SHGFI_LARGEICON = 0x0;
        private const uint SHGFI_SMALLICON = 0x1;
        [DllImport("shell32.dll")] private static extern IntPtr SHGetFileInfo(
            string pszPath, uint dwFileAttributes, ref ShFileInfo psfi, 
            uint cbSizeFileInfo, uint uFlags);

        #region GetAssociatedIcon[s]()
        public static ImageSource GetAssociatedIcon(this FileInfo fi) {
            return FileExtensions.GetAssociatedIcon(fi.ToString(), false);
        }
        public static ImageSource GetAssociatedIcon(this FileInfo fi, 
        bool small) {
            return FileExtensions.GetAssociatedIcon(fi.ToString(), small);
        }
        public static ImageSource GetAssociatedIcon(string path) {
            return FileExtensions.GetAssociatedIcon(path, false);
        }
        private static ImageSource GetAssociatedIcon(string path, bool small) {
            ImageSource[] i = FileExtensions.GetAssociatedIcons(path);
            int index = (small && i.Length > 1) ? 1 : 0;

            return (i == null || i.Length == 0) ? new BitmapImage() : i[index];
        }
        public static ImageSource[] GetAssociatedIcons(string path) {
            ShFileInfo srcIcons = new ShFileInfo();
            List<ImageSource> destImages = new List<ImageSource>();
            IntPtr iconSmall = IntPtr.Zero, iconLarge = IntPtr.Zero;

            try {
                iconLarge= FileExtensions.SHGetFileInfo(path, 0, ref srcIcons,
                    (uint)Marshal.SizeOf(srcIcons), SHGFI_ICON | SHGFI_LARGEICON
                );
                if (iconLarge != IntPtr.Zero)
                    destImages.Add(Imaging.CreateBitmapSourceFromHIcon(
                        srcIcons.hIcon,
                        System.Windows.Int32Rect.Empty,
                        BitmapSizeOptions.FromEmptyOptions()
                ));
                iconSmall = FileExtensions.SHGetFileInfo(path, 0, ref srcIcons,
                    (uint)Marshal.SizeOf(srcIcons), SHGFI_ICON | SHGFI_SMALLICON
                );
                if (iconLarge != IntPtr.Zero)
                    destImages.Add(Imaging.CreateBitmapSourceFromHIcon(
                        srcIcons.hIcon,
                        System.Windows.Int32Rect.Empty,
                        BitmapSizeOptions.FromEmptyOptions()
                ));
            } catch { } finally { GC.Collect(); }

            return destImages.ToArray();
        }
        #endregion GetAssociatedIcon[s]()
        #region Touch()
        public static bool Touch(this FileInfo fi) {
            return FileExtensions.Touch(fi.ToString());
        }
        public static bool Touch(this FileInfo fi, DateTime modified) {
            return FileExtensions.Touch(fi.ToString(), modified);
        }
        public static bool Touch(this FileStream fs) {
            return FileExtensions.Touch(fs, DateTime.Now);
        }
        public static bool Touch(this FileStream fs, DateTime modified) {
            fs.Close();
            return FileExtensions.Touch(fs.Name, modified);
        }
        public static bool Touch(string path) {
            return FileExtensions.Touch(path, DateTime.Now);
        }
        public static bool Touch(string path, DateTime modified) {
            try {
                using (FileStream fs = 
                new FileStream(path, 
                    FileMode.OpenOrCreate, 
                    FileAccess.ReadWrite,
                    FileShare.ReadWrite
                )) {
                    fs.Close();
                    fs.Dispose();
                    File.SetLastWriteTime(path, modified);
                }
            } catch (Exception e) {
                if (e.GetBaseException().GetType() ==
                typeof(FileNotFoundException))
                    throw e.GetBaseException();
                return false;
            }
            return true;
        }
        #endregion Touch()
    }
    
    /**
     * Numeric extension to provide signed strings as opposed to constantly
     * stating the String.Formatting "+#;-#;0". Also implements 
     * ToOrdinalString, which appends 'st', 'nd', 'rd', or 'th'.
     * Almost bullet-proof. 
     */
    public static class NumericExtensions {
        public static string ToSignedString<T>(
        this T i) where T: struct, IComparable, IComparable<T>, 
        IConvertible, IEquatable<T>, IFormattable {
            return i.ToDouble(CultureInfo.InvariantCulture).
                ToString("+#;-#;0");
        }
        public static string ToOrdinalString<T>(
        this T i) where T: struct, IComparable, IComparable<T>, 
        IConvertible, IEquatable<T>, IFormattable {
            int n = i.ToInt32(CultureInfo.InvariantCulture);
            if (n <= 0 ) return i.ToString();
            switch (n % 100) {
                case 11: case 12: case 13: return i.ToString() + "th";
            }
            switch (n % 10) {
                case 1: return i.ToString() + "st";
                case 2: return i.ToString() + "nd";
                case 3: return i.ToString() + "rd";
                default: return i.ToString() + "th";
            }
        }
    }
    /**
     * String extensions to provide common conversions
     */
    public static partial class StringExtensions {
        public static string Base64Encode(this String s) {
            return Convert.ToBase64String(Encoding.UTF8.GetBytes(s));
        }
        public static string Base64Decode(this String s) {
            return Encoding.UTF8.GetString(Convert.FromBase64String(s));
        }
        public static string CollapseWhitespace(this String s) {
            string result = s.Trim();
            if (result.Length > 3) {
                StringBuilder sb = new StringBuilder(0, result.Length);
                for (int i = 0; i < result.Length; i++) {
                    if (!char.IsWhiteSpace(result[i]) ||
                        (char.IsWhiteSpace(result[i]) &&
                        !char.IsWhiteSpace(result[i - 1])))
                        sb.Append(result[i]);
                }
                result = sb.ToString();
            }
            return result;
        }
        public static decimal ToDecimalPercent(this String s) {
            decimal d;
            if (decimal.TryParse(s.TrimEnd(new char[] { '%', ' ' }), out d)) {
                return (d / 100M);
            }
            return 1;
        }
/**
 * Usage:
    class Program { static void Main(string[] args) {
        // Base64 [En|De]coding
        String s = "Hello, World!", sb64 = "SGVsbG8sIFdvcmxkIQ==";
        Console.WriteLine(String.Format(
            "String s '{0}' == String sb64 '{1}'", s, sb64));
        Console.WriteLine(String.Format(
            "String s '{0}' == Base64Decode(sb64) '{1}'", 
            s, sb64.Base64Decode()));
        Console.WriteLine(String.Format(
            "String sb64 '{0}' == Base64Encode(s) '{1}'", 
            sb64, s.Base64Encode()));
        // Percentile Parsing
        Console.WriteLine(String.Format(
            "String literal '42%' == Decimal literal '0.42'"));
        Console.WriteLine(String.Format(
            "StringExtensions.ToDecimalPercent(\"42%\") == {0}",
            StringExtensions.ToDecimalPercent("42%")));
        Console.WriteLine(); Console.WriteLine("Press any key to exit...");
        Console.ReadKey();
    }}
*/
    }
    /**
     * Screw typing "Console.WriteLine()" every time; just use 
     * "String to print".COut();
     *   Character length comparison (non-inclusive string):
     *     printf("string here") // 8 chars
     *     std::cout &lt;&lt; "string here" // 11 chars
     *     Debug.Log("string here") // 11 chars
     *     Console.Log("string here") // 13 chars
     *     Console.WriteLine("string here") // 19 chars, more than double a 
     *                                      // printf!
     *    
     *     // My implementation:
     *     "string here".COut() // 7 chars, or...
     *     COut("string here")  // 6 chars
     *   
     * optionally supports arguments.
     * 
     * WONTFIX: Now with CIn, string returns, and the madness of eternal I/O!
     */
    public static partial class StringExtensions {
        public static string CIn(this string s, bool key=true) {
            if (key) return Console.ReadKey().KeyChar.ToString();
            return Console.ReadLine();
        }
        public static string COut(this string s, params object[] args) {
            Console.WriteLine((args.Length > 0) ? String.Format(s, args) : s);
            return (args.Length > 0) ? String.Format(s, args) : s;
        }
        // Eg: "Press enter to exit, trollolol...".COut().CIn();
    }
    /**
     * Beware, here be Monsters.
     *   usage:
     *   string s = Object.ToJson();
     *   <T>o = <T>ObjectType.FromJson(<string> json);
     */
    public static partial class ObjectExtensions {
        public static string Dump<T>(this T o) {
            return o.Dump(false);
        }
        public static string Dump<T>(this T o, bool verbose) {
            StringBuilder sb = new StringBuilder();
            List<string> ls = new List<string>();
            BindingFlags bf = BindingFlags.Static | BindingFlags.Instance | 
                              BindingFlags.Public | BindingFlags.DeclaredOnly;

            if (verbose) bf = BindingFlags.Static | BindingFlags.Instance | 
                              BindingFlags.Public | BindingFlags.NonPublic;

            try {
                sb.AppendLine("{");
                // Class Object Type: 
                sb.AppendLine(String.Format("    \"class\": \"{0}\", ",
                    o.GetType().Name
                ));
                // Events: 
                sb.AppendLine("    \"events\": {");
                ls.Clear();
                foreach (EventInfo ev in o.GetType().GetEvents(bf)) {
                    ls.Add(String.Format("        \"{0}\": \"{1}\"", 
                        ev.Name, 
                        ev.EventHandlerType.Name
                    ));
                }
                sb.AppendLine(String.Join(", " + Environment.NewLine, ls));
                sb.AppendLine("    }, ");

                // Methods: 
                sb.AppendLine("    \"methods\": {");
                ls.Clear();
                foreach(MethodInfo method in o.GetType().GetMethods(bf)) {
                    List<string> lm = new List<string>();
                    foreach (ParameterInfo param in method.GetParameters()) {
                        lm.Add(String.Format("            \"{0}\": \"{1}\"", 
                            param.Name,
                            param.ParameterType.Name
                        ));
                    }
                    if (lm.Count > 0) {
                        ls.Add(String.Format(
                            "        \"{0}\": {{ {2}{1}{2}        }}",
                            method.Name,
                            String.Join(", " + Environment.NewLine, lm),
                            Environment.NewLine
                        ));
                    } else {
                        ls.Add(String.Format("        \"{0}\": {{ }}",
                            method.Name
                        ));
                    }
                    lm.Clear();
                }
                sb.AppendLine(String.Join(", " + Environment.NewLine, ls));
                sb.AppendLine("    }, ");
                
                // Properties: 
                sb.AppendLine("    \"properties\": {");
                ls.Clear();
                foreach (PropertyInfo prop in o.GetType().GetProperties(bf)) {
                    string propValue = "(null)";
                    if (prop.GetValue(o) != null)
                        propValue = prop.GetValue(o).ToString();

                    ls.Add(String.Format("        \"{0} <{1}>\": \"{2}\"",
                        prop.Name,
                        prop.PropertyType.Name, 
                        propValue
                    ));
                }
                sb.AppendLine(String.Join(", " + Environment.NewLine, ls));
                sb.AppendLine("    }");
            } catch (Exception e) {
                /**
                 * If a failure should occur, close out the json object and 
                 * include the Exception type and message under an "exception" 
                 * json property.
                 */
                int openBraces = sb.ToString().Split('{').Length - 1;
                int closedBraces = sb.ToString().Split('}').Length - 1;
                int openBrackets = sb.ToString().Split('[').Length - 1;
                int closedBrackets = sb.ToString().Split(']').Length - 1;

                while (openBrackets - closedBrackets > 0) {
                    sb.AppendLine("], ");
                    openBrackets--;
                }
                while (openBraces - closedBraces > 1) {
                    sb.AppendLine("}, ");
                    openBraces--;
                }

                sb.AppendLine("    \"exception\": {");
                sb.AppendLine(String.Format("        \"{0}\": \"{1}\"", 
                    e.GetType().Name, e.Message
                ));
                sb.AppendLine("    }");
            } finally {
                sb.AppendLine("}");
                ls.Clear();
            }
            return sb.ToString();
        }


        public static T FromJson<T>(this string json) {
            DataContractJsonSerializer serializer =
                new DataContractJsonSerializer(typeof(T));

            using (MemoryStream ms = new MemoryStream()) {
                var sw = new StreamWriter(ms);
                sw.Write(json); sw.Flush(); ms.Position = 0;
                T o = (T)serializer.ReadObject(ms);
                return o;
            }
        }
        public static string GetAttributeString(this object o, 
        Attribute attribute) {
            string ret = null;
            try {
                Type AttributeType = attribute.GetType();
                var MemberInfo = AttributeType.GetMember(o.ToString());
                var Attributes = 
                    MemberInfo[0].GetCustomAttributes(AttributeType, false);

                ret = ((Attribute)Attributes[0]).ToString();
            } catch { throw new MissingFieldException(); }
            return ret;
        }
        public static bool HasMethod(this object o, string func) {
            try { return (o.GetType().GetMethod(func) != null) ? true : false; }
            catch (AmbiguousMatchException) { return true; }
        }
        public static string ToJson<T>(this T o) where T : class {
            // Restrict serializing to an actual DataContract'ed object.
            var attributes = typeof(T)
                .GetCustomAttributes(typeof(DataContractAttribute), true);
            if (attributes.Length == 0) throw new ArgumentException(
                "Unable to serialize an object without DataContractAttribute", 
                String.Format("this ({0} o)", o.GetType().ToString())
            );
            
            DataContractJsonSerializer serializer = 
                new DataContractJsonSerializer(typeof(T));

            using(MemoryStream ms = new MemoryStream()) {
                serializer.WriteObject(ms, o); ms.Position = 0;
                var sr = new StreamReader(ms);
                return sr.ReadToEnd();
            }
        }
/**
 * Usage (ToJson()):
    class Program { static void Main(string[] args) {
        [Serializable][DataContract]
        public partial class UdpMessage {
            [DataMember(IsRequired=false)]
            public DateTime? Timestamp { get; set; }
            [DataMember(IsRequired = true)]
            public object Source { get; set; }
            [DataMember(IsRequired = true)]
            public object Destination { get; set; }
            [DataMember(IsRequired = true)]
            public object Payload { get; set; }
        }
        UdpMessage udpmsg = new UdpMessage() {
            Timestamp = DateTime.Now, 
            Source = "127.0.0.1", 
            Destination = "127.0.0.1", 
            Payload = "Payload object is a string"
        }
        Console.WriteLine(udpmsg.ToJson());
    }
 */
       // Beware: Monster!
        //public static bool TryParse<T>(
        //this T o, object unknown, out T results) {
        //    if(!o.GetType().HasMethod("TryParse")) {
        //        throw new NotImplementedException("");
        //        try {
        //            // TODO: Try Parsing the object, obviously. Do you not?
        //            results = (T)new object();
        //            return true;
        //        } catch {
        //            results = (T)new object();
        //            return false;
        //        }
        //    }
        //    return typeof(T).TryParse(unknown, out results);
        //}
    }
}
