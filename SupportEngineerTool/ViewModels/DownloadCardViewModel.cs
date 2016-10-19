using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using SupportEngineerTool.Items;
using SupportEngineerTool.Models;
using SupportEngineerTool.Utilities;

namespace SupportEngineerTool.ViewModels {
    public class DownloadCardViewModel : INotifyCollectionChanged {
        private DownloadUrlModel downloadUrlModel;
        private ObservableCollection<DownloadCategory> _observableDownloadUrls =
        new ObservableCollection<DownloadCategory>();

        public ICommand DownloadCommand { get; set; }


        public ObservableCollection<DownloadCategory> ObservableDownloadUrls {
            get { return _observableDownloadUrls; }
        }
        public DownloadCardViewModel() {
            downloadUrlModel = new DownloadUrlModel();

            foreach (var downloadCategory in downloadUrlModel.CategorizedList) {
                _observableDownloadUrls.Add(downloadCategory);
                this.OnNotifyCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Add,
                    downloadCategory));
            }

            LoadCommands();

        }

        private void LoadCommands() {
            DownloadCommand = new CustomCommand(InitiateDownload, CanInitiateDownload);
        }

        private void InitiateDownload(object obj) {
            //TODO: Implement downloading, async? Background worker so not to lock UI?
        }

        private bool CanInitiateDownload(object obj) {
            return true;
        }

        private void OnNotifyCollectionChanged(NotifyCollectionChangedEventArgs args) {
            this.CollectionChanged?.Invoke(this, args);
        }
        public event NotifyCollectionChangedEventHandler CollectionChanged;
    }
}
