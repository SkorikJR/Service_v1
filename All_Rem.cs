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
    public partial class All_Rem : Form
    {
        public All_Rem()
        {
            InitializeComponent();
        }
        public int Столбцы = 11;
        private void All_Rem_Load(object sender, EventArgs e)//Нужно довести до ума!
        {
            
            int строка = 1;
            Auth.Remont_All = new string[Auth.ВсегоРемонтов, Столбцы];
            MySqlConnection Коннектор = new MySqlConnection(Auth.СтрокаПодключения);// Обьявляем cBase как MySqlConnection(переменная строки подключения)
            for (int i = 0; i < Auth.ВсегоРемонтов; i++)
            {
                Auth.Запрос = $"SELECT `ID`,`Type`,`Proizv`,`Model`,`DateOfPriem`,`Filial_Now`,`Prinyal`,`Prodiagnostirovan`,`Otremontirovano`,`Problem`,`Vidano` FROM `remont` WHERE `ID`={строка}";
                MySqlCommand Комманда = new MySqlCommand(Auth.Запрос, Коннектор);
                Коннектор.Open();
                MySqlDataReader Результат = Комманда.ExecuteReader();
                Результат.Read();
                for (int ячейка = 0; ячейка < Столбцы; ячейка ++)
                {
                    Auth.Remont_All[i, ячейка] = Результат[ячейка].ToString();
                    if (ячейка == 5)
                    {
                        int temp = int.Parse(Auth.Remont_All[i, ячейка]) - 1;
                        Auth.Remont_All[i, ячейка] = Auth.Filial_All[temp, 2];
                    }//попытка автозамены "На лету"
                    if (ячейка == 6)
                    {
                        int temp = int.Parse(Auth.Remont_All[i, ячейка]) - 1;
                        Auth.Remont_All[i, ячейка] = Auth.Sotrudnik_All[temp, 2];
                    }//попытка автозамены "На лету"
                    if (ячейка == 7 | ячейка == 8 | ячейка == 9 | ячейка == 10)
                    {
                        if (int.Parse(Auth.Remont_All[i, ячейка]) == 0)
                        {
                            Auth.Remont_All[i, ячейка] = "false";
                        }
                        else if (int.Parse(Auth.Remont_All[i, ячейка]) == 1)
                        {
                            Auth.Remont_All[i, ячейка] = "true";
                        }
                    }//Меняем "0" и "1" на чекбокс
                }
                строка = ++строка;
                Отключиться(Коннектор);
            }//Выгруз из базы


            int L = Auth.Remont_All.Length;
            int H = Auth.Remont_All.Length / Столбцы;
            L /= H;
            for (int i = 0; i < H; i++)
            {
                string[] Tmp = new string[L];
                for (int j = 0; j < L; j++)
                {
                    Tmp[j] = Auth.Remont_All[i, j];
                }
                ПодготовкаDataGrid(Tmp, dataGridView1);
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

        private void DataGridView1_CellMouseDoubleClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            string Выбрано = dataGridView1.CurrentCell.Value.ToString();
            string Столбец = dataGridView1.CurrentCell.ColumnIndex.ToString();

            if (Столбец == "0")
            {
                Auth.Запрос = $"SELECT * FROM `remont` WHERE `ID`={Выбрано}";
                MySqlConnection Коннектор = new MySqlConnection(Auth.СтрокаПодключения);
                MySqlCommand Комманда = new MySqlCommand(Auth.Запрос, Коннектор);
                Коннектор.Open();
                MySqlDataReader Результат = Комманда.ExecuteReader();
                Результат.Read();
                for (int ячейка = 0; ячейка < Столбцы; ячейка++)
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
    }
}
