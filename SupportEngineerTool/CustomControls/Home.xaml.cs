using System;
using System.Collections.Generic;
using System.ComponentModel;
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
using SupportEngineerTool.ViewModels;

namespace SupportEngineerTool.CustomControls {
    /// <summary>
    /// Interaction logic for Home.xaml
    /// </summary>
    
    public partial class Home : UserControl {
        public InstallationCardViewModel InstallationViewModel;

        public Home() {
            InitializeComponent();
            InstallationViewModel = new InstallationCardViewModel();
        }

        private void GitHubButton_OnClick(object sender, RoutedEventArgs e) {
            Process.Start("https://github.com/Fausto-Payano-Axomic/SupportEngineerTool");
        }

        private void ZenDeskButton_OnClick(object sender, RoutedEventArgs e) {
            Process.Start("https://axomic.zendesk.com");
        }

        private void SyncButton_OnClick(object sender, RoutedEventArgs e) {
            InstallationControlProgressBar.Visibility = Visibility.Visible;
            var syncWorker = new BackgroundWorker();
            syncWorker.DoWork += syncWorker_DoWork;
            syncWorker.RunWorkerCompleted += syncWorker_WorkCompleted;
            syncWorker.RunWorkerAsync();

        }

        private void syncWorker_DoWork(object sender, System.ComponentModel.DoWorkEventArgs e) {
           
           InstallationViewModel.installInfo.RefreshConfigFile();
        }

        private void syncWorker_WorkCompleted(object sender, System.ComponentModel.RunWorkerCompletedEventArgs e) {
            InstallationControlProgressBar.Visibility = Visibility.Hidden;

        }
    }
}
