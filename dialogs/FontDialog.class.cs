/**      1         2         3         4         5         6         7         8
 * 45678901234567890123456789012345678901234567890123456789012345678901234567890
 *
 * Common.Dialogs <Font>: Common library providing WPF dialogs, v.0.0.1
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
    using Microsoft.Win32;
    using System.Collections.ObjectModel;
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Data;
    using System.Windows.Interop;
    using System.Windows.Media;

    using Common.Extensions;

    public partial class FontDialog : CommonDialog {
#region CommonDialog implementation
        public override void Reset() {
            throw new NotImplementedException();
        }
        protected override bool RunDialog(IntPtr hwndOwner) {
            bool result = false;
            try {
                HwndSource hSrc = HwndSource.FromHwnd(hwndOwner);
                w.Owner = (Window)hSrc.RootVisual;
                w.ShowDialog();
            } catch (Exception e) {
                MessageBox.Show(e.Message);
            } finally {
                result = (bool)w.DialogResult;
            }
            return result;
        }
#endregion CommonDialog implementation
    }
    public partial class FontDialog {
#region Datatypes
        private struct FontStyling {
            public string Name { get; set; }
            public FontWeight Weight { get; set; }
            public FontStyle Style { get; set; }
        }
#endregion Datatypes
    }
    public partial class FontDialog {
#region Properties
        public FontFamily FontFamily { get {
            return this.tPreview.FontFamily;
        } set {
            this.tPreview.FontFamily = value;
        }}
        public FontStyle FontStyle { get {
            return this.tPreview.FontStyle;
        } set {
            this.tPreview.FontStyle = value;
        }}
        public FontWeight FontWeight { get {
            return this.tPreview.FontWeight;
        } set {
            this.tPreview.FontWeight = value;
        }}
        public double FontSize { get {
            return this.tPreview.FontSize;
        } set {
            this.tPreview.FontSize = value;
        }}
#endregion Properties
    }
    public partial class FontDialog {
#region Components
        private static Thickness ButtonThickness = new Thickness(1);
        private static Thickness BoxThickness = new Thickness(1);
        private static Thickness LabelMargin = new Thickness(10, 10, 10, 0);
        private static Thickness ListMargin = new Thickness(10, 41, 10, 10);
        private const int LabelHeight = 26;
        private const int ButtonWidth = 96;
        private const int ButtonHeight = 28;

        private Window w = new Window() {
            AllowDrop = false, Height = 360, MinHeight = 300, MinWidth = 400, 
            Name ="FontDialog", ResizeMode = ResizeMode.CanResizeWithGrip,
            ShowInTaskbar = false, Title = "Pick a Font", Topmost = true, 
            Width = 480, Icon = Resources_Common.FontDialog_Icon.ToImageSource(),
            WindowStartupLocation = WindowStartupLocation.CenterOwner,
            WindowState = WindowState.Normal, 
            WindowStyle = WindowStyle.ToolWindow
        };
        private Label lbFamily = new Label() {
            Content = "_Family:", Name = "FamilyLabel",
            Height = LabelHeight, Margin = LabelMargin, Padding = BoxThickness,
            VerticalAlignment = VerticalAlignment.Top
        };
        private Label lbStyle = new Label() {
            Content = "_Style:", Name = "StyleLabel",
            Height = LabelHeight, Margin = LabelMargin, Padding = BoxThickness,
            VerticalAlignment = VerticalAlignment.Top
        };
        private Label lbSize = new Label() {
            Content = "Si_ze:", Name = "SizeLabel",
            Height = LabelHeight, Margin = LabelMargin, Padding = BoxThickness,
            VerticalAlignment = VerticalAlignment.Top
        };
        private Label lbPreview = new Label() {
            Content = "_Preview:", Name = "PreviewLabel",
            Height = LabelHeight, Margin = LabelMargin, Padding = BoxThickness,
            VerticalAlignment = VerticalAlignment.Top
        };
        private ListBox lFamily = new ListBox() {
            ItemsSource = new ObservableCollection<FontFamily>(
                Fonts.SystemFontFamilies
            ), Name = "FontFamilyList", 
            AllowDrop = false, Margin = ListMargin, Padding = BoxThickness
        };
        private ListBox lStyle = new ListBox() {
            DisplayMemberPath="Name", 
            ItemsSource = new FontStyling[] {
                new FontStyling() { 
                    Name="Normal", 
                    Style=FontStyles.Normal, 
                    Weight=FontWeights.Normal 
                }, new FontStyling() { 
                    Name="Bold", 
                    Style=FontStyles.Normal, 
                    Weight=FontWeights.Bold 
                }, new FontStyling() { 
                    Name="Italic", 
                    Style=FontStyles.Italic, 
                    Weight=FontWeights.Normal
                }, new FontStyling() { 
                    Name="Bold Italic", 
                    Style=FontStyles.Italic, 
                    Weight=FontWeights.Bold 
                }
            }, Name = "FontStyleList",
            AllowDrop = false, Margin = ListMargin, Padding = BoxThickness
        };
        private ListBox lSize = new ListBox() {
            ItemsSource = new double[] {
                 6,  7,  8,  9, 10, 
                11, 12, 13, 14, 15, 
                16, 17, 18, 20, 22, 
                24, 26, 28, 32, 36, 
                40, 48, 56, 64, 72 
            },
            Name = "FontSizeList", 
            AllowDrop = false, Margin = new Thickness(10, 69, 10, 10), 
            Padding = BoxThickness
        };
        private TextBox tSize = new TextBox() {
            AllowDrop = false, Height = 23, 
            HorizontalContentAlignment = HorizontalAlignment.Center, 
            Margin = new Thickness(10, 41, 10, 0), Name = "FontSizeText", 
            Padding = BoxThickness, TextWrapping = TextWrapping.NoWrap, 
            VerticalAlignment = VerticalAlignment.Top,
            VerticalContentAlignment = VerticalAlignment.Center
        };
        private TextBox tPreview = new TextBox() {
            AllowDrop = false, 
            HorizontalContentAlignment = HorizontalAlignment.Left, 
            Margin = new Thickness(10, 36, 10, 10), MinHeight = 23, 
            Name = "PreviewText", Padding = BoxThickness, 
            Text = 
                "Grumpy wizards make toxic brew for the evil Queen and Jack.", 
            TextWrapping = TextWrapping.NoWrap, 
            VerticalContentAlignment = VerticalAlignment.Center
        };
        private Button bOk = new Button() {
            Content = "_OK", IsDefault = true, 
            Margin = new Thickness(0, 0, 47, 10), Name = "OkButton",
            AllowDrop = false, Height = ButtonHeight, 
            HorizontalAlignment = HorizontalAlignment.Right,
            Padding = ButtonThickness, 
            VerticalAlignment = VerticalAlignment.Bottom, Width = ButtonWidth
        };
        private Button bCancel = new Button() {
            Content = "_Cancel", IsCancel = true, 
            Margin = new Thickness(0, 0, 10, 10), Name = "CancelButton",
            AllowDrop = false, Height = ButtonHeight, 
            HorizontalAlignment = HorizontalAlignment.Right,
            Padding = ButtonThickness, 
            VerticalAlignment = VerticalAlignment.Bottom, Width = ButtonWidth
        };

        private Grid gLayout = new Grid() { Margin = new Thickness(10) };
            private RowDefinition rdTop = new RowDefinition() {
                Height = new GridLength(1, GridUnitType.Star)
            };
            private RowDefinition rdMiddle = new RowDefinition() {
                Height = new GridLength(1, GridUnitType.Auto)
            };
            private RowDefinition rdBottom = new RowDefinition() {
                Height = new GridLength(48, GridUnitType.Pixel)
            };
            private ColumnDefinition cdLeft = new ColumnDefinition() {
                Width = new GridLength(1, GridUnitType.Star)
            };
            private ColumnDefinition cdMiddle = new ColumnDefinition() {
                Width = new GridLength(1, GridUnitType.Star)
            };
            private ColumnDefinition cdRight = new ColumnDefinition() {
                Width = new GridLength(72, GridUnitType.Pixel)
            };
#endregion Components
    }
    public partial class FontDialog {
#region Methods
        private void bOk_Click(object o, RoutedEventArgs e) {
            this.w.DialogResult = true;
        }
#endregion Methods
    }
    public partial class FontDialog {
#region Constructors & Destructor
        public FontDialog() {
            // Gridlines
            this.gLayout.RowDefinitions.Add(rdTop);
            this.gLayout.RowDefinitions.Add(rdMiddle);
            this.gLayout.RowDefinitions.Add(rdBottom);
            this.gLayout.ColumnDefinitions.Add(cdLeft);
            this.gLayout.ColumnDefinitions.Add(cdMiddle);
            this.gLayout.ColumnDefinitions.Add(cdRight);

            // Labels
            this.gLayout.Children.Add(lbFamily);
            Grid.SetColumn(lbFamily, 0);
            this.gLayout.Children.Add(lbStyle);
            Grid.SetColumn(lbStyle, 1);
            this.gLayout.Children.Add(lbSize);
            Grid.SetColumn(lbSize, 2);
            this.gLayout.Children.Add(lbPreview);
            Grid.SetRow(lbPreview, 1);
            Grid.SetColumnSpan(lbPreview, 3);

            // Font Settings Selectors
            this.gLayout.Children.Add(lFamily);
            Grid.SetColumn(lFamily, 0);
            this.gLayout.Children.Add(lStyle);
            Grid.SetColumn(lStyle, 1);
            this.gLayout.Children.Add(tSize);
            Grid.SetColumn(tSize, 2);
            this.gLayout.Children.Add(lSize);
            Grid.SetColumn(lSize, 2);
            lSize.SetValue(
                ScrollViewer.HorizontalScrollBarVisibilityProperty, 
                ScrollBarVisibility.Disabled
            );
            this.gLayout.Children.Add(tPreview);
            Grid.SetRow(tPreview, 1);
            Grid.SetColumnSpan(tPreview, 3);
                // Text Preview Binding
                Binding fFamily = new Binding() {
                    Source = tPreview, Path = new PropertyPath("FontFamily"), 
                    Mode = BindingMode.TwoWay, 
                    UpdateSourceTrigger = UpdateSourceTrigger.PropertyChanged
                };
                Binding fStyle = new Binding() {
                    Source = lStyle, 
                    Path = new PropertyPath("SelectedItem.Style"), 
                    Mode = BindingMode.TwoWay, 
                    UpdateSourceTrigger = UpdateSourceTrigger.PropertyChanged
                };
                Binding fWeight = new Binding() {
                    Source = lStyle, 
                    Path = new PropertyPath("SelectedItem.Weight"), 
                    Mode = BindingMode.TwoWay, 
                    UpdateSourceTrigger = UpdateSourceTrigger.PropertyChanged
                };
                Binding fSize = new Binding() {
                    Source = tPreview, Path = new PropertyPath("FontSize"), 
                    Mode = BindingMode.TwoWay, 
                    UpdateSourceTrigger = UpdateSourceTrigger.PropertyChanged
                };
                BindingOperations.SetBinding(
                    lFamily, ListBox.SelectedItemProperty, fFamily
                );
                BindingOperations.SetBinding(
                    tPreview, TextBox.FontStyleProperty, fStyle
                );
                BindingOperations.SetBinding(
                    tPreview, TextBox.FontWeightProperty, fWeight
                );
                BindingOperations.SetBinding(
                    tSize, TextBox.TextProperty, fSize
                );
                BindingOperations.SetBinding(
                    lSize, ListBox.SelectedItemProperty, fSize
                );
            // Completion Buttons
            this.gLayout.Children.Add(bOk);
            Grid.SetRow(bOk, 2);
            Grid.SetColumn(bOk, 1);
            bOk.Click += bOk_Click;
            this.gLayout.Children.Add(bCancel);
            Grid.SetRow(bCancel, 2);
            Grid.SetColumn(bCancel, 1);
            Grid.SetColumnSpan(bCancel, 2);

            w.Content = gLayout;
            lFamily.ScrollIntoView(lFamily.SelectedItem);
            lStyle.ScrollIntoView(lStyle.SelectedItem);
            lSize.ScrollIntoView(lSize.SelectedItem);
        }
#endregion Constructors & Destructor
    }
}
