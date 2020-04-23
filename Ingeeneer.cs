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
    public partial class Ingeeneer : Form
    {
        public Ingeeneer()
        {
            InitializeComponent();
        }
        public int Столбцы = 7;
        public int Num = 0;
        public string ID_Master = ДанныеИнженера.Инженер[0];
        private void Ingeeneer_Load(object sender, EventArgs e)
        {
            Num = NRem(ДанныеИнженера.Инженер[0]);
            if (Num > 0)
            {
                string[] Tmp_IDRem = IDRem(ДанныеИнженера.Инженер[0], Num);
                string[,] ремонты = Ремонты(Tmp_IDRem, Num, ID_Master);
                int L = ремонты.Length;
                int H = ремонты.Length / 7;
                L /= H;
                for (int i = 0; i < H; i++)
                {
                    string[] Tmp = new string[L];
                    for (int j = 0; j < L; j++)
                    {
                        Tmp[j] = ремонты[i, j];
                    }
                    ПодготовкаDataGrid(Tmp, dataGridView1);
                }
            }
            else if (Num <= 0)
            {
                MessageBox.Show("У данного инженера нет ремонтов в работе!");
            }
            this.Text += $" ИД = {ДанныеИнженера.Инженер[0]}";
            tName.Text = $"{ДанныеИнженера.Инженер[1]}";
            tFam.Text = $"{ДанныеИнженера.Инженер[2]}";
            tOtch.Text = $"{ДанныеИнженера.Инженер[3]}";
            tNoTel.Text = $"{ДанныеИнженера.Инженер[4]}";
            tSpec.Text = $"{ДанныеИнженера.Инженер[5]}";
            tDatPriem.Text = $"{ДанныеИнженера.Инженер[6]}";
            

        }

        private string[] IDRem(string ID_Master,int Num)
        {
            string[] Ids = new string[Num];
            Auth.Запрос = $"SELECT `ID` FROM `remont` WHERE `ID_Master`={ID_Master} AND `Vidano` = 0";
            MySqlConnection Коннектор = new MySqlConnection(Auth.СтрокаПодключения);
            Коннектор.Open();
            MySqlCommand Комманда = new MySqlCommand(Auth.Запрос, Коннектор);
            MySqlDataReader Результат = Комманда.ExecuteReader();
            int Clc = 0;
            while (Результат.Read())
            {
                Результат[0].ToString();
                string Ячейка = Результат[0].ToString();
                    Ids[Clc] = Ячейка;
                Clc = ++Clc;
            }
            Clc = 0;
            Отключиться(Коннектор);
            return Ids;
        }//Получаем Ид ремонтов(Не выданных)

        private string[,] Ремонты(string[] Ids, int Num, string ID_Master)
        {
            string[,] ремонты = new string[Num, Столбцы];
            MySqlConnection Коннектор = new MySqlConnection(Auth.СтрокаПодключения);
            for (int i = 0; i < Num; i++)
            {
                Auth.Запрос = $"SELECT `ID`,`Type`,`Model`,`SN`,`Neispravnost`,`DateOfPriem`,`RezDiag` FROM `remont` WHERE `ID`={Ids[i]}";
                MySqlCommand Комманда = new MySqlCommand(Auth.Запрос, Коннектор);
                Коннектор.Open();
                MySqlDataReader Результат = Комманда.ExecuteReader();
                int Clc = 0;
                Результат.Read();
                for (; Clc < 7;)
                {
                    Результат[0].ToString();
                    string Ячейка = Результат[Clc].ToString();
                    ремонты[i, Clc] = Ячейка;
                    Clc = ++Clc;
                } 
                Clc = 0;
                Отключиться(Коннектор);
            }

            return ремонты;
        }//Получаем список ремонтов(Не выданных)

        private int NRem(string ID_Master)
        {
            string Tmp = "";
            MySqlConnection Коннектор = new MySqlConnection(Auth.СтрокаПодключения);
            Auth.Запрос = $"SELECT COUNT(*) FROM `remont` WHERE `ID_Master`={ID_Master}";
            MySqlCommand Комманда = new MySqlCommand(Auth.Запрос, Коннектор);
            Коннектор.Open();
            MySqlDataReader Результат = Комманда.ExecuteReader();
            while (Результат.Read())
            {
                Результат[0].ToString();
                Tmp = Результат[0].ToString();
            }
            Отключиться(Коннектор);
            int Num = int.Parse(Tmp);
            return Num;
        }//получаем количество ремонтов(Не выданных)
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
    }
}
