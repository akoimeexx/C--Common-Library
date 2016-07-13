/**      1         2         3         4         5         6         7         8
 * 45678901234567890123456789012345678901234567890123456789012345678901234567890
 *
 * Common.Dialogs <File>: Common library providing WPF dialogs, v.0.0.1
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
    using Microsoft.Win32;
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Globalization;
    using System.IO;
    using System.Linq;
    using System.Runtime.InteropServices;
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Data;
    using System.Windows.Interop;
    using System.Windows.Media;
    using System.Windows.Media.Imaging;

    public partial class OpenDialog : CommonDialog {
#region CommonDialog implementation
        public override void Reset() {
            this.fd.Reset();
        }
        protected override bool RunDialog(IntPtr hwndOwner) {
            bool result = false;
            try {
                HwndSource hSrc = HwndSource.FromHwnd(hwndOwner);
                //fd.Owner = (Window)hSrc.RootVisual;
                result = (bool)fd.ShowDialog();
            } catch (Exception e) {
                MessageBox.Show(e.Message);
            } finally { }
            return result;
        }
#endregion CommonDialog implementation
    }
    public partial class OpenDialog {
#region Properties
        public bool AddExtension { get {
            return this.fd.AddExtension;
        } set {
            this.fd.AddExtension = value;
        }}
        public bool CheckFileExists { get {
            return this.fd.CheckFileExists;
        } set {
            this.fd.CheckFileExists = value;
        }}
        public bool CheckPathExists { get {
            return this.fd.CheckPathExists;
        } set {
            this.fd.CheckPathExists = value;
        }}
        public System.Collections.Generic.IList<FileDialogCustomPlace> 
        CustomPlaces { get {
            return this.fd.CustomPlaces;
        } set {
            this.fd.CustomPlaces = value;
        }}
        public string DefaultExt { get {
            return this.fd.DefaultExt;
        } set {
            this.fd.DefaultExt = value;
        }}
        public bool DereferenceLinks { get {
            return this.fd.DereferenceLinks;
        } set {
            this.fd.DereferenceLinks = value;
        }}
        public string FileName { get {
            return this.fd.FileName;
        } set {
            this.fd.FileName = value;
        }}
        public string[] FileNames { get {
            return this.fd.FileNames;
        }}
        public System.ComponentModel.CancelEventHandler FileOk { set {
            this.fd.FileOk += value;
        }}
        public string Filter { get {
            return this.fd.Filter;
        } set {
            this.fd.Filter = value;
        }}
        public int FilterIndex { get {
            return this.fd.FilterIndex;
        } set {
            this.fd.FilterIndex = value;
        }}
        public string InitialDirectory { get {
            return this.fd.InitialDirectory;
        } set {
            this.fd.InitialDirectory = value;
        }}
        public bool Multiselect { get {
            return this.fd.Multiselect;
        } set {
            this.fd.Multiselect = value;
        }}
        public bool ReadOnlyChecked { get {
            return this.fd.ReadOnlyChecked;
        } set {
            this.fd.ReadOnlyChecked = value;
        }}
        public bool RestoreDirectory { get {
            return this.fd.RestoreDirectory;
        } set {
            this.fd.RestoreDirectory = value;
        }}
        public string SafeFileName { get {
            return this.fd.SafeFileName;
        }}
        public string[] SafeFileNames { get {
            return this.fd.SafeFileNames;
        }}
        public bool ShowReadOnly { get {
            return this.fd.ShowReadOnly;
        } set {
            this.fd.ShowReadOnly = value;
        }}
        public new object Tag { get {
            return this.fd.Tag;
        } set {
            this.fd.Tag = value;
        }}
        public string Title { get {
            return this.fd.Title;
        } set {
            this.fd.Title = value;
        }}
        public bool ValidateNames { get {
            return this.fd.ValidateNames;
        } set {
            this.fd.ValidateNames = value;
        }}
#endregion Properties
    }
    public partial class OpenDialog {
#region Components
        private OpenFileDialog fd = new OpenFileDialog();
#endregion Components
    }
    public partial class OpenDialog {
#region Methods
        public System.IO.Stream OpenFile() {
            return this.fd.OpenFile();
        }
        public System.IO.Stream[] OpenFiles() {
            return this.fd.OpenFiles();
        }
#endregion Methods
    }
    public partial class OpenDialog {
#region Constructors & Destructor
        public OpenDialog() : this("All Files (*.*)|*.*;") { }
        public OpenDialog(string Filters) {
            this.fd.Filter = Filters;
        }
#endregion Constructors & Destructor
    }


    public partial class PathDialog : Window {
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
    public partial class PathDialog {
#region Properties
        private ObservableCollection<string> _drives = 
            new ObservableCollection<string>(Directory.GetLogicalDrives());
        public ObservableCollection<string> Drives { get { 
            return this._drives;
        }}
        public string Path { get { return this.tbPath.Text; }}
#endregion Properties
    }
    public partial class PathDialog {
#region Components
        private class PathToIconConverter : 
        System.Windows.Data.IValueConverter {
            public static PathToIconConverter Instance = 
                new PathToIconConverter();

            public object Convert(object v, Type t, object p, 
            CultureInfo c) {
                return Extensions.FileExtensions.GetAssociatedIcon(v as string);
            }

            public object ConvertBack(object v, Type t, object p, 
            CultureInfo c) {
                throw new NotSupportedException("Cannot convert back");
            }
        }
#endregion Components
    }
    public partial class PathDialog {
#region Methods
        private void bOk_Click(object sender, RoutedEventArgs e) {
            this.DialogResult = true;
        }
        private void bCreateFolder_Click(object o, RoutedEventArgs e) {
            try {
                Common.Dialogs.PromptDialog pd = new PromptDialog(
                    "Enter a name for the new folder:",
                    "Create New Folder...",
                    "New Folder"
                );
                if ((bool)pd.ShowDialog() && 
                !String.IsNullOrWhiteSpace(pd.Result)) Directory.CreateDirectory(
                    String.Format(@"{0}\{1}", tbPath.Text, pd.Result)
                );
            } catch (Exception ex) {
                MessageBox.Show(
                    ex.Message, ex.GetType().Name,
                    MessageBoxButton.OK, MessageBoxImage.Error
                );
            }
        }
        private void PathExpanded_Event(object o, RoutedEventArgs e) {
            TreeViewItem tvi = null; string subpath = null;
            try {
                tvi = e.OriginalSource as TreeViewItem;
            
                if (tvi != null) {
                    if (Directory.Exists((string)(tvi.Tag)))
                        subpath = (string)(tvi.Tag);
                    if (tvi.Items.Count == 0 || 
                    ((TreeViewItem)tvi.Items[0]).Tag == null) {
                        tvi.Items.Clear();
                        if (Directory.GetDirectories(subpath).Length > 0)
                        foreach (string s in Directory.GetDirectories(
                        subpath)) {
                            TreeViewItem stvi = new TreeViewItem();
                            stvi.Header = s.Substring(s.LastIndexOf("\\") + 1);
                            stvi.Tag = s;
                            stvi.FontWeight = FontWeights.Normal;
                            stvi.Items.Add(new TreeViewItem() {
                                Header = "Loading...", Tag = null
                            });
                            tvi.Items.Add(stvi);
                        }
                    }
                }
            } catch (Exception ex) { Console.WriteLine(subpath); }
        }
#endregion Methods
    }
    public partial class PathDialog {
#region Constructors & Destructor
        public PathDialog() {
            InitializeComponent();
        }
        ~PathDialog() {

        }
#endregion Constructors & Destructor
    }
    
    
    public partial class SaveDialog : CommonDialog {
#region CommonDialog implementation
        public override void Reset() {
            this.fd.Reset();
        }
        protected override bool RunDialog(IntPtr hwndOwner) {
            bool result = false;
            try {
                HwndSource hSrc = HwndSource.FromHwnd(hwndOwner);
                //fd.Owner = (Window)hSrc.RootVisual;
                result = (bool)fd.ShowDialog();
            } catch (Exception e) {
                MessageBox.Show(e.Message);
            } finally { }
            return result;
        }
#endregion CommonDialog implementation
    }
    public partial class SaveDialog {
#region Properties
        public bool AddExtension { get {
            return this.fd.AddExtension;
        } set {
            this.fd.AddExtension = value;
        }}
        public bool CheckFileExists { get {
            return this.fd.CheckFileExists;
        } set {
            this.fd.CheckFileExists = value;
        }}
        public bool CheckPathExists { get {
            return this.fd.CheckPathExists;
        } set {
            this.fd.CheckPathExists = value;
        }}
        public System.Collections.Generic.IList<FileDialogCustomPlace> 
        CustomPlaces { get {
            return this.fd.CustomPlaces;
        } set {
            this.fd.CustomPlaces = value;
        }}
        public string DefaultExt { get {
            return this.fd.DefaultExt;
        } set {
            this.fd.DefaultExt = value;
        }}
        public bool DereferenceLinks { get {
            return this.fd.DereferenceLinks;
        } set {
            this.fd.DereferenceLinks = value;
        }}
        public string FileName { get {
            return this.fd.FileName;
        } set {
            this.fd.FileName = value;
        }}
        public string[] FileNames { get {
            return this.fd.FileNames;
        }}
        public System.ComponentModel.CancelEventHandler FileOk { set {
            this.fd.FileOk += value;
        }}
        public string Filter { get {
            return this.fd.Filter;
        } set {
            this.fd.Filter = value;
        }}
        public int FilterIndex { get {
            return this.fd.FilterIndex;
        } set {
            this.fd.FilterIndex = value;
        }}
        public string InitialDirectory { get {
            return this.fd.InitialDirectory;
        } set {
            this.fd.InitialDirectory = value;
        }}
        public bool OverwritePrompt { get {
            return this.fd.OverwritePrompt;
        } set {
            this.fd.OverwritePrompt = value;
        }}
        public bool RestoreDirectory { get {
            return this.fd.RestoreDirectory;
        } set {
            this.fd.RestoreDirectory = value;
        }}
        public string SafeFileName { get {
            return this.fd.SafeFileName;
        }}
        public string[] SafeFileNames { get {
            return this.fd.SafeFileNames;
        }}
        public new object Tag { get {
            return this.fd.Tag;
        } set {
            this.fd.Tag = value;
        }}
        public string Title { get {
            return this.fd.Title;
        } set {
            this.fd.Title = value;
        }}
        public bool ValidateNames { get {
            return this.fd.ValidateNames;
        } set {
            this.fd.ValidateNames = value;
        }}
#endregion Properties
    }
    public partial class SaveDialog {
#region Components
        private SaveFileDialog fd = new SaveFileDialog();
#endregion Components
    }
    public partial class SaveDialog {
#region Methods
        public System.IO.Stream OpenFile() {
            return this.fd.OpenFile();
        }
#endregion Methods
    }
    public partial class SaveDialog {
#region Constructors & Destructor
        public SaveDialog() : this("All Files (*.*)|*.*;") { }
        public SaveDialog(string Filters) {
            this.fd.Filter = Filters;
        }
#endregion Constructors & Destructor
    }
}
