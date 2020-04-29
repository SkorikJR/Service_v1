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
        public int Столбцы = 7;
        public int NumOfRem = 0;
        public string[,] Rems;
        public string[] ID_Rem;
        public int Foto;
        private void Remont_Load(object sender, EventArgs e)
        {
            ////Подгрузка переменных в поля.
            this.Text += ДанныеДляОтбора.Ремонт[0];
            tType.Text = ДанныеДляОтбора.Ремонт[2];
            tProizv.Text = ДанныеДляОтбора.Ремонт[11];
            tModel.Text = ДанныеДляОтбора.Ремонт[3];
            tSerial.Text = ДанныеДляОтбора.Ремонт[4];
            tNeispr.Text = ДанныеДляОтбора.Ремонт[5];
            tDataPriema.Text = DateTime.Parse(ДанныеДляОтбора.Ремонт[6]).ToString("dd'.'MM'.'yyyy");
            tCommit.Text = ДанныеДляОтбора.Ремонт[24];
            if (int.Parse(ДанныеДляОтбора.Ремонт[7]) == 1)
            {
                cbDiagn.Checked = true;
                tRezDiagn.Text = ДанныеДляОтбора.Ремонт[8];
                tKto_Diagn.Text = Auth.Sotrudnik_All[int.Parse(ДанныеДляОтбора.Ремонт[28]) - 1, 2];
                textBox2.Text = DateTime.Parse(ДанныеДляОтбора.Ремонт[27]).ToString("dd'.'MM'.'yyyy");
            }
            if (int.Parse(ДанныеДляОтбора.Ремонт[7]) == 0)
            {
                cbDiagn.Checked = false;
            }
            
            if (int.Parse(ДанныеДляОтбора.Ремонт[13]) == 1)
            {
                cbReady.Checked = true;
                tMaster.Text = Auth.Sotrudnik_All[int.Parse(ДанныеДляОтбора.Ремонт[12]) - 1, 2];
                tRemEnd.Text = DateTime.Parse(ДанныеДляОтбора.Ремонт[14]).ToString("dd'.'MM'.'yyyy");
                
            }
            if (int.Parse(ДанныеДляОтбора.Ремонт[13]) == 0)
            {
                cbReady.Checked = false;
            }
            
            if (int.Parse(ДанныеДляОтбора.Ремонт[15]) == 1)
            {
                cbUvedomlen.Checked = true;
            }
            if (int.Parse(ДанныеДляОтбора.Ремонт[15]) == 0)
            {
                cbUvedomlen.Checked = false;
            }

            if (int.Parse(ДанныеДляОтбора.Ремонт[16]) == 1)
            {
                cbProblem.Checked = true;
                tProblem.Text = ДанныеДляОтбора.Ремонт[17];
            }
            if (int.Parse(ДанныеДляОтбора.Ремонт[16]) == 0)
            {
                cbProblem.Checked = false;
            }
            
            if (int.Parse(ДанныеДляОтбора.Ремонт[18]) == 1)
            {
                cbVidano.Checked = true;
                tDataVid.Text = DateTime.Parse(ДанныеДляОтбора.Ремонт[19]).ToString("dd'.'MM'.'yyyy");
                this.Text += $" (Выдано {DateTime.Parse(ДанныеДляОтбора.Ремонт[19]).ToString("dd'.'MM'.'yyyy")} Сотрудником: {Auth.Sotrudnik_All[int.Parse(ДанныеДляОтбора.Ремонт[29]) - 1, 2]})";
            }//Чек бокс выдачи + приставка в Caption!
            if (int.Parse(ДанныеДляОтбора.Ремонт[18]) == 0)
            {
                cbVidano.Checked = false;
            }
            
            tPriemshik.Text = Auth.Sotrudnik_All[int.Parse(ДанныеДляОтбора.Ремонт[22]) - 1, 2];
            if (int.Parse(ДанныеДляОтбора.Ремонт[21]) == 1)
            {
                cbWarrantly.Checked = true;
            }
            if (int.Parse(ДанныеДляОтбора.Ремонт[21]) == 0)
            {
                cbWarrantly.Checked = false;
            }
            if (int.Parse(ДанныеДляОтбора.Ремонт[23]) == 1)
            {
                cbVnutrenny.Checked = true;
            }
            if (int.Parse(ДанныеДляОтбора.Ремонт[23]) == 0)
            {
                cbVnutrenny.Checked = false;
            }
            
           
            ////
            ////
            tName.Text = Auth.Klient_All[int.Parse(ДанныеДляОтбора.Ремонт[1])-1, 1];
            tFam.Text = Auth.Klient_All[int.Parse(ДанныеДляОтбора.Ремонт[1]) - 1, 2];
            tOtch.Text = Auth.Klient_All[int.Parse(ДанныеДляОтбора.Ремонт[1]) - 1, 3];
            tNoTel.Text = Auth.Klient_All[int.Parse(ДанныеДляОтбора.Ремонт[1]) - 1, 4];
            tHar.Text = Auth.Klient_All[int.Parse(ДанныеДляОтбора.Ремонт[1]) - 1, 5];
            tskid.Text = Auth.Klient_All[int.Parse(ДанныеДляОтбора.Ремонт[1]) - 1, 6];
            tDateReg.Text = DateTime.Parse(Auth.Klient_All[int.Parse(ДанныеДляОтбора.Ремонт[1]) - 1, 7]).ToString("dd'.'MM'.'yyyy");
            ////
            ////
            tFilial_Priema.Text = Auth.Filial_All[int.Parse(ДанныеДляОтбора.Ремонт[25]) - 1, 2];
            tNowPlace.Text = Auth.Filial_All[int.Parse(ДанныеДляОтбора.Ремонт[26]) - 1, 2];
            ////Подгрузка переменных в поля.
            try
            {
                //Загрузка Фото
                MySqlConnection Коннектор = new MySqlConnection(Auth.СтрокаПодключения);
                    Auth.Запрос = $"SELECT COUNT(*) FROM `remont_f` WHERE `Id` = {ДанныеДляОтбора.Ремонт[0]}";
                    MySqlCommand Комманда = new MySqlCommand(Auth.Запрос, Коннектор);
                    Коннектор.Open();
                    MySqlDataReader Результат = Комманда.ExecuteReader();
                    while (Результат.Read())
                    {
                        Foto = int.Parse(Результат[0].ToString());
                    }
                    Отключиться(Коннектор);
            }
            catch (Exception)
            {
                MessageBox.Show("Не удалось проверить наличие фото, проверьте подключение к БД!", "Err");
                throw;
            }

            try
            {
            Подгрузка_фото(int.Parse(ДанныеДляОтбора.Ремонт[0]));
            }
            catch (Exception)
            {
            MessageBox.Show("Ошибка загрузки Фото!", "Err");
            throw;
            }
            Ремонты();
            Привелегии();
            ЗагрКомбо();
        }

        private static void Отключиться(MySqlConnection Коннектор)
        {
            Коннектор.Close();
        }//отключение от БД

        //Загрузка Фото
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
            //ЗАГРУЗКА ИзО
            if (Foto > 0)
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
            else
            {
                MessageBox.Show("Фото не загружено! Загрузите фото устройства.");
            }
            
        }

        private void BReNew_Click(object sender, EventArgs e)
        {
            try
            {
                Удалить_фото(int.Parse(ДанныеДляОтбора.Ремонт[0]));
            }
            catch (Exception)
            {

                throw;
            } 
            Загрузить_фото(int.Parse(ДанныеДляОтбора.Ремонт[0]));
        }
        //Участок, отвечающий за подгрузку Фото ремонта.
        private void Привелегии()
        {
            if (Auth.AcMode == "God")
            {
                Перемещение();
                ИзменениеКлиента();
                Выдача();
                Списание();
                ВРемонт();
            }
            else if (Auth.AcMode == "Admin")//Админ
            {
                Перемещение();
                ИзменениеКлиента();
                Выдача();
                Списание();
                ВРемонт();
            }
            else if (Auth.AcMode == "Ingeen")//Инженер
            {
                ВРемонт();
            }
            else if (Auth.AcMode == "Base")//Приемщик(Продавец)
            {
                Перемещение();
                Выдача();
            }
            void Перемещение()
            {
                //Перемещение
                comboBox1.Enabled = true;
                comboBox2.Enabled = true;
                button7.Enabled = true;
                button6.Enabled = true;
                button5.Enabled = true;
                //Перемещение
            }
            void ИзменениеКлиента()
            {
                if (Auth.AcMode == "God")
                {
                    ИзменениеКл();
                    tskid.Enabled = true;
                }
                else if (Auth.AcMode == "Admin")//Админ
                {
                    ИзменениеКл();
                    tskid.Enabled = true;
                }
                else if (Auth.AcMode == "Ingeen")//Инженер
                {
                }
                else if (Auth.AcMode == "Base")//Приемщик(Продавец)
                {
                    ИзменениеКл();
                }
                void ИзменениеКл()
                {
                    bChangeKl.Enabled = true;
                    tName.Enabled = true;
                    tFam.Enabled = true;
                    tOtch.Enabled = true;
                    tNoTel.Enabled = true;
                    tHar.Enabled = true;
                    tDateReg.Enabled = true;
                }
            }
            void Выдача()
            {
                bEndRem.Enabled = true;
                bPrint.Enabled = true;
                button9.Enabled = true;
                bReNew.Enabled = true;
            }
            void ВРемонт()
            {
                button4.Enabled = true;
                button3.Enabled = true;
                button2.Enabled = true;
                button1.Enabled = true;
            }
            void Списание()
            {
                button8.Enabled = true;
            }

        }//ТРЕБУЕТ ДОРАБОТКИ
        private void ЗагрКомбо()
        {
            for (int i = 0; i < ДанныеДляОтбора.КолФилиалов; i++)
            {
                comboBox1.Items.Add($"{Auth.Filial_All[i, 2]}");
            }
            comboBox2.Items.Add("На диагностику");
            comboBox2.Items.Add("В ремонт");
            comboBox2.Items.Add("На выдачу");
            comboBox2.Items.Add("Списание");
            listCompl.Items.Insert(0,"Дисплей");
            //checkedListBox1.Items.Insert(0, "Copenhagen");
        }// Подгрузка филиалов в "Перемещение"
        private void Ремонты()
        {
           
            MySqlConnection Коннектор = new MySqlConnection(Auth.СтрокаПодключения);
            try
            {
                Auth.Запрос = $"SELECT COUNT(*) FROM `remont` WHERE `ID_Klient`=\"{Auth.Klient_All[int.Parse(ДанныеДляОтбора.Ремонт[1]) - 1, 0]}\"";
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
                Auth.Запрос = $"SELECT `ID` FROM `remont` WHERE `ID_Klient`={Auth.Klient_All[int.Parse(ДанныеДляОтбора.Ремонт[1]) - 1, 0]}";
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
                SpisoK();
            }

            void SpisoK()
            {
                Rems = new string[NumOfRem, Столбцы];
                for (int i = 0; i < NumOfRem; i++)
                {
                    Auth.Запрос = $"SELECT `ID`,`Type`,`Model`,`SN`,`Neispravnost`,`DateOfPriem`,`Vidano` FROM `remont` WHERE `ID_Klient`=\"{Auth.Klient_All[int.Parse(ДанныеДляОтбора.Ремонт[1]) - 1, 0]}\" AND `ID`=\"{ID_Rem[i]}\"";
                    MySqlCommand Комманда = new MySqlCommand(Auth.Запрос, Коннектор);
                    Коннектор.Open();
                    MySqlDataReader Результат = Комманда.ExecuteReader();
                    Результат.Read();
                    for (int Clc = 0; Clc < 7; Clc++)
                    {
                        string Ячейка = Результат[Clc].ToString();
                        Rems[i, Clc] = Ячейка;
                        if (Clc == 5)
                        {
                            Rems[i, Clc] = DateTime.Parse(Rems[i, Clc]).ToString("dd'.'MM'.'yyyy");
                        }
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
            

        }//Получаем список ремонтов
        public void ПодготовкаDataGrid(string[] N, DataGridView Grid)

        {
            while (N.Length > Grid.ColumnCount)
            {
                Grid.Columns.Add("", "");
            }
            Grid.Rows.Add(N);
        }//Выгрузка в DataGrid
    }
}
