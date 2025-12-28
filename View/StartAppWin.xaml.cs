using System;
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
using System.Windows.Shapes;
using Testovoe.ViewModel;

namespace Testovoe.View
{
    /// <summary>
    /// Логика взаимодействия для StartAppWin.xaml
    /// </summary>
    public partial class StartAppWin : Window
    {
        public StartAppWin()
        {
            InitializeComponent();
            DataContext = new StartAppWinViewModel(this);
        }
    }
}
