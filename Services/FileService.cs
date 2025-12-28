using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Xml.Serialization;
using Testovoe.Model;

namespace Testovoe.Services
{
    /// <summary>
    /// Класс для работы с файлом
    /// </summary>
    public static class FileService
    {
        static string saveSettingSource = AppDomain.CurrentDomain.BaseDirectory;
        const string pattern = @"\b\d+\.\d+\b";

        static async Task<string> GetDataFromFile(string path)
        {
            if (path == null || path == "")
            {
                return null;
            }
            using (FileStream fstream = File.OpenRead(path))
            {
                    byte[] buffer = new byte[fstream.Length];
                    await fstream.ReadAsync(buffer, 0, buffer.Length);
                    return Encoding.Default.GetString(buffer);

            }
        }
        /// <summary>
        /// Получение параметров из текста
        /// </summary>
        /// <param name="path">Путь к файлу</param>
        /// <returns></returns>
        public static async Task<List<float>> GetParametersFromFileText(string path)
        {
            EventManager.OnGetStateOperation(false);
            string text = await GetDataFromFile(path);
            MatchCollection matches = Regex.Matches(text, pattern);
            if (matches.Count % 6 == 0)
            {
                List<float> numbers = new List<float>();
                EventManager.OnGetMaxProgressValue(matches.Count);
                foreach (Match match in matches)
                {
                    float number = float.Parse(match.Value, CultureInfo.InvariantCulture);
                    Console.WriteLine(number);
                    numbers.Add(number);
                    EventManager.OnUpdateProgress();
                }
                EventManager.OnGetStateOperation(true);
                return numbers;
            }
            else {
                     MessageBox.Show("Кол-во параметров в файле не кратно кол-ву столбцов",
                              "Внимание", MessageBoxButton.OK, MessageBoxImage.Warning);
                EventManager.OnGetStateOperation(true);
                return null;
            }
        }

        /// <summary>
        /// Генерация файла размером ~50мб
        /// </summary>
        /// <param name="Path">Путь к файлу</param>
        /// <returns></returns>
        public static async Task GenerateFile(string path)
        {
            Random random = new Random();
            using (FileStream fstream = new FileStream(path, FileMode.OpenOrCreate))
            {
                EventManager.OnGetStateOperation(false);
                double num;
                StringBuilder sb = new StringBuilder("speed_2_1000; speed_10_1000;accel_1000, movement_2_1000; movement_10_1000;\r\n");
                EventManager.OnGetMaxProgressValue(10000008);
                for (int i = 0; i < 10000008; i++)
                {
                    if (i % 6 == 0)
                    {
                        sb.Append("\r\n");
                    }
                        num = random.Next(1, 9);
                    num-= random.NextDouble();

                    sb.Append(num.ToString("F2", CultureInfo.InvariantCulture) + ";");
                    EventManager.OnUpdateProgress();

                }
                byte[] buffer = Encoding.Default.GetBytes(sb.ToString());
                await fstream.WriteAsync(buffer, 0, buffer.Length);
                Console.WriteLine("Текст записан в файл");
                EventManager.OnGetStateOperation(true);
            }
        }


        public static void SaveConnectionSetting(ConnectionParamModel model)
        {
            try
            {
                string jsonPatch = Path.Combine(saveSettingSource, "saveSettingSource");
                string json = JsonConvert.SerializeObject(model);
                File.WriteAllText(jsonPatch, json);
            }
            catch
            {
                MessageBox.Show("Ошибка сохранения настроек");
            }
        }
        public static ConnectionParamModel GetConnectionSetting()
        {
            try
            {
                string jsonPatch = Path.Combine(saveSettingSource, "saveSettingSource");
                string readJson = File.ReadAllText(jsonPatch);
                ConnectionParamModel model = JsonConvert.DeserializeObject<ConnectionParamModel>(readJson);
                return model;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка получения данных настроек соеденения  - {ex.Message}");
                return default;
            }
        }

    }
}
