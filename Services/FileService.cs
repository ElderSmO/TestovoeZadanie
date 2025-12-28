using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Shapes;
using static System.Net.Mime.MediaTypeNames;

namespace Testovoe.Services
{
    /// <summary>
    /// Класс для работы с файлом
    /// </summary>
    public static class FileService
    {
        const string pattern = @"\b\d+\.\d+\b";

        static async Task<string> GetDataFromFile(string path)
        {
            using (FileStream fstream = File.OpenRead(path))
            {
                byte[] buffer = new byte[fstream.Length];
                await fstream.ReadAsync(buffer, 0, buffer.Length);
                Console.WriteLine(Encoding.Default.GetString(buffer));
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
            string text = await GetDataFromFile(path);
            List<float> numbers = new List<float>();
            MatchCollection matches = Regex.Matches(text, pattern);
            EventManager.OnGetMaxProgressValue(matches.Count);
            foreach (Match match in matches)
            {
                float number = float.Parse(match.Value, CultureInfo.InvariantCulture);
                    
                    Console.WriteLine(number);
                    numbers.Add(number);
                    EventManager.OnUpdateProgress();
                
            }
            return numbers;
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
                double num;
                StringBuilder sb = new StringBuilder("speed_2_1000; speed_10_1000;accel_1000, movement_2_1000; movement_10_1000;\r\n");
                for (int i = 0; i < 10000000; i++)
                {
                    if (i % 6 == 0)
                    {
                        sb.Append("\r\n");
                    }
                        num = random.Next(1, 9);
                    num-= random.NextDouble();

                    sb.Append(num.ToString("F2", CultureInfo.InvariantCulture) + ";");
                    

                }
                byte[] buffer = Encoding.Default.GetBytes(sb.ToString());
                await fstream.WriteAsync(buffer, 0, buffer.Length);
                Console.WriteLine("Текст записан в файл");
            }
        }

    }
}
