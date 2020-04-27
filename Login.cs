using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using MySql.Data.MySqlClient;
using System.Diagnostics;

/*
 Окно авторизации.
 Принцип таков: пара логин и пароль используется для подключения к Базе, а затем проверяется соответсвтвие в таблице(Двойная Авторизация).
 */
namespace Сервис
{
    public partial class Login : Form
    {
        public Login()
        {
            InitializeComponent();
        }
        private void Auth_FormClosing(object sender, FormClosingEventArgs e)
        {
            Exit("server.exe");
            Exit("httpd_usbwv8.exe");
            Exit("mysqld_usbwv8.exe");
            System.Environment.Exit(0);
        }//Событие закрытия формы
        public void Exit(string Exit)
        {
            Process[] ps1 = System.Diagnostics.Process.GetProcessesByName($"{Exit}"); //Имя процесса
            foreach (Process p1 in ps1)
            {
                p1.Kill();
            }
        }//Завершение работы внешней программы
        class Пользователь
        {
            public string[] ИнфоПользователя { get; set; }
        }

        private void Login_Load(object sender, EventArgs e)
        {
        System.Diagnostics.Process.Start(@"C:\Service\Server\server.exe");//Запускаем MySQL и Apache
        }
        private static void Отключиться(MySqlConnection Коннектор)
        {
            Коннектор.Close();
        }//отключение от БД
        private void Button1_Click(object sender, EventArgs e)
        {
            if(tLogin.Text == "" | tPwd.Text == "")
                MessageBox.Show("Поля не могут быть пустыми","Обнаружено пустое поле!");
            else
            {
                try
                {
                    string Log_t = "";
                    Log_t = "Подключаюсь к БД...";
                    Log.Text = $"{Log_t}" + "\n";
                    Auth.Login = tLogin.Text;
                    Auth.Pwd = "Skorik2020";//Static_BD
                    Auth.uPwd = tPwd.Text;
                    Auth.server = "localhost";
                    Auth.user = $"SkorikJR";//Static_BD
                    Auth.basename = "service_null";
                    Auth.pwd = $"{Auth.Pwd}";
                    Auth.СтрокаПодключения = $"charset= utf8;server={Auth.server};user={Auth.user};database={Auth.basename};password={Auth.pwd};";
                    Auth.Запрос = $"SELECT * FROM `auth` WHERE `login`=\"{Auth.Login}\"";
                    MySqlConnection Коннектор = new MySqlConnection(Auth.СтрокаПодключения);
                    Коннектор.Open();
                    Log_t = "Подключено...";
                    Log.Text += $"{Log_t}\n";
                    MySqlCommand Комманда = new MySqlCommand(Auth.Запрос, Коннектор);
                    MySqlDataReader Результат = Комманда.ExecuteReader();
                    Результат.Read();
                    Пользователь Юзер = new Пользователь
                    {
                        ИнфоПользователя = new string[4]
                    };
                    Log_t = "Загружаю данные пользователя...";
                    Log.Text = Log.Text + "\n" + $"{ Log_t}" + "\n";
                    //while (Результат.Read())
                    for (int i = 0; i < 4; i++)
                    {
                        Результат[0].ToString();
                        Юзер.ИнфоПользователя[i] = Результат[i].ToString();
                    }//загружаем логин и пароль
                    Отключиться(Коннектор);
                    if (Юзер.ИнфоПользователя[1] == Auth.uPwd)
                    {
                        Log_t = "Авторизация прошла успешно, загружаю программу...";
                        Log.Text = Log.Text + $"{Log_t}" + "\n";
                        Loading mainForm;
                        Auth.AcMode = Юзер.ИнфоПользователя[2];
                        ЗагрузкаДанныхПользователя(int.Parse(Юзер.ИнфоПользователя[3]));
                        mainForm = new Loading();
                        mainForm.Show();
                        this.Hide();
                    }
                    else
                    {
                        MessageBox.Show("Что-то пошло не так проверьте Логин и Пароль!", "Err");
                        Exit("server");
                        Exit("httpd_usbwv8.exe");
                        Exit("mysqld_usbwv8.exe");
                        System.Environment.Exit(0);
                    }
                }
                catch (Exception)
                {
                    MessageBox.Show("Что-то пошло не так проверьте доступность БД!", "Err:100");
                    Exit("server");
                    Exit("httpd_usbwv8.exe");
                    Exit("mysqld_usbwv8.exe");
                    System.Environment.Exit(0);
                    throw;
                }
            }
        }

        private void ЗагрузкаДанныхПользователя(int ID)
        {
            Auth.Запрос = $"SELECT * FROM `ingeeneer` WHERE `ID`={ID}";
            MySqlConnection Коннектор = new MySqlConnection(Auth.СтрокаПодключения);
            MySqlCommand Комманда = new MySqlCommand(Auth.Запрос, Коннектор);
            Коннектор.Open();
            MySqlDataReader Результат = Комманда.ExecuteReader();
            Результат.Read();
            for (int ячейка = 0; ячейка < 8; ячейка++)
            {
                Результат[ячейка].ToString();
               Auth.Sotrudnik[ячейка] = Результат[ячейка].ToString();
            }
            Отключиться(Коннектор);
        }

        private void PictureBox1_Click(object sender, EventArgs e)
        {

        }
    }
}
