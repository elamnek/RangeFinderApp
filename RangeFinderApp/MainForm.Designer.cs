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
            this.txtMeasureInterval = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.btnGo = new System.Windows.Forms.Button();
            this.chkStoreInDB = new System.Windows.Forms.CheckBox();
            this.label3 = new System.Windows.Forms.Label();
            this.txtDBConn = new System.Windows.Forms.TextBox();
            this.btnStop = new System.Windows.Forms.Button();
            this.txtData = new System.Windows.Forms.TextBox();
            this.rtb = new System.Windows.Forms.RichTextBox();
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
            this.txtMaxRange.Location = new System.Drawing.Point(276, 277);
            this.txtMaxRange.Name = "txtMaxRange";
            this.txtMaxRange.Size = new System.Drawing.Size(100, 26);
            this.txtMaxRange.TabIndex = 2;
            this.txtMaxRange.Text = "10";
            // 
            // txtMeasureInterval
            // 
            this.txtMeasureInterval.Location = new System.Drawing.Point(276, 327);
            this.txtMeasureInterval.Name = "txtMeasureInterval";
            this.txtMeasureInterval.Size = new System.Drawing.Size(100, 26);
            this.txtMeasureInterval.TabIndex = 3;
            this.txtMeasureInterval.Text = "250";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(45, 280);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(206, 20);
            this.label1.TabIndex = 4;
            this.label1.Text = "Maximum range to store (m)";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(45, 330);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(216, 20);
            this.label2.TabIndex = 5;
            this.label2.Text = "Measurement frequency (ms)";
            // 
            // btnGo
            // 
            this.btnGo.Location = new System.Drawing.Point(454, 280);
            this.btnGo.Name = "btnGo";
            this.btnGo.Size = new System.Drawing.Size(158, 60);
            this.btnGo.TabIndex = 6;
            this.btnGo.Text = "Measure!";
            this.btnGo.UseVisualStyleBackColor = true;
            this.btnGo.Click += new System.EventHandler(this.btnGo_Click);
            // 
            // chkStoreInDB
            // 
            this.chkStoreInDB.AutoSize = true;
            this.chkStoreInDB.Location = new System.Drawing.Point(64, 399);
            this.chkStoreInDB.Name = "chkStoreInDB";
            this.chkStoreInDB.Size = new System.Drawing.Size(161, 24);
            this.chkStoreInDB.TabIndex = 7;
            this.chkStoreInDB.Text = "Store in database";
            this.chkStoreInDB.UseVisualStyleBackColor = true;
            this.chkStoreInDB.CheckedChanged += new System.EventHandler(this.chkStoreInDB_CheckedChanged);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(25, 459);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(183, 20);
            this.label3.TabIndex = 9;
            this.label3.Text = "DT database connection";
            // 
            // txtDBConn
            // 
            this.txtDBConn.Location = new System.Drawing.Point(27, 482);
            this.txtDBConn.Name = "txtDBConn";
            this.txtDBConn.Size = new System.Drawing.Size(601, 26);
            this.txtDBConn.TabIndex = 8;
            // 
            // btnStop
            // 
            this.btnStop.Location = new System.Drawing.Point(454, 346);
            this.btnStop.Name = "btnStop";
            this.btnStop.Size = new System.Drawing.Size(158, 60);
            this.btnStop.TabIndex = 10;
            this.btnStop.Text = "Stop";
            this.btnStop.UseVisualStyleBackColor = true;
            this.btnStop.Click += new System.EventHandler(this.btnStop_Click);
            // 
            // txtData
            // 
            this.txtData.Location = new System.Drawing.Point(276, 432);
            this.txtData.Name = "txtData";
            this.txtData.Size = new System.Drawing.Size(336, 26);
            this.txtData.TabIndex = 11;
            // 
            // rtb
            // 
            this.rtb.Location = new System.Drawing.Point(29, 548);
            this.rtb.Name = "rtb";
            this.rtb.Size = new System.Drawing.Size(583, 634);
            this.rtb.TabIndex = 12;
            this.rtb.Text = "";
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(640, 1216);
            this.Controls.Add(this.rtb);
            this.Controls.Add(this.txtData);
            this.Controls.Add(this.btnStop);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.txtDBConn);
            this.Controls.Add(this.chkStoreInDB);
            this.Controls.Add(this.btnGo);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.txtMeasureInterval);
            this.Controls.Add(this.txtMaxRange);
            this.Controls.Add(this.groupBox1);
            this.Name = "MainForm";
            this.Text = "Submarine Range Finder";
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label lblRange;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.TextBox txtMaxRange;
        private System.Windows.Forms.TextBox txtMeasureInterval;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button btnGo;
        private System.Windows.Forms.CheckBox chkStoreInDB;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox txtDBConn;
        private System.Windows.Forms.Button btnStop;
        private System.Windows.Forms.TextBox txtData;
        private System.Windows.Forms.RichTextBox rtb;
    }
}

