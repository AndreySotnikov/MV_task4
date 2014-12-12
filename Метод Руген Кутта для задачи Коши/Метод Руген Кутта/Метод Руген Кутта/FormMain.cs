using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Метод_Руген_Кутта
{
    public partial class FormMain : Form
    {
        private string FileName;
        private Func f;

        public FormMain()
        {
            InitializeComponent();
            FileName = "";
            comboBoxFunc.Items.Add("x^2");
            comboBoxFunc.Items.Add("Exp(x)");
            comboBoxFunc.Items.Add("cos(x)");
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
            if (FileName == "")
            {
                MessageBox.Show("Имя файла не дожно быть пустым", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            switch(comboBoxFunc.SelectedIndex)
            {
                case 0:
                    {
                        f = (x,y) => Math.Pow(x, 2);
                        break;
                    }
                case 1:
                    {
                        f = (x, y) => Math.Exp(x);
                        break;
                    }
                case 2:
                    {
                        f = (x, y) => Math.Cos(x);
                        break;
                    }
            }
            Ruger_Kutta R_K = new Ruger_Kutta(FileName, f);
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
