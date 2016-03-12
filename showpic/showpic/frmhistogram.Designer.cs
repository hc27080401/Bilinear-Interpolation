namespace showpic
{
    partial class frmhistogram
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
            this.btngray = new System.Windows.Forms.Button();
            this.btnequal = new System.Windows.Forms.Button();
            this.btnmatch = new System.Windows.Forms.Button();
            this.btnlocal = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // btngray
            // 
            this.btngray.Location = new System.Drawing.Point(46, 30);
            this.btngray.Name = "btngray";
            this.btngray.Size = new System.Drawing.Size(115, 48);
            this.btngray.TabIndex = 4;
            this.btngray.Text = "灰度直方图";
            this.btngray.UseVisualStyleBackColor = true;
            this.btngray.Click += new System.EventHandler(this.btngray_Click);
            // 
            // btnequal
            // 
            this.btnequal.Location = new System.Drawing.Point(46, 95);
            this.btnequal.Name = "btnequal";
            this.btnequal.Size = new System.Drawing.Size(115, 48);
            this.btnequal.TabIndex = 5;
            this.btnequal.Text = "直方图均衡化";
            this.btnequal.UseVisualStyleBackColor = true;
            this.btnequal.Click += new System.EventHandler(this.btnequal_Click);
            // 
            // btnmatch
            // 
            this.btnmatch.Location = new System.Drawing.Point(46, 163);
            this.btnmatch.Name = "btnmatch";
            this.btnmatch.Size = new System.Drawing.Size(115, 48);
            this.btnmatch.TabIndex = 6;
            this.btnmatch.Text = "直方图匹配";
            this.btnmatch.UseVisualStyleBackColor = true;
            this.btnmatch.Click += new System.EventHandler(this.btnmatch_Click);
            // 
            // btnlocal
            // 
            this.btnlocal.Location = new System.Drawing.Point(46, 231);
            this.btnlocal.Name = "btnlocal";
            this.btnlocal.Size = new System.Drawing.Size(115, 57);
            this.btnlocal.TabIndex = 7;
            this.btnlocal.Text = "局部直方图均衡化";
            this.btnlocal.UseVisualStyleBackColor = true;
            this.btnlocal.Click += new System.EventHandler(this.btnlocal_Click);
            // 
            // frmhistogram
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoScroll = true;
            this.ClientSize = new System.Drawing.Size(1350, 730);
            this.Controls.Add(this.btnlocal);
            this.Controls.Add(this.btnmatch);
            this.Controls.Add(this.btnequal);
            this.Controls.Add(this.btngray);
            this.Name = "frmhistogram";
            this.Text = "直方图";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button btngray;
        private System.Windows.Forms.Button btnequal;
        private System.Windows.Forms.Button btnmatch;
        private System.Windows.Forms.Button btnlocal;
    }
}