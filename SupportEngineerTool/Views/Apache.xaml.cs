using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace SupportEngineerTool.Views {
    /// <summary>
    /// Interaction logic for Apache.xaml
    /// </summary>
    public partial class Apache : UserControl {
        public Apache() {
            InitializeComponent();
        }

        private void Card_DragEnter(object sender, DragEventArgs e) {
            if (e.Data.GetDataPresent(DataFormats.FileDrop, true) == true) {
                e.Effects = DragDropEffects.Copy;
            }
            else {
                e.Effects = DragDropEffects.None;
            }

        }

        private void Card_Drop(object sender, DragEventArgs e) {
            if (e.Data.GetDataPresent(DataFormats.FileDrop, true) == true) {

                string filename = ((string[])e.Data.GetData(DataFormats.FileDrop, true)).First();
                string extension = System.IO.Path.GetExtension(filename);
                if (extension.Equals(".pfx") || extension.Equals(".pkcs")) {
                    //This if just to test if the file path came in.
                    //I still need to add the path to another textbox so the user can see the 
                    //file that will be processed
                    MessageBox.Show(filename);
                }
                else {
                    MessageBox.Show("Error: " + extension + " is not a valid file type.");
                }

            }
        }
    }
}
