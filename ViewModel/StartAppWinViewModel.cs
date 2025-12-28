using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using Testovoe.Model;
using Testovoe.Services;
using Testovoe.View;

namespace Testovoe.ViewModel
{
    public class StartAppWinViewModel : INotifyPropertyChanged
    {
        Window currentWin;
        CommandsMVVM saveCommand;
        CommandsMVVM continueCommand;
        private ConnectionParamModel conModel;



        public CommandsMVVM ContinueCommand
        {
            get
            {
                return continueCommand ??
                  (continueCommand = new CommandsMVVM(obj =>
                  {
                      Application.Current.Dispatcher.Invoke(() =>
                      {
                          WorkWin workWin = new WorkWin();
                          workWin.Show();
                          currentWin.Close();
                      });
                          
                      
                  }));
            }
        }

        public CommandsMVVM SaveCommand
        {
            get
            {
                return saveCommand ??
                  (saveCommand = new CommandsMVVM(obj =>
                  {
                      if (ConModel != null)
                      {
                          Application.Current.Dispatcher.Invoke(() =>
                          {
                              FileService.SaveConnectionSetting(ConModel);
                              WorkWin workWin = new WorkWin();
                              workWin.Show();

                              currentWin.Close();
                          });
                      }
                      else MessageBox.Show("Ошибка сохранения");
                  }));
            }
        }
        public ConnectionParamModel ConModel
        {
            get => conModel;
            set { conModel = value; OnPropertyChanged("ConModel"); }
        }

        public StartAppWinViewModel(Window currentWin)
        {

            ConModel = new ConnectionParamModel()
            {
                Host = "localhost",
                DataBase = "testBD",
                Password = "3894",
                Port = "5432",
                Username = "postgres"
            };
            this.currentWin = currentWin;
        }

        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged([CallerMemberName] string prop = "")
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(prop));
        }
    }
}
