namespace RestaurantClient
{
    partial class addOrder
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(addOrder));
            this.listBox1 = new System.Windows.Forms.ListBox();
            this.btngetraenke = new System.Windows.Forms.Button();
            this.btnEssen = new System.Windows.Forms.Button();
            this.rtbextras = new System.Windows.Forms.RichTextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.btnreset = new System.Windows.Forms.Button();
            this.btnsaveclose = new System.Windows.Forms.Button();
            this.btnsavenext = new System.Windows.Forms.Button();
            this.label2 = new System.Windows.Forms.Label();
            this.nudcount = new System.Windows.Forms.NumericUpDown();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            ((System.ComponentModel.ISupportInitialize)(this.nudcount)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.SuspendLayout();
            // 
            // listBox1
            // 
            this.listBox1.FormattingEnabled = true;
            this.listBox1.Location = new System.Drawing.Point(12, 69);
            this.listBox1.Name = "listBox1";
            this.listBox1.Size = new System.Drawing.Size(586, 225);
            this.listBox1.TabIndex = 0;
            // 
            // btngetraenke
            // 
            this.btngetraenke.Location = new System.Drawing.Point(12, 11);
            this.btngetraenke.Name = "btngetraenke";
            this.btngetraenke.Size = new System.Drawing.Size(264, 54);
            this.btngetraenke.TabIndex = 1;
            this.btngetraenke.Text = "Getränke";
            this.btngetraenke.UseVisualStyleBackColor = true;
            // 
            // btnEssen
            // 
            this.btnEssen.Location = new System.Drawing.Point(334, 11);
            this.btnEssen.Name = "btnEssen";
            this.btnEssen.Size = new System.Drawing.Size(264, 54);
            this.btnEssen.TabIndex = 2;
            this.btnEssen.Text = "Essen";
            this.btnEssen.UseVisualStyleBackColor = true;
            // 
            // rtbextras
            // 
            this.rtbextras.Location = new System.Drawing.Point(12, 328);
            this.rtbextras.Name = "rtbextras";
            this.rtbextras.Size = new System.Drawing.Size(375, 110);
            this.rtbextras.TabIndex = 4;
            this.rtbextras.Text = "";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 312);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(83, 13);
            this.label1.TabIndex = 5;
            this.label1.Text = "Extra Wünsche:";
            // 
            // btnreset
            // 
            this.btnreset.BackColor = System.Drawing.Color.IndianRed;
            this.btnreset.Location = new System.Drawing.Point(12, 454);
            this.btnreset.Name = "btnreset";
            this.btnreset.Size = new System.Drawing.Size(175, 90);
            this.btnreset.TabIndex = 6;
            this.btnreset.Text = "Löschen";
            this.btnreset.UseVisualStyleBackColor = false;
            // 
            // btnsaveclose
            // 
            this.btnsaveclose.BackColor = System.Drawing.Color.Khaki;
            this.btnsaveclose.Location = new System.Drawing.Point(217, 454);
            this.btnsaveclose.Name = "btnsaveclose";
            this.btnsaveclose.Size = new System.Drawing.Size(175, 90);
            this.btnsaveclose.TabIndex = 7;
            this.btnsaveclose.Text = "Speichern\r\nund\r\nSchließen\r\n";
            this.btnsaveclose.UseVisualStyleBackColor = false;
            // 
            // btnsavenext
            // 
            this.btnsavenext.BackColor = System.Drawing.Color.DarkSeaGreen;
            this.btnsavenext.Location = new System.Drawing.Point(422, 454);
            this.btnsavenext.Name = "btnsavenext";
            this.btnsavenext.Size = new System.Drawing.Size(175, 90);
            this.btnsavenext.TabIndex = 8;
            this.btnsavenext.Text = "Speichern\r\nund\r\nWeiter";
            this.btnsavenext.UseVisualStyleBackColor = false;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(422, 312);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(40, 13);
            this.label2.TabIndex = 9;
            this.label2.Text = "Menge";
            // 
            // nudcount
            // 
            this.nudcount.Location = new System.Drawing.Point(425, 328);
            this.nudcount.Name = "nudcount";
            this.nudcount.Size = new System.Drawing.Size(143, 20);
            this.nudcount.TabIndex = 10;
            // 
            // pictureBox1
            // 
            this.pictureBox1.Image = global::RestaurantClient.Properties.Resources.Gasthoficon;
            this.pictureBox1.Location = new System.Drawing.Point(282, 15);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(46, 48);
            this.pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pictureBox1.TabIndex = 11;
            this.pictureBox1.TabStop = false;
            // 
            // addOrder
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(609, 556);
            this.Controls.Add(this.pictureBox1);
            this.Controls.Add(this.nudcount);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.btnsavenext);
            this.Controls.Add(this.btnsaveclose);
            this.Controls.Add(this.btnreset);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.rtbextras);
            this.Controls.Add(this.btnEssen);
            this.Controls.Add(this.btngetraenke);
            this.Controls.Add(this.listBox1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "addOrder";
            this.Text = "Artikel hinzufügen";
            ((System.ComponentModel.ISupportInitialize)(this.nudcount)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ListBox listBox1;
        private System.Windows.Forms.Button btngetraenke;
        private System.Windows.Forms.Button btnEssen;
        private System.Windows.Forms.RichTextBox rtbextras;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button btnreset;
        private System.Windows.Forms.Button btnsaveclose;
        private System.Windows.Forms.Button btnsavenext;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.NumericUpDown nudcount;
        private System.Windows.Forms.PictureBox pictureBox1;
    }
}