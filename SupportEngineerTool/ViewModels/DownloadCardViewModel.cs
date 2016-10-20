using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using Serilog;
using SupportEngineerTool.Items;
using SupportEngineerTool.Models;
using SupportEngineerTool.Services;
using SupportEngineerTool.Utilities;

namespace SupportEngineerTool.ViewModels {
    public class DownloadCardViewModel : INotifyCollectionChanged {
        private DownloadUrlModel downloadUrlModel;
        private ObservableCollection<DownloadCategory> _observableDownloadUrls =
        new ObservableCollection<DownloadCategory>();

        public DownloadUrl SelectedItem { get; set; }

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
            try {
                FileDownloader downloader = new FileDownloader(SelectedItem.Link);
                downloader.DownloadFile("");
                }
            
            catch (Exception downloadFileException) {
                Log.Logger.Error($"Error downloading file from link {this.SelectedItem.Link}, stack trace printout: {downloadFileException}");
            }
        }

        private bool CanInitiateDownload(object obj) {
            if (SelectedItem != null) {
                return true;
            }
            return false;
        }

        private void OnNotifyCollectionChanged(NotifyCollectionChangedEventArgs args) {
            this.CollectionChanged?.Invoke(this, args);
        }
        public event NotifyCollectionChangedEventHandler CollectionChanged;
    }
}
