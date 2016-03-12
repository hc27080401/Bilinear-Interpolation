namespace showpic
{
    partial class frmmatch
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
            this.btnopen1 = new System.Windows.Forms.Button();
            this.btnopen2 = new System.Windows.Forms.Button();
            this.btnmatch = new System.Windows.Forms.Button();
            this.btnsave = new System.Windows.Forms.Button();
            this.btncolor = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.lbl1 = new System.Windows.Forms.Label();
            this.lbl2 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.lbl3 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // btnopen1
            // 
            this.btnopen1.Location = new System.Drawing.Point(20, 27);
            this.btnopen1.Name = "btnopen1";
            this.btnopen1.Size = new System.Drawing.Size(101, 51);
            this.btnopen1.TabIndex = 0;
            this.btnopen1.Text = "打开原图像";
            this.btnopen1.UseVisualStyleBackColor = true;
            this.btnopen1.Click += new System.EventHandler(this.btnopen1_Click);
            // 
            // btnopen2
            // 
            this.btnopen2.Location = new System.Drawing.Point(20, 101);
            this.btnopen2.Name = "btnopen2";
            this.btnopen2.Size = new System.Drawing.Size(101, 49);
            this.btnopen2.TabIndex = 1;
            this.btnopen2.Text = "打开匹配图像";
            this.btnopen2.UseVisualStyleBackColor = true;
            this.btnopen2.Click += new System.EventHandler(this.btnopen2_Click);
            // 
            // btnmatch
            // 
            this.btnmatch.Location = new System.Drawing.Point(20, 171);
            this.btnmatch.Name = "btnmatch";
            this.btnmatch.Size = new System.Drawing.Size(101, 46);
            this.btnmatch.TabIndex = 2;
            this.btnmatch.Text = "匹配(灰)";
            this.btnmatch.UseVisualStyleBackColor = true;
            this.btnmatch.Click += new System.EventHandler(this.btnmatch_Click);
            // 
            // btnsave
            // 
            this.btnsave.Location = new System.Drawing.Point(20, 310);
            this.btnsave.Name = "btnsave";
            this.btnsave.Size = new System.Drawing.Size(101, 49);
            this.btnsave.TabIndex = 3;
            this.btnsave.Text = "保存图像";
            this.btnsave.UseVisualStyleBackColor = true;
            this.btnsave.Click += new System.EventHandler(this.btnsave_Click);
            // 
            // btncolor
            // 
            this.btncolor.Location = new System.Drawing.Point(20, 239);
            this.btncolor.Name = "btncolor";
            this.btncolor.Size = new System.Drawing.Size(101, 45);
            this.btncolor.TabIndex = 4;
            this.btncolor.Text = "匹配(彩)";
            this.btncolor.UseVisualStyleBackColor = true;
            this.btncolor.Click += new System.EventHandler(this.btncolor_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("微软雅黑", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label1.Location = new System.Drawing.Point(29, 582);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(126, 26);
            this.label1.TabIndex = 5;
            this.label1.Text = "已加载原图像";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Square721 BT", 27.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.Location = new System.Drawing.Point(12, 522);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(184, 46);
            this.label2.TabIndex = 6;
            this.label2.Text = "STATUS:";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("微软雅黑", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label3.Location = new System.Drawing.Point(29, 623);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(145, 26);
            this.label3.TabIndex = 7;
            this.label3.Text = "已加载匹配图像";
            // 
            // lbl1
            // 
            this.lbl1.AutoSize = true;
            this.lbl1.Font = new System.Drawing.Font("Verdana", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbl1.Location = new System.Drawing.Point(186, 587);
            this.lbl1.Name = "lbl1";
            this.lbl1.Size = new System.Drawing.Size(0, 18);
            this.lbl1.TabIndex = 8;
            // 
            // lbl2
            // 
            this.lbl2.AutoSize = true;
            this.lbl2.Font = new System.Drawing.Font("Verdana", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbl2.Location = new System.Drawing.Point(186, 628);
            this.lbl2.Name = "lbl2";
            this.lbl2.Size = new System.Drawing.Size(0, 18);
            this.lbl2.TabIndex = 9;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("微软雅黑", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label4.Location = new System.Drawing.Point(29, 666);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(107, 26);
            this.label4.TabIndex = 10;
            this.label4.Text = "已保存图像";
            // 
            // lbl3
            // 
            this.lbl3.AutoSize = true;
            this.lbl3.Font = new System.Drawing.Font("Verdana", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbl3.Location = new System.Drawing.Point(186, 674);
            this.lbl3.Name = "lbl3";
            this.lbl3.Size = new System.Drawing.Size(0, 18);
            this.lbl3.TabIndex = 11;
            // 
            // frmmatch
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1350, 730);
            this.Controls.Add(this.lbl3);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.lbl2);
            this.Controls.Add(this.lbl1);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.btncolor);
            this.Controls.Add(this.btnsave);
            this.Controls.Add(this.btnmatch);
            this.Controls.Add(this.btnopen2);
            this.Controls.Add(this.btnopen1);
            this.Name = "frmmatch";
            this.Text = "直方图匹配";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnopen1;
        private System.Windows.Forms.Button btnopen2;
        private System.Windows.Forms.Button btnmatch;
        private System.Windows.Forms.Button btnsave;
        private System.Windows.Forms.Button btncolor;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label lbl1;
        private System.Windows.Forms.Label lbl2;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label lbl3;
    }
}