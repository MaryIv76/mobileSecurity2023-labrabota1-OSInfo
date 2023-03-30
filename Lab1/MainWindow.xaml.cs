using System;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Text.Json;
using System.Text.Encodings.Web;
using System.Text.Unicode;

namespace Lab1
{
    public partial class MainWindow : Window
    {
        private bool antivirusShown = false;
        private bool firewallShown = false;
        private bool OSShown = false;
        private bool hardwareShown = false;
        private bool jsonShown = false;

        public MainWindow()
        {
            InitializeComponent();
            hideComponents();
        }

        private async void onBtnGetInfoClick(object sender, RoutedEventArgs e)
        {
            waitTb.Visibility = Visibility.Visible;
            await Task.Run(() =>
            {
                if (Dispatcher.Invoke(() => (bool)antivirusChB.IsChecked && !antivirusShown))
                {
                    setAntivirusData();
                }
                if (Dispatcher.Invoke(() => (bool)firewallChB.IsChecked && !firewallShown))
                {
                    setFirewallData();
                }
                if (Dispatcher.Invoke(() => (bool)OSChB.IsChecked && !OSShown))
                {
                    setOSData();
                }
                if (Dispatcher.Invoke(() => (bool)hardwareChB.IsChecked && !hardwareShown))
                {
                    setHardwareData();
                }
                if (Dispatcher.Invoke(() => (bool)jsonChB.IsChecked && !jsonShown))
                {
                    setJSONData();
                }
            });
            waitTb.Visibility = Visibility.Collapsed;
        }

        public void setAntivirusData()
        {
            Dispatcher.Invoke(() =>
            {
                antivirusShown = true;
                antivirusTb.Visibility = Visibility.Visible;
                antivirusLv.Visibility = Visibility.Visible;
            });

            AntivirusData antivirusData = new AntivirusData();

            Dispatcher.Invoke(() =>
            {
                for (int i = 0; i < antivirusData.antiviruses.Count; i++)
                {
                    antivirusLv.Items.Add(new Entry { key = "Antivirus name", value = antivirusData.antiviruses[i].displayName, keyFontWeight = "Bold", valueFontWeight = "Bold" });
                    antivirusLv.Items.Add(new Entry { key = "Product state", value = antivirusData.antiviruses[i].productState });
                    antivirusLv.Items.Add(new Entry { key = "Path to signed product exe", value = antivirusData.antiviruses[i].pathToSignedProductExe });
                    antivirusLv.Items.Add(new Entry { key = "Path to signed reporting exe", value = antivirusData.antiviruses[i].pathToSignedReportingExe });
                    antivirusLv.Items.Add(new Entry { key = "Instance GUID", value = antivirusData.antiviruses[i].instanceGuid });
                }
            });
        }

        public void setFirewallData()
        {
            Dispatcher.Invoke(() =>
            {
                firewallShown = true;
                firewallTb.Visibility = Visibility.Visible;
                firewallLv.Visibility = Visibility.Visible;
            });

            Firewall firewall = new Firewall();

            Dispatcher.Invoke(() =>
            {
                firewallLv.Items.Add(new Entry { key = "Firewall", value = firewall.firewallState });
            });
        }

        public void setOSData()
        {
            Dispatcher.Invoke(() =>
            {
                OSShown = true;
                OSTb.Visibility = Visibility.Visible;
                OSLv.Visibility = Visibility.Visible;
                numOSUpdatesTb.Visibility = Visibility.Visible;
                OSUpdatesLv.Visibility = Visibility.Visible;
            });

            OS os = new OS();

            Dispatcher.Invoke(() =>
            {
                OSLv.Items.Add(new Entry { key = "Windows version", value = os.version });
                OSLv.Items.Add(new Entry { key = "OS type", value = os.type });
                OSLv.Items.Add(new Entry { key = "Bit version", value = os.bits });
                numOSUpdatesTb.Text = os.numUpdates;
                for (int i = 0; i < os.updates.Count; i++)
                {
                    OSUpdatesLv.Items.Add(new Entry { key = $"{i + 1}.", value = os.updates[i] });
                }
            });
        }

        public void setHardwareData()
        {
            Dispatcher.Invoke(() =>
            {
                hardwareShown = true;
                hardwareTb.Visibility = Visibility.Visible;
                hardwareLv.Visibility = Visibility.Visible;
            });

            Hardware hardware = new Hardware();

            Dispatcher.Invoke(() =>
            {
                hardwareLv.Items.Add(new Entry { key = "Processor", keyFontWeight = "Bold" });
                hardwareLv.Items.Add(new Entry { key = "Number of cores", value = hardware.numCores });
                hardwareLv.Items.Add(new Entry { key = "Number of logical processors", value = hardware.numLogProcs });
                hardwareLv.Items.Add(new Entry { key = "Load percentage", value = hardware.loadPercentage });

                hardwareLv.Items.Add(new Entry { });
                hardwareLv.Items.Add(new Entry { key = "Physical memory", keyFontWeight = "Bold" });
                hardwareLv.Items.Add(new Entry { key = "Total memory size", value = hardware.totalMemorySize });
                hardwareLv.Items.Add(new Entry { key = "Free physical memory", value = hardware.freePhysMemory });

                hardwareLv.Items.Add(new Entry { });
                hardwareLv.Items.Add(new Entry { key = "Disks", keyFontWeight = "Bold" });
                for (int i = 0; i < hardware.disks.Count; i++)
                {
                    hardwareLv.Items.Add(new Entry { key = "Name", value = hardware.disks[i].name, keyFontWeight = "Bold", valueFontWeight = "Bold" });
                    hardwareLv.Items.Add(new Entry { key = "Size", value = hardware.disks[i].size });
                    hardwareLv.Items.Add(new Entry { key = "Free space", value = hardware.disks[i].freeSpace });
                }
            });
        }

        public void setJSONData()
        {
            Dispatcher.Invoke(() =>
            {
                jsonShown = true;
                jsonTb.Visibility = Visibility.Visible;
                jsonLv.Visibility = Visibility.Visible;
            });

            var options = new JsonSerializerOptions
            {
                Encoder = JavaScriptEncoder.Create(UnicodeRanges.BasicLatin, UnicodeRanges.Cyrillic, UnicodeRanges.GeneralPunctuation),
                WriteIndented = true
            };

            AntivirusData antivirusData = new AntivirusData();
            string antivirusJSON = JsonSerializer.Serialize(antivirusData, options);
            Firewall firewall = new Firewall();
            string firewallJSON = JsonSerializer.Serialize(firewall, options);
            OS os = new OS();
            string osJSON = JsonSerializer.Serialize(os, options);
            Hardware hardware = new Hardware();
            string hardwareJSON = JsonSerializer.Serialize(hardware, options);

            Dispatcher.Invoke(() =>
            {
                jsonLv.Items.Add(new Entry { value = "Antivirus", valueFontWeight = "Bold" });
                jsonLv.Items.Add(new Entry { value = antivirusJSON });
                jsonLv.Items.Add(new Entry { value = "Firewall", valueFontWeight = "Bold" });
                jsonLv.Items.Add(new Entry { value = firewallJSON });
                jsonLv.Items.Add(new Entry { value = "OS", valueFontWeight = "Bold" });
                jsonLv.Items.Add(new Entry { value = osJSON });
                jsonLv.Items.Add(new Entry { value = "Hardware", valueFontWeight = "Bold" });
                jsonLv.Items.Add(new Entry { value = hardwareJSON });
            });
        }

        private void allChBChecked(object sender, RoutedEventArgs e)
        {
            antivirusChB.IsChecked = true;
            firewallChB.IsChecked = true;
            OSChB.IsChecked = true;
            hardwareChB.IsChecked = true;
            jsonChB.IsChecked = true;
        }

        private void allChBUnchecked(object sender, RoutedEventArgs e)
        {
            antivirusChB.IsChecked = false;
            firewallChB.IsChecked = false;
            OSChB.IsChecked = false;
            hardwareChB.IsChecked = false;
            jsonChB.IsChecked = false;
        }

        private void chBChecked(object sender, RoutedEventArgs e)
        {
            if ((bool)antivirusChB.IsChecked && (bool)firewallChB.IsChecked && (bool)OSChB.IsChecked && (bool)hardwareChB.IsChecked && (bool)jsonChB.IsChecked)
            {
                allChB.IsChecked = true;
            }
        }

        private void chBUnchecked(object sender, RoutedEventArgs e)
        {
            bool antivirusBool = (bool)antivirusChB.IsChecked;
            bool firewallsBool = (bool)firewallChB.IsChecked;
            bool OSBool = (bool)OSChB.IsChecked;
            bool hardwareBool = (bool)hardwareChB.IsChecked;
            bool jsonBool = (bool)jsonChB.IsChecked;

            allChB.IsChecked = false;

            antivirusChB.IsChecked = antivirusBool;
            firewallChB.IsChecked = firewallsBool;
            OSChB.IsChecked = OSBool;
            hardwareChB.IsChecked = hardwareBool;
            jsonChB.IsChecked = jsonBool;
        }

        public void hideComponents()
        {
            antivirusTb.Visibility = Visibility.Collapsed;
            antivirusLv.Visibility = Visibility.Collapsed;
            firewallTb.Visibility = Visibility.Collapsed;
            firewallLv.Visibility = Visibility.Collapsed;
            OSTb.Visibility = Visibility.Collapsed;
            OSLv.Visibility = Visibility.Collapsed;
            numOSUpdatesTb.Visibility = Visibility.Collapsed;
            OSUpdatesLv.Visibility = Visibility.Collapsed;
            hardwareTb.Visibility = Visibility.Collapsed;
            hardwareLv.Visibility = Visibility.Collapsed;
            jsonTb.Visibility = Visibility.Collapsed;
            jsonLv.Visibility = Visibility.Collapsed;
            waitTb.Visibility = Visibility.Collapsed;
        }

        private void instScrollLoaded(object sender, RoutedEventArgs e)
        {
            antivirusLv.AddHandler(MouseWheelEvent, new RoutedEventHandler(myMouseWheelH), true);
            firewallLv.AddHandler(MouseWheelEvent, new RoutedEventHandler(myMouseWheelH), true);
            OSLv.AddHandler(MouseWheelEvent, new RoutedEventHandler(myMouseWheelH), true);
            OSUpdatesLv.AddHandler(MouseWheelEvent, new RoutedEventHandler(myMouseWheelH), true);
            hardwareLv.AddHandler(MouseWheelEvent, new RoutedEventHandler(myMouseWheelH), true);
            jsonLv.AddHandler(MouseWheelEvent, new RoutedEventHandler(myMouseWheelH), true);
        }

        private void myMouseWheelH(object sender, RoutedEventArgs e)
        {
            MouseWheelEventArgs eargs = (MouseWheelEventArgs)e;
            double x = (double)eargs.Delta;
            double y = instScroll.VerticalOffset;
            instScroll.ScrollToVerticalOffset(y - x);
        }
    }
}
