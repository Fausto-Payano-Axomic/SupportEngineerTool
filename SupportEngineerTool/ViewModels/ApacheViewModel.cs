using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using SupportEngineerTool.Annotations;
using SupportEngineerTool.Models;


namespace SupportEngineerTool.ViewModels {

    /// <summary>
    /// TODO Generate a viewmodel for the Apache page. Do not bind directly to any Models. This will sync up with our ViewModelLocator class.
    /// </summary>
    /// 
    public class ApacheViewModel : INotifyPropertyChanged {
        private SslCertCreator _sslCertCreator;

        public ApacheViewModel() {
            _sslCertCreator = new SslCertCreator();
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
