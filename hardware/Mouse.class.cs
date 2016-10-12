/**      1         2         3         4         5         6         7         8
 * 45678901234567890123456789012345678901234567890123456789012345678901234567890
 *
 * Common.Hardware.Mouse: Common library include for mouse methods, v.0.0.1
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
namespace Common.Hardware {
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Diagnostics;
    using System.Runtime.InteropServices;
    using System.Windows;
    using System.Windows.Input;
    
    public partial class MouseHook {
#region Components
        [DllImport(
            "user32.dll", 
            CharSet=CharSet.Auto, 
            CallingConvention=CallingConvention.StdCall
        )]
        private static extern int SetWindowsHookEx(
            int idHook, HookProc lpfn, IntPtr hInstance, int threadId
        );
        [DllImport(
            "user32.dll", 
            CharSet=CharSet.Auto, 
            CallingConvention=CallingConvention.StdCall
        )]
        private static extern bool UnhookWindowsHookEx(int idHook);
		
        [DllImport(
            "user32.dll", 
            CharSet=CharSet.Auto, 
            CallingConvention=CallingConvention.StdCall
        )]
        private static extern int CallNextHookEx(
            int idHook, int nCode, IntPtr wParam, IntPtr lParam
        );
        [DllImport("kernel32.dll", CharSet=CharSet.Auto, SetLastError=true)]
        private static extern IntPtr GetModuleHandle(string lpModuleName);

        // Window Hook int for Mouse
        private enum HookEvents : int {
            WH_MOUSE = 7,
            WH_MOUSE_LL = 14
        }
        private enum MouseMessages {
            WM_LBUTTONDOWN = 0x0201,
            WM_LBUTTONUP = 0x0202,
            WM_MOUSEMOVE = 0x0200,
            WM_MOUSEWHEEL = 0x020A,
            WM_RBUTTONDOWN = 0x0204,
            WM_RBUTTONUP = 0x0205
        }

        private int hookPtr = 0;

        private delegate int HookProc(int nCode, IntPtr wParam, IntPtr lParam);
        //Declare MouseHookProcedure as a HookProc type.
        private HookProc globalMouseHookProcedure;			

//Declare the wrapper managed POINT class.
[StructLayout(LayoutKind.Sequential)]
private class POINT {
	public int x;
	public int y;
}

//Declare the wrapper managed MouseHookStruct class.
[StructLayout(LayoutKind.Sequential)]
private class MouseHookStruct {
	public POINT pt;
	public int hwnd;
	public int wHitTestCode;
	public int dwExtraInfo;
}

        public class GlobalMouseEventArgs : EventArgs {
            public MouseButton Button { get; set; }
            public Point Point { get; set; }
            public int Timestamp {
                get { return _timestamp; }
                set { _timestamp = value; }
            } private int _timestamp = (int)DateTime.Now.TimeOfDay.Ticks;
            public object WindowHandle { get; set; }

            public GlobalMouseEventArgs() : this(
                System.Windows.Input.Mouse.PrimaryDevice
            ) { }
            public GlobalMouseEventArgs(MouseDevice mouse) : this(
                mouse, mouse.GetPosition(null)
            ) { }
            public GlobalMouseEventArgs(Point point) : this(
                System.Windows.Input.Mouse.PrimaryDevice, point
            ) { }
            public GlobalMouseEventArgs(MouseDevice mouse, Point point) { }
        }
        #endregion Components
    }
    public partial class MouseHook {
#region Properties
        public bool IsHookEnabled {
            get { return _isHookEnabled; }
        } private bool _isHookEnabled = false;
#endregion Properties
    }
    public partial class MouseHook {
#region Events & Delegates
        public delegate void GlobalClickDelegate(
            object sender, GlobalMouseEventArgs e
        );
        public event GlobalClickDelegate GlobalClick;
#endregion Events & Delegates
    }
    public partial class MouseHook {
#region Methods
        private int globalMouseHook(int nCode, IntPtr wParam, IntPtr lParam) {
            if (nCode >= 0 && MouseMessages.WM_LBUTTONDOWN == (MouseMessages)wParam) {
                MouseHookStruct mouseData = null;
                try {
                    mouseData = (MouseHookStruct)Marshal.PtrToStructure(
                        lParam, typeof(MouseHookStruct)
                    );
                    GlobalClick(wParam, new GlobalMouseEventArgs(
                        new Point(mouseData.pt.x, mouseData.pt.y)
                    ));
                } catch { }
            }
            return CallNextHookEx(hookPtr, nCode, wParam, lParam);
        }
#endregion Methods
    }
    public partial class MouseHook {
#region Constructors & Destructor
        public MouseHook() { // See https://support.microsoft.com/en-us/kb/318804 | http://stackoverflow.com/questions/11607133/global-mouse-event-handler
            throw new NotImplementedException("TODO: Figure out why we get hella lag on Hook/Unhook events; also, why threadId must be 0 instead of the current thread. Also, refactor like a mofo.");
            globalMouseHookProcedure = new HookProc(globalMouseHook);

            using (
                Process p = Process.GetCurrentProcess()
            ) using (ProcessModule m = p.MainModule) {
                hookPtr = SetWindowsHookEx(
                    (int)HookEvents.WH_MOUSE_LL,
                    globalMouseHookProcedure,
                    GetModuleHandle(m.ModuleName),
                    0//System.Threading.Thread.CurrentThread.ManagedThreadId // Instead of the deprecated AppDomain.GetCurrentThreadId()
                );
            }

            if (hookPtr == 0) throw new Win32Exception(
                "Could not hook into global mouse events with SetWindowsHookEx."
            );
        }
        public MouseHook(GlobalClickDelegate clickHandler) : this(
            new GlobalClickDelegate[] { clickHandler }
        ) { }
        public MouseHook(IEnumerable<GlobalClickDelegate> clickHandlers) : this() {
            try {
                foreach (GlobalClickDelegate d in clickHandlers)
                    GlobalClick += d;
            } catch { }
        }
        ~MouseHook() {
            //throw new NotImplementedException("TODO: Remove global mouse hook that wraps GlobalClick(o, e).");
            try {
                foreach (
                    var listener in GlobalClick.GetInvocationList() ?? 
                    new Delegate[] { }
                ) GlobalClick -= (GlobalClickDelegate)listener;
            } finally {
                UnhookWindowsHookEx(hookPtr);
                hookPtr = 0;
            }
        }
#endregion Constructors & Destructor
    }
}
