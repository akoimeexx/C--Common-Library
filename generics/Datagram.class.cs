/**      1         2         3         4         5         6         7         8
 * 45678901234567890123456789012345678901234567890123456789012345678901234567890
 *
 * Common.Generics.Datagram: basic class to provide data uri strings, v.0.5.3
 *    Johnathan Graham McKnight <akoimeexx@gmail.com>
 *
 *
 * Copyright (c) 2016, Johnathan Graham McKnight
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
namespace Common.Generics {
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Runtime.Serialization.Formatters.Binary;
    
    public partial class Datagram : Notifiable {
        public bool Base64 => (!((string)this.MediaType).StartsWith("text/"));
        public string Charset { get { return this._charset; } set {
            this.Set(ref this._charset, value); }}
        private string _charset = "utf-8";
        public object Content { get {
            return this._content;
        } set {
            this.Set(
                ref this._mediaType, 
                Datagram.MimeTypes.GetMimeType(value), 
                "MediaType"
            );
            this.Set(ref this._content, value);
        }}
        private object _content;
        public string MediaType => this._mediaType;
        private string _mediaType="";
    }

    public partial class Datagram {
        public static object Base64Decode(string s) {
            object o = null; if (s == null) return o;
            BinaryFormatter bf = new BinaryFormatter();
            byte[] b = Convert.FromBase64String(s);
            using (MemoryStream ms = new MemoryStream(b)) {
                o = bf.Deserialize(ms);
                ms.Close();
            }
            return o;
        }
        public static string Base64Encode(object o) {
            string b64 = null; if (o == null) return b64;
            BinaryFormatter bf = new BinaryFormatter();
            using (MemoryStream ms = new MemoryStream()) {
                bf.Serialize(ms, o);
                b64 = Convert.ToBase64String(ms.ToArray());
                ms.Close();
            }
            return b64;
        }

        public bool LoadFile(string path) {
            bool results = false;
            if (!File.Exists(path)) return results;

            using (FileStream fs =
            new FileStream(path, FileMode.Open, FileAccess.Read)) {
                try {
                    byte[] buffer = new byte[(int)fs.Length];
                    int count, sum = 0;
                    while ((count = fs.Read(buffer, sum, (int)fs.Length - sum))
                        > 0) sum += count;

                    this.Content = buffer;
                    results = true;
                }
                catch (Exception e) { results = false; }
                finally { fs.Close(); }
            }
            return results;
        }
        
        public static Datagram FromString(string s) {
            string[] data = s.Split(new char[] { ',' }, 2);
            string[] tokens = data[0].Split(';');
            
            string charset = tokens[1].Substring(tokens[1].IndexOf('=') + 1);
            object content = data[1];
            foreach (string token in tokens)
                if (token == "base64") content = Datagram.Base64Decode(data[1]);
            
            return new Datagram() { Charset = charset, Content = content };
        }
        
        public override string ToString() {
            List<string> tokens = new List<string>();
            if (!String.IsNullOrWhiteSpace((string)this.MediaType))
                tokens.Add(this.MediaType.ToString());
            if (!String.IsNullOrWhiteSpace(this.Charset))
                tokens.Add("charset=" + this.Charset);
            if (this.Base64) tokens.Add("base64");

            return String.Format("data:{0},{1}",
                String.Join(";", tokens.ToArray()),
                (this.Base64) ?
                    Datagram.Base64Encode(this.Content) :
                    this.Content.ToString()
            );
        }
    }

    public partial class Datagram {
        public static partial class MimeTypes {
            public static List<KeyValuePair<string, string>> Extensions = 
            new List<KeyValuePair<string, string>>() {
                // Defaults
                new KeyValuePair<string, string>("", "application/octet"), 
                new KeyValuePair<string, string>("FILE", "file/{0}"), 
                new KeyValuePair<string, string>("OBJECT", "object/{0}"), 
                // Plaintext
                new KeyValuePair<string, string>("asm", "text/plain"), 
                new KeyValuePair<string, string>("c", "text/plain"), 
                new KeyValuePair<string, string>("cpp", "text/plain"), 
                new KeyValuePair<string, string>("cs", "text/plain"), 
                new KeyValuePair<string, string>("css", "text/css"), 
                new KeyValuePair<string, string>("csv", "text/csv"), 
                new KeyValuePair<string, string>("h", "text/plain"), 
                new KeyValuePair<string, string>("html", "text/html"), 
                new KeyValuePair<string, string>("txt", "text/plain"), 
                new KeyValuePair<string, string>("xml", "text/xml"), 
                new KeyValuePair<string, string>("xsd", "text/xml"), 
                new KeyValuePair<string, string>("xsl", "text/xml"), 
                new KeyValuePair<string, string>("xslt", "text/xml"), 
                // App-Text
                new KeyValuePair<string, string>(
                    "js", "application/javascript"
                ), 
                new KeyValuePair<string, string>("json", "application/json"), 
                new KeyValuePair<string, string>(
                    "xaml", "application/xaml+xml"
                ), 
                new KeyValuePair<string, string>(
                    "xhtml", "application/xhtml+xml"
                ), 
                // Documents
                new KeyValuePair<string, string>(
                    "man", "application/x-troff-man"
                ), 
                new KeyValuePair<string, string>("pdf", "application/pdf"), 
                new KeyValuePair<string, string>("rtf", "application/rtf"), 
                // Images
                new KeyValuePair<string, string>("gif", "image/gif"), 
                new KeyValuePair<string, string>("jpeg", "image/jpeg"), 
                new KeyValuePair<string, string>("jpg", "image/jpeg"), 
                new KeyValuePair<string, string>("png", "image/png"), 
                new KeyValuePair<string, string>("svg", "image/svg+xml"), 
                // Audio/Video
                new KeyValuePair<string, string>("mp3", "audio/mpeg"), 
                new KeyValuePair<string, string>("mp4", "video/mp4"), 
                // Archives
                new KeyValuePair<string, string>("gz", "application/x-gzip"), 
                new KeyValuePair<string, string>("tar", "application/x-tar"), 
                new KeyValuePair<string, string>(
                    "tgz", "application/x-compressed"
                ), 
                new KeyValuePair<string, string>(
                    "zip", "application/x-zip-compressed"
                )
            };
        }
        public static partial class MimeTypes {
            public static string GetMimeType(object o) {
                string res = null;
                if (o is string) {
                    res = MimeTypes.Extensions.Find(
                        _ => (_.Key == "txt")).Value;
                    if (File.Exists((string)o)) {
                        res = MimeTypes.Extensions.Find(
                            _ => (_.Key == "FILE")).Value;

                        var ext = Path.GetExtension((string)o).Trim(
                            new char[] { ' ', '.' }
                        ).ToLower();
                        if (MimeTypes.Extensions.Exists(
                            _ => (_.Key.ToLower() == ext)))
                            res = MimeTypes.Extensions.Find(
                                _ => (_.Key.ToLower() == ext)).Value;
                    }
                } else {
                    res = String.Format(
                        MimeTypes.Extensions.Find(
                            _ => (_.Key == "OBJECT")).Value,
                        o.GetType().Name
                    );
                }
                return res;
            }
        }
    }
    /**
     * Usage:
Datagram DGM = new Datagram() {
    Content = "The Quick Brown Fox Jumps Over the Lazy Dog."
};
Console.WriteLine(DGM.ToString());
// Console Output:
//  data:text/plain;charset=utf-8,The Quick Brown Fox Jumps Over the Lazy Dog.

Datagram JGM = new Datagram() { Content = new string[] { 
    "Sphinx", "of", "black", "quartz,", "judge", "my", "vow."
}};
Console.WriteLine(String.Format("{0}{1}{2}",
    JGM.ToString(), Environment.NewLine, String.Join(" ", 
        (string[])Datagram.Base64Decode(Datagram.Base64Encode(JGM.Content))
)));
// Console Output:
//  data:object/String[];charset=utf-8;base64,AAEAAAD/////AQAAAAAAAAARAQAAAAcAAAAGAgAAAAZTcGhpbngGAwAAAAJvZgYEAAAABWJsYWNrBgUAAAAHcXVhcnR6LAYGAAAABWp1ZGdlBgcAAAACbXkGCAAAAAR2b3cuCw==
//  Sphinx of black quartz, judge my vow.

Console.WriteLine(String.Join(" ", (string[])Datagram.FromString(
    "data:object/String[];charset=utf-8;base64,AAEAAAD/////AQAAAAAAAAARAQAAAAIAAAAGAgAAAAVIZWxsbwYDAAAABldvcmxkIQs="
).Content));
// Console Output:
//  Hello World!
     */
}
