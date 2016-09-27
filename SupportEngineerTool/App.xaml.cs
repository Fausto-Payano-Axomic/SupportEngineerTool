using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

namespace SupportEngineerTool {
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application {

        protected override void OnStartup(StartupEventArgs e) {
            base.OnStartup(e);

           /* var log = new LoggerConfiguration()
                .MinimumLevel.Debug()
                .WriteTo.RollingFile("logs\\SupportEngineerTool-{Date}.txt")
                .CreateLogger();

            Log.Logger = log;*/
        }

        
    }
}

