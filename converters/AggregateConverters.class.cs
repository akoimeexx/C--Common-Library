/**      1         2         3         4         5         6         7         8
 * 45678901234567890123456789012345678901234567890123456789012345678901234567890
 *
 * Common.Converters: Common library include for aggregated converters, v.0.0.1
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
namespace Common.Converters {
    using System;
    using System.Linq;
    using System.Collections.Generic;
    using System.Globalization;
    using System.Windows.Data;

    public class AggregateConverters : List<IValueConverter>, IValueConverter {
        public object Convert(object v, Type t, object p,  CultureInfo c) {
            return this.Aggregate(v, (current, converter) => 
                converter.Convert(current, t, p, c));
        }
        public object ConvertBack(object v, Type t, object p, CultureInfo c) {
            throw new NotSupportedException(
                "Multiple conversions processed; ConvertBack not supported!"
            );
        }
    }
}
/**
 * XAML sample: 
<OBJECT [...] xmlns:lc="clr-namespace:Common.Converters">
    <OBJECT.Resources>
        <BooleanToVisibilityConverter x:Key="BooleanToVisibility" />
        <lc:AggregateConverters x:Key="InverseBooleanToVisibility">
            <lc:BooleanToInverseConverter />
            <lc:BoolToVisibility />
        </lc:AggregateConverters>
    </OBJECT.Resources>
    <PasswordBox Name="pwdBox" Password="password" Visibility="{Binding ElementName=chkBox,Path=IsChecked, Converter={StaticResource InverseBooleanToVisibility}}" />
    <TextBox Name="txtBoxPW" Text="{Binding ElementName=pwdBox,Mode=TwoWay,Path=Password}" Visibility="{Binding ElementName=chkBox,Path=IsChecked, Converter={StaticResource BooleanToVisibility}}" />
    <CheckBox Name="chkBox" Content="Show Password" />
</OBJECT>
 */
