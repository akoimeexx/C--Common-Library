/**      1         2         3         4         5         6         7         8
 * 45678901234567890123456789012345678901234567890123456789012345678901234567890
 *
 * Common.Generics.DelimitedTextFile: Base class to provide csv-munging, v.0.0.1
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
    using System.Collections;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    
    public partial class DelimitedTextFile : IDisposable {
#region IDisposable Support
        private bool disposedValue = false; // To detect redundant calls

        protected virtual void Dispose(bool disposing) {
            if (!disposedValue) {
                if (disposing) {
                    // TODO: dispose managed state (managed objects).
                }

                // TODO: free unmanaged resources (unmanaged objects) and override a finalizer below.
                // TODO: set large fields to null.

                disposedValue = true;
            }
        }

        // TODO: override a finalizer only if Dispose(bool disposing) above has code to free unmanaged resources.
        // ~DelimitedTextFile() {
        //   // Do not change this code. Put cleanup code in Dispose(bool disposing) above.
        //   Dispose(false);
        // }

        // This code added to correctly implement the disposable pattern.
        void IDisposable.Dispose() {
            // Do not change this code. Put cleanup code in Dispose(bool disposing) above.
            Dispose(true);
            // TODO: uncomment the following line if the finalizer is overridden above.
            // GC.SuppressFinalize(this);
        }
#endregion IDisposable Support
    }
    public partial class DelimitedTextFile : 
    IEnumerator<List<KeyValuePair<int, object>>> {
#region IEnumerator Support
        object IEnumerator.Current {
            get {
                throw new NotImplementedException();
            }
        }

        List<KeyValuePair<int, object>> IEnumerator<List<KeyValuePair<int, object>>>.Current {
            get {
                throw new NotImplementedException();
            }
        }

        bool IEnumerator.MoveNext() {
            throw new NotImplementedException();
        }

        void IEnumerator.Reset() {
            throw new NotImplementedException();
        }
#endregion IEnumerator Support
    }
//    public partial class DelimitedTextFile {
//#region Components
//        private FileStream _fs;
//#endregion Components
//    }
    public partial class DelimitedTextFile {
#region Properties
        private static readonly string DEF_DELIMITER = ",";
        private static readonly string DEF_QUOTEDELIMITER = "\"";
        private static readonly string DEF_RECORDDELIMITER = 
            Environment.NewLine;

        private List<string> _delimiters = new List<string>() { DEF_DELIMITER };
        public string Delimiter { get {
            return (this.Delimiters.Count > 0) ? 
                this.Delimiters[0] : DEF_DELIMITER;
        } set {
            this.Delimiters = null;
        }}
        public List<string> Delimiters { get {
            return this._delimiters;
        } set {
            this._delimiters = value;
            if (this._delimiters == null || this._delimiters.Count < 1)
                this._delimiters = new List<string>() { DEF_DELIMITER };
        }}

        private List<string> _quotechars = 
            new List<string>() { DEF_QUOTEDELIMITER };
        public string QuoteDelimiter { get {
            return (this.QuoteDelimiters.Count > 0) ? 
                this.QuoteDelimiters[0] : DEF_QUOTEDELIMITER;
        } set {
            this.QuoteDelimiters = null;
        }}
        public List<string> QuoteDelimiters { get {
            return this._quotechars;
        } set {
            this._quotechars = value;
            if (this._quotechars == null || this._quotechars.Count < 1)
                this._quotechars = new List<string>() { DEF_QUOTEDELIMITER };
        }}

        private List<string> _recordChars = 
            new List<string>() { DEF_RECORDDELIMITER };
        public string RecordDelimiter { get {
            return (this.RecordDelimiters.Count > 0) ? 
                this.RecordDelimiters[0] : DEF_RECORDDELIMITER;
        } set {
            this.RecordDelimiters = null;
        }}
        public List<string> RecordDelimiters { get {
            return this._recordChars;
        } set {
            this._recordChars = value;
            if (this._recordChars == null || this._recordChars.Count < 1)
                this._recordChars = new List<string>() { DEF_RECORDDELIMITER };
        }}

        private List<KeyValuePair<int, Type>> _columnTypes = 
            new List<KeyValuePair<int, Type>>();
        public List<KeyValuePair<int, Type>> ColumnTypes {
            get { return this._columnTypes; }
            set {
                this._columnTypes = value;
                if (this._columnTypes == null) {
                    this._columnTypes = new List<KeyValuePair<int, Type>>();
                    this._columnHeaders = new List<KeyValuePair<int, string>>();
                    this._records = new List<List<KeyValuePair<int, object>>>();
                }
            }}
        private List<KeyValuePair<int, string>> _columnHeaders = 
            new List<KeyValuePair<int, string>>();
        public List<KeyValuePair<int, string>> ColumnHeaders{
            get { return this._columnHeaders; }
            set {
                this._columnHeaders = value;
                if (this._columnHeaders == null)
                    this._columnHeaders = new List<KeyValuePair<int, string>>();
            }
        }
        private bool _hasHeaders = false;
        public bool HasHeaders {
            get { return this._hasHeaders; }
            set { this._hasHeaders = value; }
        }

        private List<List<KeyValuePair<int, object>>> _records = 
            new List<List<KeyValuePair<int, object>>>();
        public List<List<KeyValuePair<int, object>>> Records { get {
            return this._records;
        } set { 
            this._records = value;
            if (this._records == null) {
                this._columnTypes = new List<KeyValuePair<int, Type>>();
                this._records = new List<List<KeyValuePair<int, object>>>();
            }
        }}
        public int Count { get { return this.Records.Count; }}
#endregion Properties
    }
    public partial class DelimitedTextFile {
#region Indexers
        public List<KeyValuePair<int, object>> this[int index] {
            get { return this.Records[index]; } // Get Row
            set {
                this.Records[index] = value;
                if (value == null) this.Records.RemoveAt(index);
            }
        }
        public List<object> this[string index] {
            get { // Get Column by string header
                List<object> lo = new List<object>();
                int i = -1;

                foreach (KeyValuePair<int, string> kv in this.ColumnHeaders) {
                    if (kv.Value == index) {
                        i = kv.Key;
                        break;
                    }
                }
                if (i != -1) foreach (List<KeyValuePair<int, object>> lkv 
                in this.Records) {
                    lkv.ForEach( _ => {
                        if (_.Key == i) lo.Add(_.Value);
                    });
                } else { throw new KeyNotFoundException(index); }
                return lo;
            }
        }
#endregion Indexers
    }
    public partial class DelimitedTextFile {
#region Methods
        private bool ParseDTF(string path) {
            bool b = false;
            using (FileStream fs = File.OpenRead(path)) {

            }
            return b;
        }
#endregion Methods
    }
}
