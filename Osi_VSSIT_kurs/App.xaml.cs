using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

namespace Osi_VSSIT_kurs
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        private Booststraper? _bootstrapper;
        protected override void OnStartup(StartupEventArgs e)
        {
            _bootstrapper = new Booststraper();
            MainWindow = _bootstrapper.Run();

        }
    }

}
