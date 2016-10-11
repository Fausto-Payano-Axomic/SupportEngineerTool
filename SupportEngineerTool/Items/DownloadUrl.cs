using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SupportEngineerTool.Items {
    public class DownloadUrl {
        private string Category { get; set; }
        private Dictionary<string, string> LinkPair = new Dictionary<string, string>();

        public DownloadUrl(string category, string name, string link) {
            this.Category = category;
            this.LinkPair.Add(name, link);
        }
    }
}
