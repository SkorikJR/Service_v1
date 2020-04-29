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
    public partial class Find_Kl : Form
    {
        public Find_Kl()
        {
            InitializeComponent();
        }
        public int Столбцы = 5;
        public int КоличествоГР = 0;
        public int Count_Kl;
        public string Выбрано;
        public string Столбец;
        private void Button1_Click(object sender, EventArgs e)
        {
            int find = 0;
            dataGridView1.Rows.Clear();
            try
            {
                MySqlConnection Коннектор = new MySqlConnection(Auth.СтрокаПодключения);// Обьявляем cBase как MySqlConnection(переменная строки подключения)
                Auth.Запрос = $"SELECT COUNT(*) FROM `klient` WHERE `Phone`=\"{textBox4.Text}\"";
                MySqlCommand Комманда = new MySqlCommand(Auth.Запрос, Коннектор);
                Коннектор.Open();
                MySqlDataReader Результат = Комманда.ExecuteReader();
                while (Результат.Read())
                {
                    find = int.Parse(Результат[0].ToString());
                }
                Отключиться(Коннектор);
            }
            catch (Exception)
            {
                MessageBox.Show("Не удалось найти клиента с таким номером телефона!");
                throw;
            }
             
            if (find == 1)
            {
                Auth.Запрос = $"SELECT `ID`,`Name`,`Family`,`Otchestvo`,`Phone` FROM `klient` WHERE `Phone`=\"{textBox4.Text}\"";
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
                dataGridView1.Rows.Clear();
                ПодготовкаDataGrid(Auth.Klient, dataGridView1);
            }
            else if (find == 0)
            {
                MessageBox.Show("Ничего не найдено");
            }
            else if (find > 1)
            {
                int строка = 1;
                Auth.All_Klient_vNewRem = new string[Count_Kl, Столбцы];//+1 это вычисляемое поле!
                for (int i = 0; i < Count_Kl; i++)
                {
                    Auth.Запрос = $"SELECT `ID`,`Name`,`Family`,`Otchestvo`,`Phone` FROM `klient` WHERE `Phone`=\"{textBox4.Text}\"";
                    MySqlConnection Коннектор = new MySqlConnection(Auth.СтрокаПодключения);
                    MySqlCommand Комманда = new MySqlCommand(Auth.Запрос, Коннектор);
                    Коннектор.Open();
                    MySqlDataReader Результат = Комманда.ExecuteReader();
                    Результат.Read();
                    for (int ячейка = 0; ячейка < Столбцы; ячейка++)
                    {
                        Auth.All_Klient_vNewRem[i, ячейка] = Результат[ячейка].ToString();
                    }
                    строка = ++строка;
                    Отключиться(Коннектор);
                }//Выгруз из базы
                dataGridView1.Rows.Clear();
                int L = Auth.All_Klient_vNewRem.Length;
                int H = Auth.All_Klient_vNewRem.Length / Столбцы;
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
        }

        private void Find_Kl_Load(object sender, EventArgs e)
        {
            dataGridView1.Rows.Clear();
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
                MessageBox.Show("Не удалось получить количество клиентов! Проверьте подключение к БД.", "Err");
                throw;
            }
            if (Count_Kl > 0)
            {
                int строка = 1;
                Auth.All_Klient_vNewRem = new string[Count_Kl, Столбцы];//+1 это вычисляемое поле!
                for (int i = 0; i < Count_Kl; i++)
                {
                    Auth.Запрос = $"SELECT `ID`,`Name`,`Family`,`Otchestvo`,`Phone` FROM `klient` WHERE `ID`={строка}";
                    MySqlCommand Комманда = new MySqlCommand(Auth.Запрос, Коннектор);
                    Коннектор.Open();
                    MySqlDataReader Результат = Комманда.ExecuteReader();
                    Результат.Read();
                    for (int ячейка = 0; ячейка < Столбцы; ячейка++)
                    {
                            Auth.All_Klient_vNewRem[i, ячейка] = Результат[ячейка].ToString();
                    }
                    строка = ++строка;
                    Отключиться(Коннектор);
                }//Выгруз из базы

                int L = Auth.All_Klient_vNewRem.Length;
                int H = Auth.All_Klient_vNewRem.Length / Столбцы;
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
                MessageBox.Show("Клиентов в БД нет.");
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
            Выбрано = dataGridView1.CurrentCell.Value.ToString();
            Столбец = dataGridView1.CurrentCell.ColumnIndex.ToString();
            Auth.Klient_vNewRem = new string[5];
            if (Столбец == "0")
            {
                Auth.Запрос = $"SELECT `ID`,`Name`,`Family`,`Otchestvo`,`Phone` FROM `klient` WHERE `ID`={Выбрано}";
                MySqlConnection Коннектор = new MySqlConnection(Auth.СтрокаПодключения);
                MySqlCommand Комманда = new MySqlCommand(Auth.Запрос, Коннектор);
                Коннектор.Open();
                MySqlDataReader Результат = Комманда.ExecuteReader();
                Результат.Read();
                for (int ячейка = 0; ячейка < Столбцы; ячейка++)
                {
                    Результат[ячейка].ToString();
                    Auth.Klient_vNewRem[ячейка] = Результат[ячейка].ToString();
                }
                dataGridView1.Rows.Clear();
                ПодготовкаDataGrid(Auth.Klient_vNewRem, dataGridView1);
                Отключиться(Коннектор);
                bOK.Enabled = true;
            }
        }

        private void BOK_Click(object sender, EventArgs e)
        {
            Auth.ID_Клиента = int.Parse(Auth.Klient_vNewRem[0]);
            this.Hide();
        }

        private void Button2_Click(object sender, EventArgs e)
        {
            N_klient Nkl;
            Nkl = new N_klient();
            Nkl.Show();
        }
    }
}
