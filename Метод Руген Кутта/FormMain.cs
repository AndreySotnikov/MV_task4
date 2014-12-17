using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Rugen_Kutt
{
    public partial class FormMain : Form
    {
        private string FileName;
        private Func f;

        public FormMain()
        {
            InitializeComponent();
            FileName = "";
            comboBoxFunc.Items.Add("0");
            comboBoxFunc.Items.Add("2*x");
            comboBoxFunc.Items.Add("2*x-y^2+x^4");
            comboBoxFunc.Items.Add("12*x^2");
            comboBoxFunc.SelectedIndex = 0;
        }

        private void buttonTask_Click(object sender, EventArgs e)
        {
            if (openFileDialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                FileName = openFileDialog.FileName;
                textBoxFileName.Text = FileName;
            }
        }

        private void buttonTask_Click_1(object sender, EventArgs e)
        {
            dataGridView1.Rows.Clear();
            if (FileName == "")
            {
                MessageBox.Show("Имя файла не дожно быть пустым", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            switch(comboBoxFunc.SelectedIndex)
            {
                case 0:
                    {
                        f = (x, y) => 0; //x - y;
                        break;
                    }
                case 1:
                    {
                        f = (x, y) => 2*x;
                        break;
                    }
                case 2:
                    {
                        f = (x, y) => 2*x-Math.Pow(y,2)+Math.Pow(x,4);
                        break;
                    }
                case 3:
                    {
                        f = (x, y) => 12*Math.Pow(x,2);
                        break;
                    }
            }
            Ruger_Kutta R_K = new Ruger_Kutta(FileName, f, dataGridView1, label4,label5,label6);
            if (!R_K.LoadFomFile())
            {
                MessageBox.Show("Ошибка при загрузки данных из файла", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            R_K.Task();
            MessageBox.Show("Задача успешно выполнена. Данные сохранены в файле " + Ruger_Kutta.outFileName, "Внимание", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
    }
}
