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
    public partial class All_Ing : Form
    {
        public All_Ing()
        {
            InitializeComponent();
        }

        public int Столбцы = 10;

        private void All_Ing_Load(object sender, EventArgs e)
        {
            int строка = 1;
            Auth.Sotrudnik_All = new string[Auth.ВсегоСотрудников, Столбцы];
            MySqlConnection Коннектор = new MySqlConnection(Auth.СтрокаПодключения);// Обьявляем cBase как MySqlConnection(переменная строки подключения)
            for (int i = 0; i < Auth.ВсегоРемонтов; i++)
            {
                Auth.Запрос = $"SELECT * FROM `ingeeneer` WHERE `ID`={строка}";
                MySqlCommand Комманда = new MySqlCommand(Auth.Запрос, Коннектор);
                Коннектор.Open();
                MySqlDataReader Результат = Комманда.ExecuteReader();
                Результат.Read();
                for (int ячейка = 0; ячейка < Столбцы; ячейка++)
                {
                    Auth.Sotrudnik_All[i, ячейка] = Результат[ячейка].ToString();
                    if (ячейка == 9)
                    {
                        int temp = int.Parse(Auth.Sotrudnik_All[i, ячейка]) - 1;
                        Auth.Sotrudnik_All[i, ячейка] = Auth.Filial_All[temp, 2];
                    }//попытка автозамены "На лету"
                }
                строка = ++строка;
                Отключиться(Коннектор);
            }//Выгруз из базы


            int L = Auth.Sotrudnik_All.Length;
            int H = Auth.Sotrudnik_All.Length / Столбцы;
            L /= H;
            for (int i = 0; i < H; i++)
            {
                string[] Tmp = new string[L];
                for (int j = 0; j < L; j++)
                {
                    Tmp[j] = Auth.Sotrudnik_All[i, j];
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
            Auth.Запрос = $"SELECT * FROM `ingeeneer` WHERE `ID`={Выбрано}";
            MySqlConnection Коннектор = new MySqlConnection(Auth.СтрокаПодключения);
            MySqlCommand Комманда = new MySqlCommand(Auth.Запрос, Коннектор);
            Коннектор.Open();
            MySqlDataReader Результат = Комманда.ExecuteReader();
            Результат.Read();
            for (int ячейка = 0; ячейка < Столбцы; ячейка++)
            {
                    Результат[ячейка].ToString();
                    ДанныеИнженера.Инженер[ячейка] = Результат[ячейка].ToString();
            }
            Отключиться(Коннектор);
                Ingeeneer O_ing;
                O_ing = new Ingeeneer();
                O_ing.Show();
            }
        }
    }
}
