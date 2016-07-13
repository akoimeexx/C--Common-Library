/**      1         2         3         4         5         6         7         8
 * 45678901234567890123456789012345678901234567890123456789012345678901234567890
 *
 * Common.Generics: Common library include for ObservableColor objects, v.0.0.1
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
    using System.Globalization;
    using System.Windows;
    using System.Windows.Media;

    using Common.Extensions;

    public partial class ObservableColor : Notifiable {
#region Properties
    #region Dependency Properties
        //public static readonly DependencyProperty AProperty = 
        //    DependencyProperty.Register(
        //        "A", 
        //        typeof(byte), 
        //        typeof(ObservableColor)
        //    );
        //public static readonly DependencyProperty BProperty = 
        //    DependencyProperty.Register(
        //        "B", 
        //        typeof(byte), 
        //        typeof(ObservableColor)
        //    );
        //public static readonly DependencyProperty ColorProperty = 
        //    DependencyProperty.Register(
        //        "Color", 
        //        typeof(byte), 
        //        typeof(ObservableColor)
        //    );
        //public static readonly DependencyProperty ColorBrushProperty = 
        //    DependencyProperty.Register(
        //        "ColorBrush", 
        //        typeof(byte), 
        //        typeof(ObservableColor)
        //    );
        //public static readonly DependencyProperty GProperty = 
        //    DependencyProperty.Register(
        //        "G", 
        //        typeof(byte), 
        //        typeof(ObservableColor)
        //    );
        //public static readonly DependencyProperty RProperty = 
        //    DependencyProperty.Register(
        //        "R", 
        //        typeof(byte), 
        //        typeof(ObservableColor)
        //    );
    #endregion Dependency Properties
        public byte A { get { return this.Color.A; } set {
            this.SendPropertyChanging("A");
            this.SendPropertyChanging("Color");
            this.SendPropertyChanging("ColorBrush");
            this._color.A = value;
            this.SendPropertyChanged("A");
            this.SendPropertyChanged("Color");
            this.SendPropertyChanged("ColorBrush");
        }}
        public byte B { get { return this.Color.B; } set {
            this.SendPropertyChanging("B");
            this.SendPropertyChanging("Color");
            this.SendPropertyChanging("ColorBrush");
            this._color.B = value;
            this.SendPropertyChanged("B");
            this.SendPropertyChanged("Color");
            this.SendPropertyChanged("ColorBrush");
        }}
        private Color _color = new Color();
        public Color Color { get { return this._color; } set {
            this.SendPropertyChanging("A");
            this.SendPropertyChanging("R");
            this.SendPropertyChanging("G");
            this.SendPropertyChanging("B");
            this.SendPropertyChanging("Color");
            this.SendPropertyChanging("ColorBrush");
            this.Set(ref this._color, value);
            this.SendPropertyChanged("A");
            this.SendPropertyChanged("R");
            this.SendPropertyChanged("G");
            this.SendPropertyChanged("B");
            this.SendPropertyChanged("Color");
            this.SendPropertyChanged("ColorBrush");
        }}
        public SolidColorBrush ColorBrush { get {
            return new SolidColorBrush(this.Color);
        } set {
            if (value != null)
                this.Color = value.Color;
        }}
        public ColorContext ColorContext { get {
            return this._color.ColorContext;
        }}
        public byte G { get { return this.Color.G; } set {
            this.SendPropertyChanging("G");
            this.SendPropertyChanging("Color");
            this.SendPropertyChanging("ColorBrush");
            this._color.G = value;
            this.SendPropertyChanged("G");
            this.SendPropertyChanged("Color");
            this.SendPropertyChanged("ColorBrush");
        }}
        public byte R { get { return this.Color.R; } set {
            this.SendPropertyChanging("R");
            this.SendPropertyChanging("Color");
            this.SendPropertyChanging("ColorBrush");
            this._color.R = value;
            this.SendPropertyChanged("R");
            this.SendPropertyChanged("Color");
            this.SendPropertyChanged("ColorBrush");
        }}
        public float ScA { get { return this.Color.ScA; } set {
            this.SendPropertyChanging("ScA");
            this.SendPropertyChanging("Color");
            this.SendPropertyChanging("ColorBrush");
            this._color.ScA = value;
            this.SendPropertyChanged("ScA");
            this.SendPropertyChanged("Color");
            this.SendPropertyChanged("ColorBrush");
        }}
        public float ScR { get { return this.Color.ScR; } set {
            this.SendPropertyChanging("ScR");
            this.SendPropertyChanging("Color");
            this.SendPropertyChanging("ColorBrush");
            this._color.ScR = value;
            this.SendPropertyChanged("ScR");
            this.SendPropertyChanged("Color");
            this.SendPropertyChanged("ColorBrush");
        }}
        public float ScG { get { return this.Color.ScG; } set {
            this.SendPropertyChanging("ScG");
            this.SendPropertyChanging("Color");
            this.SendPropertyChanging("ColorBrush");
            this._color.ScG = value;
            this.SendPropertyChanged("ScG");
            this.SendPropertyChanged("Color");
            this.SendPropertyChanged("ColorBrush");
        }}
        public float ScB { get { return this.Color.ScB; } set {
            this.SendPropertyChanging("ScB");
            this.SendPropertyChanging("Color");
            this.SendPropertyChanging("ColorBrush");
            this._color.ScB = value;
            this.SendPropertyChanged("ScB");
            this.SendPropertyChanged("Color");
            this.SendPropertyChanged("ColorBrush");
        }}
#endregion Properties
    }
    public partial class ObservableColor {
#region Methods
        public static ObservableColor Add(ObservableColor Color1, 
        ObservableColor Color2) {
            return new ObservableColor(
                Color.Add(Color1.Color, Color2.Color)
            );
        }
        public static bool AreClose(ObservableColor Color1, 
        ObservableColor Color2) {
            return Color.AreClose(Color1.Color, Color2.Color);
        }
        public void Clamp() {
            bool scA, scR, scG, scB;
            scA = scR = scG = scB = false;
            if (this._color.ScA > 1 || this._color.ScA < 0) scA = true;
            if (this._color.ScR > 1 || this._color.ScR < 0) scR = true;
            if (this._color.ScG > 1 || this._color.ScG < 0) scG = true;
            if (this._color.ScB > 1 || this._color.ScB < 0) scB = true;
            if (scA) this.SendPropertyChanging("ScA");
            if (scR) this.SendPropertyChanging("ScR");
            if (scG) this.SendPropertyChanging("ScG");
            if (scB) this.SendPropertyChanging("ScB");
            this.SendPropertyChanging("Color");
            this.SendPropertyChanging("ColorBrush");
            this._color.Clamp();
            if (scA) this.SendPropertyChanged("ScA");
            if (scR) this.SendPropertyChanged("ScR");
            if (scG) this.SendPropertyChanged("ScG");
            if (scB) this.SendPropertyChanged("ScB");
            this.SendPropertyChanged("Color");
            this.SendPropertyChanged("ColorBrush");
        }
        public static ObservableColor FromArgb(byte a, byte r, byte g, byte b) {
            return new ObservableColor(
                Color.FromArgb(a, r, g, b)
            );
        }
        public static ObservableColor FromAvalues(
        float a, float[] values, Uri profileUri) {
            return new ObservableColor(
                Color.FromAValues(a, values, profileUri)
            );
        }
        public static ObservableColor FromRgb(byte r, byte g, byte b) {
            return ObservableColor.FromArgb(255, r, g, b);
        }
        public static ObservableColor FromScRgb(
        float a, float r, float g, float b) {
            return new ObservableColor(
                Color.FromScRgb(a, r, g, b)
            );
        }
        public static ObservableColor FromValues(
        float[] values, Uri profileUri) {
            return ObservableColor.FromAvalues(1, values, profileUri);
        }
        public float[] GetNativeColorValues() {
            return this._color.GetNativeColorValues();
        }
        public static ObservableColor Multiply(ObservableColor Color1, 
        float coefficient) {
            return new ObservableColor(
                Color.Multiply(Color1.Color, coefficient)
            );
        }
        public static ObservableColor Parse(string s) {
            ObservableColor c = null;
            if (ObservableColor.TryParse(s, out c))
                return c;
            throw new FormatException(String.Format(
                "'{0}' is not a recognized color value", s
            ));
        }
        public static ObservableColor Subtract(ObservableColor Color1, 
        ObservableColor Color2) {
            return new ObservableColor(
                Color.Subtract(Color1.Color, Color2.Color)
            );
        }
        public override string ToString() {
            return this._color.ToString();
        }
        public string ToString(IFormatProvider provider) {
            return this._color.ToString(provider);
        }
        public static bool TryParse(string s, out ObservableColor o) {
            ObservableColor c = null; bool? result = null;
            try {
                byte[] argb = new byte[4];
                string hex = ((string)s).Trim().Trim(new char[] { '#', ';' });
                switch (hex.Length) {
                    case 6:
                        hex = "FF" + hex;
                        break;
                    case 8:
                        break;
                    default:
                        throw new FormatException("Unrecognized string format");
                }
                for (int i = 0; i < hex.Length; i += 2) {
                    if (!byte.TryParse(
                        hex.Substring(i, 2), 
                        NumberStyles.HexNumber, null, 
                        out argb[(i != 0 ? i/2 : 0)]
                    )) throw new FormatException("Unrecognized string format");
                }
                c = new ObservableColor() {
                    A = argb[0], R = argb[1], G = argb[2], B = argb[3]
                };
            } catch (Exception e) {
                result = false;
            } finally {
                if (result == null) result = true;
                o = c;
            }
            return (bool)result;
        }
    #region Common.Extensions Methods
        public float GetBrightness() {
            if (this._color.HasMethod("GetBrightness"))
                return this._color.GetBrightness();
            throw new NotImplementedException(
                "Common.Extensions method not available"
            );
        }
        public float GetHue() {
            if (this._color.HasMethod("GetHue"))
                return this._color.GetHue();
            throw new NotImplementedException(
                "Common.Extensions method not available"
            );
        }
        public float GetLuminosity() {
            if (this._color.HasMethod("GetLuminosity"))
                return this._color.GetLuminosity();
            throw new NotImplementedException(
                "Common.Extensions method not available"
            );
        }
        public float GetSaturation() {
            if (this._color.HasMethod("GetSaturation"))
                return this._color.GetSaturation();
            throw new NotImplementedException(
                "Common.Extensions method not available"
            );
        }
    #endregion Common.Extensions Methods
#endregion Methods
    }
    public partial class ObservableColor {
#region Constructors & Destructor
        public ObservableColor() { }
        public ObservableColor(Color c) { this._color = c; }
        public ObservableColor(string hexcode) {
            this._color = ObservableColor.Parse(hexcode).Color;
        }
        public ObservableColor(byte r, byte g, byte b) : this(255, r, g, b) { }
        public ObservableColor(byte a, byte r, byte g, byte b) {
            this._color.A = a;
            this._color.R = r;
            this._color.G = g;
            this._color.B = b;
        }
        ~ObservableColor() { }
#endregion Constructors & Destructor
    }
}