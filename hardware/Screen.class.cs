/**      1         2         3         4         5         6         7         8
 * 45678901234567890123456789012345678901234567890123456789012345678901234567890
 *
 * Common.Hardware.Screen: Common library include for screen methods, v.0.0.1
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
    using Drawing = System.Drawing;
    using System.Linq;
    using System.Runtime.InteropServices;
    using System.Windows;
    using System.Windows.Input;
    using Media = System.Windows.Media;

    public static partial class Screen {
        [DllImport("gdi32.dll")] 
        private static extern bool BitBlt(IntPtr hdcDest, int nXDest, 
            int nYDest, int nWidth, int nHeight, IntPtr hdcSrc, int nXSrc, 
            int nYSrc, int dwRop);
        [DllImport("gdi32.dll")] 
        private static extern IntPtr CreateCompatibleBitmap(IntPtr hdc, 
            int nWidth, int nHeight);
        [DllImport("gdi32.dll")] 
        private static extern IntPtr CreateCompatibleDC(IntPtr hdc);
        [DllImport("gdi32.dll")] 
        private static extern IntPtr DeleteDC(IntPtr hdc);
        [DllImport("gdi32.dll")] 
        private static extern IntPtr DeleteObject(IntPtr hObject);
        [DllImport("gdi32.dll")] 
        private static extern IntPtr SelectObject(IntPtr hdc, 
            IntPtr hgdiobjBmp);

        [DllImport("user32.dll")] 
        private static extern IntPtr GetDesktopWindow();
        [DllImport("user32.dll")] 
        private static extern int GetSystemMetrics(int nIndex);

        [DllImport("gdi32.dll")] 
        private static extern uint GetPixel(IntPtr hdc, int nXPos, int nYPos);
        [DllImport("user32.dll")] 
        private static extern IntPtr GetDC(IntPtr hwnd);
        [DllImport("user32.dll")] 
        private static extern Int32 ReleaseDC(IntPtr hwnd, IntPtr hdc);

        private enum SystemMetrics {
            CxScreen            =  0, // Primary Screen Width
            CyScreen            =  1, // Primary Screen Height
            VirtualScreenLeft   = 76, // Desktop Left Point
            VirtualScreenTop    = 77, // Desktop Top Point
            VirtualScreenWidth  = 78, // Desktop Bounding Width
            VirtualScreenHeight = 79, // Desktop Bounding Height
            Monitors            = 80, // Number of Active Monitors
        }


        public static int Count() {
            try {
                return GetSystemMetrics((int)SystemMetrics.Monitors);
            } catch (Exception e) { return 0; }
        }

        public static Media.Color GetColorAt(int x, int y) {
            return Screen.GetColorAtPoint(new Point(x, y));
        }
        public static Media.Color GetColorAtMousePoint<T>(this T o) 
        where T : Window {
            Point p = o.PointToScreen(Mouse.GetPosition(o));
            return GetColorAtPoint(p);
        }
        public static Media.Color GetColorAtPoint(Point p) {
            Media.Color c = Media.Colors.Transparent;
            IntPtr hdc = GetDC(IntPtr.Zero);
            uint pixel = GetPixel(hdc, (int)p.X, (int)p.Y);
            ReleaseDC(IntPtr.Zero, hdc);
            c = Media.Color.FromRgb(
                (byte)(pixel & 0x000000FF),
                (byte)((pixel & 0x0000FF00) >> 8),
                (byte)((pixel & 0x00FF0000) >> 16));

            return c;
        }
        public static Media.Color GetColorFromDesktop<T>(this T o) 
        where T : Window {
            Point p = Mouse.GetPosition(o);
            return Screen.GetColorFromDesktop(p);
        }
        public static Media.Color GetColorFromDesktop(Point p) {
            Media.Color c = Media.Colors.Transparent;
            IntPtr hBmp = Screen.GetDesktop().GetHbitmap();
            IntPtr hdc = GetDC(hBmp);
            uint pixel = GetPixel(hdc, (int)p.X, (int)p.Y);
            ReleaseDC(hBmp, hdc);
            c = Media.Color.FromRgb(
                (byte)(pixel & 0x000000FF),
                (byte)((pixel & 0x0000FF00) >> 8),
                (byte)((pixel & 0x00FF0000) >> 16));
            return c;
        }
        public static Drawing.Bitmap GetDesktop() {
            Drawing.Bitmap bmp = null;
            IntPtr hdcScreen = IntPtr.Zero, hdcCompatible = IntPtr.Zero, 
                hBmp = IntPtr.Zero;
            try {
                int sX = GetSystemMetrics(
                    (int)SystemMetrics.VirtualScreenWidth), 
                sY = GetSystemMetrics(
                    (int)SystemMetrics.VirtualScreenHeight);

                hdcScreen = GetDC(GetDesktopWindow());
                hdcCompatible = CreateCompatibleDC(hdcScreen);
                hBmp = CreateCompatibleBitmap(hdcScreen, sX, sY);

                IntPtr hOldBmp = (IntPtr)SelectObject(hdcCompatible, hBmp);
                BitBlt(hdcCompatible, 0,0, sX, sY, hdcScreen,
                    GetSystemMetrics((int)SystemMetrics.VirtualScreenLeft),
                    GetSystemMetrics((int)SystemMetrics.VirtualScreenTop), 
                    13369376);

                bmp = Drawing.Image.FromHbitmap(hBmp);
            } catch (Exception e) { } finally {
                DeleteDC(hdcCompatible);
                ReleaseDC(GetDesktopWindow(), hdcScreen);
                DeleteObject(hBmp);
                GC.Collect();
            }
            return bmp;
        }

        public static Drawing.Bitmap GetMonitor() {
            return Screen.GetMonitor(0);
        }
        public static Drawing.Bitmap GetMonitor(int index) {
            MessageBox.Show(String.Format("TODO: REF: {0}{1}{2}{3}{4}{5}",
                Environment.NewLine, 
                "https://msdn.microsoft.com/en-us/library/windows/desktop/ms724385(v=vs.85).aspx", 
                Environment.NewLine, 
                "https://msdn.microsoft.com/en-us/library/windows/desktop/dd144877(v=vs.85).aspx", 
                Environment.NewLine,
                "https://support.microsoft.com/en-us/kb/892462"
            ));
            throw new NotImplementedException();
            return null;
        }
    }
}
