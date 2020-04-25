using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Сервис
{
    static class Auth
    {
        public static string server;//Static_BD
        public static string user;//Static_BD
        public static string basename;//Static_BD
        public static string pwd;//Static_BD
        public static string СтрокаПодключения = "";
        public static string Запрос = "";
        public static string Login = "";
        public static string Pwd = "";
        public static string uPwd = "";
        public static string AcMode = "";
        public static string[] Sotrudnik = new string[8];//Данные сотрудника
        public static string[,] Sotrudnik_All;//Данные сотрудников(Всех, для выборки)
        public static string[,] Klient_All;//Данные сотрудников(Всех, для выборки)
        public static string[,] Filial_All;//Данные филиалов(Всех, для выборки)
        public static string[,] Remont_All;//Данные филиалов(Всех, для выборки)
        public static int ВсегоРемонтов;
        public static int ВсегоСотрудников;
    }
}
