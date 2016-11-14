using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using SupportEngineerTool.Annotations;
using SupportEngineerTool.HelperClasses;
using SupportEngineerTool.Models;


namespace SupportEngineerTool.ViewModels {

    /// <summary>
    /// TODO Generate a viewmodel for the Apache page. Do not bind directly to any Models. This will sync up with our ViewModelLocator class.
    /// </summary>
    /// 
    public class ApacheViewModel : INotifyPropertyChanged {
        public SslCertCreator _sslCertCreator;
        public ICommand DragDropCommand { get; set; }

        public ApacheViewModel() {
            _sslCertCreator = new SslCertCreator();
            LoadCommands();
        }

        private void LoadCommands() {
            DragDropCommand = new CustomCommand(DragAndDropRead, CanDragDrop);
        }

        private void DragAndDropRead(object obj) {
            try {
                var file = obj as FileInfo;
                if (file != null) {
                    string fileName = file.Name;
                    MessageBox.Show($"Filename: {fileName}");
                }
            }
            catch (Exception dragFileException) {
                MessageBox.Show(dragFileException.ToString());
            }
        }

        private bool CanDragDrop(object obj) {
            try {
                var file = obj as FileInfo;

                if (file != null && file.Extension == ".pdf") {
                    return true;
                }
                return false;
            }
            catch (Exception fileTypeCheckException) {
                MessageBox.Show(fileTypeCheckException.ToString());
                return false;
            }
        }
        #region NotifyPropertyChanged
        public event PropertyChangedEventHandler PropertyChanged;
        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null) {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));

        }
        #endregion
    }
}
