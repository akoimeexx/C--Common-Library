/**      1         2         3         4         5         6         7         8
 * 45678901234567890123456789012345678901234567890123456789012345678901234567890
 *
 * Common.RelayCommand: Common library include for ICommand wrapper, v.0.0.1
 *    Johnathan Graham McKnight <akoimeexx@gmail.com>
 *
 *
 * Copyright (c) 2015, Johnathan Graham McKnight
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
    //using System.Collections.Generic;
    using System.Windows.Input;

// ://www.kellydun.com/wpf-relaycommand-with-parameter/
//namespace System.Windows.Input {
    public partial class RelayCommand<T> : ICommand {
        #region Property Fields
        private readonly Action<T> _execute = null;
        private readonly Predicate<T> _canExecute = null;
        //// ICommand Property field
        //private event EventHandler _canExecuteChangedInternal;
        #endregion
        
        
        #region Constructors & Destructors
        // New, always executable command
        public RelayCommand(Action<T> execute) : this(execute, null) { }
        // New, conditionally executable command
        public RelayCommand(Action<T> execute, Predicate<T> canExecute) {
            if (execute == null) throw new ArgumentNullException("execute");
            this._execute = execute; this._canExecute = canExecute;
        }
        // Dereference our commands
        ~RelayCommand() { this.Destroy(); }
        #endregion


        #region Public methods
        public void Destroy() {
            /*this._canExecute = _ => false; this._execute = _ => { return; };*/
        }
        #endregion


        #region ICommand Methods & Events
        public bool CanExecute(object parameter) { return 
            (this._canExecute == null) ? true : _canExecute((T)parameter);
        }
        public event EventHandler CanExecuteChanged {
            add { if (this._canExecute != null) 
                CommandManager.RequerySuggested += value; 
            } remove { if (this._canExecute != null) 
                CommandManager.RequerySuggested -= value;
            }
        }
        public void Execute(object parameter) {
            //this._canExecute((T)parameter);
            this._execute((T)parameter);
        }
        #endregion
    }
}
