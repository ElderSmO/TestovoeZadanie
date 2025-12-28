using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
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
        public static void AddParametersData(List<Parameters> parameters)
        {
            using (ApplicationContext db = new ApplicationContext())
            {
                db.parameters.AddRange(parameters);
                db.SaveChanges();
            }
        }

        public static void CleanTable()
        {
            using (ApplicationContext db = new ApplicationContext())
            {
                db.Database.ExecuteSqlRaw("TRUNCATE TABLE parameters RESTART IDENTITY;");
                db.SaveChanges();
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
