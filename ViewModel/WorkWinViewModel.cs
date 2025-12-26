using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Net;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using Testovoe.Model;
using Testovoe.Services;

namespace Testovoe.ViewModel
{
    public class WorkWinViewModel : INotifyPropertyChanged
    {
        CommandsMVVM readDataFromFile;
        CommandsMVVM selectFileSource;
        CommandsMVVM getDataFromDb;
        CommandsMVVM writeDataToDb;

        private int progressBasValue;
        private string fileSource;
        private string fileText;

        public string FileText 
        {
            get => fileText;
            set
            {
                fileText = value;
                OnPropertyChanged("FileText");
            }
            
        }

        public string FileSource
        {
            get => fileSource;
            set
            {
                fileSource = value;
                OnPropertyChanged("FileSource");
            }
        }


        public int ProgressBasValue 
        { 
            get => progressBasValue;
            set
            {
                progressBasValue = value;
                OnPropertyChanged("ProgressBasValue");
            }
        }

        public ObservableCollection<Parameters> parametersCollection { get; set; }
        public WorkWinViewModel()
        { }

        public CommandsMVVM ReadDataFromFile
        {
            get
            {
                return readDataFromFile ??
                  (readDataFromFile = new CommandsMVVM(obj =>
                  {
                      MessageBox.Show("ReadFile");
                  }));
            }
        }


        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged([CallerMemberName] string prop = "")
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(prop));
        }
    }
}
