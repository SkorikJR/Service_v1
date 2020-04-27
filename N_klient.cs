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
    public partial class N_klient : Form
    {
        public N_klient()
        {
            InitializeComponent();
        }
        public int NumOfKl = 0;
        private void N_klient_Load(object sender, EventArgs e)
        {
            //Считаем ID
            try
            {
                MySqlConnection Коннектор = new MySqlConnection(Auth.СтрокаПодключения);
                Auth.Запрос = $"SELECT COUNT(*) FROM `klient`";
                Коннектор.Open();
                MySqlCommand Комманда = new MySqlCommand(Auth.Запрос, Коннектор);
                MySqlDataReader Результат = Комманда.ExecuteReader();
                while (Результат.Read())
                {
                    NumOfKl = int.Parse(Результат[0].ToString());
                }
                Отключиться(Коннектор);
            }
            catch (Exception)
            {
                MessageBox.Show("Не удалось загрузить количество клиентов, проверьте соеденение с БД!", "Err");
                throw;
            }
            tID.Text = string.Concat(NumOfKl + 1);
            //Считаем ID

            //Получим дату
            tDateReg.Text = DateTime.Today.ToShortDateString();
            //Получим дату



        }

        private static void Отключиться(MySqlConnection Коннектор)
        {
            Коннектор.Close();
        }//отключение от БД


        private void Button10_Click(object sender, EventArgs e)
        {
            if (tName.Text == ""|tFam.Text == ""|tOtch.Text==""|tNoTel.Text==""|tHar.Text=="")
            {
                MessageBox.Show("Обнаружены пустые поля, так не пойдет!", "Err");
            }
            else
            {
                try
                {
                    Auth.Запрос = $"INSERT INTO klient(Id, Name, Family, Otchestvo, Phone, Harakteristia, Date, Skidka) VALUES(@ID,@Name,@Family,@Otchestvo,@Phone,@Harakteristia,@Date,@Skidka)";
                    MySqlConnection Коннектор = new MySqlConnection(Auth.СтрокаПодключения);
                    //Auth.Запрос = $"INSERT INTO remont_f(Id, Img) VALUES(@Id,@Img)";
                    MySqlCommand Комманда = new MySqlCommand(Auth.Запрос, Коннектор);

                    Комманда.Parameters.Add("@ID", MySqlDbType.Int32);
                    Комманда.Parameters["@ID"].Value = tID.Text;

                    Комманда.Parameters.Add("@Name", MySqlDbType.VarChar);
                    Комманда.Parameters["@Name"].Value = tName.Text;

                    Комманда.Parameters.Add("@Family", MySqlDbType.VarChar);
                    Комманда.Parameters["@Family"].Value = tFam.Text;

                    Комманда.Parameters.Add("@Otchestvo", MySqlDbType.VarChar);
                    Комманда.Parameters["@Otchestvo"].Value = tOtch.Text;

                    Комманда.Parameters.Add("@Phone", MySqlDbType.VarChar);
                    Комманда.Parameters["@Phone"].Value = tNoTel.Text;

                    Комманда.Parameters.Add("@Harakteristia", MySqlDbType.VarChar);
                    Комманда.Parameters["@Harakteristia"].Value = tHar.Text;

                    Комманда.Parameters.Add("@Date", MySqlDbType.VarChar);
                    Комманда.Parameters["@Date"].Value = tDateReg.Text;

                    Комманда.Parameters.Add("@Skidka", MySqlDbType.Int32);
                    Комманда.Parameters["@Skidka"].Value = tskid.Text;

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
    }
}
