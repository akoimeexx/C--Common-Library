using System;
using System.CodeDom;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Json;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;


namespace __UNIT__ {
    using Common;
    using Common.Comparers;
    using Common.Converters;
    using Common.Dialogs;
    using Common.Events;
    using Common.Extensions;
    using Common.Generics;
    using Common.Hardware;
    using Common.Localization;
    using Common.OS;

    class Program {
        public static Application App = new Application();
        [STAThread]
        static void Main(string[] args) {
            App.ShutdownMode = ShutdownMode.OnLastWindowClose;
            Console.WriteLine("This is simply the testing suite for Johnathan Graham McKnight's ");
            Console.WriteLine("Common.Libraries source files. The Common.Libraries sources found in this");
            Console.WriteLine("solution are all licensed under the BSD3 license, and are available online at:");
            Console.WriteLine("");
            Console.WriteLine("    https://github.com/akoimeexx/Common.Libraries.NET");
            Console.WriteLine("    http://pastebin.com/pnEwKWJN");
            Console.WriteLine("");
            Console.WriteLine("Press any key to continue...");
            Console.ReadKey();
            WindowExamples we = new WindowExamples();
            App.Run(we);
            Console.WriteLine("Press any key to exit.");
            Console.ReadKey();
            App.Shutdown(0);
        }
    }

    public partial class WindowExamples : Window {
        public struct Library {
            public Categories Name;
            public String[] Books;
        }
        public enum Categories {
            [DescriptionAttribute("Unknown")]
            Unknown = 1 << 0,
            [DescriptionAttribute("IValueCompare Definitions")]
            Comparers = 1 << 1,
            [DescriptionAttribute("IValueConverter Definitions, primarily for  for Xaml")]
            Converters = 1 << 2,
            [DescriptionAttribute("WPF-based Dialog classes that implement the CommonDialog interface and behavior")]
            Dialogs = 1 << 3,
            [DescriptionAttribute("Actionable Event Definitions (Mousewheel, currently)")]
            Events = 1 << 4,
            [DescriptionAttribute("Data Type Class Extensions, implements common functions (Name of this library, nyy?)")]
            Extensions = 1 << 5,
            [DescriptionAttribute("Generic Class Objects and Data Structures that were previously undefined")]
            Generics = 1 << 6,
            [DescriptionAttribute("Hardware Access")]
            Hardware = 1 << 7,
            [DescriptionAttribute("Xaml snippits")]
            Xaml = 1 << 8,
        }
        public WindowExamples() {
            Console.WriteLine("Starting up Common.Libraries Testbed...");
            List<Library> Libraries = new List<Library>();
            Libraries.Add(new Library { Name=Categories.Comparers, 
                Books=new String[] { "RegexComparer", }, 
            });
            Libraries.Add(new Library { Name=Categories.Converters, 
            Books=new String[] {
                "AggregateConverters", "BooleanToInverse", "BooleanToTransform",
                "BooleanToVisibility.Base (unused)", "DoubleToByte", 
                "IconToImageSource", "SolidColorBrushToAlphaHex", 
                "SolidColorBrushToHex",
            }, });
            Libraries.Add(new Library { Name=Categories.Dialogs, 
            Books=new String[] {
                "ColorDialog", "FontDialog", "OpenDialog", "PathDialog", 
                "SaveDialog", 
            }, });
            Libraries.Add(new Library { Name=Categories.Events, 
                Books=new String[] { "WheelGesture", }, 
            });
            Libraries.Add(new Library { Name=Categories.Extensions, 
            Books=new String[] {
                "TypeExtensions", "WpfExtensions", 
            }, });
            Libraries.Add(new Library { Name=Categories.Generics, 
            Books=new String[] {
                "Datagram", "Notifiable", "NotifyIcon", "ObservableColor", "OS",
                "RelayCommand", 
            }, });
            Libraries.Add(new Library { Name=Categories.Hardware, 
                Books=new String[] { "Screen", }, 
            });

            foreach (Library lib in Libraries) {
                Console.WriteLine("  Loading Common Library {0}...", 
                    typeof(Categories).GetEnumName(lib.Name));
                foreach(string book in lib.Books) {
                    Console.WriteLine("    {0}...", book);
                }
            }

            InitializeComponent();
            Common.Extensions.PasswordBoxExtensions.ShowPasswordHandler(
                this.pwdBox, this.txtBox);
            lblEnumDesc.Content = "[Enum] " + typeof(Categories)
                .GetEnumName(Categories.Extensions);
            lblEnumDesc.ToolTip = Common.Extensions.EnumExtensions
                .Description(Categories.Extensions);

            lblNumericOrdinal.Content = (8).ToSignedString() + ", " +
                Common.Extensions.NumericExtensions.ToOrdinalString(23);
            lblNumericOrdinal.ToolTip = "Signed String and Ordinal String";

            lblStringPercent.Content = Common.Extensions.StringExtensions
                .ToDecimalPercent("42%");
            lblStringPercent.ToolTip = ("99.9%").ToDecimalPercent();

            lblObjectToJson.Content = "Anon. JSON Object";
            lblObjectToJson.ToolTip = new SerializableObjectClass().ToJson();
            lblHasMethod.Content = new object().HasMethod("Equals").ToString();
            //menuColorDialog.Icon = Resources_Common.ColorDialog_Icon;
            lClock.DataContext = new Clock();

            Console.WriteLine((lblAssemblyAttribute.FindAncestor(typeof(Grid)))
                .ToString());
            TextBoxTotalMemory.Text = ((float)Memory.Total / 1024 / 1024 / 1024).ToString("0.00") + "GB";
        }
        ~WindowExamples() {
            Console.WriteLine("Shutting down Common.Libraries Testbed...");
        }
    }
    public partial class WindowExamples {
#region Interface Methods
        public void MenuCollapseWhitespace_Click(object o, RoutedEventArgs e) {
            string s = "  This    is   a\t\t\t test.   ";
            MessageBox.Show(String.Format(
                "Original ({0} char):" + Environment.NewLine + "\"{1}\"" + 
                Environment.NewLine + "New ({2} char):" + Environment.NewLine + 
                "\"{3}\"", s.Count(), s, s.CollapseWhitespace().Count(), 
                s.CollapseWhitespace()
            ), "CollapseWhitespace() String Extension");
        }
        public void MenuDisableHitTesting_Click(object o, RoutedEventArgs e) {
            if (MessageBox.Show(
                "You will no longer be able to access this window, are you sure you want to continue?",
                "", MessageBoxButton.YesNo
            ) == MessageBoxResult.Yes) this.DisableHitTesting();
        }
        public void MenuDisableMaximize_Click(object o, RoutedEventArgs e) {
            this.DisableMaximize();
        }
        public void MenuDumpObject_Click(object o, RoutedEventArgs e) {
            MessageBox.Show(
                o.Dump().Replace(Environment.NewLine, "").CollapseWhitespace(),
                String.Format("Dumped Object {0}", o.GetType().Name), 
                MessageBoxButton.OK, 
                MessageBoxImage.Information
            );
            Clipboard.SetText(o.Dump());
        }
        public void MenuEnableMaximize_Click(object o, RoutedEventArgs e) {
            this.EnableMaximize();
        }
        public void MenuColorDialog_Click(object o, RoutedEventArgs e) {
            Random r = new Random(Environment.TickCount);
            ColorDialog cd = new ColorDialog() {
                Color = new System.Windows.Media.Color() {
                    A = 255,
                    R = (byte)r.Next(0, 255),
                    G = (byte)r.Next(0, 255),
                    B = (byte)r.Next(0, 255)
                }
            };
            if (cd.ShowDialog() == true)
                MessageBox.Show(String.Format("Color: #{0}{1}{2}",
                    cd.Color.R.ToString("X2"), cd.Color.G.ToString("X2"), 
                    cd.Color.B.ToString("X2")
                ));
        }
        public void MenuFontDialog_Click(object o, RoutedEventArgs e) {
            FontDialog fd = new FontDialog();
            if ((bool)fd.ShowDialog()) MessageBox.Show(
                String.Format("Family: {0}, Size: {1}, Style: {2}, Weight: {3}",
                    fd.FontFamily, fd.FontSize, fd.FontStyle, fd.FontWeight
                ), "Selected Font", MessageBoxButton.OK, 
                MessageBoxImage.Information
            );
        }
        public void MenuFullname_Click(object o, RoutedEventArgs e) {
            FullName fn = new FullName() {
                Prefix = "Technician, Second Class",
                FirstName = "Arnold",
                MiddleName = "Judas", 
                LastName = "Rimmer", 
                Suffix = new List<string>() {
                    "BSc", "SSc"
                }
            };
            MessageBox.Show(String.Format("Full Name: {0}", fn.ToString()));
        }
        public void MenuCreateIcon_Click(object o, RoutedEventArgs e) {
            NotificationIcon ni = new NotificationIcon() {
                HWnd = this.GetHandle(), 
                ToolTip = "Test for Shell32 Icon"
            };
        }
        public void MenuOpenDialog_Click(object o, RoutedEventArgs e) {
            OpenDialog od = new OpenDialog();
            if (od.ShowDialog() == true)
                MessageBox.Show(String.Format("{0}", od.FileName));
        }
        public void MenuSaveDialog_Click(object o, RoutedEventArgs e) {
            SaveDialog sd = new SaveDialog();
            if (sd.ShowDialog() == true)
                MessageBox.Show(String.Format("{0}", sd.FileName));
        }
        public void MenuPathDialog_Click(object o, RoutedEventArgs e) {
            PathDialog pd = new PathDialog();
            if (pd.ShowDialog() == true)
                MessageBox.Show(String.Format("Selected Path: {0}", pd.Path));
        }
        public void MenuPromptDialog_Click(object o, RoutedEventArgs e) {
            PromptDialog pd = new PromptDialog();
            if (pd.ShowDialog() == true)
                MessageBox.Show(String.Format("Prompt Input: {0}", pd.Result));
        }
#endregion Interface Methods
    }


    [DataContract][Serializable]
    class SerializableObjectClass {
        [DataMember] int id = 00101010;
        [DataMember] String name = "Otto Mattic";
    }
}
