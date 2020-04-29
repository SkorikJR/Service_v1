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
using System.Diagnostics;

namespace Сервис
{
    public partial class Loading : Form
    {
        public string[] СписокТаблиц = { "filial", "ingeeneer", "klient", "remont" };
        public int filial;
        public int ingeeneer;
        public int klient;
        public int remont;
        public int КоличествоСписаний = 0;
        public string ДатаПодСписание = "";
        public Loading()
        {
            InitializeComponent();
        }
        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
           Exit("server");
           Exit("httpd_usbwv8.exe");
           Exit("mysqld_usbwv8.exe");
           System.Environment.Exit(0);
        }//Событие закрытия формы
        public void Exit(string Exit)
        {
            Process[] ps1 = System.Diagnostics.Process.GetProcessesByName($"{Exit}"); //Имя процесса
            foreach (Process p1 in ps1)
            {
                p1.Kill();
            }
        }//Завершение работы внешней программы

        private void Form1_Load(object sender, EventArgs e)
        {
            dgProblem.Rows.Clear();
            dgReady.Rows.Clear();
            dgSpisanie.Rows.Clear();
            try
            {
                for (int i = 0; i < 4; i++)
                {
                    MySqlConnection Коннектор = new MySqlConnection(Auth.СтрокаПодключения);
                    Auth.Запрос = $"SELECT COUNT(*) FROM {СписокТаблиц[i]}";
                    MySqlCommand Комманда = new MySqlCommand(Auth.Запрос, Коннектор);
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
                            ДанныеДляОтбора.КолФилиалов = filial;
                            Auth.ВсегоФилиалов = filial;
                            break;
                        case 1:
                            ingeeneer = int.Parse(Результат[0].ToString());
                            Auth.ВсегоСотрудников = ingeeneer;
                            break;
                        case 2:
                            klient = int.Parse(Результат[0].ToString());
                            Auth.ВсегоКлиентов = klient;
                            break;
                        case 3:
                            remont = int.Parse(Результат[0].ToString());
                            Auth.ВсегоРемонтов = remont;
                            break;
                        default:
                            MessageBox.Show("Что-то пошло не так, обратитесь к поставщику БД!");
                            break;
                    }

                    Отключиться(Коннектор);
                }//загружаем количество записей в каждой из таблиц
            }
            catch (Exception)
            {
                Exit("server.exe");
                Exit("httpd_usbwv8.exe");
                Exit("mysqld_usbwv8.exe");
                System.Environment.Exit(0);
                throw;
            }
            ПодготовкаМассивов("1");//Филиалы
            ПодготовкаМассивов("2");
            ПодготовкаМассивов("3");
            MainData_n("Ready");
            MainData_n("Problem");
            MainData_n("Spisanie");
            Access(Auth.AcMode);//Auth.AcMode для отображения Прав пользователя
            label1.Text = $"Всего ремонтов в Базе: {remont}";
        }

        public void Access(string Acc)
        {
            if (Acc=="God")
            {
                this.Text = $"Добро пожаловать {Auth.Sotrudnik[1]}  {Auth.Sotrudnik[3]}! Вы подключены к базе {Auth.basename} на сервере {Auth.server} в режиме SuperAdmin";
            }
            else if (Acc == "Admin")
            {
                this.Text = $"Добро пожаловать {Auth.Sotrudnik[1]}  {Auth.Sotrudnik[3]}! Вы подключены к базе {Auth.basename} на сервере {Auth.server} в режиме Администратора";
            }
            else if (Acc == "Ingeen")
            {
                this.Text = $"Добро пожаловать {Auth.Sotrudnik[1]}  {Auth.Sotrudnik[3]}! Вы подключены к базе {Auth.basename} на сервере {Auth.server} в режиме Инженера";
            }
            else if (Acc == "Base")
            {
                this.Text = $"Добро пожаловать {Auth.Sotrudnik[1]}  {Auth.Sotrudnik[3]}! Вы подключены к базе {Auth.basename} на сервере {Auth.server} в режиме Пользователя";
            }
        }//Деление по привилегиям

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
            public string[] ГотовыеРемонты { get; set; }
            public string[] ПроблемныеРемонты { get; set; }
            public string[,] МассивГотовыхРемонтов { get; set; }
            public string[,] МассивПроблемныхРемонтов { get; set; }

        }//обьявляем массив(Иначе почему-то не фурычит) для видимости отовсюду

        private static void Отключиться(MySqlConnection Коннектор)
        {
            Коннектор.Close();
        }//отключение от БД

        public void ПодготовкаМассивов(string НомерТаблицы)
        {
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
                Auth.Filial_All = new string[filial, Row];
            }
            else if(int.Parse(НомерТаблицы)==2)
            {
                Table = СписокТаблиц[1];//название таблицы из массива
                Count = ingeeneer;//количество записей, получаем в Form_load
                Row = 8;//статическое количество столбцов
                Auth.Sotrudnik_All = new string[ingeeneer, Row];//инициализируем массив
            }
            else if(int.Parse(НомерТаблицы) == 3)
            {
                Table = СписокТаблиц[2];
                Count = klient;
                Row = 8;
                Auth.Klient_All = new string[klient, Row];
            }
            else if(int.Parse(НомерТаблицы) == 4)
            {
                Table = СписокТаблиц[3];
                Count = remont;
                Row = ДанныеДляОтбора.КолСтр;
                ДанныеДляОтбора.Все_Ремонты = new string[remont, Row];
            }
            else {MessageBox.Show("Что-то пошло не так, обратитесь к поставщику БД!");}
            MySqlConnection Коннектор = new MySqlConnection(Auth.СтрокаПодключения);// Обьявляем cBase как MySqlConnection(переменная строки подключения)
            int строка = 1;
            for (int i = 0; i < Count; i++)//цикл выгрузки значений в обьявленные массивы (Внешний цикл - строки, а внутренний - ячейки)
            {
                
                Auth.Запрос = $"SELECT * FROM `{Table}` WHERE `ID`={строка}";
                MySqlCommand Комманда = new MySqlCommand(Auth.Запрос, Коннектор);
                Коннектор.Open();
                MySqlDataReader Результат = Комманда.ExecuteReader();
                Результат.Read();
                for (int ячейка = 0; ячейка < Row; ячейка++)
                {
                    Результат[ячейка].ToString();
                    //Отбор логики и переменных по входному числу, которое отвечает за выбор массива, который соответствует таблице
                    if (int.Parse(НомерТаблицы) == 1)
                    {

                        Auth.Filial_All[(строка - 1), ячейка] = Результат[ячейка].ToString();
                    }
                    else if (int.Parse(НомерТаблицы) == 2)
                    {
                        
                        Auth.Sotrudnik_All[(строка - 1), ячейка] = Результат[ячейка].ToString();
                    }
                    else if (int.Parse(НомерТаблицы) == 3)
                    {
                        
                        Auth.Klient_All[(строка - 1), ячейка] = Результат[ячейка].ToString();
                    }
                    else if (int.Parse(НомерТаблицы) == 4)
                    {

                        ДанныеДляОтбора.Все_Ремонты[(строка - 1), ячейка] = Результат[ячейка].ToString();
                    }
                    else { MessageBox.Show("Что-то пошло не так, обратитесь к поставщику БД!"); }

                }
                строка = ++строка;
                Отключиться(Коннектор);
            }
                this.Text = $"Подключение к {Auth.server}  выполнено успешно!";
        }

        public void ПодготовкаDataGrid(string[] N, DataGridView Grid)

        {
            while (N.Length > Grid.ColumnCount)
            {
                Grid.Columns.Add("", "");
            }
            Grid.Rows.Add(N);
        }//Выгрузка в DataGrid

        public void MainData_n(string Acc)//получение количества ID записей (Проблемных и готовых ремонтов)
        {
            int КоличествоСтрок;
            if (Acc=="Ready")
            {
                try
                {
                    Auth.Запрос = $"SELECT COUNT(*) FROM `remont` WHERE `Otremontirovano` = 1 AND `Vidano` = 0";
                    MySqlConnection Коннектор = new MySqlConnection(Auth.СтрокаПодключения);
                    Коннектор.Open();
                    MySqlCommand Комманда = new MySqlCommand(Auth.Запрос, Коннектор);
                    MySqlDataReader Результат = Комманда.ExecuteReader();
                    Результат.Read();
                    for (int i = 0; i < 1; i++)
                    {
                        КоличествоСтрок = int.Parse(Результат[0].ToString());
                    }
                    Отключиться(Коннектор);
                    MainData("Otremontirovano");
                }
                catch (Exception)
                {

                    MessageBox.Show("Не удалось получить количество готовых ремонтов.", "Err");
                    Exit("server.exe");
                    Exit("httpd_usbwv8.exe");
                    Exit("mysqld_usbwv8.exe");
                    System.Environment.Exit(0);
                    throw;
                }
                
            }
            else if (Acc=="Problem")
            {
                try
                {
                    Auth.Запрос = $"SELECT COUNT(*) FROM `remont` WHERE `Problem` = 1 AND `Vidano` = 0";
                    MySqlConnection Коннектор = new MySqlConnection(Auth.СтрокаПодключения);
                    Коннектор.Open();
                    MySqlCommand Комманда = new MySqlCommand(Auth.Запрос, Коннектор);
                    MySqlDataReader Результат = Комманда.ExecuteReader();
                    Результат.Read();
                    for (int i = 0; i < 1; i++)
                    {
                        Результат[0].ToString();
                        КоличествоСтрок = int.Parse(Результат[0].ToString());
                    }
                    Отключиться(Коннектор);
                    MainData("Problem");
                }
                catch (Exception)
                {

                    MessageBox.Show("Не удалось получить количество проблемных ремонтов.", "Err");
                    Exit("server.exe");
                    Exit("httpd_usbwv8.exe");
                    Exit("mysqld_usbwv8.exe");
                    System.Environment.Exit(0);
                    throw;
                }
                
            }
            else if (Acc == "Spisanie")
            {
                ДатаПодСписание = DateTime.Today.AddDays(-Auth.Время_Списания).ToString("yyyy'-'MM'-'dd");
                try
                {
                    Auth.Запрос = $"SELECT COUNT(*) FROM `remont` WHERE `dOconchaniyaR` <= \"{ДатаПодСписание}\" AND `Vidano` = 0 ";
                    MySqlConnection Коннектор = new MySqlConnection(Auth.СтрокаПодключения);
                    Коннектор.Open();
                    MySqlCommand Комманда = new MySqlCommand(Auth.Запрос, Коннектор);
                    MySqlDataReader Результат = Комманда.ExecuteReader();
                    Результат.Read();
                    for (int i = 0; i < 1; i++)
                    {
                        Результат[0].ToString();
                        КоличествоСписаний = int.Parse(Результат[0].ToString());
                    }
                    Отключиться(Коннектор);
                }
                catch (Exception)
                {

                    MessageBox.Show("Не удалось получить количество ремонтов под списание!", "Err");
                    Exit("server.exe");
                    Exit("httpd_usbwv8.exe");
                    Exit("mysqld_usbwv8.exe");
                    System.Environment.Exit(0);
                    throw;
                }
                if (КоличествоСписаний > 0)
                {
                    MainData("Spisanie");
                }
            }
        }

        public void MainData(string Условие)//Генерация списка IDs ремонтов, которые подходят по условию.
        {

            if (Условие=="Spisanie")
            {
                int Столбцы_Spis = 6;//Ко-во столбцов под списание.
                try
                {
                    Auth.Spisanie_ID = new string[КоличествоСписаний];
                    Auth.Запрос = $"SELECT `ID` FROM `remont` WHERE `dOconchaniyaR` <= \"{ДатаПодСписание}\" AND `Vidano` = 0 ";
                    MySqlConnection Коннектор = new MySqlConnection(Auth.СтрокаПодключения);
                    Коннектор.Open();
                    MySqlCommand Комманда = new MySqlCommand(Auth.Запрос, Коннектор);
                    MySqlDataReader Результат = Комманда.ExecuteReader();
                    int Clc = 0;
                    while (Результат.Read())
                    {
                        string Ячейка = Результат[0].ToString();
                            Auth.Spisanie_ID[Clc] = Ячейка;
                        Clc = ++Clc;
                    }
                    Clc = 0;
                    Отключиться(Коннектор);
                }
                catch (Exception)
                {
                    MessageBox.Show("Не удалось получить список ремонтов под списание.", "Err");
                    Exit("server.exe");
                    Exit("httpd_usbwv8.exe");
                    Exit("mysqld_usbwv8.exe");
                    System.Environment.Exit(0);
                    throw;
                }
                try//Формируем таблицу
                {
                    int строка = 1;
                    Auth.Spisanie = new string[КоличествоСписаний, Столбцы_Spis];
                    MySqlConnection Коннектор = new MySqlConnection(Auth.СтрокаПодключения);// Обьявляем cBase как MySqlConnection(переменная строки подключения)
                    for (int i = 0; i < КоличествоСписаний; i++)
                    {
                        Auth.Запрос = $"SELECT `ID`,`Type`,`Proizv`,`Model`,`dOconchaniyaR`,`Filial_Now` FROM `remont` WHERE `ID`={Auth.Spisanie_ID[i]}";
                        MySqlCommand Комманда = new MySqlCommand(Auth.Запрос, Коннектор);
                        Коннектор.Open();
                        MySqlDataReader Результат = Комманда.ExecuteReader();
                        Результат.Read();
                        for (int ячейка = 0; ячейка < Столбцы_Spis; ячейка++)
                        {
                            Auth.Spisanie[i, ячейка] = Результат[ячейка].ToString();
                            if (ячейка == 4)
                            {
                                Auth.Spisanie[i, ячейка] = DateTime.Parse(Auth.Spisanie[i, ячейка]).ToString("dd'.'MM'.'yyyy");
                            }//попытка автозамены даты "На лету"
                            if (ячейка == 5)
                            {
                                Auth.Spisanie[i, ячейка] = Auth.Filial_All[int.Parse(Auth.Spisanie[i, ячейка])-1, 2];
                            }//попытка автозамены филиала "На лету"
                        }
                        строка = ++строка;
                        Отключиться(Коннектор);
                    }
                    int L = Auth.Spisanie.Length;
                    int H = Auth.Spisanie.Length / Столбцы_Spis;
                    L /= H;
                    for (int i = 0; i < H; i++)
                    {
                        string[] Tmp = new string[L];
                        for (int j = 0; j < L; j++)
                        {
                            Tmp[j] = Auth.Spisanie[i, j];
                        }
                        ПодготовкаDataGrid(Tmp, dgSpisanie);
                    }
                }
                catch (Exception)
                {
                    MessageBox.Show("Не удалось сформировать список ремонтов под списание.", "Err");
                    Exit("server.exe");
                    Exit("httpd_usbwv8.exe");
                    Exit("mysqld_usbwv8.exe");
                    System.Environment.Exit(0);
                    throw;
                }
            }
            else
            {
                int nGot = 0;
                int nProbl = 0;
                try
                {
                    Auth.Запрос = $"SELECT COUNT(*) FROM `remont` WHERE `{Условие}` = 1 AND Vidano = 0";
                    MySqlConnection Коннектор = new MySqlConnection(Auth.СтрокаПодключения);
                    Коннектор.Open();
                    MySqlCommand Комманда = new MySqlCommand(Auth.Запрос, Коннектор);
                    MySqlDataReader Результат = Комманда.ExecuteReader();
                    int Clc = 0;
                    while (Результат.Read())
                    {
                        string sN = Результат[0].ToString();
                        if (Условие == "Otremontirovano")
                        {
                            nGot = int.Parse(sN);
                        }
                        else if (Условие == "Problem")
                        {
                            nProbl = int.Parse(sN);
                        }
                        Clc = ++Clc;
                    }
                    Clc = 0;
                    Отключиться(Коннектор);
                }
                catch (Exception)
                {
                    throw;
                }
                Ремонты Rem = new Ремонты();
                int Столбцы = 6;
                Rem.ГотовыеРемонты = new string[nGot];
                Rem.МассивГотовыхРемонтов = new string[nGot, Столбцы];
                Rem.ПроблемныеРемонты = new string[nProbl];
                Rem.МассивПроблемныхРемонтов = new string[nProbl, Столбцы];
                if (nGot != 0)
                {
                    try
                    {
                        Auth.Запрос = $"SELECT `ID` FROM `remont` WHERE {Условие} = 1 AND Vidano = 0";
                        MySqlConnection Коннектор = new MySqlConnection(Auth.СтрокаПодключения);
                        Коннектор.Open();
                        MySqlCommand Комманда = new MySqlCommand(Auth.Запрос, Коннектор);
                        MySqlDataReader Результат = Комманда.ExecuteReader();
                        int Clc = 0;
                        while (Результат.Read())
                        {
                            Результат[0].ToString();
                            string Ячейка = Результат[0].ToString();
                            if (Условие == "Otremontirovano")
                            {
                                Rem.ГотовыеРемонты[Clc] = Ячейка;
                            }
                            Clc = ++Clc;
                        }
                        Clc = 0;
                        Отключиться(Коннектор);
                    }
                    catch (Exception)
                    {
                        MessageBox.Show("Не удалось получить количество готовых ремонтов.", "Err");
                        Exit("server.exe");
                        Exit("httpd_usbwv8.exe");
                        Exit("mysqld_usbwv8.exe");
                        System.Environment.Exit(0);
                        throw;
                    }
                    if (Условие == "Otremontirovano")
                    {
                        int строка = 1;
                        MySqlConnection Коннектор = new MySqlConnection(Auth.СтрокаПодключения);// Обьявляем cBase как MySqlConnection(переменная строки подключения)
                        for (int i = 0; i < nGot; i++)
                        {
                            Коннектор.Open();
                            if (Условие == "Otremontirovano")
                            {
                                Auth.Запрос = $"SELECT `ID`,`Type`,`Proizv`,`Model`,`dOconchaniyaR`,`ID_Master` FROM `remont` WHERE `ID`={Rem.ГотовыеРемонты[i]}";
                            }
                            MySqlCommand Комманда = new MySqlCommand(Auth.Запрос, Коннектор);
                            MySqlDataReader Результат = Комманда.ExecuteReader();
                            Результат.Read();
                            for (int ячейка = 0; ячейка < Столбцы; ячейка++)
                            {
                                Результат[ячейка].ToString();
                                if (Условие == "Otremontirovano")
                                {
                                    Rem.МассивГотовыхРемонтов[(строка - 1), ячейка] = Результат[ячейка].ToString();
                                    if (ячейка == 4)
                                    {
                                        Rem.МассивГотовыхРемонтов[(строка - 1), ячейка] = DateTime.Parse(Rem.МассивГотовыхРемонтов[(строка - 1), ячейка]).ToString("dd'.'MM'.'yyyy");
                                    }
                                    if (ячейка == 5)
                                    {
                                        int temp = int.Parse(Rem.МассивГотовыхРемонтов[(строка - 1), ячейка]) - 1;
                                        Rem.МассивГотовыхРемонтов[(строка - 1), ячейка] = Auth.Sotrudnik_All[temp, 2];
                                    }//попытка автозамены "На лету"
                                }
                            }
                            строка = ++строка;
                            Отключиться(Коннектор);
                        }

                        if (Условие == "Otremontirovano")
                        {
                            int L = Rem.МассивГотовыхРемонтов.Length;
                            int H = Rem.МассивГотовыхРемонтов.Length / 6;
                            L /= H;
                            for (int i = 0; i < H; i++)
                            {
                                string[] Tmp = new string[L];
                                for (int j = 0; j < L; j++)
                                {
                                    Tmp[j] = Rem.МассивГотовыхРемонтов[i, j];
                                }
                                ПодготовкаDataGrid(Tmp, dgReady);
                            }
                        }
                    }
                    else if (true)
                    {
                        MessageBox.Show("Ошибка инициализации", "Критическая ошибка");
                        Exit("server.exe");
                        Exit("httpd_usbwv8.exe");
                        Exit("mysqld_usbwv8.exe");
                        System.Environment.Exit(0);
                    }
                }
                else if (nProbl != 0)
                {
                    try
                    {
                        Auth.Запрос = $"SELECT `ID` FROM `remont` WHERE {Условие} = 1 AND Vidano = 0";
                        MySqlConnection Коннектор = new MySqlConnection(Auth.СтрокаПодключения);
                        Коннектор.Open();
                        MySqlCommand Комманда = new MySqlCommand(Auth.Запрос, Коннектор);
                        MySqlDataReader Результат = Комманда.ExecuteReader();
                        int Clc = 0;
                        while (Результат.Read())
                        {
                            Результат[0].ToString();
                            string Ячейка = Результат[0].ToString();
                            if (Условие == "Otremontirovano")
                            {
                                Rem.ГотовыеРемонты[Clc] = Ячейка;
                            }
                            else if (Условие == "Problem")
                            {
                                Rem.ПроблемныеРемонты[Clc] = Ячейка;
                            }
                            Clc = ++Clc;
                        }
                        Clc = 0;
                        Отключиться(Коннектор);
                    }
                    catch (Exception)
                    {
                        MessageBox.Show("Не удалось получить количество готовых ремонтов.", "Err");
                        Exit("server.exe");
                        Exit("httpd_usbwv8.exe");
                        Exit("mysqld_usbwv8.exe");
                        System.Environment.Exit(0);
                        throw;
                    }
                    if (Условие == "Problem")
                    {
                        int строка = 1;
                        MySqlConnection Коннектор = new MySqlConnection(Auth.СтрокаПодключения);// Обьявляем cBase как MySqlConnection(переменная строки подключения)
                        for (int i = 0; i < nProbl; i++)
                        {
                            Коннектор.Open();
                            if (Условие == "Problem")
                            {
                                Auth.Запрос = $"SELECT `ID`,`Type`,`Proizv`,`Model`,`DateOfPriem`,`Prinyal` FROM `remont` WHERE `ID`={Rem.ПроблемныеРемонты[i]}";
                            }

                            MySqlCommand Комманда = new MySqlCommand(Auth.Запрос, Коннектор);
                            MySqlDataReader Результат = Комманда.ExecuteReader();
                            Результат.Read();
                            for (int ячейка = 0; ячейка < Столбцы; ячейка++)
                            {
                                Результат[ячейка].ToString();
                                if (Условие == "Problem")
                                {
                                    Rem.МассивПроблемныхРемонтов[(строка - 1), ячейка] = Результат[ячейка].ToString();
                                    if (ячейка == 4)
                                    {
                                        Rem.МассивПроблемныхРемонтов[i, ячейка] = DateTime.Parse(Rem.МассивПроблемныхРемонтов[i, ячейка]).ToString("dd'.'MM'.'yyyy");
                                    }
                                    if (ячейка == 5)
                                    {
                                        int temp = int.Parse(Rem.МассивПроблемныхРемонтов[(строка - 1), ячейка]) - 1;
                                        Rem.МассивПроблемныхРемонтов[(строка - 1), ячейка] = Auth.Sotrudnik_All[temp, 2];
                                    }//попытка автозамены "На лету"
                                }

                            }
                            строка = ++строка;
                            Отключиться(Коннектор);
                        }

                        if (Условие == "Problem")
                        {
                            int L = Rem.МассивПроблемныхРемонтов.Length;
                            int H = Rem.МассивПроблемныхРемонтов.Length / 6;
                            L /= H;
                            for (int i = 0; i < H; i++)
                            {
                                string[] Tmp = new string[L];
                                for (int j = 0; j < L; j++)
                                {
                                    Tmp[j] = Rem.МассивПроблемныхРемонтов[i, j];
                                }
                                ПодготовкаDataGrid(Tmp, dgProblem);
                            }
                        }
                    }
                }
            }
            
        }

        private void Button5_Click(object sender, EventArgs e)//Settings
        {

        }

        private void DgReady_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            string Выбрано = dgReady.CurrentCell.Value.ToString();
            string Столбец = dgReady.CurrentCell.ColumnIndex.ToString();
            
            if (Столбец == "0")
            {
                Auth.Запрос = $"SELECT * FROM `remont` WHERE `ID`={Выбрано}";
                MySqlConnection Коннектор = new MySqlConnection(Auth.СтрокаПодключения);
                MySqlCommand Комманда = new MySqlCommand(Auth.Запрос, Коннектор);
                Коннектор.Open();
                MySqlDataReader Результат = Комманда.ExecuteReader();
                Результат.Read();
                for (int ячейка = 0; ячейка < ДанныеДляОтбора.КолСтр; ячейка++)
                {
                    Результат[ячейка].ToString();
                    ДанныеДляОтбора.Ремонт[ячейка] = Результат[ячейка].ToString();

                }
                Отключиться(Коннектор);
                Remont O_Rem;
                O_Rem = new Remont();
                O_Rem.Show();
            }
        }

        private void DgProblem_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            string Выбрано = dgProblem.CurrentCell.Value.ToString();
            string Столбец = dgProblem.CurrentCell.ColumnIndex.ToString();

            if (Столбец == "0")
            {
                Auth.Запрос = $"SELECT * FROM `remont` WHERE `ID`={Выбрано}";
                MySqlConnection Коннектор = new MySqlConnection(Auth.СтрокаПодключения);
                MySqlCommand Комманда = new MySqlCommand(Auth.Запрос, Коннектор);
                Коннектор.Open();
                MySqlDataReader Результат = Комманда.ExecuteReader();
                Результат.Read();
                for (int ячейка = 0; ячейка < ДанныеДляОтбора.КолСтр; ячейка++)
                {
                    Результат[ячейка].ToString();
                    ДанныеДляОтбора.Ремонт[ячейка] = Результат[ячейка].ToString();

                }
                Отключиться(Коннектор);
                Remont O_Rem;
                O_Rem = new Remont();
                O_Rem.Show();
            }
        }

        private void BExit_Click(object sender, EventArgs e)
        {
            this.Hide();
            Login Out;
            Out = new Login();
           Out.Show();
        }//Возврат на страницу авторизации

        private void BOpenAll_Click(object sender, EventArgs e)
        {
            All_Rem Open_all;
            Open_all = new All_Rem();
            Open_all.Show();
            ///////
        }

        private void BOpenIng_Click(object sender, EventArgs e)
        {
            All_Ing Open_all;
            Open_all = new All_Ing();
            Open_all.Show();
            ///////
        }

        private void Button1_Click(object sender, EventArgs e)
        {
            Open Find;
            Find = new Open();
            Find.Show();
        }

        private void BOpenKl_Click(object sender, EventArgs e)
        {
            All_klients Open_all;
            Open_all = new All_klients();
            Open_all.Show();
        }

        private void BnKl_Click(object sender, EventArgs e)
        {
            N_klient Nkl;
            Nkl = new N_klient();
            Nkl.Show();
        }

        private void Button2_Click(object sender, EventArgs e)
        {
            N_ingeeneer Ning;
            Ning = new N_ingeeneer();
            Ning.Show();
        }

        private void DgSpisanie_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            string Выбрано = dgSpisanie.CurrentCell.Value.ToString();
            string Столбец = dgSpisanie.CurrentCell.ColumnIndex.ToString();

            if (Столбец == "0")
            {
                Auth.Запрос = $"SELECT * FROM `remont` WHERE `ID`={Выбрано}";
                MySqlConnection Коннектор = new MySqlConnection(Auth.СтрокаПодключения);
                MySqlCommand Комманда = new MySqlCommand(Auth.Запрос, Коннектор);
                Коннектор.Open();
                MySqlDataReader Результат = Комманда.ExecuteReader();
                Результат.Read();
                for (int ячейка = 0; ячейка < ДанныеДляОтбора.КолСтр; ячейка++)
                {
                    Результат[ячейка].ToString();
                    ДанныеДляОтбора.Ремонт[ячейка] = Результат[ячейка].ToString();

                }
                Отключиться(Коннектор);
                Remont O_Rem;
                O_Rem = new Remont();
                O_Rem.Show();
            }
        }

        private void BNew_Click(object sender, EventArgs e)
        {
            N_Remont NRem;
            NRem = new N_Remont();
            NRem.Show();
        }
    }
}
