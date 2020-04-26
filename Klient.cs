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
    public partial class Klient : Form
    {
        public Klient()
        {
            InitializeComponent();
        }

        private void SplitContainer1_Panel2_Paint(object sender, PaintEventArgs e)
        {

        }
        public int Столбцы = 7;
        public int NumOfRem = 0;
        public string[,] Rems;
        public string[] ID_Rem;
        private void Klient_Load(object sender, EventArgs e)
        {
            this.Text += $"{Auth.Klient[0]}";
            tName.Text = Auth.Klient[1];
            tFam.Text = Auth.Klient[2];
            tOtch.Text = Auth.Klient[3];
            tNoTel.Text = Auth.Klient[4];
            tHar.Text = Auth.Klient[5];
            tskid.Text = Auth.Klient[6];
            tDateReg.Text = Auth.Klient[7];
            try
            {
                MySqlConnection Коннектор = new MySqlConnection(Auth.СтрокаПодключения);
                Auth.Запрос = $"SELECT COUNT(*) FROM `remont` WHERE `ID_Klient`=\"{Auth.Klient[0]}\"";
                Коннектор.Open();
                MySqlCommand Комманда = new MySqlCommand(Auth.Запрос, Коннектор);
                MySqlDataReader Результат = Комманда.ExecuteReader();
                while (Результат.Read())
                {
                    NumOfRem = int.Parse(Результат[0].ToString());
                }
                Отключиться(Коннектор);
            }
            catch (Exception)
            {
                MessageBox.Show("Не удалось загрузить количество ремонтов, проверьте соеденение с БД!", "Err");
                throw;
            }
            if (NumOfRem > 0)
            {
                ID_Rem = new string[this.NumOfRem];
                Auth.Запрос = $"SELECT `ID` FROM `remont` WHERE `ID_Klient`={Auth.Klient[0]}";
                MySqlConnection Коннектор = new MySqlConnection(Auth.СтрокаПодключения);
                Коннектор.Open();
                MySqlCommand Комманда = new MySqlCommand(Auth.Запрос, Коннектор);
                MySqlDataReader Результат = Комманда.ExecuteReader();
                int Clc = 0;
                while (Результат.Read())
                {
                    Результат[0].ToString();
                    string Ячейка = Результат[0].ToString();
                    ID_Rem[Clc] = Ячейка;
                    Clc = ++Clc;
                }
                Отключиться(Коннектор);
                Ремонты();
                int L = Rems.Length;
                int H = Rems.Length / Столбцы;
                L /= H;
                for (int i = 0; i < H; i++)
                {
                    string[] Tmp = new string[L];
                    for (int j = 0; j < L; j++)
                    {
                        Tmp[j] = Rems[i, j];
                    }
                    ПодготовкаDataGrid(Tmp, dataGridView1);
                }
            }
            else if (NumOfRem <= 0)
            {
                MessageBox.Show("У данного клиента еще нет ремонтов!");
            }
        }
        private void Ремонты()
        {
            Rems = new string[NumOfRem, Столбцы];
            MySqlConnection Коннектор = new MySqlConnection(Auth.СтрокаПодключения);
            for (int i = 0; i < NumOfRem; i++)
            {
                Auth.Запрос = $"SELECT `ID`,`Type`,`Model`,`SN`,`Neispravnost`,`DateOfPriem`,`Vidano` FROM `remont` WHERE `ID_Klient`=\"{Auth.Klient[0]}\" AND `ID`=\"{ID_Rem[i]}\"";
                MySqlCommand Комманда = new MySqlCommand(Auth.Запрос, Коннектор);
                Коннектор.Open();
                MySqlDataReader Результат = Комманда.ExecuteReader();
                Результат.Read();
                for (int Clc = 0; Clc < 7; Clc++)
                {
                    string Ячейка = Результат[Clc].ToString();
                    Rems[i, Clc] = Ячейка;
                    if (Clc == 6)
                    {
                        if (Rems[i, Clc] == "0")
                        {
                            Rems[i, Clc] = "false";
                        }
                        else if (Rems[i, Clc] == "1")
                        {
                            Rems[i, Clc] = "true";
                        }
                    }//Подмена "На лету"
                }
                Отключиться(Коннектор);
            }
        }//Получаем список ремонтов

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
