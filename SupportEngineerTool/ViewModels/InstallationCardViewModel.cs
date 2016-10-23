using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Serilog;
using SupportEngineerTool.HelperClasses;
using SupportEngineerTool.Models;

namespace SupportEngineerTool.ViewModels {
    public class InstallationCardViewModel : INotifyPropertyChanged {
        private InstallInformationModel installInfo;
        private string _codeBase;
        private string _dataFolder;
        private string _imageStore;

        public ICommand RefreshCommand { get; set; }

        public InstallationCardViewModel() {
            installInfo = new InstallInformationModel();
            CodeBase = installInfo.CodeBase;
            DataFolder = installInfo.DataStore;
            ImageStore= installInfo.ImageStore;
            LoadCommands();


        }

        private void LoadCommands () {
            RefreshCommand = new CustomCommand(RefreshInformation, CanRefreshInformation);
        }

        private void RefreshInformation(object obj) {
            installInfo.RefreshConfigFile();
            CodeBase = installInfo.CodeBase;
            DataFolder = installInfo.DataStore;
            ImageStore = installInfo.ImageStore;
        }

        private bool CanRefreshInformation(object obj) {
            return true;
        }


        #region Notify_Declerations

        public string CodeBase {
            get { return _codeBase; }
            set {
                _codeBase = value;
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

        public void UpdateConfigInformation() {
            try {
                installInfo.RefreshConfigFile();
                CodeBase = installInfo.CodeBase;
                DataFolder = installInfo.DataStore;
                ImageStore = installInfo.ImageStore;
                Log.Logger.Information("Updated new configuration information.");
            }
            catch(Exception failedUpdateConfigInformation) {
                Log.Logger.Error(failedUpdateConfigInformation, "Failed to update configuration settings");
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
