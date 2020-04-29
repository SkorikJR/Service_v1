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
    public partial class N_Remont : Form
    {
        public N_Remont()
        {
            InitializeComponent();
        }

        public int NumOfRem;
        public string DataPriema;
        public string ID;
        public string nFoto = "0";
        public int Vnutenny;
        public int Warrantly;

        public int Vnutrenny { get; private set; }

        private void N_Remont_Load(object sender, EventArgs e)
        {
            for (int i = 0; i < Auth.ВсегоФилиалов; i++)
            {
                cbFilial.Items.Add(Auth.Filial_All[i, 2]);
            }

            //Считаем ID
            Получить_ID();
            ID = string.Concat(NumOfRem + 1);
            //Считаем ID

            //Получим дату
            DataPriema = DateTime.Parse(DateTime.Today.ToShortDateString()).ToString("yyyy'-'MM'-'dd");
            tDataPriema.Text = DataPriema;
            //Получим дату

            tPriemshik.Text = Auth.Sotrudnik[2];
        }
        //отключение от БД
        private static void Отключиться(MySqlConnection Коннектор)
        {
            Коннектор.Close();
        }
        //Фото
        private void BReNew_Click(object sender, EventArgs e)
        {
            Загрузить_фото();
        }
        private void Загрузить_фото()
        {
            OpenFileDialog opn = new OpenFileDialog
            {
                Filter = "Выберите изображение(*.jpg; *.png; *.jpeg)|*.jpg; *.png; *.jpeg"
            };
            if (opn.ShowDialog() == DialogResult.OK)
            {
                pbFoto.Image = Image.FromFile(opn.FileName);
                nFoto = "1";
            }
        }
        //Считаем ID
        private void Получить_ID()
        {
            try
            {
                MySqlConnection Коннектор = new MySqlConnection(Auth.СтрокаПодключения);
                Auth.Запрос = $"SELECT COUNT(*) FROM `remont`";
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
                MessageBox.Show("Не удалось загрузить количество клиентов, проверьте соеденение с БД!", "Err");
                throw;
            }
        }
        private void refresh()
        {

            if (Auth.ID_Клиента > 0)
            {
                bKlient.Text = $"Выбран: {Auth.Klient_vNewRem[0]}: {Auth.Klient_vNewRem[1]}";
                bKlient.Enabled = false;
                bPrint.Enabled = true;
                cbType.Enabled = true;
                tProizv.Enabled = true;
                tModel.Enabled = true;
                tSerial.Enabled = true;
                tNeispr.Enabled = true;
                tDefects.Enabled = true;
                cbWarrantly.Enabled = true;
                cbVnutrenny.Enabled = true;
                
              if (cbFilial.Items.Count != Auth.ВсегоФилиалов)
              {
                    for (int i = 0; i < Auth.ВсегоФилиалов; i++)
                    {
                        cbFilial.Items.Add(Auth.Filial_All[i, 2]);
                    }
              }
            }
        }
        private void BSave_Click(object sender, EventArgs e)
        {
            if (int.Parse(nFoto) == 1)
            {
                Auth.Запрос = $"INSERT INTO remont(ID, ID_Klient, Type, Proizv, Model, SN, DateOfPriem, Neispravnost, Komment, Filial_Pr, Filial_Now, Warrantly, Vnutreny, Prinyal ) VALUES(@ID,@ID_Klient,@Type,@Proizv,@Model,@SN,@DateOfPriem,@Neispravnost,@Komment,@Filial_Pr,@Filial_Now,@Warrantly,@Vnutreny,@Prinyal )";
                MySqlConnection Коннектор = new MySqlConnection(Auth.СтрокаПодключения);
                MySqlCommand Комманда = new MySqlCommand(Auth.Запрос, Коннектор);

                Комманда.Parameters.Add("@ID", MySqlDbType.Int32);
                Комманда.Parameters["@ID"].Value = ID;

                Комманда.Parameters.Add("@ID_Klient", MySqlDbType.Int32);
                Комманда.Parameters["@ID_Klient"].Value = Auth.ID_Клиента;

                Комманда.Parameters.Add("@Type", MySqlDbType.VarChar);
                Комманда.Parameters["@Type"].Value = cbType.Text;

                Комманда.Parameters.Add("@Proizv", MySqlDbType.VarChar);
                Комманда.Parameters["@Proizv"].Value = tProizv.Text;

                Комманда.Parameters.Add("@Model", MySqlDbType.VarChar);
                Комманда.Parameters["@Model"].Value = tModel.Text;

                Комманда.Parameters.Add("@SN", MySqlDbType.VarChar);
                Комманда.Parameters["@SN"].Value = tSerial.Text;

                Комманда.Parameters.Add("@Neispravnost", MySqlDbType.VarChar);
                Комманда.Parameters["@Neispravnost"].Value = tNeispr.Text;

                Комманда.Parameters.Add("@Komment", MySqlDbType.VarChar);
                Комманда.Parameters["@Komment"].Value = tDefects.Text;

                Комманда.Parameters.Add("@Filial_Pr", MySqlDbType.Int32);
                Комманда.Parameters["@Filial_Pr"].Value = 1 + cbFilial.Items.IndexOf(cbFilial.SelectedItem);//

                Комманда.Parameters.Add("@Filial_Now", MySqlDbType.Int32);
                Комманда.Parameters["@Filial_Now"].Value = 1 + cbFilial.Items.IndexOf(cbFilial.SelectedItem);//

                Комманда.Parameters.Add("@DateOfPriem", MySqlDbType.VarChar);
                Комманда.Parameters["@DateOfPriem"].Value = DataPriema;

                if (cbWarrantly.Checked)
                {
                    Warrantly = 1;
                }
                else
                {
                    Warrantly = 0;
                }
                Комманда.Parameters.Add("@Warrantly", MySqlDbType.Int32);
                Комманда.Parameters["@Warrantly"].Value = Warrantly;//

                if (cbVnutrenny.Checked)
                {
                    Vnutrenny = 1;
                }
                else
                {
                    Vnutrenny = 0;
                }
                Комманда.Parameters.Add("@Vnutreny", MySqlDbType.Int32);
                Комманда.Parameters["@Vnutreny"].Value = Vnutrenny;

                Комманда.Parameters.Add("@Prinyal", MySqlDbType.VarChar);
                Комманда.Parameters["@Prinyal"].Value = Auth.Sotrudnik[0];

                Коннектор.Open();
                MySqlDataReader Результат = Комманда.ExecuteReader();
                Коннектор.Close();
                MessageBox.Show("Сохранено!", "Сообщение");
                this.Hide();

                        MySqlConnection Коннектор_1 = new MySqlConnection(Auth.СтрокаПодключения);
                        MemoryStream ms = new MemoryStream();
                        pbFoto.Image.Save(ms, pbFoto.Image.RawFormat);
                        byte[] img = ms.ToArray();
                        Auth.Запрос = $"INSERT INTO remont_f(Id, Img) VALUES(@Id,@Img)";
                        MySqlCommand Комманда_1 = new MySqlCommand(Auth.Запрос, Коннектор);
                        Комманда_1.Parameters.Add("@Img", MySqlDbType.LongBlob);
                        Комманда_1.Parameters.Add("@Id", MySqlDbType.Int32);
                        Комманда_1.Parameters["@Id"].Value = ID;
                        Комманда_1.Parameters["@Img"].Value = img;
                        Коннектор.Open();
                        MySqlDataReader Результат_1 = Комманда_1.ExecuteReader();
                        Коннектор_1.Close();
            }
            else
            {
                MessageBox.Show("Добавьте Фото!","Сообщение.");
            }
        }

        private void BPrint_Click(object sender, EventArgs e)
        {
            bSave.Enabled = true;
        }

        private void BKlient_Click(object sender, EventArgs e)
        {
            Find_Kl Find;
            Find = new Find_Kl();
            Find.Show();
        }

        private void PbFoto_Click(object sender, EventArgs e)
        {

        }

        private void CbFilial_SelectedIndexChanged(object sender, EventArgs e)
        {
        }

        private void Button1_Click(object sender, EventArgs e)
        {
            refresh();
        }
    }
}
