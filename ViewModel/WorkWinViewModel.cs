using Microsoft.Win32;
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
using Testovoe.DataBase;
using Testovoe.Model;
using Testovoe.Services;

namespace Testovoe.ViewModel
{
    /// <summary>
    /// View Model для окна WorkWinViewModel
    /// </summary>
    public class WorkWinViewModel : INotifyPropertyChanged
    {
        private string readaedFileText; // Прочитанный текст с файла

        CommandsMVVM readDataFromFile;
        CommandsMVVM selectFile;
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

        public CommandsMVVM SelectFile
        {
            get
            {
                return selectFile ??
                  (selectFile = new CommandsMVVM(async obj =>
                  {
                      OpenFileDialog openFileDialog = new OpenFileDialog();
                      openFileDialog.ShowDialog();
                      await Task.Run(() =>
                      {
                              FileSource = openFileDialog.FileName;
                          
                           if (fileSource != null || fileSource != "")
                          readaedFileText = System.IO.File.ReadAllText(fileSource);
                      });
                  }));
            }
        }

        public CommandsMVVM GetDataFromDB
        {
            get
            {
                return getDataFromDb ??
                  (getDataFromDb = new CommandsMVVM(obj =>
                  {
                      MessageBox.Show("ReadFile");
                  }));
            }
        }

        public CommandsMVVM WriteDataToDb
        {
            get
            {
                return writeDataToDb ??
                  (writeDataToDb = new CommandsMVVM(obj =>
                  {
                      DBManager.AddParametersData(new Parameters()
                      {
                          Speed_10_1000 = 2.2f,
                          Accel_10_1000 = 1.2f,
                          Accel_2_1000 = 2.1f,
                          Speed_2_1000 = 3.1f,
                          Movement_10_1000 = 4.1f,
                          Movement_2_1000 = 2.3f
                      });
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
