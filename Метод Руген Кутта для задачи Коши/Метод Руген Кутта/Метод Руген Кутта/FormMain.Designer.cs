namespace Метод_Руген_Кутта
{
    partial class FormMain
    {
        /// <summary>
        /// Требуется переменная конструктора.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Освободить все используемые ресурсы.
        /// </summary>
        /// <param name="disposing">истинно, если управляемый ресурс должен быть удален; иначе ложно.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Код, автоматически созданный конструктором форм Windows

        /// <summary>
        /// Обязательный метод для поддержки конструктора - не изменяйте
        /// содержимое данного метода при помощи редактора кода.
        /// </summary>
        private void InitializeComponent()
        {
            this.buttonOpenFile = new System.Windows.Forms.Button();
            this.openFileDialog = new System.Windows.Forms.OpenFileDialog();
            this.textBoxFileName = new System.Windows.Forms.TextBox();
            this.buttonTask = new System.Windows.Forms.Button();
            this.labelFileName = new System.Windows.Forms.Label();
            this.comboBoxFunc = new System.Windows.Forms.ComboBox();
            this.labelFunc = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // buttonOpenFile
            // 
            this.buttonOpenFile.Location = new System.Drawing.Point(15, 41);
            this.buttonOpenFile.Name = "buttonOpenFile";
            this.buttonOpenFile.Size = new System.Drawing.Size(107, 28);
            this.buttonOpenFile.TabIndex = 0;
            this.buttonOpenFile.Text = "Выбрать файл";
            this.buttonOpenFile.UseVisualStyleBackColor = true;
            this.buttonOpenFile.Click += new System.EventHandler(this.buttonTask_Click);
            // 
            // openFileDialog
            // 
            this.openFileDialog.DefaultExt = "0";
            this.openFileDialog.FileName = "openFileDialog1";
            this.openFileDialog.Filter = "Текстовый файл| *.txt";
            // 
            // textBoxFileName
            // 
            this.textBoxFileName.Location = new System.Drawing.Point(12, 128);
            this.textBoxFileName.Name = "textBoxFileName";
            this.textBoxFileName.ReadOnly = true;
            this.textBoxFileName.Size = new System.Drawing.Size(425, 20);
            this.textBoxFileName.TabIndex = 1;
            // 
            // buttonTask
            // 
            this.buttonTask.Location = new System.Drawing.Point(176, 41);
            this.buttonTask.Name = "buttonTask";
            this.buttonTask.Size = new System.Drawing.Size(138, 28);
            this.buttonTask.TabIndex = 2;
            this.buttonTask.Text = "Выполнить вычисления";
            this.buttonTask.UseVisualStyleBackColor = true;
            this.buttonTask.Click += new System.EventHandler(this.buttonTask_Click_1);
            // 
            // labelFileName
            // 
            this.labelFileName.AutoSize = true;
            this.labelFileName.Location = new System.Drawing.Point(12, 102);
            this.labelFileName.Name = "labelFileName";
            this.labelFileName.Size = new System.Drawing.Size(64, 13);
            this.labelFileName.TabIndex = 3;
            this.labelFileName.Text = "Имя файла";
            // 
            // comboBoxFunc
            // 
            this.comboBoxFunc.FormattingEnabled = true;
            this.comboBoxFunc.Location = new System.Drawing.Point(12, 188);
            this.comboBoxFunc.Name = "comboBoxFunc";
            this.comboBoxFunc.Size = new System.Drawing.Size(141, 21);
            this.comboBoxFunc.TabIndex = 4;
            // 
            // labelFunc
            // 
            this.labelFunc.AutoSize = true;
            this.labelFunc.Location = new System.Drawing.Point(12, 172);
            this.labelFunc.Name = "labelFunc";
            this.labelFunc.Size = new System.Drawing.Size(59, 13);
            this.labelFunc.TabIndex = 5;
            this.labelFunc.Text = "Функция f";
            // 
            // FormMain
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(449, 280);
            this.Controls.Add(this.labelFunc);
            this.Controls.Add(this.comboBoxFunc);
            this.Controls.Add(this.labelFileName);
            this.Controls.Add(this.buttonTask);
            this.Controls.Add(this.textBoxFileName);
            this.Controls.Add(this.buttonOpenFile);
            this.Name = "FormMain";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button buttonOpenFile;
        private System.Windows.Forms.OpenFileDialog openFileDialog;
        private System.Windows.Forms.TextBox textBoxFileName;
        private System.Windows.Forms.Button buttonTask;
        private System.Windows.Forms.Label labelFileName;
        private System.Windows.Forms.ComboBox comboBoxFunc;
        private System.Windows.Forms.Label labelFunc;
    }
}

