/**      1         2         3         4         5         6         7         8
 * 45678901234567890123456789012345678901234567890123456789012345678901234567890
 *
 * Common.OS: Common library include providing system-level services, v.0.0.2
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
namespace Common.OS {
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Microsoft.Win32;

    public static partial class Programs {
        private static string UNINSTALL_KEYPATH = 
            @"SOFTWARE\Microsoft\Windows\CurrentVersion\Uninstall";
        /// <summary>
        /// Polls the LocalMachine Registry to retrieve a list of installed 
        /// programs
        /// </summary>
        /// <returns>List&lt;string&gt; Program names</returns>
        public static List<string> Installed() {
            List<string> ls = new List<string>();
            using (RegistryKey HKEY_LM = RegistryKey.OpenBaseKey(
            RegistryHive.LocalMachine, RegistryView.Registry64)) {
                using (RegistryKey key = HKEY_LM.OpenSubKey(
                UNINSTALL_KEYPATH)) {
                    foreach(string subrecord in key.GetSubKeyNames()) {
                        using(RegistryKey subkey = key.OpenSubKey(subrecord)) {
                            ls.Add(subkey.GetValue("DisplayName") as string);
                        }
                    }
                }
            }
            ls.RemoveAll(s => { return String.IsNullOrWhiteSpace(s); });
            return ls;
        }
        public static string InstalledPath(string appname) {
            string path = "";
            // TODO: Have this auto-detect 32bit vs. 64bit.
            using (RegistryKey HKEY_LM = RegistryKey.OpenBaseKey(
            RegistryHive.LocalMachine, RegistryView.Registry64)) {
                using (RegistryKey appkey = HKEY_LM.OpenSubKey(
                UNINSTALL_KEYPATH + @"\" + appname)) {
                    IEnumerable<string> lsPathStack = appkey.GetValueNames()
                    .ToList().Intersect(new List<string>() {
                        "InstallLocation", "InstallPath", "UninstallLocation", 
                        "UninstallPath", "UninstallString"
                    });
                    if (lsPathStack.Count() == 0) return path;
                    string needle = 
                        appkey.GetValue(lsPathStack.First()) as string;
                    path = 
                        needle.Trim('"').Substring(0, needle.LastIndexOf(@"\"));
                }
            }
            return path;
        }
    }
}
