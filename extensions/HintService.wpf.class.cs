/**      1         2         3         4         5         6         7         8
 * 45678901234567890123456789012345678901234567890123456789012345678901234567890
 *
 * Common.Extensions.Wpf.HintService: Common library include providing 
 * "Cue Banner"-like hints for wpf controls, v.0.0.1
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
namespace Common.Extensions.Wpf {
    using System;
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Documents;
    using System.Windows.Media;
    
    public static partial class HintService {
        public static Predicate<Control> CanHint = new Predicate<Control>(_ => {
            DependencyProperty p = GetDependencyProperty(_);
            if (p == null) return true;
            return _.GetValue(p).Equals("");
        });

        public static readonly DependencyProperty HintProperty =
            DependencyProperty.RegisterAttached(
                "Hint",
                typeof(object),
                typeof(HintService),
                new FrameworkPropertyMetadata(null, hintPropertyChanged)
        );
        public static object GetHint(Control c) {
            return c.GetValue(HintProperty);
        }
        public static void SetHint(Control c, object value) {
            c.SetValue(HintProperty, value);
        }

        private static void hintPropertyChanged(
            DependencyObject o, DependencyPropertyChangedEventArgs e
        ) {
            Control c = (Control)o;
            c.Loaded += control_Loaded;
            if (o is ComboBox || o is TextBox) {
                c.GotFocus += control_GotFocus;
                c.LostFocus += control_Loaded;
            }
        }

        internal class HintAdorner : Adorner {
            private readonly ContentPresenter _contentPresenter;
            private Control _control {
                get { return (Control)AdornedElement; }
            }
            protected override int VisualChildrenCount {
                get { return 1; }
            }


            protected override Size ArrangeOverride(Size finalSize) {
                _contentPresenter.Arrange(new Rect(finalSize));
                return finalSize;
            }
            protected override Visual GetVisualChild(int index) {
                return _contentPresenter;
            }
            protected override Size MeasureOverride(Size constraint) {
                _contentPresenter.Measure(_control.RenderSize);
                return _control.RenderSize;
            }

            public HintAdorner(UIElement ele, object hint) : base(ele) {
                IsHitTestVisible = false;
                HorizontalAlignment h = HorizontalAlignment.Left;
                VerticalAlignment v = VerticalAlignment.Center;

                if (ele is Control) {
                    h = ((Control)ele).HorizontalContentAlignment;
                    v = ((Control)ele).VerticalContentAlignment;
                }

                _contentPresenter = new ContentPresenter() {
                    Content = hint,
                    HorizontalAlignment = h,
                    Margin = new Thickness(
                        _control.Padding.Left, 
                        _control.Padding.Top, 
                        _control.Padding.Right,
                        _control.Padding.Bottom
                    ), 
                    Opacity = 0.66,
                    VerticalAlignment = v
                };
            }
        }

        private static void control_GotFocus(object o, RoutedEventArgs e) {
            Control c = (Control)o;
            if (CanHint(c)) RemoveHint(c);
        }
        private static void control_Loaded(object o, RoutedEventArgs e) {
            Control c = (Control)o;
            if (CanHint(c)) AddHint(c);
        }

        private static void AddHint(Control c) {
            AdornerLayer layer = AdornerLayer.GetAdornerLayer(c);
            layer.Add(new HintAdorner(c, GetHint(c)));
        }
        private static void RemoveHint(UIElement c) {
            AdornerLayer layer = AdornerLayer.GetAdornerLayer(c);
            foreach (Adorner a in layer.GetAdorners(c) ?? new Adorner[0]) {
                if (a is HintAdorner) {
                    a.Visibility = Visibility.Hidden;
                    layer.Remove(a);
                }
            }
        }

        private static DependencyProperty GetDependencyProperty(Control c) {
            if (c is ComboBox) return ComboBox.TextProperty;
            if (c is TextBox) return TextBox.TextProperty;
            return null;
        }
    }
    /**
     * Usage:

<Window x:Class="SomeWpfApplication.MainWindow"
    xmlns="http://schemas.microsoft.com/winfx/2006/xaml/presentation"
    xmlns:x="http://schemas.microsoft.com/winfx/2006/xaml"
    xmlns:d="http://schemas.microsoft.com/expression/blend/2008"
    xmlns:mc="http://schemas.openxmlformats.org/markup-compatibility/2006"
    xmlns:local="clr-namespace:SomeWpfApplication"
    xmlns:extensions="clr-namespace:Common.Extensions.Wpf"
    mc:Ignorable="d"
    Title="" Height="96" Width="128">
    <Grid>
        <TextBox 
            Margin="10" 
            extensions:HintService.Hint="Placeholder Hint." 
            HorizontalContentAlignment="Center" 
            VerticalContentAlignment="Center"
        />
    </Grid>
</Window>
     */
}
