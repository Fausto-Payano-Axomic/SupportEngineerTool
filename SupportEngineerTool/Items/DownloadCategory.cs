using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SupportEngineerTool.Items {
   public class DownloadCategory {
        public string CategoryName { get; set; }

        public List<DownloadUrl> ContentsOfCategory { get; set; }

       public DownloadCategory(string name, DownloadUrl contents) {
           this.ContentsOfCategory = new List<DownloadUrl>();
           this.CategoryName = name;
           this.ContentsOfCategory.Add(contents);

       }

       public override string ToString() {
           return this.CategoryName;
       }
   }
}
