using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Runtime;
namespace showpic
{
    public partial class frmzoom : Form
    {
        string filename;
        System.Drawing.Bitmap bitmap, oribitmap;
        private static double[,] a = new double[4, 4];
        public frmzoom()
        {
            InitializeComponent();
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
            lbl1.Text = filename;
            pictureBox1.Image = bitmap;
            oribitmap = new Bitmap(bitmap);
        }
        private byte getvalue(double tmp)
        {
            if (tmp < 0) return 0;
            if (tmp > 255) return 255;
            return (byte)tmp;
        }
        private void btnstart_Click(object sender, EventArgs e)
        {
            if (oribitmap == null) { MessageBox.Show("请先加载图片", "出错", MessageBoxButtons.OK, MessageBoxIcon.Warning); return; }
            double k1 = Convert.ToDouble(txtn.Text); //hengxiang
            double k2 = Convert.ToDouble(txtm.Text);  //zongxiang            
            int h = oribitmap.Height, w = oribitmap.Width;           
            int hh = (int)(k2 * h), ww = (int)(k1 * w);
            bitmap = new Bitmap(ww, hh);
            int[,] p=new int[4,4];
            
            byte[] pixel = new byte[4];
            byte[,,] oripixel=new byte[h,w,4];
            Rectangle rect1 = new Rectangle(0, 0, w, h);
            System.Drawing.Imaging.BitmapData bmpdata1 = oribitmap.LockBits(rect1, System.Drawing.Imaging.ImageLockMode.ReadWrite, oribitmap.PixelFormat);
            IntPtr ptr1 = bmpdata1.Scan0;
            int bytes1 = bmpdata1.Stride * bmpdata1.Height;
            byte[] rgbvalues1 = new byte[bytes1];
            System.Runtime.InteropServices.Marshal.Copy(ptr1, rgbvalues1, 0, bytes1); //copy pixel
            for (int x = 0; x < h; x++)
                for (int y = 0; y < w*4;y+=4 )
                {
                    oripixel[x, y/4, 0] = rgbvalues1[x * bmpdata1.Stride + y + 3]; //Al
                    oripixel[x, y/4, 1] = rgbvalues1[x * bmpdata1.Stride + y + 2]; //R
                    oripixel[x, y/4, 2] = rgbvalues1[x * bmpdata1.Stride + y + 1]; //G
                    oripixel[x, y/4, 3] = rgbvalues1[x * bmpdata1.Stride + y + 0]; //B
                }
            //NEW RECTANGLE;
            oribitmap.UnlockBits(bmpdata1);

            Rectangle rect2 = new Rectangle(0, 0, ww, hh);           
            System.Drawing.Imaging.BitmapData bmpdata2 = bitmap.LockBits(rect2, System.Drawing.Imaging.ImageLockMode.ReadWrite, bitmap.PixelFormat);
            IntPtr ptr2 = bmpdata2.Scan0;
            int bytes2 = bmpdata2.Stride * bmpdata2.Height;
            byte[] rgbvalues2 = new byte[bytes2];            
            System.Runtime.InteropServices.Marshal.Copy(ptr2, rgbvalues2, 0, bytes2);
            for (int x = 0; x < hh; x++)
            {
                int tx = (int)(x / k2);
                double px = x / k2;
                double qx = px - tx;
                double[] rx = new double[4];
                for (int u = 1; u < 4; u++) rx[u] = Math.Pow(qx, u);
                for (int y = 0; y < ww*4; y+=4)
                {
                    int ty = (int)((y/4) / k1);
                    double py = (y/4) / k1;
                    double qy = py - ty;
                    double[] ry = new double[4];
                    for (int u = 1; u < 4; u++) ry[u] = Math.Pow(qy, u);
                    for (int id = 0; id < 4; id++)
                    {
                        for (int i = -1; i <= 2; i++)
                            for (int j = -1; j <= 2; j++)
                            {
                                int xx = tx + i;
                                if (xx < 0) xx = 0; if (xx >= h) xx = h - 1;
                                int yy = ty + j;
                                if (yy < 0) yy = 0; if (yy >= w) yy = w - 1;
                                if (id == 0) p[i + 1, j + 1] = oripixel[xx, yy, 0];
                                else if (id == 1) p[i + 1, j + 1] = oripixel[xx, yy, 1];
                                    else if (id == 2) p[i + 1, j + 1] = oripixel[xx, yy, 2];
                                        else p[i + 1, j + 1] = oripixel[xx, yy, 3];
                            }
                        a[0, 0] = 1.00 * p[1, 1];
                        a[0, 1] = -0.50 * p[1, 0] + 0.50 * p[1, 2];
                        a[0, 2] = 1.00 * p[1, 0] - 2.50 * p[1, 1] + 2.00 * p[1, 2] - 0.50 * p[1, 3];
                        a[0, 3] = -0.50 * p[1, 0] + 1.50 * p[1, 1] - 1.50 * p[1, 2] + 0.50 * p[1, 3];
                        a[1, 0] = -0.50 * p[0, 1] + 0.50 * p[2, 1];
                        a[1, 1] = 0.25 * p[0, 0] - 0.25 * p[0, 2] - 0.25 * p[2, 0] + 0.25 * p[2, 2];
                        a[1, 2] = -0.50 * p[0, 0] + 1.25 * p[0, 1] - 1.00 * p[0, 2] + 0.25 * p[0, 3] + 0.50 * p[2, 0] - 1.25 * p[2, 1] + 1.00 * p[2, 2] - 0.25 * p[2, 3];
                        a[1, 3] = 0.25 * p[0, 0] - 0.75 * p[0, 1] + 0.75 * p[0, 2] - 0.25 * p[0, 3] - 0.25 * p[2, 0] + 0.75 * p[2, 1] - 0.75 * p[2, 2] + 0.25 * p[2, 3];
                        a[2, 0] = 1.00 * p[0, 1] - 2.50 * p[1, 1] + 2.00 * p[2, 1] - 0.50 * p[3, 1];
                        a[2, 1] = -0.50 * p[0, 0] + 0.50 * p[0, 2] + 1.25 * p[1, 0] - 1.25 * p[1, 2] - 1.00 * p[2, 0] + 1.00 * p[2, 2] + 0.25 * p[3, 0] - 0.25 * p[3, 2];
                        a[2, 2] = 1.00 * p[0, 0] - 2.50 * p[0, 1] + 2.00 * p[0, 2] - 0.50 * p[0, 3] - 2.50 * p[1, 0] + 6.25 * p[1, 1] - 5.00 * p[1, 2] + 1.25 * p[1, 3] + 2.00 * p[2, 0] - 5.00 * p[2, 1] + 4.00 * p[2, 2] - 1.00 * p[2, 3] - 0.50 * p[3, 0] + 1.25 * p[3, 1] - 1.00 * p[3, 2] + 0.25 * p[3, 3];
                        a[2, 3] = -0.50 * p[0, 0] + 1.50 * p[0, 1] - 1.50 * p[0, 2] + 0.50 * p[0, 3] + 1.25 * p[1, 0] - 3.75 * p[1, 1] + 3.75 * p[1, 2] - 1.25 * p[1, 3] - 1.00 * p[2, 0] + 3.00 * p[2, 1] - 3.00 * p[2, 2] + 1.00 * p[2, 3] + 0.25 * p[3, 0] - 0.75 * p[3, 1] + 0.75 * p[3, 2] - 0.25 * p[3, 3];
                        a[3, 0] = -0.50 * p[0, 1] + 1.50 * p[1, 1] - 1.50 * p[2, 1] + 0.50 * p[3, 1];
                        a[3, 1] = 0.25 * p[0, 0] - 0.25 * p[0, 2] - 0.75 * p[1, 0] + 0.75 * p[1, 2] + 0.75 * p[2, 0] - 0.75 * p[2, 2] - 0.25 * p[3, 0] + 0.25 * p[3, 2];
                        a[3, 2] = -0.50 * p[0, 0] + 1.25 * p[0, 1] - 1.00 * p[0, 2] + 0.25 * p[0, 3] + 1.50 * p[1, 0] - 3.75 * p[1, 1] + 3.00 * p[1, 2] - 0.75 * p[1, 3] - 1.50 * p[2, 0] + 3.75 * p[2, 1] - 3.00 * p[2, 2] + 0.75 * p[2, 3] + 0.50 * p[3, 0] - 1.25 * p[3, 1] + 1.00 * p[3, 2] - 0.25 * p[3, 3];
                        a[3, 3] = 0.25 * p[0, 0] - 0.75 * p[0, 1] + 0.75 * p[0, 2] - 0.25 * p[0, 3] - 0.75 * p[1, 0] + 2.25 * p[1, 1] - 2.25 * p[1, 2] + 0.75 * p[1, 3] + 0.75 * p[2, 0] - 2.25 * p[2, 1] + 2.25 * p[2, 2] - 0.75 * p[2, 3] - 0.25 * p[3, 0] + 0.75 * p[3, 1] - 0.75 * p[3, 2] + 0.25 * p[3, 3];
                        pixel[id] = getvalue(a[0, 0] + a[0, 1] * ry[1] + a[0, 2] * ry[2] + a[0, 3] * ry[3] +
                                           (a[1, 0] + a[1, 1] * ry[1] + a[1, 2] * ry[2] + a[1, 3] * ry[3]) * rx[1]
                                           + (a[2, 0] + a[2, 1] * ry[1] + a[2, 2] * ry[2] + a[2, 3] * ry[3]) * rx[2]
                                           + (a[3, 0] + a[3, 1] * ry[1] + a[3, 2] * ry[2] + a[3, 3] * ry[3]) * rx[3]);
                    }
                    rgbvalues2[x * bmpdata2.Stride + y + 3] = pixel[0];
                    rgbvalues2[x * bmpdata2.Stride + y + 2] = pixel[1];
                    rgbvalues2[x * bmpdata2.Stride + y + 1] = pixel[2];
                    rgbvalues2[x * bmpdata2.Stride + y + 0] = pixel[3];
                }
            }       
            System.Runtime.InteropServices.Marshal.Copy(rgbvalues2,0,ptr2,bytes2);
            bitmap.UnlockBits(bmpdata2);
            pictureBox2.Image = bitmap;            
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
                lbl2.Text = filename;
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