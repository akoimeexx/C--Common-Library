/**      1         2         3         4         5         6         7         8
 * 45678901234567890123456789012345678901234567890123456789012345678901234567890
 *
 * Common.Dialogs <Prompt>: Common library providing WPF dialogs, v.0.0.1
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


namespace Common.Dialogs {
    using System;
    using System.Windows;
    using System.Windows.Interop;

    public partial class PromptDialog : Window {
#region CommonDialog pseudo-implementation
        public void Reset() {
            throw new NotImplementedException();
        }
        protected bool RunDialog(IntPtr hwndOwner) {
            bool result = false;
            try {
                HwndSource hSrc = HwndSource.FromHwnd(hwndOwner);
                this.Owner = (Window)hSrc.RootVisual;
                this.ShowDialog();
            } catch (Exception e) {
                MessageBox.Show(e.Message);
            } finally {
                result = (bool)this.DialogResult;
            }
            return result;
        }
#endregion CommonDialog pseudo-implementation
    }
    public partial class PromptDialog {
#region Properties
        private bool _usePlaceHolder = false;
        public string Caption {
            get { return this.lCaption.Content.ToString(); }
            set { this.lCaption.Content = value; }
        }
        public string Result {
            get { return this.tbPromptInput.Text; }
            set { this.tbPromptInput.Text = value; }
        }
        private string _placeholder;
#endregion Properties
    }
    public partial class PromptDialog {
#region Methods
        private void bOk_Click(object sender, RoutedEventArgs e) {
            this.DialogResult = true;
        }
#endregion Methods
    }
    public partial class PromptDialog {
#region Constructors & Destructor
        public PromptDialog() : this("Input:") { }
        public PromptDialog(string caption) : this(caption, "Input Text") { }
        public PromptDialog(string caption, string title) : 
            this(caption, title, "") { }
        public PromptDialog(string caption, string title, string defaultValue) :
            this(caption, title, defaultValue, "") { }
        public PromptDialog(string caption, string title, string defaultValue, 
        string placeholder) {
            InitializeComponent();
            this.Caption = caption;
            this.Title = title;
            this.Result = defaultValue;
            this._placeholder = placeholder;
        }
        ~PromptDialog() { }
#endregion Constructors & Destructor
    }
}
