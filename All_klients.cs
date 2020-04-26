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
using System.IO;

namespace Сервис
{
    public partial class All_klients : Form
    {
        public All_klients()
        {
            InitializeComponent();
        }

        public int Столбцы = 8;
        public int КоличествоГР = 0;
        public int Count_Kl;
        private void All_klients_Load(object sender, EventArgs e)
        {
            MySqlConnection Коннектор = new MySqlConnection(Auth.СтрокаПодключения);// Обьявляем cBase как MySqlConnection(переменная строки подключения)
            try
            {
                Auth.Запрос = $"SELECT COUNT(*) FROM `klient`";
                MySqlCommand Комманда = new MySqlCommand(Auth.Запрос, Коннектор);
                Коннектор.Open();
                MySqlDataReader Результат = Комманда.ExecuteReader();
                while (Результат.Read())
                {
                    Count_Kl = int.Parse(Результат[0].ToString());
                }
                Отключиться(Коннектор);
            }
            catch (Exception)
            {
                MessageBox.Show("Не удалось получить количество клиентов! Проверьте подключение к БД.","Err");
                throw;
            }
            if (Count_Kl > 0)
            {
                int строка = 1;
                Auth.Klient_All = new string[Count_Kl, Столбцы + 1];//+1 это вычисляемое поле!
                for (int i = 0; i < Count_Kl; i++)
                {
                    Auth.Запрос = $"SELECT * FROM `klient` WHERE `ID`={строка}";
                    MySqlCommand Комманда = new MySqlCommand(Auth.Запрос, Коннектор);
                    Коннектор.Open();
                    MySqlDataReader Результат = Комманда.ExecuteReader();
                    Результат.Read();
                    for (int ячейка = 0; ячейка < Столбцы; ячейка++)
                    {
                        Auth.Klient_All[i, ячейка] = Результат[ячейка].ToString();
                    }
                    строка = ++строка;
                    Отключиться(Коннектор);
                }//Выгруз из базы
                try
                {
                    for (int i = 0; i < Auth.ВсегоКлиентов; i++)
                    {
                        Auth.Запрос = $"SELECT COUNT(*) FROM `remont` WHERE `ID_Klient` = {(i + 1)} AND `Vidano` = 1 AND `Vidano_BR` = 0";
                        MySqlCommand Комманда = new MySqlCommand(Auth.Запрос, Коннектор);
                        Коннектор.Open();
                        MySqlDataReader Результат = Комманда.ExecuteReader();
                        while (Результат.Read())
                        {
                            //КоличествоГР = int.Parse(Результат[0].ToString());
                            Auth.Klient_All[i, 8] = Результат[0].ToString();
                        }
                        Отключиться(Коннектор);
                    }
                }
                catch (Exception)
                {
                    MessageBox.Show("Не удалось посчитать ремонты!", "Err");
                    throw;
                }


                int L = Auth.Klient_All.Length;
                int H = Auth.Klient_All.Length / Столбцы;
                L /= H;
                for (int i = 0; i < H; i++)
                {
                    string[] Tmp = new string[L];
                    for (int j = 0; j < L; j++)
                    {
                        Tmp[j] = Auth.Klient_All[i, j];
                    }
                    ПодготовкаDataGrid(Tmp, dataGridView1);
                }
            }
            else
            {
                MessageBox.Show("Клиентов в БД нет. Плохо работаете =)");
            }
        }

        public void ПодготовкаDataGrid(string[] N, DataGridView Grid)

        {
            while (N.Length > Grid.ColumnCount)
            {
                Grid.Columns.Add("", "");
            }
            Grid.Rows.Add(N);
        }//Выгрузка в DataGrid
        private static void Отключиться(MySqlConnection Коннектор)
        {
            Коннектор.Close();
        }//отключение от БД
        private void DataGridView1_CellMouseDoubleClick_1(object sender, DataGridViewCellMouseEventArgs e)
        {
            string Выбрано = dataGridView1.CurrentCell.Value.ToString();
            string Столбец = dataGridView1.CurrentCell.ColumnIndex.ToString();

            if (Столбец == "0")
            {
                Auth.Запрос = $"SELECT * FROM `klient` WHERE `ID`={Выбрано}";
                MySqlConnection Коннектор = new MySqlConnection(Auth.СтрокаПодключения);
                MySqlCommand Комманда = new MySqlCommand(Auth.Запрос, Коннектор);
                Коннектор.Open();
                MySqlDataReader Результат = Комманда.ExecuteReader();
                Результат.Read();
                for (int ячейка = 0; ячейка < Столбцы; ячейка++)
                {
                    Результат[ячейка].ToString();
                    Auth.Klient[ячейка] = Результат[ячейка].ToString();
                }
                Отключиться(Коннектор);
                Klient O_Kl;
                O_Kl = new Klient();
                O_Kl.Show();
            }
        }
    }
}
