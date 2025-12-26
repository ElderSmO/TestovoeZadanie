

using System;
using System.Collections.Generic;
using System.Linq;
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
        public static void AddParametersData(Parameters parameters)
        {
            using (ApplicationContext db = new ApplicationContext())
            {
                db.parameters.AddRange(parameters);
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
