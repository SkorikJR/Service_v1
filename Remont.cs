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
    public partial class Remont : Form
    {
        public Remont()
        {
            InitializeComponent();
        }
        private void Remont_Load(object sender, EventArgs e)
        {
            this.Text += ДанныеДляОтбора.Ремонт[0];
            //try//Загрузка Фото
            //{
                //Подгрузка_фото(int.Parse(ДанныеДляОтбора.Ремонт[0]));
            //}
            //catch (Exception)
            //{
                //MessageBox.Show("Ошибка загрузки Фото!", "Err");

                //throw;
            //}
        }
        private void Удалить_фото(int Id)
        {
            MessageBox.Show("Изображение будет удалено из базы без возможности восстановления, и загружено новое.", "Внимание!");
            MySqlConnection Коннектор = new MySqlConnection(Auth.СтрокаПодключения);
            Auth.Запрос = $"DELETE FROM `remont_f` WHERE `Id`={Id}";
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
                    Auth.Запрос = $"INSERT INTO remont_f(Id, Img) VALUES(@Id,@Img)";
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
            String qry = "SELECT * FROM remont_f WHERE Id = '" + Id + "'";
            cmd = new MySqlCommand(qry, Коннектор);
            da = new MySqlDataAdapter(cmd);
            DataTable table = new DataTable();
            da.Fill(table);
            byte[] img = (byte[])table.Rows[0][1];
            MemoryStream ms = new MemoryStream(img);
            pbFoto.Image = Image.FromStream(ms);
            da.Dispose();
        }
        //Участок, отвечающий за подгрузку Фото ремонта.
    }
}
