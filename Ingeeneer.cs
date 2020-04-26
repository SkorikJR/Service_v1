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
    public partial class Ingeeneer : Form
    {
        public Ingeeneer()
        {
            InitializeComponent();
        }
        public int Столбцы = 8;
        public int NumOfRem = 0;
        public string ID_Master = ДанныеИнженера.Инженер[0];
        public int Foto;
        private void Ingeeneer_Load(object sender, EventArgs e)
        {
            MySqlConnection Коннектор = new MySqlConnection(Auth.СтрокаПодключения);
            Auth.Запрос = $"SELECT COUNT(*) FROM `remont` WHERE `ID_Master`={ID_Master} AND `Vidano` = 0";
            Коннектор.Open();
            MySqlCommand Комманда = new MySqlCommand(Auth.Запрос, Коннектор);
            MySqlDataReader Результат = Комманда.ExecuteReader();
            while (Результат.Read())
            {
                NumOfRem = int.Parse(Результат[0].ToString());
            }
            Отключиться(Коннектор);
            //Num = NRem(ДанныеИнженера.Инженер[0]);//?????????????????????????????
            if (NumOfRem > 0)
            {
                string[] Tmp_IDRem = IDRem(ДанныеИнженера.Инженер[0]);
                string[,] ремонты = Ремонты(Tmp_IDRem, NumOfRem);
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
            else if (NumOfRem <= 0)
            {
                MessageBox.Show("У данного инженера нет ремонтов в работе!");
            }
            try
            {
                Auth.Запрос = $"SELECT COUNT(*) FROM `ingeeneer_f` WHERE `Id` = {ID_Master}";
                MySqlCommand Комманда_f = new MySqlCommand(Auth.Запрос, Коннектор);
                Коннектор.Open();
                MySqlDataReader Результат_f = Комманда_f.ExecuteReader();
                while (Результат_f.Read())
                {
                    Foto = int.Parse(Результат_f[0].ToString());
                }
                Отключиться(Коннектор);
            }
            catch (Exception)
            {
                MessageBox.Show("Не удалось проверить наличие фото, проверьте подключение к БД!", "Err");
                throw;
            }
            if (Foto > 0)
            {
                Подгрузка_фото(int.Parse(ДанныеИнженера.Инженер[0]));
            }
            else
            {
                MessageBox.Show("Отсутствует фото! Загрузите фото инженера.");
            }
            this.Text += $" ИД = {ДанныеИнженера.Инженер[0]}";
            tName.Text = $"{ДанныеИнженера.Инженер[1]}";
            tFam.Text = $"{ДанныеИнженера.Инженер[2]}";
            tOtch.Text = $"{ДанныеИнженера.Инженер[3]}";
            tNoTel.Text = $"{ДанныеИнженера.Инженер[4]}";
            tSpec.Text = $"{ДанныеИнженера.Инженер[5]}";
            tDatPriem.Text = $"{ДанныеИнженера.Инженер[6]}";
            tNote.Text= $"{ДанныеИнженера.Инженер[7]}";
        }

        private string[] IDRem(string ID_Master)
        {
            string[] Ids = new string[this.NumOfRem];
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
            Отключиться(Коннектор);
            return Ids;
        }//Получаем Ид ремонтов(Не выданных)

        private string[,] Ремонты(string[] Ids, int Num)
        {
            string[,] ремонты = new string[Num, Столбцы];
            MySqlConnection Коннектор = new MySqlConnection(Auth.СтрокаПодключения);
            for (int i = 0; i < Num; i++)
            {
                Auth.Запрос = $"SELECT `ID`,`Type`,`Model`,`SN`,`Neispravnost`,`DateOfPriem`,`RezDiag` FROM `remont` WHERE `ID`=\"{Ids[i]}\"";
                MySqlCommand Комманда = new MySqlCommand(Auth.Запрос, Коннектор);
                Коннектор.Open();
                MySqlDataReader Результат = Комманда.ExecuteReader();
                Результат.Read();
                for (int Clc = 0; Clc < 7;Clc++)
                {
                    string Ячейка = Результат[Clc].ToString();
                    ремонты[i, Clc] = Ячейка;
                } 
                Отключиться(Коннектор);
            }

            return ремонты;
        }//Получаем список ремонтов(Не выданных)
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

        //Обновление фото
        private void BReNew_Click(object sender, EventArgs e)
        {
            try
            {
                Удалить_фото(int.Parse(ДанныеИнженера.Инженер[0]));
                Загрузить_фото(int.Parse(ДанныеИнженера.Инженер[0]));
            }
            catch (Exception)
            {
                Загрузить_фото(int.Parse(ДанныеИнженера.Инженер[0]));
                MessageBox.Show("Возможно вы выбрали слишком большое изображение!","Ошибка");
                throw;
            }
            
        }
        private void Удалить_фото(int Id)
        {
            MessageBox.Show("Изображение будет удалено из базы без возможности восстановления, и загружено новое.", "Внимание!");
            MySqlConnection Коннектор = new MySqlConnection(Auth.СтрокаПодключения);
            Auth.Запрос = $"DELETE FROM `ingeeneer_f` WHERE `Id`={Id}";
            MySqlCommand Комманда = new MySqlCommand(Auth.Запрос, Коннектор);
            Коннектор.Open();
            Комманда.ExecuteReader();
        }
        private void Загрузить_фото(int Id)
        {
            //ЗАГРУЗКА ИзО
            OpenFileDialog opn = new OpenFileDialog
            {
                Filter = "Выберите изображение(*.jpg; *.png; *.jpeg)|*.jpg; *.png; *.jpeg"
            };
            if (opn.ShowDialog() == DialogResult.OK)
            {
                pbFoto.Image = Image.FromFile(opn.FileName);
                try
                {
                    MySqlConnection Коннектор = new MySqlConnection(Auth.СтрокаПодключения);
                    MemoryStream ms = new MemoryStream();
                    pbFoto.Image.Save(ms, pbFoto.Image.RawFormat);
                    byte[] img = ms.ToArray();
                    Auth.Запрос = $"INSERT INTO ingeeneer_F(Id, Img) VALUES(@Id,@Img)";
                    MySqlCommand Комманда = new MySqlCommand(Auth.Запрос, Коннектор);
                    Комманда.Parameters.Add("@Img", MySqlDbType.LongBlob);
                    Комманда.Parameters.Add("@Id", MySqlDbType.Int32);
                    Комманда.Parameters["@Id"].Value = Id;
                    Комманда.Parameters["@Img"].Value = img;
                    Коннектор.Open();
                    MySqlDataReader Результат = Комманда.ExecuteReader();
                    Коннектор.Close();
                }
                catch (Exception)
                {

                    throw;
                }
            }
        }
        private void Подгрузка_фото(int Id)
        { 
                MySqlConnection Коннектор = new MySqlConnection(Auth.СтрокаПодключения);
                MySqlCommand cmd; MySqlDataAdapter da;
                String qry = "SELECT * FROM ingeeneer_F WHERE Id = '" + Id + "'";
                cmd = new MySqlCommand(qry, Коннектор);
                da = new MySqlDataAdapter(cmd);
                DataTable table = new DataTable();
                da.Fill(table);
                byte[] img = (byte[])table.Rows[0][1];
                MemoryStream ms = new MemoryStream(img);
                pbFoto.Image = Image.FromStream(ms);
                da.Dispose();
        }

        private void SplitContainer1_Panel1_Paint(object sender, PaintEventArgs e)
        {

        }
    }
}
