using SupportEngineerTool.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace SupportEngineerTool.HelperClasses {
    public class ViewModelLocator {
        private static InstallationCardViewModel _installationCardViewModel = new InstallationCardViewModel();
        private static ApacheViewModel _apacheViewModel = new ApacheViewModel();
        private static DownloadCardViewModel _downloadCardViewModel = new DownloadCardViewModel();



        public static InstallationCardViewModel InstallationCardViewModel {
            get { return _installationCardViewModel; }
        }

        public static ApacheViewModel ApacheViewModel {
            get { return _apacheViewModel; }
        }

        public static DownloadCardViewModel DownloadCardViewModel {
            get { return _downloadCardViewModel; }
        }
    }
}
