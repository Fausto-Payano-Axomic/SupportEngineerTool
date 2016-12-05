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
    public class InstallationCardViewModel : INotifyPropertyChanged, INotifyCollectionChanged {
        public InstallInformationModel installInfo;
        private string _codeBase;
        private string _dataFolder;
        private string _imageStore;
        private string _sslCertAU;
        private string _sslCertFile;
        private string _sslCertKey;
        private string _versionNumber;

        public ICommand RefreshCommand { get; set; }
 

        public InstallationCardViewModel() {
            installInfo = new InstallInformationModel();
            CodeBase = installInfo.CodeBase;
            this.DataFolder = installInfo.DataStore;
            this.ImageStore = installInfo.ImageStore;
            this.SslCertAuth = installInfo.ApacheConfigFile.SslCertificateAuthority;
            this.SslCertFile = installInfo.ApacheConfigFile.SslCertificateFile;
            this.SslCertKey = installInfo.ApacheConfigFile.SslCertificateKey;
            LoadCommands();
        }

        private void LoadCommands() {
            RefreshCommand = new CustomCommand(RefreshInformation, CanRefreshInformation);
            VersionNumber = GetPublishedVersion();

        }
        private void RefreshInformation(object obj) {
            installInfo.RefreshConfigFile();
            this.CodeBase = installInfo.CodeBase;
            this.DataFolder = installInfo.DataStore;
            this.ImageStore = installInfo.ImageStore;
            this.SslCertAuth = installInfo.ApacheConfigFile.SslCertificateAuthority;
            this.SslCertFile = installInfo.ApacheConfigFile.SslCertificateFile;
            this.SslCertKey = installInfo.ApacheConfigFile.SslCertificateKey;
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
        public string SslCertAuth {
            get { return _sslCertAU; }
            set {
                _sslCertAU = value;
                NotifyPropertyChanged();
            }
        }
        public string SslCertFile {
            get { return _sslCertFile; }
            set {
                _sslCertFile = value;
                NotifyPropertyChanged();
            }
        }
        public string SslCertKey {
            get { return _sslCertKey; }
            set {
                _sslCertKey = value;
                NotifyPropertyChanged();
            }
        }

        public string VersionNumber {
            get { return _versionNumber; }
            set {
                _versionNumber = value;
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
            catch (Exception failedUpdateConfigInformation) {
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
        private string GetPublishedVersion() {
            if (System.Deployment.Application.ApplicationDeployment.IsNetworkDeployed) {
                return System.Deployment.Application.ApplicationDeployment.CurrentDeployment.
                    CurrentVersion.ToString();
            }
            return "No Version information Available.";
        }

    }
}
