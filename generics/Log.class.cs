/**      1         2         3         4         5         6         7         8
 * 45678901234567890123456789012345678901234567890123456789012345678901234567890
 *
 * Common.Generics.Log: Static debug-logging class, v.0.0.1
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
    using System.IO;
    using System.Reflection;
    using System.Runtime.CompilerServices;

    public static partial class Log {
        // 
        private static string _defaultPath = Path.GetTempPath();
        public static string DefaultPath {
            get { return Log._defaultPath; }
            set { Log._defaultPath = value; }
        }
        private static string _defaultFile = String.Format(
            "{0}_{1}.log",
            DateTime.Now.ToString("yyyy-MM-dd"),
            (!String.IsNullOrWhiteSpace(Assembly.GetCallingAssembly().GetName().Name)) ?
                Assembly.GetCallingAssembly().GetName().Name :
                Assembly.GetExecutingAssembly().GetName().Name
        );
        public static string DefaultFile {
            get { return Log._defaultFile; }
            set { Log._defaultFile = value; }
        }
        public static string FullName {
            get { return String.Format("{0}{1}", DefaultPath, DefaultFile); }
        }
        private static bool _truncateCallerPath = false;
        public static bool TruncateSource {
            get { return Log._truncateCallerPath; }
            set { Log._truncateCallerPath = value; }
        }
        public static bool WriteLine(string message, 
        [CallerMemberName] string CallerName = null, 
        [CallerFilePath] string CallerFile = null, 
        [CallerLineNumber] int CallerLineNumber = 0) {
            return Log.WriteLine(
                message, 
                Log.DefaultPath + Log.DefaultFile, 
                CallerName, 
                CallerFile, 
                CallerLineNumber
            );
        }
        public static bool WriteLine(string message, string path, 
        [CallerMemberName] string CallerName = null, 
        [CallerFilePath] string CallerFile = null, 
        [CallerLineNumber] int CallerLineNumber = 0) {
            return Log.WriteLine(
                message, 
                new FileInfo(path), 
                CallerName, 
                CallerFile, 
                CallerLineNumber
            );
        }
        public static bool WriteLine(string message, FileInfo file, 
        [CallerMemberName] string CallerName = null, 
        [CallerFilePath] string CallerFile = null, 
        [CallerLineNumber] int CallerLineNumber = 0) {
            bool b = true;
            try {
                using (StreamWriter COut = new StreamWriter(new FileStream(
                    file.FullName, 
                    FileMode.Append, FileAccess.Write, FileShare.Write, 
                    4096, FileOptions.Asynchronous | FileOptions.WriteThrough
                ))) {
                    COut.WriteLine("{0} [{1}:{2}] {3}(): {4}",
                        DateTime.Now.ToString("HH:mm:ss.ffffff"), 
                        (Log.TruncateSource) ? 
                            Log.TruncatePath(CallerFile) : 
                            CallerFile, 
                        CallerLineNumber, CallerName, message
                    );
                    COut.Close(); COut.Dispose();
                }
            } catch { b = false; }
            return b;
        }
        public static string TruncatePath(string path) {
            string[] p = path.Split('\\');
            if (p.Length > 3) return String.Format(@"{0}\..\{1}\{2}", 
                p[0], p[p.Length - 2], p[p.Length - 1]
            );
            return path;
        }
    }
}