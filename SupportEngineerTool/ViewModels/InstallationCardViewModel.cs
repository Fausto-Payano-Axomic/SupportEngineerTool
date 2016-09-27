using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using SupportEngineerTool.Models;

namespace SupportEngineerTool.ViewModels {
    public class InstallationCardViewModel : INotifyCollectionChanged, INotifyPropertyChanged {
        public InstallInformation installInfo;
        private string _installFolderPath;
        private string _dataFolder;
        private string _imageStore;

        public InstallationCardViewModel() {
            installInfo = new InstallInformation();
            InstallFolderPath = installInfo.InstallFolder;
            DataFolder = installInfo.DataStore;
            ImageStore= installInfo.ImageStore;


        }

        #region Notify_Declerations

        public string InstallFolderPath {
            get { return _installFolderPath; }
            set {
                _installFolderPath = value;
                NotifyPropertyChanged();
            }
        }

        public string DataFolder {
            get { return _dataFolder; }
            set {
                _dataFolder = value;
                NotifyPropertyChanged();
            }
        }

        public string ImageStore {
            get { return _imageStore; }
            set {
                _imageStore = value;
                NotifyPropertyChanged();
            }
        }

        #endregion
        #region INotifyCollectionChanged

        private void OnNotifyCollectionChanged(NotifyCollectionChangedEventArgs args) {
            if (this.CollectionChanged != null) {
                this.CollectionChanged(this, args);
            }
        }
        public event NotifyCollectionChangedEventHandler CollectionChanged;
        #endregion
        #region INotifyPropertyChanged

        private void NotifyPropertyChanged([CallerMemberName] String propertyName = "") {
            if (PropertyChanged != null) {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        #endregion

    }
}
