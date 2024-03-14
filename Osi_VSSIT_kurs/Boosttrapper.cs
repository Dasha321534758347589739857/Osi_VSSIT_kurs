using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using Osi_VSSIT_kurs.VievModel;
using Osi_VSSIT_kurs.service;


namespace Osi_VSSIT_kurs
{
    public class Booststraper
    {
        public Window Run()
        {
            var mainWindow = new MainWindow();

            mainWindow.DataContext = new MainVievModel( new Excel());
            mainWindow.Show();
            return mainWindow;
        }
    }
}
    