using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace showpic
{
    public partial class frmsamplequan : Form
    {
        string filename;
        System.Drawing.Bitmap bitmap,oribitmap;
        public frmsamplequan()
        {
            InitializeComponent();
            cbo.SelectedIndex = 0;
        }

        private void btnstart_Click(object sender, EventArgs e)
        {
            if (oribitmap == null) { MessageBox.Show("请先加载图片", "出错", MessageBoxButtons.OK, MessageBoxIcon.Warning); return; }            
            int n = Convert.ToInt32(txtn.Text); //hengxiang
            int m= Convert.ToInt32(txtm.Text);  //zongxiang
            int level = Convert.ToInt32(cbo.Text);
            int h = oribitmap.Height, w = oribitmap.Width;
            if (w % n != 0 || h % m != 0) { MessageBox.Show("请重新输入采样间隔","出错",MessageBoxButtons.OK,MessageBoxIcon.Exclamation); return; }
            Color curcolor;
            bitmap = new Bitmap(oribitmap);
            for (int i=0;i<w;i+=n)
            {
                for (int j=0;j<h;j+=m)
                {
                    int r = 0, g = 0, b = 0;
                    for (int k=0;k<n;k++)
                    {
                        for (int l=0;l<m;l++)
                        {
                            curcolor = oribitmap.GetPixel(i + k, j + l);
                            r += curcolor.R;
                            g += curcolor.G;
                            b += curcolor.B;
                        }                        
                    }
                    r /= (n * m);
                    g /= (n * m);
                    b /= (n * m);
                    for (int k = 0; k < n; k++)
                    {
                        for (int l = 0; l < m; l++)
                        {
                            bitmap.SetPixel(i + k, j + l, Color.FromArgb(r, g, b));
                        }
                    }
                }
            }
            level=256/level;
            for (int i = 0; i < w; i++)
                for (int j = 0; j < h;j++ )
                {
                    int r = 0, g = 0, b = 0;
                    curcolor = bitmap.GetPixel(i, j);
                    r = curcolor.R;
                    g = curcolor.G;
                    b = curcolor.B;
                    r = r / level * level;
                    g = g / level * level;
                    b = b / level * level;
                    bitmap.SetPixel(i, j, Color.FromArgb(r, g, b));
                }
            pictureBox2.Image = bitmap;
            //pictureBox1.Image = oribitmap;
        }

        private void btnopen_Click(object sender, EventArgs e)
        {
            OpenFileDialog opndlg = new OpenFileDialog();
            opndlg.InitialDirectory = "E:\\";
            opndlg.Filter = "所有图像文件 | *.bmp; *.png; *.jpg; *.gif; *.tif; *.ico; *.emf|" + "位图(*.bmp; *.png; *.jpg; *.gif; ...) | *.bmp; *.png; *.jpg; *.gif; *.ico|" + "矢量图(*.emf) | *.emf";
            opndlg.Title = "打开图像文件";
            if (opndlg.ShowDialog() == DialogResult.OK)
            {
                filename = opndlg.FileName;
                try
                {
                    bitmap = (Bitmap)Image.FromFile(filename);
                }
                catch (Exception exp)
                {
                    MessageBox.Show(exp.Message);
                }
            }
            if (bitmap == null) return;
            label3.Text = filename;
            pictureBox1.Image = bitmap;
            oribitmap = new Bitmap(bitmap);
        }        
        private void btnsave_Click(object sender, EventArgs e)
        {
            if (bitmap == null) return;
            SaveFileDialog savedlg = new SaveFileDialog();
            savedlg.Title = "另存为";
            savedlg.OverwritePrompt = true;
            savedlg.Filter = "BMP文件(*.bmp) | *.bmp|" + "GIF文件(*.gif) | *.gif|" + "JPEG文件(*.jpg) | *.jpg|" + "PNG文件(*.png) | *.png";
            if (savedlg.ShowDialog() == DialogResult.OK)
            {
                string filename = savedlg.FileName;
                string tmp = filename.Remove(0, filename.Length - 3);
                label4.Text = filename;
                switch (tmp)
                {
                    case "bmp":
                        bitmap.Save(filename, System.Drawing.Imaging.ImageFormat.Bmp);
                        break;
                    case "jpg":
                        bitmap.Save(filename, System.Drawing.Imaging.ImageFormat.Jpeg);
                        break;
                    case "gif":
                        bitmap.Save(filename, System.Drawing.Imaging.ImageFormat.Gif);
                        break;
                    case "png":
                        bitmap.Save(filename, System.Drawing.Imaging.ImageFormat.Png);
                        break;
                    default:
                        break;
                }
            }
        }          
    }
}
