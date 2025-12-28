using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Threading.Tasks;
using Testovoe.Model;

namespace Testovoe.DataBase
{
    /// <summary>
    /// Класс управляющий БД
    /// </summary>
    static class DBManager
    {
        /// <summary>
        /// Добавляет строку параметров в БД
        /// </summary>
        public static async Task AddParametersData(List<Parameters> parameters)
        {
            using (ApplicationContext db = new ApplicationContext())
            {  
                await db.parameters.AddRangeAsync(parameters);
                db.SaveChanges();
                
            }
        }

        public static async Task CleanTable()
        {
            using (ApplicationContext db = new ApplicationContext())
            {
                await Task.Run(() =>
                {
                    db.RemoveRange(db.parameters);
                    db.SaveChanges();
                });
            }
        }

        /// <summary>
        /// Получение параметров из БД
        /// </summary>
        public static List<Parameters> GetParametersData()
        {
            using (ApplicationContext db = new ApplicationContext())
            {
                return db.parameters.ToList();
            }
        }
       
    }
}
