namespace Common.Generics {
    using System;
    using System.ComponentModel;
    using System.Runtime.InteropServices;
    using System.Windows;

    public partial class NotificationIcon : Notifiable { }
    public partial class NotificationIcon {
#region P/Invoke Implementation
        private static class NI_Shell32 {
            public enum NI_BallonIcon {
                None = 0,
                Info = 1,
                Warning = 2,
                Error = 3,
                User = 4,
                NoSound = 16, 
                LargeIcon = 32, 
                RespectQuietTime = 128, 
                IconMask = 15, 
            }
            public enum NI_Command {
                Add = 0, Modify = 1, Delete = 2, SetFocus = 3, SetVersion = 4
            }
            [FlagsAttribute] public enum NI_Flags {
                None     = 0, 
                Message  = 1 << 0, 
                Icon     = 1 << 1, 
                Tip      = 1 << 2, 
                State    = 1 << 3, 
                Info     = 1 << 4, 
                Guid     = 1 << 5, 
                Realtime = 1 << 6, 
                ShowTip  = 1 << 7,
            }
            public enum NI_Version {
                None, Version_1, Version_2, Version_3, Version_4
            }

            [StructLayout(LayoutKind.Sequential, CharSet=CharSet.Auto)]
            public struct NI_Data {
                public UInt32 cbSize;// = Marshal.SizeOf(typeof(NotifyIconData));
                public IntPtr hWnd;
                public UInt32 uID;
                public UInt32 uFlags;
                public UInt32 uCallbackMessage;
                public IntPtr hIcon;
                [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 128)]
                public string szTip;
                public UInt32 dwState;
                public UInt32 dwStateMask;
                [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 256)]
                public string szInfo;
                public UInt32 uTimeoutOrVersion;
                [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 64)]
                public string szInfoTitle;
                public UInt32 dwInfoFlags;
                public Guid guidItem;
                public IntPtr hBalloonIcon;
            }
            [DllImport("shell32.dll")] 
            private static extern bool Shell_NotifyIcon(
                uint dwMessage, [In] ref NI_Data pnid);

            public static bool Add(ref NI_Data pnid) {
                bool b = false;
                try {
                    b = Shell_NotifyIcon((uint)NI_Command.Add, ref pnid);
                    b = Shell_NotifyIcon((uint)NI_Command.SetVersion, ref pnid);
                } catch (Exception e) {
#if DEBUG
Log.WriteLine(e.Message);
#endif
                }
                return b;
            }
            public static bool Modify(ref NI_Data pnid) {
                bool b = false;
                try {
                    b = Shell_NotifyIcon((uint)NI_Command.Modify, ref pnid);
                } catch (Exception e) {
#if DEBUG
Log.WriteLine(e.Message);
#endif
                }
                return b;
            }
            public static bool Delete(ref NI_Data pnid) {
                bool b = false;
                try {
                    b = Shell_NotifyIcon((uint)NI_Command.Delete, ref pnid);
                } catch (Exception e) {
#if DEBUG
Log.WriteLine(e.Message);
#endif
                }
                return b;
            }
        }
        private static class NI_Kernel32 {
            [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Auto)]
            public struct NI_WndClassEx {
                public UInt32 cbSize;
                public UInt32 style;
                public IntPtr lpfnWndProc;
                public UInt32 cbClsExtra;
                public UInt32 cbWndExtra;
                public IntPtr hInstance;
                public IntPtr hIcon;
                public IntPtr hCursor;
                public IntPtr hBrush;
                public string lpszMenuName;
                public string lpszClassName;
                public IntPtr hIconSm;
            }

            [DllImport("kernel32.dll")]
            public static extern IntPtr GetModuleHandle(string lpModuleName);
            public static IntPtr GetImmediateModuleHandle() {
                return Marshal.GetHINSTANCE(typeof(NotificationIcon).Module);
            }
        }
        private static class NI_User32 {
            [DllImport("user32.dll", SetLastError = false)]
            public static extern IntPtr GetDesktopWindow();
        }
        #endregion P/Invoke Implementation
    }
    public partial class NotificationIcon {
#region Internal-Only Fields
        private NI_Shell32.NI_Data nid = new NI_Shell32.NI_Data() { };
        private Window _unassignedWindow;
#endregion
#region Properties
        private System.Windows.Controls.ContextMenu _contextMenu = null;
        public System.Windows.Controls.ContextMenu ContextMenu {
            get { return this._contextMenu; }
            set { this.Set(ref this._contextMenu, value); }
        }
        private IntPtr _hWnd = NI_User32.GetDesktopWindow();
        public IntPtr HWnd {
            get { return this._hWnd; }
            set { this.Set(ref this._hWnd, value); }
        }
        private System.Drawing.Icon _icon = System.Drawing.SystemIcons.Application;
        public System.Drawing.Icon Icon {
            get { return this._icon; }
            set { this.Set(ref this._icon, value); }
        }
        private string _toolTip = "";
        public string ToolTip {
            get { return this._toolTip; }
            set { this.Set(ref this._toolTip, value); }
        }
#endregion Properties
    }
    public partial class NotificationIcon {
#region Methods
        public void Create() {
            try {
                this.nid = new NI_Shell32.NI_Data();
                this.nid.cbSize = (uint)Marshal.SizeOf(this.nid);
                this.nid.hWnd = this.HWnd;
                this.nid.uFlags = (UInt32)(
                    NI_Shell32.NI_Flags.Icon |
                    NI_Shell32.NI_Flags.Message |
                    NI_Shell32.NI_Flags.Tip |
                    NI_Shell32.NI_Flags.Guid | 
                    NI_Shell32.NI_Flags.ShowTip
                );
                this.nid.guidItem = new Guid(this.GetHashCode(), (short)25, (short)64, new byte[] { 0, 1, 2, 3, 4, 5, 6, 7 });
                this.nid.szTip = String.Copy(this.ToolTip);
                this.nid.hIcon = this.Icon.Handle;
                ////uTimeoutOrVersion = 15000, 
                //this.nid.hWnd = this.HWnd;
                //this.nid.uID = (uint)this.GetHashCode();
                //this.nid.szInfo = this.ToolTip;
                //this.nid.hIcon = this.Icon.Handle;
                //this.nid.uTimeoutOrVersion = (UInt32)NI_Shell32.NI_Version.Version_4;
                //this.nid.guidItem = new Guid(this.GetHashCode(), (short)25, (short)64, new byte[] { 0, 1, 2, 3, 4, 5, 6, 7 });
                NI_Shell32.Add(ref this.nid);
            } catch (Exception e) {
#if DEBUG
Log.WriteLine(String.Format("{0}: {1}", e.GetType().Name, e.Message));
#endif
            } finally { }
        }
        public void Update() {
            try {
                //this.nid.hWnd = this.HWnd;
                //this.nid.uID = (uint)this.GetHashCode();
                //this.nid.szInfo = this.ToolTip;
                //this.nid.hIcon = this.Icon.Handle;
                //this.nid.uTimeoutOrVersion = (UInt32)NI_Shell32.NI_Version.Version_4;
                //this.nid.guidItem = new Guid(this.GetHashCode(), (short)25, (short)64, new byte[] { 0, 1, 2, 3, 4, 5, 6, 7 });

                //NI_Shell32.Modify(ref this.nid);
            } catch (Exception e) {
#if DEBUG
Log.WriteLine(String.Format("{0}: {1}", e.GetType().Name, e.Message));
#endif
            } finally { }
        }
        public void Delete() {
            try {
                NI_Shell32.Delete(ref this.nid);
            } catch (Exception e) {
#if DEBUG
Log.WriteLine(String.Format("{0}: {1}", e.GetType().Name, e.Message));
#endif
            } finally { }
        }
        private void NI_PropertyChanged(
        object sender, PropertyChangedEventArgs e) {
            this.Update();
        }
        public static NotificationIcon NewInstance() {
            NotificationIcon ni = null;
            try {
                throw new NotImplementedException(
                    "Generate new instance of NI, based on Ver 4."
                );
            } catch (Exception e) {
#if DEBUG
Log.WriteLine(e.Message);
#endif
                if (e is NotImplementedException) throw e;
            }
            return ni;
        }
#endregion Methods
    }
    public partial class NotificationIcon {
#region Constructors/Destructor
        public NotificationIcon() {
            this.Create();
//Log.WriteLine(String.Format("Resources:{0}{1}", Environment.NewLine, 
//    String.Join(Environment.NewLine, new string[] {
//        "Using P/Invokes", "https://www.talksharp.com/what-is-pinvoke", "", 
//        "http://www.codeproject.com/Articles/2043/Displaying-a-Notify-Icon-s-Balloon-Tool-Tip",
//        "https://msdn.microsoft.com/en-us/library/windows/desktop/dd940367(v=vs.85).aspx",
//        "https://msdn.microsoft.com/en-us/library/windows/desktop/ee330740(v=vs.85).aspx",
//        "https://msdn.microsoft.com/en-us/library/windows/desktop/bb762159(v=vs.85).aspx",
//        "https://msdn.microsoft.com/en-us/library/windows/desktop/bb773352(v=vs.85).aspx", 
//        "http://computer-programming-forum.com/4-csharp/33af8742ce86c427.htm", 
//        "http://www.codeproject.com/Articles/4768/Basic-use-of-Shell_NotifyIcon-in-Win32",
//        "http://www.pinvoke.net/default.aspx/shell32.shell_notifyicon",
//        "http://www.pinvoke.net/default.aspx/Structures.NOTIFYICONDATA", 
//        "http://www.pinvoke.net/default.aspx/shell32/Shell_NotifyIcon.html", 
//        "http://www.vbforums.com/showthread.php?585942-RESOLVED-NotifyIcon-and-ContextMenu-without-using-System-Windows-Forms", 
//        "http://stackoverflow.com/questions/9450645/notification-icon-tooltip-not-showing-despite-properly-setting-sztip", 
//        "", "Mimicking Unions in C#", 
//        "https://social.msdn.microsoft.com/Forums/en-US/60150e7b-665a-49a2-8e2e-2097986142f3/c-equivalent-to-c-union?forum=csharplanguage", 
//        "", "(unwanted) using pre-made wpf notifyicon: ", 
//        "http://www.codeproject.com/Articles/36468/WPF-NotifyIcon", 
//        "http://www.hardcodet.net/wpf-notifyicon"
//    }
//)));
            this.PropertyChanged += NI_PropertyChanged;
        }
        ~NotificationIcon() {
            this.Delete();
#if DEBUG
Log.WriteLine("Destroying instance");
#endif
        }
#endregion Constructors/Destructor
    }
}


#if false
namespace Common.Generics {
    using System;
    using System.ComponentModel;
    using System.Runtime.InteropServices;
    using System.Windows;
    
    public partial class NotificationIcon : Notifiable {
#region Notification Icon Interop Defintions
        private enum NI_Command {
            Add = 0, Modify = 1, Delete = 2, SetFocus = 3, SetVersion = 4, 
        }
        [Flags]
        private enum NI_Flags {
            None    = 0, 
            Message = 1 << 0, 
            Icon    = 1 << 1, 
            Tip     = 1 << 2, 
            State   = 1 << 3, 
            Info    = 1 << 4, 
            Guid    = 1 << 5, 
        }
        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Auto)]
        private struct NotifyIconData {
            public UInt32 cbSize;// = Marshal.SizeOf(typeof(NotifyIconData));
            public IntPtr hWnd;
            public UInt32 uID;
            public NI_Flags uFlags;
            public UInt32 uCallbackMessage;
            public IntPtr hIcon;
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 128)]
            public string szTip;
            public UInt32 dwState;
            public UInt32 dwStateMask;
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 256)]
            public string szInfo;
            public UInt32 uTimeoutOrVersion;
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 64)]
            public string szInfoTitle;
            public UInt32 dwInfoFlags;
        }
        [DllImport("shell32.dll")]
        private static extern bool Shell_NotifyIcon(uint dwMessage, [In] ref NotifyIconData pnid);
#endregion Notification Icon Interop Defintions
    }
    public partial class NotificationIcon {
#region Internal-Only Fields
        private NotifyIconData nid = new NotifyIconData();
#endregion Internal-Only Fields
#region Properties
        private System.Windows.Controls.ContextMenu _contextMenu = 
            new System.Windows.Controls.ContextMenu();
        public System.Windows.Controls.ContextMenu ContextMenu {
            get { return this._contextMenu; }
            set { this.Set(ref this._contextMenu, value); }
        }
        private System.Drawing.Icon _icon = new System.Drawing.Icon(@"G:\tmp\UPDATES\common\_meta_\CommonLibrary.resource.ico");
        public System.Drawing.Icon Icon {
            get { return this._icon; }
            set { this.Set(ref this._icon, value); }
        }
        private string _toolTip = "";
        public string ToolTip {
            get { return this._toolTip; }
            set { this.Set(ref this._toolTip, value); }
        }
#endregion Properties
    }
    public partial class NotificationIcon {
#region Methods
        public void Create() {
            try {
                Shell_NotifyIcon((uint)NI_Command.Add, ref this.nid);
            } catch (Exception e) {
#if DEBUG
Log.WriteLine(String.Format("{0}: {1}", e.GetType().Name, e.Message));
#endif
            } finally { }
        }
        public void Update() {
            try {
                this.nid.szInfo = this.ToolTip;
                this.nid.hIcon = this.Icon.Handle;
                Shell_NotifyIcon((uint)NI_Command.Modify, ref this.nid);
            } catch (Exception e) {
#if DEBUG
Log.WriteLine(String.Format("{0}: {1}", e.GetType().Name, e.Message));
#endif
            } finally { }
        }
        public void Delete() {
            try {
//                Shell_NotifyIcon((uint)NI_Command.Delete, ref this.nid);
            } catch (Exception e) {
#if DEBUG
Log.WriteLine(String.Format("{0}: {1}", e.GetType().Name, e.Message));
#endif
            } finally { }
        }
        private void NI_PropertyChanged(
        object sender, PropertyChangedEventArgs e) {
            this.Update();
            //throw new NotImplementedException(String.Format(
            //    "(object)sender: {0}{1}(PropertyChangedEventArgs)e: {2}", 
            //    sender.GetType().Name, Environment.NewLine, e.PropertyName
            //));
        }
#endregion Methods
    }
    public partial class NotificationIcon {
#region Constructors & Destructor
        public NotificationIcon() {
            this.nid = new NotifyIconData() {
                cbSize = (uint)Marshal.SizeOf(new NotifyIconData()), 
                hWnd = IntPtr.Zero, 
                uID = (uint)this.GetHashCode(), 
                uFlags = NI_Flags.Icon | NI_Flags.Message | NI_Flags.Tip, 
                //szInfo = this.ToolTip,
                //hIcon = this.Icon.Handle, 
            };
            this.Create();
//            this.Update();
            this.PropertyChanged += NI_PropertyChanged;
#if false
Log.WriteLine(String.Format("Resources:{0}{1}", Environment.NewLine, 
    String.Join(Environment.NewLine, new string[] {
        "Using P/Invokes", "https://www.talksharp.com/what-is-pinvoke", "", 
        "http://www.codeproject.com/Articles/2043/Displaying-a-Notify-Icon-s-Balloon-Tool-Tip",
        "https://msdn.microsoft.com/en-us/library/windows/desktop/dd940367(v=vs.85).aspx",
        "https://msdn.microsoft.com/en-us/library/windows/desktop/ee330740(v=vs.85).aspx",
        "https://msdn.microsoft.com/en-us/library/windows/desktop/bb762159(v=vs.85).aspx",
        "https://msdn.microsoft.com/en-us/library/windows/desktop/bb773352(v=vs.85).aspx", 
        "http://computer-programming-forum.com/4-csharp/33af8742ce86c427.htm", 
        "http://www.codeproject.com/Articles/4768/Basic-use-of-Shell_NotifyIcon-in-Win32",
        "http://www.pinvoke.net/default.aspx/shell32.shell_notifyicon",
        "http://www.pinvoke.net/default.aspx/Structures.NOTIFYICONDATA", 
        "http://www.pinvoke.net/default.aspx/shell32/Shell_NotifyIcon.html", 
        "http://www.vbforums.com/showthread.php?585942-RESOLVED-NotifyIcon-and-ContextMenu-without-using-System-Windows-Forms", 
        "http://stackoverflow.com/questions/9450645/notification-icon-tooltip-not-showing-despite-properly-setting-sztip", 
        "", "Mimicking Unions in C#", 
        "https://social.msdn.microsoft.com/Forums/en-US/60150e7b-665a-49a2-8e2e-2097986142f3/c-equivalent-to-c-union?forum=csharplanguage", 
        "", "(unwanted) using pre-made wpf notifyicon: ", 
        "http://www.codeproject.com/Articles/36468/WPF-NotifyIcon", 
        "http://www.hardcodet.net/wpf-notifyicon"
    }
)));
#endif
        }
        ~NotificationIcon() {
#if DEBUG
Log.WriteLine("Destroying instance");
#endif
            this.Delete();
        }
#endregion Constructors & Destructor
    }

    public partial class NotifyIcon : FrameworkElement, IDisposable, 
    INotifyPropertyChanging, INotifyPropertyChanged {
#region IDisposable Support
        private bool disposedValue = false; // To detect redundant calls
        protected virtual void Dispose(bool disposing) {
            if (!disposedValue) {
                if (disposing) {
                    // TODO: dispose managed state (managed objects).
                }

                // TODO: free unmanaged resources (unmanaged objects) and 
                //       override a finalizer below.
                // TODO: set large fields to null.
                disposedValue = true;
            }
        }

        // TODO: override a finalizer only if Dispose(bool disposing) above has 
        //       code to free unmanaged resources.
        // ~NotifyIcon() {
        //   // Do not change this code. Put cleanup code in 
        //   // Dispose(bool disposing) above.
        //   Dispose(false);
        // }

        // This code added to correctly implement the disposable pattern.
        public void Dispose() {
            // Do not change this code. Put cleanup code in 
            // Dispose(bool disposing) above.
            Dispose(true);
            // TODO: uncomment the following line if the finalizer is overridden
            //       above.
            // GC.SuppressFinalize(this);
        }
#endregion
#region INotifyProperty Support
        protected virtual void SendPropertyChanging(String propertyName, 
        Action<String, PropertyChangedEventArgs> callback = null) {
            if ((this.PropertyChanging != null)) {
                this.PropertyChanging(this, 
                    new PropertyChangingEventArgs(propertyName));
            }
            if (callback != null) {
                callback(propertyName, new PropertyChangedEventArgs(
                    propertyName)
                );
            }
        }
        protected virtual void SendPropertyChanged(String propertyName, 
        Action<String, PropertyChangedEventArgs> callback=null) {
            if ((this.PropertyChanged != null)) {
                this.PropertyChanged(this, 
                    new PropertyChangedEventArgs(propertyName));
            }
            if (callback != null) {
                callback(propertyName, new PropertyChangedEventArgs(
                    propertyName)
                );
            }
        }
        public event PropertyChangingEventHandler PropertyChanging;
        public event PropertyChangedEventHandler PropertyChanged;
#endregion
    }
    public partial class NotifyIcon {
#region Notify-specific structures
        public enum MessageType {
            None =        1 << 0, 
            Information = 1 << 1,
            Warning =     1 << 2,
            Error =       1 << 3,
        }
#endregion
    }
    public partial class NotifyIcon {
        private string _example = "Example field, used public properties.";
        public string example { get { return this._example; } set {
            if(this._example != value) {
                this.SendPropertyChanging("example");
                this._example = value;
                this.SendPropertyChanged("example");
            }
        }}
        // Read-only property
        public bool Boolean => true;
    }
    public partial class NotifyIcon {
        private void _SomeHiddenMethod() { }
        public void SomePublicMethod() { }
    }
    public partial class NotifyIcon {
#region Constructors/Destructor
        public NotifyIcon() : this(1){ }
        public NotifyIcon(int x) : this(x, 2) { }
        public NotifyIcon(int x, int y) : this(x,y,3) { }
        public NotifyIcon(int x, int y, int z) {
            Console.WriteLine("These are the integers:");
            Console.WriteLine("x: {0}, y: {1}, z: {2}", x,y,z);
        }
        ~NotifyIcon() { }
#endregion
    }
}
#endif
