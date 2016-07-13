/**      1         2         3         4         5         6         7         8
 * 45678901234567890123456789012345678901234567890123456789012345678901234567890
 *
 * Common.Localization.FullNames: Static class that provides a full name class, 
 * v.0.0.1
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
namespace Common.Localization {
    using System;
    using System.Collections.Generic;
    
    using Common.Generics;

    public class FullName : Notifiable {
        public string Prefix {
            get { return this._prefix; }
            set { this.Set(ref this._prefix, value); }
        } private string _prefix;
        public string FirstName {
            get { return this._firstname; }
            set { this.Set(ref this._firstname, value); }
        } private string _firstname;
        public string MiddleName {
            get { return this._middlename; }
            set { this.Set(ref this._middlename, value); }
        } private string _middlename;

        public string LastName {
            get { return this._lastname; }
            set { this.Set(ref this._lastname, value); }
        } private string _lastname;
        public List<string> Suffix {
            get { return this._suffix; }
            set { this.Set(ref this._suffix, value); }
        } private List<string> _suffix;

        public override string ToString() {
            List<string> tokens = new List<string>() {
                this.Prefix,
                this.FirstName,
                this.MiddleName,
                this.LastName
            };
            tokens.RemoveAll(_ => String.IsNullOrWhiteSpace(_));
            List<string> suffixTokens = this.Suffix;
            suffixTokens.RemoveAll(_ => String.IsNullOrWhiteSpace(_));

            return String.Join(", ",
                String.Join(" ", tokens), 
                String.Join(", ", suffixTokens) 
            );
        }
    }
}
