using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SupportEngineerTool.Items {
    public class DownloadUrl {
        public string Category { get; set; }
        public string DisplayedName { get; set; }
        public string Link { get; set; }
       
        public DownloadUrl(string category, string name, string link) {
            this.Category = category;
            this.DisplayedName = name;
            this.Link = link;
        }
    }
}
