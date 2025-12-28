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
        const int columCount = 6;
        List<float> paramList;
        CommandsMVVM readDataFromFile;
        CommandsMVVM selectFile;
        CommandsMVVM getDataFromDb;
        CommandsMVVM writeDataToDb;
        CommandsMVVM generateFile;
        CommandsMVVM deleteAllRowsFromDB;

        private double progressBarValue;
        private string fileSource;
        private string fileText;
        private int maxProgressValue;
        private bool operationContinue;
        private ObservableCollection<Parameters> parametersCollection;

        public bool OperationContinue
        {
            get => operationContinue;
            set
            {
                operationContinue = value;
                OnPropertyChanged("OperationContinue");
            }
        }

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

        public ObservableCollection<Parameters> ParametersCollection
        {
            get => parametersCollection;
            set
            {
                parametersCollection = value;
                OnPropertyChanged("ParametersCollection");
            }
        }
        public WorkWinViewModel()
        {
            OperationContinue = true;
            Services.EventManager.maxProgresHandler += GetMaxValueProgress;
            Services.EventManager.updateProgressHandler += UpdateProgressValue;
            Services.EventManager.stateOperationHandler += GetOperationState;
            MaxProgressValue = 100;
            ProgressBarValue = 0;


        }

        /// <summary>
        /// Получение состояния операции
        /// </summary>
        /// <param name="state">Флаг состояния</param>
        void GetOperationState(bool state)
        {
            OperationContinue = state;
        }

        /// <summary>
        /// Получение мак. значения для ProgressBar
        /// </summary>
        /// <param name="progressCount">Max value</param>
        void GetMaxValueProgress(int progressCount)
        {
            
            MaxProgressValue = progressCount;
            Console.WriteLine(MaxProgressValue + "MaxProgressValue");
        }
        
        /// <summary>
        /// Обновление прогреса при каждой итерации
        /// </summary>
        void UpdateProgressValue()
        {
                if (ProgressBarValue < MaxProgressValue)
                {
                    ProgressBarValue++;
                    Console.WriteLine("Update");
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
                      var t = new StringBuilder();
                      await Task.Run(async () =>
                      {
                          paramList = await FileService.GetParametersFromFileText(fileSource);
                          if (paramList != null)
                          {
                              int strCount = 120;
                              if (strCount > paramList.Count) strCount = paramList.Count;
                              for (int i = 0; i < strCount; i++)
                              {
                                  t.Append(paramList[i] + "; ");
                                  if (i == strCount - 1)
                                      t.Append("конец отображённого отрывка параметров...");

                              }
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
                      });
                  }));
            }
        }


        public CommandsMVVM DeleteAllRowsFromDB
        {
            get
            {
                return deleteAllRowsFromDB ??
                  (deleteAllRowsFromDB = new CommandsMVVM(async obj =>
                  {
                      Services.EventManager.OnGetStateOperation(false);
                      MessageBox.Show("Eсли параметров много то операция " +
                          "займёт много времени, дождитесь сообщения об окончании");
                      await Task.Run(async () =>
                      {
                         await DBManager.CleanTable();
                      });
                      MessageBox.Show("Таблица успешно очищена!");
                      Services.EventManager.OnGetStateOperation(true);
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
                      ParametersCollection = new ObservableCollection<Parameters>(DBManager.GetParametersData());
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
                  (writeDataToDb = new CommandsMVVM(async obj =>
                  {
                      
                      List<Parameters> parametersListForDb = new List<Parameters>();
                      if (paramList != null)
                      {
                          if (paramList.Count % columCount == 0)
                          {
                              await Task.Run(async () =>
                              {
                                  
                                  Services.EventManager.OnGetStateOperation(false);
                                  MessageBox.Show("Параметры отправляются одним пакетом, если параметров много то операция займёт много времени, дождитесь сообщения об окончании");
                                  for (int i = 0; i < paramList.Count; i += columCount)
                                  {
                                      parametersListForDb.Add(new Parameters
                                      {
                                          Speed_2_1000 = paramList[i],
                                          Speed_10_1000 = paramList[i + 1],
                                          Accel_2_1000 = paramList[i + 2],
                                          Accel_10_1000 = paramList[i + 3],
                                          Movement_2_1000 = paramList[i + 4],
                                          Movement_10_1000 = paramList[i + 5]
                                      });

                                  }
                                  await DBManager.AddParametersData(parametersListForDb);

                                  Services.EventManager.OnGetStateOperation(true);
                                  MessageBox.Show("Успешно сохранено! Нажмите счиать с бд что бы подгрузить актульную информацию");
                              });
                          }
                          else MessageBox.Show("Кол-во параметров считанных не кратно кол-ву столбцов",
                              "Внимание", MessageBoxButton.OK, MessageBoxImage.Warning);

                          
                          
                      }
                      else MessageBox.Show("Вы не прочитали файл!", "Внимание", MessageBoxButton.OK, MessageBoxImage.Warning);
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
