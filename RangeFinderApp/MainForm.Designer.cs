namespace RangeFinderApp
{
    partial class MainForm
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
            this.lblRange = new System.Windows.Forms.Label();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.txtMaxRange = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.btnGo = new System.Windows.Forms.Button();
            this.chkStoreInDB = new System.Windows.Forms.CheckBox();
            this.label3 = new System.Windows.Forms.Label();
            this.txtDBConn = new System.Windows.Forms.TextBox();
            this.btnStop = new System.Windows.Forms.Button();
            this.rtb = new System.Windows.Forms.RichTextBox();
            this.txtPort = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // lblRange
            // 
            this.lblRange.AutoSize = true;
            this.lblRange.Font = new System.Drawing.Font("Microsoft Sans Serif", 70F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblRange.Location = new System.Drawing.Point(36, 32);
            this.lblRange.Name = "lblRange";
            this.lblRange.Size = new System.Drawing.Size(145, 159);
            this.lblRange.TabIndex = 0;
            this.lblRange.Text = "0";
            // 
            // groupBox1
            // 
            this.groupBox1.BackColor = System.Drawing.Color.LawnGreen;
            this.groupBox1.Controls.Add(this.lblRange);
            this.groupBox1.ForeColor = System.Drawing.SystemColors.ControlText;
            this.groupBox1.Location = new System.Drawing.Point(27, 26);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(585, 226);
            this.groupBox1.TabIndex = 1;
            this.groupBox1.TabStop = false;
            // 
            // txtMaxRange
            // 
            this.txtMaxRange.Location = new System.Drawing.Point(256, 311);
            this.txtMaxRange.Name = "txtMaxRange";
            this.txtMaxRange.Size = new System.Drawing.Size(46, 26);
            this.txtMaxRange.TabIndex = 2;
            this.txtMaxRange.Text = "10";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(45, 314);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(206, 20);
            this.label1.TabIndex = 4;
            this.label1.Text = "Maximum range to store (m)";
            // 
            // btnGo
            // 
            this.btnGo.Location = new System.Drawing.Point(332, 277);
            this.btnGo.Name = "btnGo";
            this.btnGo.Size = new System.Drawing.Size(135, 60);
            this.btnGo.TabIndex = 6;
            this.btnGo.Text = "Measure!";
            this.btnGo.UseVisualStyleBackColor = true;
            this.btnGo.Click += new System.EventHandler(this.btnGo_Click);
            // 
            // chkStoreInDB
            // 
            this.chkStoreInDB.AutoSize = true;
            this.chkStoreInDB.Location = new System.Drawing.Point(64, 398);
            this.chkStoreInDB.Name = "chkStoreInDB";
            this.chkStoreInDB.Size = new System.Drawing.Size(161, 24);
            this.chkStoreInDB.TabIndex = 7;
            this.chkStoreInDB.Text = "Store in database";
            this.chkStoreInDB.UseVisualStyleBackColor = true;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(26, 458);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(183, 20);
            this.label3.TabIndex = 9;
            this.label3.Text = "DT database connection";
            // 
            // txtDBConn
            // 
            this.txtDBConn.Location = new System.Drawing.Point(27, 482);
            this.txtDBConn.Name = "txtDBConn";
            this.txtDBConn.Size = new System.Drawing.Size(585, 26);
            this.txtDBConn.TabIndex = 8;
            // 
            // btnStop
            // 
            this.btnStop.Location = new System.Drawing.Point(476, 277);
            this.btnStop.Name = "btnStop";
            this.btnStop.Size = new System.Drawing.Size(136, 60);
            this.btnStop.TabIndex = 10;
            this.btnStop.Text = "Stop";
            this.btnStop.UseVisualStyleBackColor = true;
            this.btnStop.Click += new System.EventHandler(this.btnStop_Click);
            // 
            // rtb
            // 
            this.rtb.Location = new System.Drawing.Point(28, 548);
            this.rtb.Name = "rtb";
            this.rtb.Size = new System.Drawing.Size(583, 635);
            this.rtb.TabIndex = 12;
            this.rtb.Text = "";
            // 
            // txtPort
            // 
            this.txtPort.Location = new System.Drawing.Point(75, 258);
            this.txtPort.Name = "txtPort";
            this.txtPort.Size = new System.Drawing.Size(88, 26);
            this.txtPort.TabIndex = 13;
            this.txtPort.Text = "COM6";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(31, 261);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(38, 20);
            this.label2.TabIndex = 14;
            this.label2.Text = "Port";
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(640, 1062);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.txtPort);
            this.Controls.Add(this.rtb);
            this.Controls.Add(this.btnStop);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.txtDBConn);
            this.Controls.Add(this.chkStoreInDB);
            this.Controls.Add(this.btnGo);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.txtMaxRange);
            this.Controls.Add(this.groupBox1);
            this.Name = "MainForm";
            this.Text = "Submarine Range Finder";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.MainForm_FormClosing);
            this.Load += new System.EventHandler(this.MainForm_Load);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label lblRange;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.TextBox txtMaxRange;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button btnGo;
        private System.Windows.Forms.CheckBox chkStoreInDB;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox txtDBConn;
        private System.Windows.Forms.Button btnStop;
        private System.Windows.Forms.RichTextBox rtb;
        private System.Windows.Forms.TextBox txtPort;
        private System.Windows.Forms.Label label2;
    }
}

