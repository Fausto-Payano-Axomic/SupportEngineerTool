using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using SupportEngineerTool.HelperClasses;
using SupportEngineerTool.Items;

namespace SupportEngineerTool.Views {
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

        private void TreeViewControl_OnSelectedItemChanged(object sender, RoutedPropertyChangedEventArgs<object> e) {
            ViewModelLocator.DownloadCardViewModel.SelectedItem = TreeViewControl.SelectedItem as DownloadUrl;
        }
    }
}
