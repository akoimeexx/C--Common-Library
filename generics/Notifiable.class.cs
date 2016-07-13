/**      1         2         3         4         5         6         7         8
 * 45678901234567890123456789012345678901234567890123456789012345678901234567890
 *
 * Common.Generics.Notifiable: Abstract class to provide easy wpf update 
 *    integration with INotifyPropertyChang[ing|ed]-enabled classes, v.0.3.2
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
    using System.ComponentModel;
    using System.Runtime.CompilerServices;

    public abstract partial class Notifiable : INotifiable {
#region INotifyPropertyChang[ing|ed] Implementation
        public event PropertyChangingEventHandler PropertyChanging;
        public event PropertyChangedEventHandler PropertyChanged;

        public bool Set<T>(ref T field, T value, 
        [CallerMemberName]string propertyName = null) {
#if DEBUG
            Console.WriteLine(DebugString(field, value, propertyName));
#endif
            if (!field.Equals(value)) try {
                SendPropertyChanging(propertyName);
                field = value;
                SendPropertyChanged(propertyName);
                //return true;
            } catch { /*if (field.Equals(value)) return true; */}
            //return false;
            return Object.Equals(field, value);
        }
        public void SendPropertyChanging(
        [CallerMemberName]string propertyName = null) {
            PropertyChanging?.Invoke(
                this, new PropertyChangingEventArgs(propertyName)
            );
        }
        public void SendPropertyChanged(
        [CallerMemberName]string propertyName = null) {
            PropertyChanged?.Invoke(
                this, new PropertyChangedEventArgs(propertyName)
            );
        }

#if DEBUG
        private string DebugString<T>(T field, T value, string property) {
            return String.Format("<{0}> ({1}){2}.Set(ref field = ({3}){4}, " +
                "value = ({3}){5}, property = {6})", 
                this.GetHashCode().ToString(), this.GetType().Name, 
                this.GetType().BaseType.Name, typeof(T).Name, 
                field, value, property
            );
        }
#endif
        ~Notifiable() {
            // Clean up any EventHandler Listeners/Subscribers
            if (this.PropertyChanging != null)
            foreach (var listener in PropertyChanging.GetInvocationList())
                this.PropertyChanging -= (PropertyChangingEventHandler)listener;

            if (this.PropertyChanged != null)
            foreach (var listener in PropertyChanged.GetInvocationList())
                this.PropertyChanged -= (PropertyChangedEventHandler)listener;
        }
#endregion INotifyPropertyChang[ing|ed] Implementation
    }
}
/**
 * Usage:
    public partial class NotifiableClass : Notifiable {
        private string _example = "'string in field '_example''";
        public string Example { get { return this._example; } set {
            this.Set(ref this._example, value);
        }}
        // Read-only property
        public bool Boolean => true;
    }
    public partial class NotifiableClass {
        private void _someHiddenMethod() { }
        public void SomePublicMethod() { }
    }
    public partial class NotifiableClass {
        #region Constructors/Destructor
        public NotifiableClass() : this(1){ }
        public NotifiableClass(int x) : this(x, 2) { }
        public NotifiableClass(int x, int y) : this(x,y,3) { }
        public NotifiableClass(int x, int y, int z) {
            Console.WriteLine("These are the integers:");
            Console.WriteLine("x: {0}, y: {1}, z: {2}", x,y,z);
            
            this.PropertyChanged += new PropertyChangedEventHandler(
            delegate(Object o, PropertyChangedEventArgs args) {
                Console.WriteLine(String.Format("Instance: {0}; Property: {1}", 
                    String.Format("{0}<{1}>", 
                        o.GetType().Name, 
                        o.GetHashCode().ToString()
                    ), 
                    args.PropertyName
                ));
            });
            
            this.Example = "'I used to be an adventurer like you'";
        }
        ~NotifiableClass() { }
        #endregion
    }

    var x = new NotifiableClass();

// Console Output:
//  These are the integers:
//  x: 1, y: 2, z: 3
//  <16234388> (NotifiableClass)Notifiable.Set(ref field = \
//  (String)string in field '_example', value = (String)'I used to be an \
//  adventurer like you', property = Example)
//  Instance: NotifiableClass<49628414>; Property: Example
 */
