using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SupportEngineerTool.Models {
    class OpenAssetConfigurationFile {
        public string InstallPath { get; set; }
        public string Data_Path { get; set; }
        public string[] ApacheListeningPorts { get; set; }
    }
}
