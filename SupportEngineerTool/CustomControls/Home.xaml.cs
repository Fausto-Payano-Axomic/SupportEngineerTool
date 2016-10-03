using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace SupportEngineerTool.CustomControls {
    /// <summary>
    /// Interaction logic for Home.xaml
    /// </summary>
    public partial class Home : UserControl {
        public Home() {
            InitializeComponent();
        }

        private void GitHubButton_OnClick(object sender, RoutedEventArgs e) {
            Process.Start("https://github.com/Fausto-Payano-Axomic/SupportEngineerTool");
        }

        private void ZenDeskButton_OnClick(object sender, RoutedEventArgs e) {
            Process.Start("https://axomic.zendesk.com");
        }
    }
}
