using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SupportEngineerTool.Models;

namespace SupportEngineerTool.ViewModels {
    public class DownloadCardViewModel : INotifyCollectionChanged {
        private DownloadUrlModel downloadUrlModel;
        private ObservableCollection<DownloadUrlModel> _observableDownloadUrls =
        new ObservableCollection<DownloadUrlModel>();


        public ObservableCollection<DownloadUrlModel> ObservableDownloadUrls {
            get { return _observableDownloadUrls; }
        }
        public DownloadCardViewModel() {
            downloadUrlModel = new DownloadUrlModel();
            foreach (var downloadUrl in downloadUrlModel.XmlOutput) {
                _observableDownloadUrls.Add(downloadUrlModel);
                this.OnNotifyCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Add,
                    downloadUrl));
            }

        }
        private void OnNotifyCollectionChanged(NotifyCollectionChangedEventArgs args) {
            this.CollectionChanged?.Invoke(this, args);
        }
        public event NotifyCollectionChangedEventHandler CollectionChanged;
    }
}
