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
    public partial class N_ingeeneer : Form
    {
        public N_ingeeneer()
        {
            InitializeComponent();
        }
        public int NumOfIng = 0;
        int Photo = 0;
        private void N_ingeeneer_Load(object sender, EventArgs e)
        {
            dateTimePicker1.Format = DateTimePickerFormat.Short;
            //Считаем ID
            try
            {
                MySqlConnection Коннектор = new MySqlConnection(Auth.СтрокаПодключения);
                Auth.Запрос = $"SELECT COUNT(*) FROM `ingeeneer`";
                Коннектор.Open();
                MySqlCommand Комманда = new MySqlCommand(Auth.Запрос, Коннектор);
                MySqlDataReader Результат = Комманда.ExecuteReader();
                while (Результат.Read())
                {
                    NumOfIng = int.Parse(Результат[0].ToString());
                }
                Отключиться(Коннектор);
            }
            catch (Exception)
            {
                MessageBox.Show("Не удалось загрузить количество сотрудников, проверьте соеденение с БД!", "Err");
                throw;
            }
            tID.Text = string.Concat(NumOfIng + 1);
            //Считаем ID

            //Заполнение бокса Филиалов
            for (int i = 0; i < Auth.ВсегоФилиалов; i++)
            {
                comboBox1.Items.Add(Auth.Filial_All[i,2]);
            }
            //Заполнение бокса Филиалов
        }
        private static void Отключиться(MySqlConnection Коннектор)
        {
            Коннектор.Close();
        }//отключение от БД

        private void BReNew_Click(object sender, EventArgs e)
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
                    Комманда.Parameters["@Id"].Value = NumOfIng + 1;
                    Комманда.Parameters["@Img"].Value = img;
                    Коннектор.Open();
                    MySqlDataReader Результат = Комманда.ExecuteReader();
                    Коннектор.Close();
                }
                catch (Exception)
                {
                    MessageBox.Show("Не удалось загрузить фото!", "Err");
                    throw;
                }
            }
            Photo = 1;
        }

        private void Button1_Click(object sender, EventArgs e)
        {
            if (tName.Text == "" | tFam.Text == "" | tOtch.Text == "" | tNoTel.Text == "" | tSpec.Text == "" | tNote.Text == "" | Photo == 0)
            {
                MessageBox.Show("Обнаружены пустые поля, так не пойдет! Добавьте фото, если этого не сделали!", "Err");
            }
            else
            {
                try
                {
                    Auth.Запрос = $"INSERT INTO ingeeneer(ID, Name, Familiya, Otchestvo, NomerTel, Specs, DataPriema, Notes, Dolzgnost, IDf) VALUES(@ID,@Name,@Familiya,@Otchestvo,@NomerTel,@Specs,@DataPriema,@Notes,@Dolzgnost,@IDf)";
                    MySqlConnection Коннектор = new MySqlConnection(Auth.СтрокаПодключения);
                    //Auth.Запрос = $"INSERT INTO remont_f(Id, Img) VALUES(@Id,@Img)";
                    MySqlCommand Комманда = new MySqlCommand(Auth.Запрос, Коннектор);

                    Комманда.Parameters.Add("@ID", MySqlDbType.Int32);
                    Комманда.Parameters["@ID"].Value = tID.Text;

                    Комманда.Parameters.Add("@Name", MySqlDbType.VarChar);
                    Комманда.Parameters["@Name"].Value = tName.Text;

                    Комманда.Parameters.Add("@Familiya", MySqlDbType.VarChar);
                    Комманда.Parameters["@Familiya"].Value = tFam.Text;

                    Комманда.Parameters.Add("@Otchestvo", MySqlDbType.VarChar);
                    Комманда.Parameters["@Otchestvo"].Value = tOtch.Text;

                    Комманда.Parameters.Add("@Specs", MySqlDbType.VarChar);
                    Комманда.Parameters["@Specs"].Value = tSpec.Text;

                    Комманда.Parameters.Add("@DataPriema", MySqlDbType.VarChar);
                    Комманда.Parameters["@DataPriema"].Value = DateTime.Parse(dateTimePicker1.Text).ToString("yyyy'-'MM'-'dd");

                    Комманда.Parameters.Add("@NomerTel", MySqlDbType.VarChar);
                    Комманда.Parameters["@NomerTel"].Value = tNoTel.Text;

                    Комманда.Parameters.Add("@Notes", MySqlDbType.VarChar);
                    Комманда.Parameters["@Notes"].Value = tNote.Text;

                    Комманда.Parameters.Add("@Dolzgnost", MySqlDbType.String);
                    Комманда.Parameters["@Dolzgnost"].Value = textBox1.Text;//comboBox2.Text;//

                    Комманда.Parameters.Add("@IDf", MySqlDbType.Int32);
                    Комманда.Parameters["@IDf"].Value = 1 + comboBox1.Items.IndexOf(comboBox1.SelectedItem);//

                    Коннектор.Open();
                    MySqlDataReader Результат = Комманда.ExecuteReader();
                    Коннектор.Close();
                    MessageBox.Show("Сохранено!", "Сообщение");
                    this.Hide();
                }
                catch (Exception)
                {
                    MessageBox.Show("Не удалось сохранить данные! Проверьте соеденение с БД.", "Err");
                    throw;
                }
            }
        }

        private void TDatPriem_Click(object sender, EventArgs e)
        {
            
        }
    }
}