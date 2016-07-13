/**      1         2         3         4         5         6         7         8
 * 45678901234567890123456789012345678901234567890123456789012345678901234567890
 *
 * Common.WpfExtensions: Common library include extending wpf objects, v.0.0.1
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
namespace Common.Extensions {
    using System;
    using System.Collections;
    using System.Drawing;
    using System.Linq;
    using System.Reflection;
    using System.Runtime.InteropServices;
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Interop;
    using System.Windows.Media;
    using System.Windows.Media.Imaging;

    public static partial class WindowExtensions {
        [DllImport("user32.dll")] private static extern int GetWindowLong(
            IntPtr hWnd, int nIndex
        );
        [DllImport("user32.dll")] private static extern int SetWindowLong(
            IntPtr hWnd, int nIndex, int dwNewLong
        );

        private const int GWL_STYLE = -16;
        private const int WS_MAXIMIZEBOX = 0x10000;
        private const int WS_EX_LAYERED = 0x00080000;
        private const int WS_EX_TRANSPARENT = 0x00000020;

        public static void DisableHitTesting(this Window w) {
            throw new NotSupportedException("TODO: Fix this.");
            var hwnd = new WindowInteropHelper(w).EnsureHandle();
            var wndMask = GetWindowLong(hwnd, GWL_STYLE);
            SetWindowLong(hwnd, GWL_STYLE, (int)(wndMask | (WS_EX_LAYERED | WS_EX_TRANSPARENT)));
        }
        public static void EnableHitTesting(this Window w) {
            throw new NotSupportedException("TODO: Fix this.");
            var hwnd = new WindowInteropHelper(w).EnsureHandle();
            var wndMask = GetWindowLong(hwnd, GWL_STYLE);
            SetWindowLong(hwnd, GWL_STYLE, (int)(wndMask & ~(WS_EX_LAYERED | WS_EX_TRANSPARENT)));
        }
        public static void DisableMaximize(this Window w) {
            var hwnd = new WindowInteropHelper(w).EnsureHandle();
            var wndMask = GetWindowLong(hwnd, GWL_STYLE);
            SetWindowLong(hwnd, GWL_STYLE, (int)(wndMask & ~WS_MAXIMIZEBOX));
        }
        public static void EnableMaximize(this Window w) {
            var hwnd = new WindowInteropHelper(w).EnsureHandle();
            var wndMask = GetWindowLong(hwnd, GWL_STYLE);
            SetWindowLong(hwnd, GWL_STYLE, (int)(wndMask | WS_MAXIMIZEBOX));
        }
        public static IntPtr GetHandle(this Window w) {
            return new WindowInteropHelper(w).EnsureHandle();
        }
    }
    public static class ColorExtensions {
        public static float GetBrightness(this System.Windows.Media.Color c) {
            return System.Drawing.Color.FromArgb(
                c.A, c.R, c.G, c.B
            ).GetBrightness();
        }
        public static float GetHue(this System.Windows.Media.Color c) {
            return System.Drawing.Color.FromArgb(c.A, c.R, c.G, c.B).GetHue();
        }
        public static float GetLuminosity(this System.Windows.Media.Color c) {
            return ColorExtensions.GetBrightness(c) * 240;
        }
        public static float GetSaturation(this System.Windows.Media.Color c) {
            return System.Drawing.Color.FromArgb(
                c.A, c.R, c.G, c.B
            ).GetSaturation();
        }
    }
    public static class ControlExtensions {
        public static FrameworkElement FindAncestor<T>(
        this T o, Type AncestorType, int AncestorLevel=1
        ) where T : FrameworkElement {
            FrameworkElement ui = o as FrameworkElement;
            while (ui.Parent != null && AncestorLevel > 0) {
                ui = ((FrameworkElement)ui.Parent);
                Console.WriteLine(ui.ToString());
                if(ui.GetType() == AncestorType) {
                    AncestorLevel--;
                }
            }
            return ui;
        }
    }
    public static class IconExtensions {
        [DllImport("gdi32.dll", SetLastError = true)]
        private static extern bool DeleteObject(IntPtr oHwnd);

        public static ImageSource ToImageSource(this Icon i) {
            ImageSource imgSrc = null;
            try {
                Bitmap bmp = i.ToBitmap();
                IntPtr hBmp = bmp.GetHbitmap();

                imgSrc = Imaging.CreateBitmapSourceFromHBitmap(
                    hBmp, IntPtr.Zero, Int32Rect.Empty,
                    BitmapSizeOptions.FromEmptyOptions()
                );
                if (!DeleteObject(hBmp))
                    throw new Exception("Error deleting object");
            } catch {
                return null as ImageSource;
            } finally { }
            return imgSrc;
        }
    }
    public static class PasswordBoxExtensions {
        private static BindingFlags HiddenFlags = 
            (BindingFlags.Instance | BindingFlags.NonPublic);
        public class TextSelection {
            private int _start = 0;
            private string _text;
            public int length => this._text.Length;
            public int start => this._start;
            public TextSelection(string text, int start, int length=0) {
                this._start = start; this._text = text.Substring(start, length);
            }
            public override string ToString() {
                return this._text;
            }
        }
        public static int GetCaretPosition(this PasswordBox p) {
            return p.Selection().start;
        }
        public static int Select(this PasswordBox p, int start, int length=0) {
            p.GetType().GetMethod("Select", HiddenFlags).Invoke(
                p, new object[] { start, length }
            );
            return start;
        }
        public static TextSelection Selection(this PasswordBox p) {
            PropertyInfo pi = p.GetType().GetProperty(
                "Selection", HiddenFlags
            );
            TextSelection results = new TextSelection(p.Password, 0);
            if(pi != null) {
                object sel = pi.GetValue(p, null);
                IEnumerable _segments = (IEnumerable)sel.GetType().BaseType
                    .GetField("_textSegments", HiddenFlags).GetValue(sel);
                object segment = _segments.Cast<object>().FirstOrDefault();

                object _start = segment.GetType().GetProperty(
                    "Start", HiddenFlags).GetValue(segment, null);
                var start = (int)_start.GetType().GetProperty(
                    "Offset", HiddenFlags).GetValue(_start, null);

                object _end = segment.GetType().GetProperty(
                    "End", HiddenFlags).GetValue(segment, null);
                var end = (int)_start.GetType().GetProperty(
                    "Offset", HiddenFlags).GetValue(_end, null);

                // FIXME: Change Selection from literal 0 to var end.
                results = new TextSelection(p.Password, (int)start, 0);
            }
            return results;
        }

        /**
         * ShowPassword-Specific code
         */
        public static void ShowPasswordHandler(PasswordBox p, TextBox t) {
            p.PasswordChanged += new RoutedEventHandler(
            (object o, RoutedEventArgs re) => {
            if (((PasswordBox)o).Password != t.Text) {
                int cur = ((PasswordBox)o).GetCaretPosition();
                t.Text = ((PasswordBox)o).Password;
                ((PasswordBox)o).Select(cur);
            }});
            t.TextChanged += new TextChangedEventHandler(
            (object o, TextChangedEventArgs te) => {
            if (((TextBox)o).Text != p.Password) {
                p.Password = ((TextBox)o).Text;
            }});
        }
    }
}
/**
 * Usage:
    PasswordBox p; 
    p.Select(2, 2);
    p.GetCaretPosition;
    p.Selection().ToString();
 */
