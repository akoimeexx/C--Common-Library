/**      1         2         3         4         5         6         7         8
 * 45678901234567890123456789012345678901234567890123456789012345678901234567890
 *
 * Common.Converters: Common library include for Color converters, v.0.0.1
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
namespace Common.Converters {
    using System;
    using System.Globalization;
    using System.Windows;
    using System.Windows.Data;
    using System.Windows.Media;

    public partial class ColorToHexConverter : IValueConverter {
        public object Convert(object v, Type t, object p, CultureInfo c) {
            Color color = (Color)v;
            if (color == null) return "";
            return String.Format("#{0}{1}{2}",
                color.R.ToString("X2"),
                color.G.ToString("X2"),
                color.B.ToString("X2")
            );
        }
        public object ConvertBack(object v, Type t, object p, CultureInfo c) {
            byte r, g, b;
            String s = ((string)v).Trim().Trim(new char[] { '#', ';' });
            if (s.Length == 6)
                if (byte.TryParse(s.Substring(0, 2), NumberStyles.HexNumber, c, 
                out r))
                    if (byte.TryParse(s.Substring(2, 2), NumberStyles.HexNumber,
                    c, out g))
                        if (byte.TryParse(s.Substring(4, 2), 
                        NumberStyles.HexNumber, c, out b))
                            return new Color() { 
                                R = r, G = g, B = b, A = 255 
                            };
            return new Color();
        }
    }
    public partial class ColorToAlphaHexConverter : IValueConverter {
        public object Convert(object v, Type t, object p, CultureInfo c) {
            Color color = (Color)v;
            if (color == null) return "";
            return color.ToString();
        }
        public object ConvertBack(object v, Type t, object p, CultureInfo c) {
            byte a, r, g, b;
            String s = ((string)v).Trim().Trim(new char[] { '#', ';' });
            if (s.Length == 8)
                if (byte.TryParse(s.Substring(0, 2), NumberStyles.HexNumber, c, 
                out a))
                    if (byte.TryParse(s.Substring(2, 2), NumberStyles.HexNumber,
                    c, out r))
                        if (byte.TryParse(s.Substring(4, 2), 
                        NumberStyles.HexNumber, c, out g))
                            if (byte.TryParse(s.Substring(6, 2),
                            NumberStyles.HexNumber, c, out b))
                                return new Color() { 
                                    A = a, R = r, G = g, B = b 
                                };
            return new Color();
        }
    }

    public partial class SolidColorBrushToHexConverter : IValueConverter {
        public object Convert(object v, Type t, object p, CultureInfo c) {
            SolidColorBrush scb = (SolidColorBrush)v;
            if (scb == null) return "";
            return String.Format("#{0}{1}{2}",
                scb.Color.R.ToString("X2"),
                scb.Color.G.ToString("X2"),
                scb.Color.B.ToString("X2")
            );
        }
        public object ConvertBack(object v, Type t, object p, CultureInfo c) {
            byte r, g, b;
            String s = ((string)v).Trim().Trim(new char[] { '#', ';' });
            if (s.Length == 6)
                if (byte.TryParse(s.Substring(0, 2), NumberStyles.HexNumber, c, 
                out r))
                    if (byte.TryParse(s.Substring(2, 2), NumberStyles.HexNumber,
                    c, out g))
                        if (byte.TryParse(s.Substring(4, 2), 
                        NumberStyles.HexNumber, c, out b))
                            return new SolidColorBrush(new Color() { 
                                R = r, G = g, B = b, A = 255 
                            });
            return new SolidColorBrush();
        }
    }
    public partial class SolidColorBrushToAlphaHexConverter : IValueConverter {
        public object Convert(object v, Type t, object p, CultureInfo c) {
            SolidColorBrush scb = (SolidColorBrush)v;
            return scb.Color.ToString();
        }
        public object ConvertBack(object v, Type t, object p, CultureInfo c) {
            byte a, r, g, b;
            String s = ((string)v).Trim().Trim(new char[] { '#', ';' });
            if (s.Length == 8)
                if (byte.TryParse(s.Substring(0, 2), NumberStyles.HexNumber, c, 
                out a))
                    if (byte.TryParse(s.Substring(2, 2), NumberStyles.HexNumber,
                    c, out r))
                        if (byte.TryParse(s.Substring(4, 2), 
                        NumberStyles.HexNumber, c, out g))
                            if (byte.TryParse(s.Substring(6, 2),
                            NumberStyles.HexNumber, c, out b))
                                return new SolidColorBrush(new Color() { 
                                    A = a, R = r, G = g, B = b 
                                });
            return new SolidColorBrush();
        }
    }
}
