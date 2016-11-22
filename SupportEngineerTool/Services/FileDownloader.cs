using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Xml;
using Serilog;

namespace SupportEngineerTool.Services {
    public class FileDownloader {
        private string DownloadLink { get; }
        private string SaveLocation { get; set; }

        public FileDownloader(string downloadLink) {
            TryCreateInstallFolder();
            DownloadLink = downloadLink;

        }

        private void TryCreateInstallFolder() {
            try {
                if (!Directory.Exists("C:/OpenAsset_Install")) {
                    Directory.CreateDirectory("C:/OpenAsset_Install/");

                }
                this.SaveLocation = "C:/OpenAsset_Install";

            }
            catch (Exception createInstallFolderException) {
                Log.Logger.Error(
                    $"Error creating the install folder on C Drive. StackTrace: {createInstallFolderException}");
                this.SaveLocation = Environment.CurrentDirectory;
            }
        }

        public void DownloadFile(string saveFileName = "") {
            BackgroundWorker bgWorker = new BackgroundWorker();
            bgWorker.DoWork += DoDownloadComponent;
            bgWorker.RunWorkerCompleted += DownloadComplete;
            bgWorker.RunWorkerAsync(saveFileName);

        }

        private void DownloadComplete(object sender, RunWorkerCompletedEventArgs e) {
            MessageBox.Show($"Download of {this.DownloadLink} complete.");
        }
        private void DoDownloadComponent(object sender, DoWorkEventArgs e) {
            try {
                using (WebClient downloadClient = new WebClient()) {
                    Uri fileName = new Uri(this.DownloadLink);
                    string saveName = fileName.PathAndQuery.Split('/').Last();
                   
                    downloadClient.DownloadFile(this.DownloadLink, $"{this.SaveLocation}/{saveName}");
                }
            }
            catch (Exception webClientException) {
                Log.Logger.Error($"Error using Webclient to download file. StackTrace: \n {webClientException}");
            }
        }
    }
}
