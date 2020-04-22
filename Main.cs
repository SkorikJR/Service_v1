using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;
using System.Windows.Forms;
using System.ComponentModel;
using System.Data;
using System.Drawing;

namespace Сервис
{
    public partial class Main : Form
    {
        public string server;
        public string user;
        public string basename;
        public string pwd;
        public string СтрокаПодключения = "";
        public string Запрос = "";
        public string[] СписокТаблиц = { "filial", "ingeeneer", "klient", "remont" };
        public int filial;
        public int ingeeneer;
        public int klient;
        public int remont;
        public Main()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            
            server = "localhost";
            user = "service_null";
            basename = "service_null";
            pwd = "Skorik2020";
            СтрокаПодключения = $"server={server};user={user};database={basename};password={pwd};";
            for (int i = 0; i < 4; i++)
            {
                MySqlConnection Коннектор = new MySqlConnection(СтрокаПодключения);
                Запрос = $"SELECT COUNT(*) FROM {СписокТаблиц[i]}";
                MySqlCommand Комманда = new MySqlCommand(Запрос, Коннектор);
                Коннектор.Open();
                MySqlDataReader Результат = Комманда.ExecuteReader();
                while (Результат.Read())
                {
                    Результат[0].ToString();
                }
                switch (i)
                {
                    case 0:
                        filial = int.Parse(Результат[0].ToString());
                        break;
                    case 1:
                        ingeeneer = int.Parse(Результат[0].ToString());
                        break;
                    case 2:
                        klient = int.Parse(Результат[0].ToString());
                        break;
                    case 3:
                        remont = int.Parse(Результат[0].ToString());
                        break;
                    default:
                        MessageBox.Show("Что-то пошло не так, обратитесь к поставщику БД!");
                        break;
                }
                
                Отключиться(Коннектор);
            }//загружаем количество записей в каждой из таблиц
            ПодготовкаМассивов("1");
            ПодготовкаМассивов("2");
            ПодготовкаМассивов("3");
            ПодготовкаМассивов("4");
        }

        /*private void BConnect_Click(object sender, EventArgs e)
        {
            int строка = 1;
            MySqlConnection Коннектор = new MySqlConnection(СтрокаПодключения);// Обьявляем cBase как MySqlConnection(переменная строки подключения)
            

            for (int i = 0;i < ingeeneer;i++ )
            {
                Коннектор.Open();
                Запрос = $"SELECT * FROM `ingeeneer` WHERE `ID`={строка}";
                MySqlCommand Комманда = new MySqlCommand(Запрос, Коннектор);
                MySqlDataReader Результат = Комманда.ExecuteReader();
                Результат.Read();
                for (int ячейка = 0; ячейка < 5; ячейка++)
                {
                    Результат[ячейка].ToString();
                    МассивИнженеров[(строка - 1), ячейка] = Результат[ячейка].ToString();
                }
                строка = ++строка;
                Отключиться(Коннектор);
                this.Text = $"Подключение к {server}  выполнено успешно!";
                bZapolnenie.Enabled = true;
            }
        }*/

        class Филиалы
        {
            public string[,] МассивФилиалов { get; set; }
        }//обьявляем массив(Иначе почему-то не фурычит) для видимости отовсюду

        class Инженеры
        {
            public string[,] МассивИнженеров { get; set; }
        }//обьявляем массив(Иначе почему-то не фурычит) для видимости отовсюду

        class Клиенты
        {
            public string[,] МассивКлиентов { get; set; }
        }//обьявляем массив(Иначе почему-то не фурычит) для видимости отовсюду

        class Ремонты
        {
            public string[,] МассивРемонтов { get; set; }
        }//обьявляем массив(Иначе почему-то не фурычит) для видимости отовсюду

        private static void Отключиться(MySqlConnection Коннектор)
        {
            Коннектор.Close();
        }//отключение от БД

        public void ПодготовкаМассивов(string НомерТаблицы)
        {
            Филиалы Filial = new Филиалы();//Создаем обьекты наших публичных массивов
            Инженеры Ing = new Инженеры();
            Клиенты Kl = new Клиенты();
            Ремонты Rem = new Ремонты();

            string Table = "";
            int Count = 0;
            int Row = 0;
            //Отбор логики и переменных по входному числу, которое отвечает за выбор таблицы и инициализацию 
            //соответствующего двумерного массива с высотой "Count" и статической шириной "Row"
            if (int.Parse(НомерТаблицы) == 1)
            {
                Table = СписокТаблиц[0];
                Count = filial;
                Row = 6;
                Filial.МассивФилиалов = new string[filial, Row];
            }
            else if(int.Parse(НомерТаблицы)==2)
            {
                Table = СписокТаблиц[1];//название таблицы из массива
                Count = ingeeneer;//количество записей, получаем в Form_load
                Row = 5;//статическое количество столбцов
                Ing.МассивИнженеров = new string[ingeeneer, Row];//инициализируем массив
            }
            else if(int.Parse(НомерТаблицы) == 3)
            {
                Table = СписокТаблиц[2];
                Count = klient;
                Row = 5;
                Kl.МассивКлиентов = new string[klient, Row];
            }
            else if(int.Parse(НомерТаблицы) == 4)
            {
                Table = СписокТаблиц[3];
                Count = remont;
                Row = 21;
                Rem.МассивРемонтов = new string[remont, Row];
            }
            else {MessageBox.Show("Что-то пошло не так, обратитесь к поставщику БД!");}
            MySqlConnection Коннектор = new MySqlConnection(СтрокаПодключения);// Обьявляем cBase как MySqlConnection(переменная строки подключения)
            int строка = 1;
            for (int i = 0; i < Count; i++)//цикл выгрузки значений в обьявленные массивы (Внешний цикл - строки, а внутренний - ячейки)
            {
                Коннектор.Open();
                Запрос = $"SELECT * FROM `{Table}` WHERE `ID`={строка}";
                MySqlCommand Комманда = new MySqlCommand(Запрос, Коннектор);
                MySqlDataReader Результат = Комманда.ExecuteReader();
                Результат.Read();
                for (int ячейка = 0; ячейка < Row; ячейка++)
                {
                    Результат[ячейка].ToString();
                    //Отбор логики и переменных по входному числу, которое отвечает за выбор массива, который соответствует таблице
                    if (int.Parse(НомерТаблицы) == 1)
                    {

                        Filial.МассивФилиалов[(строка - 1), ячейка] = Результат[ячейка].ToString();
                    }
                    else if (int.Parse(НомерТаблицы) == 2)
                    {
                        
                        Ing.МассивИнженеров[(строка - 1), ячейка] = Результат[ячейка].ToString();
                    }
                    else if (int.Parse(НомерТаблицы) == 3)
                    {
                        
                        Kl.МассивКлиентов[(строка - 1), ячейка] = Результат[ячейка].ToString();
                    }
                    else if (int.Parse(НомерТаблицы) == 4)
                    {
                        
                        Rem.МассивРемонтов[(строка - 1), ячейка] = Результат[ячейка].ToString();
                    }
                    else { MessageBox.Show("Что-то пошло не так, обратитесь к поставщику БД!"); }

                }
                строка = ++строка;
                Отключиться(Коннектор);
            }
                this.Text = $"Подключение к {server}  выполнено успешно!";
                bZapolnenie.Enabled = true;
                
        
        }
        public void ПодготовкаDataGrid(string[] N, DataGridView Grid)

        {
            while (N.Length > Grid.ColumnCount)
            {
                Grid.Columns.Add("", "");
            }
            Grid.Rows.Add(N);
        }//Выгрузка в DataGrid

        /*private void Button1_Click(object sender, EventArgs e)
        {
            int L = МассивИнженеров.Length;
            int H = (МассивИнженеров.Length) / 5;
            L = L / H;
            for (int i = 0; i < H; i++)
            {
                string[] Tmp = new string[L];
                for (int j = 0; j < L; j++)
                {
                    Tmp[j] = МассивИнженеров[i,j];
                }
                ПодготовкаDataGrid(Tmp, dataGridView1);
            }
        }*/
    }

}
