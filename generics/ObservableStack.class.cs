/**      1         2         3         4         5         6         7         8
 * 45678901234567890123456789012345678901234567890123456789012345678901234567890
 *
 * Common.Generics: Common library include for ObservableStack objects, v.0.0.1
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
    using System.Collections.Specialized;
    using System.ComponentModel;
    using System.Runtime.CompilerServices;

    public partial class ObservableStack<T> : Stack<T>, INotifyCollectionChanged, INotifiable {
#region Properties
#endregion Properties
    }
    public partial class ObservableStack<T> {
#region Methods
    #region INotifyProperty Support
        public bool Set<T>(ref T field, T value, 
        [CallerMemberName]string propertyName = null) {
            if (!field.Equals(value)) try {
                SendPropertyChanging(propertyName);
                field = value;
                SendPropertyChanged(propertyName);
                return true;
            } catch { if (field.Equals(value)) return true; }
            return false;
        }
        public void SendPropertyChanging(
        [CallerMemberName]string propertyName = null) {
            if (PropertyChanging != null) PropertyChanging(
                this, new PropertyChangingEventArgs(propertyName)
            );
        }
        public void SendPropertyChanged(
        [CallerMemberName]string propertyName = null) {
            if (PropertyChanged != null) PropertyChanged(
                this, new PropertyChangedEventArgs(propertyName)
            );
        }
    #endregion INotifyPropertySupport

        public new virtual void Clear() {
            base.Clear();
            this.OnCollectionChanged(
                new NotifyCollectionChangedEventArgs(
                    NotifyCollectionChangedAction.Reset
            ));
        }

        public new virtual T Pop() {
            var item = base.Pop();
            this.OnCollectionChanged(
                new NotifyCollectionChangedEventArgs(
                    NotifyCollectionChangedAction.Remove, item
            ));
            return item;
        }

        public new virtual void Push(T item) {
            base.Push(item);
            this.OnCollectionChanged(
                new NotifyCollectionChangedEventArgs(
                    NotifyCollectionChangedAction.Add, item
            ));
        }

        protected virtual void OnCollectionChanged(
        NotifyCollectionChangedEventArgs e) {
            this.RaiseCollectionChanged(e);
        }

        protected virtual void OnPropertyChanged(PropertyChangedEventArgs e) {
            this.RaisePropertyChanged(e);
        }

        private void RaiseCollectionChanged(
        NotifyCollectionChangedEventArgs e) {
            if (this.CollectionChanged != null)
                this.CollectionChanged(this, e);
        }

        private void RaisePropertyChanged(PropertyChangedEventArgs e) {
            if (this.PropertyChanged != null)
                this.PropertyChanged(this, e);
        }        
#endregion Methods
    }

    public partial class ObservableStack<T> {
#region Events
    public event NotifyCollectionChangedEventHandler CollectionChanged;
    public event PropertyChangingEventHandler PropertyChanging;
    public event PropertyChangedEventHandler PropertyChanged;
#endregion Events
    }

    public partial class ObservableStack<T> {
#region Constructors & Destructor
        public ObservableStack() { }
        public ObservableStack(IEnumerable<T> collection) {
            foreach (var item in collection)
                base.Push(item);
        }

        ~ObservableStack() {
            // Clean up any EventHandler Subscribers
            if (this.CollectionChanged != null)
            foreach (var subscriber in CollectionChanged.GetInvocationList()) {
                this.CollectionChanged -= 
                    (NotifyCollectionChangedEventHandler)subscriber;
            }
            if (this.PropertyChanging != null)
            foreach (var subscriber in PropertyChanging.GetInvocationList()) {
                this.PropertyChanging -= 
                    (PropertyChangingEventHandler)subscriber;
            }
            if (this.PropertyChanged != null)
            foreach (var subscriber in PropertyChanged.GetInvocationList()) {
                this.PropertyChanged -= (PropertyChangedEventHandler)subscriber;
            }
        }
#endregion Constructors & Destructor
    }
}
/*
    event PropertyChangedEventHandler INotifyPropertyChanged.PropertyChanged
    {
        add { this.PropertyChanged += value; }
        remove { this.PropertyChanged -= value; }
    }
 */
