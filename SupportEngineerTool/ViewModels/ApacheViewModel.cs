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
        private FileInfo _pfxFile { get; set; }
        public ICommand DragDropCommand { get; set; }

        public ApacheViewModel() {
            _sslCertCreator = new SslCertCreator();
            LoadCommands();
        }

        private void LoadCommands() {
            DragDropCommand = new CustomCommand(DragAndDropRead, CanDragDrop);
        }


        /// <summary>
        /// DragAndDrop file is read after CanDragDrop bool indicates its the proper type.
        /// </summary>
        /// <param name="obj"></param>
        private void DragAndDropRead(object obj) {
            try {
                if (obj != null) {
                    var file = (string)(obj as DragEventArgs).Data.GetData(DataFormats.FileDrop);
                    FileInfo draggedFile = new FileInfo(file);

                    }
                }
            catch (Exception dragFileException) {
                MessageBox.Show(dragFileException.ToString());
            }
        }
        /// <summary>
        /// Executed when file is drag and dropped and command is fired. Determines whether file type
        /// extension is a valid type.
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        private bool CanDragDrop(object obj) {
            try {
                if (obj != null) {
                    string file = (string)(obj as DragEventArgs).Data.GetData(DataFormats.FileDrop);
                    FileInfo draggedFile = new FileInfo(file);
                    if (draggedFile.Extension == ".pdf") {
                        return true;
                    }
                }
                return false;
            }
            catch
                (Exception fileTypeCheckException) {
                MessageBox.Show(fileTypeCheckException.ToString());
                return false;
            }
        }

        public FileInfo PfxFile {
            get { return _pfxFile; }
            set {
                _pfxFile = value;
                NotifyPropertyChanged();
            }
        }
        #region NotifyPropertyChanged
        public event PropertyChangedEventHandler PropertyChanged;

        private void NotifyPropertyChanged([CallerMemberName] String propertyName = "") {
            if (PropertyChanged != null) {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }
        #endregion
    }
}
