using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Security.AccessControl;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Threading;
using Testovoe.DataBase;
using Testovoe.Model;
using Testovoe.Services;

namespace Testovoe.ViewModel
{
    public class WorkWinViewModel : INotifyPropertyChanged
    {
        private string readaedFileText; // Прочитанный текст с файла

        CommandsMVVM readDataFromFile;
        CommandsMVVM selectFile;
        CommandsMVVM getDataFromDb;
        CommandsMVVM writeDataToDb;
        CommandsMVVM generateFile;

        private double progressBarValue;
        private string fileSource;
        private string fileText;
        private int maxProgressValue;

        public int MaxProgressValue 
        {
            get => maxProgressValue;
            set
            {
                maxProgressValue = value;
                OnPropertyChanged("MaxProgressValue");
            }
        }

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


        public double ProgressBarValue 
        { 
            get => progressBarValue;
            set
            {
                progressBarValue = value;
                OnPropertyChanged("ProgressBarValue");
            }
        }

        public ObservableCollection<Parameters> parametersCollection { get; set; }
        public WorkWinViewModel()
        {
            Services.EventManager.maxProgresHandler += GetMaxValueProgress;
            Services.EventManager.updateProgressHandler += UpdateProgressValue;
            
        }

        void GetMaxValueProgress(int progressCount)
        {
            MaxProgressValue = progressCount;
            Console.WriteLine(MaxProgressValue + "MaxProgressValue");
        }

        void UpdateProgressValue()
        {
                if (ProgressBarValue < MaxProgressValue)
                {
                    ProgressBarValue++;
                    Console.WriteLine("Update");
                }
                else
                {
                    MessageBox.Show("Чтение закончено!");
                    ProgressBarValue = 0;
                }
        }
        /// <summary>
        /// Чтение информации с файла
        /// </summary>
        public CommandsMVVM ReadDataFromFile
        {
            get
            {
                return readDataFromFile ??
                  (readDataFromFile = new CommandsMVVM(async obj =>
                  {
                      MessageBox.Show("Чтение!");
                      var t = new StringBuilder();
                      await Task.Run(async () =>
                      {
                          List<float> list = await FileService.GetParametersFromFileText(fileSource);
                          for (int i = 0; i < list.Count; i++)
                          {
                              t.Append(list[i] + "; ");
                              FileText = t.ToString();
                          }
                      });
                     
                 }));
            }
        }

        public CommandsMVVM GenerateFile
        {
            get
            {
                return generateFile ??
                  (generateFile = new CommandsMVVM(async obj =>
                  {
                      SaveFileDialog saveFileDialog = new SaveFileDialog();
                      saveFileDialog.Filter = "Text files (*.txt)|*.txt";
                      saveFileDialog.ShowDialog();
                      if (saveFileDialog.FileName != null && saveFileDialog.FileName != "")
                      {
                          FileSource = saveFileDialog.FileName;
                          await Task.Run(async() =>
                          {
                              await FileService.GenerateFile(FileSource);
                          });
                          
                      }
                      else
                      {
                          MessageBox.Show("Путь не был выбран!", "Ошибка",
                              MessageBoxButton.OK, MessageBoxImage.Error);
                          return;
                      }

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
                          
                           if (fileSource != null && fileSource != "")
                          readaedFileText = System.IO.File.ReadAllText(fileSource);
                      });
                  }));
            }
        }

        /// <summary>
        /// Получение таблички с БД
        /// </summary>
        public CommandsMVVM GetDataFromDB
        {
            get
            {
                return getDataFromDb ??
                  (getDataFromDb = new CommandsMVVM(obj =>
                  {
                      parametersCollection = new ObservableCollection<Parameters>(DBManager.GetParametersData());
                  }));
            }
        }

        /// <summary>
        /// Запись в БД
        /// </summary>
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
