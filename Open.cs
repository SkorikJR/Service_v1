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
    public partial class Open : Form
    {
        public Open()
        {
            InitializeComponent();
        }
        public int Столбцы = 11;
        private void BFind_Click(object sender, EventArgs e)
        {
            bool Result = int.TryParse(tFind.Text, out int Out);
            if (Result == true & Auth.ВсегоРемонтов >= Out)
            {
                Auth.Запрос = $"SELECT * FROM `remont` WHERE `ID`={Out}";
                MySqlConnection Коннектор = new MySqlConnection(Auth.СтрокаПодключения);
                MySqlCommand Комманда = new MySqlCommand(Auth.Запрос, Коннектор);
                Коннектор.Open();
                MySqlDataReader Результат = Комманда.ExecuteReader();
                Результат.Read();
                for (int ячейка = 0; ячейка < ДанныеДляОтбора.КолСтр; ячейка++)
                {
                    ДанныеДляОтбора.Ремонт[ячейка] = Результат[ячейка].ToString();
                }
                Отключиться(Коннектор);
                Remont O_Rem;
                O_Rem = new Remont();
                O_Rem.Show();
            }
            else
            {
                MessageBox.Show("Проверьде номер, возможно вы ввели номер ремонта, которого не существует","Err");
            }
        }
        private static void Отключиться(MySqlConnection Коннектор)
        {
            Коннектор.Close();
        }//отключение от БД

    }
}
