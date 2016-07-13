/**      1         2         3         4         5         6         7         8
 * 45678901234567890123456789012345678901234567890123456789012345678901234567890
 *
 * Common.Dialogs <Color>: Common library providing WPF dialogs, v.0.0.1
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
    //using Microsoft.Win32;
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.IO;
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Input;
    using System.Windows.Interop;
    using System.Windows.Media;

    using Common.Converters;
    using Common.Dialogs;
    using Common.Extensions;
    using Common.Generics;
    using Common.Hardware;

    public partial class ColorDialog : Window {
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
    public partial class ColorDialog {
#region Properties
        public Color Color { get {
            return ((SolidColorBrush)this.rectPreview.Fill).Color;
        } set {
            sbR.Value = value.R; sbG.Value = value.G; sbB.Value = value.B;
        }}
        private ObservableCollection<Color> _customSwatches =
            new ObservableCollection<Color>() {
                SystemColors.ControlLightColor,
                SystemColors.ControlLightColor,
                SystemColors.ControlLightColor,
                SystemColors.ControlLightColor,
                SystemColors.ControlLightColor,
                SystemColors.ControlLightColor,
                SystemColors.ControlLightColor,
                SystemColors.ControlLightColor,
                SystemColors.ControlLightColor,
                SystemColors.ControlLightColor,
                SystemColors.ControlLightColor,
                SystemColors.ControlLightColor,
                SystemColors.ControlLightColor,
                SystemColors.ControlLightColor,
                SystemColors.ControlLightColor,
                SystemColors.ControlLightColor
        };
        public ObservableCollection<Color> CustomSwatches { get {
            return this._customSwatches;
        } set {
            this._customSwatches = value;
        }}
        private ObservableCollection
        <KeyValuePair<string, ObservableCollection<Color>>> _palettes =
            new ObservableCollection
            <KeyValuePair<string, ObservableCollection<Color>>>() {
                new KeyValuePair<string, ObservableCollection<Color>>(
                    "Load Color Palette...", null
                ),
                new KeyValuePair<string, ObservableCollection<Color>>(
                    "Basic Colors",
                    new ObservableCollection<Color>() {
                        new Color() { A = 255, R = 255, G = 128, B = 128 },
                        new Color() { A = 255, R = 255, G = 255, B = 128 },
                        new Color() { A = 255, R = 128, G = 255, B = 128 },
                        new Color() { A = 255, R = 0, G = 255, B = 128 },
                        new Color() { A = 255, R = 128, G = 255, B = 255 },
                        new Color() { A = 255, R = 0, G = 128, B = 255 },
                        new Color() { A = 255, R = 255, G = 128, B = 192 },
                        new Color() { A = 255, R = 255, G = 128, B = 255 },
                        new Color() { A = 255, R = 255, G = 0, B = 0 },
                        new Color() { A = 255, R = 255, G = 255, B = 0 },
                        new Color() { A = 255, R = 128, G = 255, B = 0 },
                        new Color() { A = 255, R = 0, G = 255, B = 64 },
                        new Color() { A = 255, R = 0, G = 255, B = 255 },
                        new Color() { A = 255, R = 0, G = 128, B = 192 },
                        new Color() { A = 255, R = 128, G = 128, B = 192 },
                        new Color() { A = 255, R = 255, G = 0, B = 255 },
                        new Color() { A = 255, R = 128, G = 64, B = 64 },
                        new Color() { A = 255, R = 255, G = 128, B = 64 },
                        new Color() { A = 255, R = 0, G = 255, B = 0 },
                        new Color() { A = 255, R = 0, G = 128, B = 128 },
                        new Color() { A = 255, R = 0, G = 64, B = 128 },
                        new Color() { A = 255, R = 128, G = 128, B = 255 },
                        new Color() { A = 255, R = 128, G = 0, B = 64 },
                        new Color() { A = 255, R = 255, G = 0, B = 128 },
                        new Color() { A = 255, R = 128, G = 0, B = 0 },
                        new Color() { A = 255, R = 255, G = 128, B = 0 },
                        new Color() { A = 255, R = 0, G = 128, B = 0 },
                        new Color() { A = 255, R = 0, G = 128, B = 64 },
                        new Color() { A = 255, R = 0, G = 0, B = 255 },
                        new Color() { A = 255, R = 0, G = 0, B = 160 },
                        new Color() { A = 255, R = 128, G = 0, B = 128 },
                        new Color() { A = 255, R = 128, G = 0, B = 255 },
                        new Color() { A = 255, R = 64, G = 0, B = 0 },
                        new Color() { A = 255, R = 128, G = 64, B = 0 },
                        new Color() { A = 255, R = 0, G = 64, B = 0 },
                        new Color() { A = 255, R = 0, G = 64, B = 64 },
                        new Color() { A = 255, R = 0, G = 0, B = 128 },
                        new Color() { A = 255, R = 0, G = 0, B = 64 },
                        new Color() { A = 255, R = 64, G = 0, B = 64 },
                        new Color() { A = 255, R = 64, G = 0, B = 128 },
                        new Color() { A = 255, R = 0, G = 0, B = 0 },
                        new Color() { A = 255, R = 128, G = 128, B = 0 },
                        new Color() { A = 255, R = 128, G = 128, B = 64 },
                        new Color() { A = 255, R = 128, G = 128, B = 128 },
                        new Color() { A = 255, R = 64, G = 128, B = 128 },
                        new Color() { A = 255, R = 192, G = 192, B = 192 },
                        new Color() { A = 255, R = 64, G = 0, B = 64 },
                        new Color() { A = 255, R = 255, G = 255, B = 255 },
                    }
                )
        };
        public ObservableCollection
        <KeyValuePair<string, ObservableCollection<Color>>> Palettes { get {
            return this._palettes;
        } set {
            this._palettes = value;
        }}
#endregion Properties
    }
    public partial class ColorDialog {
#region Methods
        private void bOk_Click(object sender, RoutedEventArgs e) {
            this.DialogResult = true;
        }
        private void bScreenDropper_Click(object sender, RoutedEventArgs e) {
            //this.LostFocus += GetPixel_LostFocus;
            //MessageBox.Show(String.Format("Screen Count: {0}", Screen.Count()));
            //this.Background = new ImageBrush(
            //    Imaging.CreateBitmapSourceFromHBitmap(
            //        Screen.GetDesktop().GetHbitmap(),
            //        IntPtr.Zero,
            //        Int32Rect.Empty,
            //        System.Windows.Media.Imaging
            //            .BitmapSizeOptions.FromEmptyOptions()
            //));
        }
        private void GetPixel_MouseUp(object o, RoutedEventArgs e) {
            sbR.Value = this.GetColorFromDesktop().R;
            sbG.Value = this.GetColorFromDesktop().G;
            sbB.Value = this.GetColorFromDesktop().B;
        }
        private bool Load_GnuImageManipulationPalette(string filename) {
            bool result = false;
            try {
                using (StreamReader sr = new StreamReader(filename)) {
                    string line, name = "";
                    ObservableCollection<Color> swatches = 
                        new ObservableCollection<Color>();
                    while ((line = sr.ReadLine()) != null) {
                        byte r, g, b;
                        int i = 0;
                        if (line.Length > 0 && 
                            int.TryParse(line.Substring(0,1), out i)) {
                            List<string> channels = new List<string>(
                                line.CollapseWhitespace()
                                    .Split(new char[] { ' ', '\t' }, 4, 
                                    StringSplitOptions.RemoveEmptyEntries
                            ));
                            if (
                                channels.Count >= 3 && 
                                byte.TryParse(channels[0], out r) && 
                                byte.TryParse(channels[1], out g) && 
                                byte.TryParse(channels[2], out b)
                                ) swatches.Add(new Color() {
                                    A = 255, R = r, G = g, B = b
                                });
                            continue;
                        } else {
                            if (line.StartsWith("name:", true, null)) {
                                name = line.Substring(5).Trim();
                            }
                        }
                    }
                    if (String.IsNullOrWhiteSpace(name))
                        name = Path.GetFileNameWithoutExtension(name);
                    if (swatches.Count > 0) {
                        this.Palettes.Add(
                        new KeyValuePair<string, ObservableCollection<Color>>(
                            name, swatches
                        ));
                        result = true;
                    }
                }
            } catch (Exception e) {
                MessageBox.Show(
                    e.Message,
                    String.Format("{0}: {1}", e.GetType().Name, e.Source),  
                    MessageBoxButton.OK,
                    MessageBoxImage.Error
                );
            } finally { }
            return result;
        }
        private void lumen_MouseDown(object sender, MouseEventArgs e) {
            sbR.Value = this.GetColorAtMousePoint().R;
            sbG.Value = this.GetColorAtMousePoint().G;
            sbB.Value = this.GetColorAtMousePoint().B;
        }
        private void lumen_MouseMove(object sender, MouseEventArgs e) {
            rectLivePreview.Fill = new SolidColorBrush(
                this.GetColorAtMousePoint()
            );
            if (e.LeftButton == MouseButtonState.Pressed) {
                sbR.Value = this.GetColorAtMousePoint().R;
                sbG.Value = this.GetColorAtMousePoint().G;
                sbB.Value = this.GetColorAtMousePoint().B;
            }
        }
        private void Palette_Change(object o, RoutedEventArgs e) {
            if (((ComboBox)o).SelectedIndex == 0) {
                OpenDialog od = new OpenDialog() {
                    Filter = String.Join("|", new string[] {
                        "GIMP Palette File (*.gpl)|*.gpl",
                        "All Files (*.*)|*.*"
                    })
                };
                if (od.ShowDialog() == true)
                    this.Load_GnuImageManipulationPalette(od.FileName);
                ((ComboBox)o).SelectedIndex = 
                    ((ComboBox)o).Items.Count - 1;
            }
        }
        private void Palette_MouseWheel(object o, MouseWheelEventArgs e) {
            if (((ComboBox)o).SelectedIndex < 2 && 
                e.Delta > 0) e.Handled = true;
        }
        private void spectrum_MouseDown(object sender, MouseEventArgs e) {
            sbR.Value = this.GetColorAtMousePoint().R;
            sbG.Value = this.GetColorAtMousePoint().G;
            sbB.Value = this.GetColorAtMousePoint().B;
            rectBaseSpectrum.Fill = new SolidColorBrush(
                this.GetColorAtMousePoint()
            );
        }
        private void spectrum_MouseLeave(object sender, MouseEventArgs e) {
            rectLivePreview.Fill = new SolidColorBrush(Colors.Transparent);
        }
        private void spectrum_MouseMove(object sender, MouseEventArgs e) {
            rectLivePreview.Fill = new SolidColorBrush(
                this.GetColorAtMousePoint()
            );
            if (e.LeftButton == MouseButtonState.Pressed) {
                sbR.Value = this.GetColorAtMousePoint().R;
                sbG.Value = this.GetColorAtMousePoint().G;
                sbB.Value = this.GetColorAtMousePoint().B;
                rectBaseSpectrum.Fill = new SolidColorBrush(
                    this.GetColorAtMousePoint()
                );
            }
        }
#endregion Methods
    }
    public partial class ColorDialog {
#region Constructors & Destructor
        public ColorDialog() {
            InitializeComponent();
        }
        ~ColorDialog() { }
#endregion Constructors & Destructor
    }
}
