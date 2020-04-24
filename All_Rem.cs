using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Сервис
{
    public partial class All_Rem : Form
    {
        public All_Rem()
        {
            InitializeComponent();
        }

        private void All_Rem_Load(object sender, EventArgs e)//Нужно довести до ума!
        {
            int L = ДанныеДляОтбора.Все_Ремонты.Length;
            int H = ДанныеДляОтбора.Все_Ремонты.Length / 21;
            L /= H;
            for (int i = 0; i < H; i++)
            {
                string[] Tmp = new string[L];
                for (int j = 0; j < L; j++)
                {
                    Tmp[j] = ДанныеДляОтбора.Все_Ремонты[i, j];
                }
                ПодготовкаDataGrid(Tmp, dataGridView1);
            }
        }
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
