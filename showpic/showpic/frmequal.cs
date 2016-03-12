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
    public partial class frmequal : Form
    {
        string filename;
        System.Drawing.Bitmap bitmap, oribitmap;
        int[,] hashpixel = new int[3, 300];
        int[] cntpixel = new int[300];
        int[] cntcol = new int[300];
        int[] count = new int[300];
        int[] trans = new int[300];
        int[,] map = new int[3, 300];
        double[] pk = new double[300];
        public frmequal()
        {
            InitializeComponent();
        }

        private void btnopen_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < 300; i++) cntpixel[i] = 0;
            for (int i = 0; i < 300; i++) count[i] = 0;
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
            oribitmap = new Bitmap(bitmap);
            int h = oribitmap.Height, w = oribitmap.Width;
            Rectangle rect1 = new Rectangle(0, 0, w, h);
            System.Drawing.Imaging.BitmapData bmpdata1 = oribitmap.LockBits(rect1, System.Drawing.Imaging.ImageLockMode.ReadWrite, oribitmap.PixelFormat);
            IntPtr ptr1 = bmpdata1.Scan0;
            int bytes1 = bmpdata1.Stride * bmpdata1.Height;
            byte[] rgbvalues1 = new byte[bytes1];
            System.Runtime.InteropServices.Marshal.Copy(ptr1, rgbvalues1, 0, bytes1); //copy pixel            

            bitmap = new Bitmap(w, h);
            Rectangle rect2 = new Rectangle(0, 0, w, h);
            System.Drawing.Imaging.BitmapData bmpdata2 = bitmap.LockBits(rect2, System.Drawing.Imaging.ImageLockMode.ReadWrite, bitmap.PixelFormat);
            IntPtr ptr2 = bmpdata2.Scan0;
            int bytes2 = bmpdata2.Stride * bmpdata2.Height;
            byte[] rgbvalues2 = new byte[bytes2];

            for (int x = 0; x < h; x++)
                for (int y = 0; y < w * 4; y += 4)
                {
                    int Al = rgbvalues1[x * bmpdata1.Stride + y + 3];
                    int R = rgbvalues1[x * bmpdata1.Stride + y + 2];
                    int G = rgbvalues1[x * bmpdata1.Stride + y + 1];
                    int B = rgbvalues1[x * bmpdata1.Stride + y + 0];
                    int gray = Convert.ToInt32(0.299 * R + 0.587 * G + 0.114 * B);
                    cntpixel[gray]++;
                }
            double N = w * h;
            double s = 0.0;
            for (int i = 0; i < 256; i++)
            {
                s = s + Convert.ToDouble(cntpixel[i]);
                double tmp = 255.0 * s / N+0.5;
                trans[i] = (int)tmp;
            }

            oribitmap.UnlockBits(bmpdata1);

            for (int x = 0; x < h; x++)
                for (int y = 0; y < w * 4; y += 4)
                {
                    int Al = rgbvalues1[x * bmpdata1.Stride + y + 3];
                    int R = rgbvalues1[x * bmpdata1.Stride + y + 2];
                    int G = rgbvalues1[x * bmpdata1.Stride + y + 1];
                    int B = rgbvalues1[x * bmpdata1.Stride + y + 0];
                    int gray = Convert.ToInt32(0.299 * R + 0.587 * G + 0.114 * B);
                    gray = trans[gray];
                    rgbvalues2[x * bmpdata2.Stride + y + 3] = (byte)Al;
                    rgbvalues2[x * bmpdata2.Stride + y + 2] = (byte)gray;
                    rgbvalues2[x * bmpdata2.Stride + y + 1] = (byte)gray;
                    rgbvalues2[x * bmpdata2.Stride + y + 0] = (byte)gray;
                    count[gray]++;
                }
            System.Runtime.InteropServices.Marshal.Copy(rgbvalues2, 0, ptr2, bytes2);
            bitmap.UnlockBits(bmpdata2);
            //----------------------------------------------------------------paint
            Graphics g = this.CreateGraphics();
            Pen curpen = new Pen(Brushes.Black, 2);
            g.DrawLine(curpen, 180, 10, 180, 620);
            g.DrawLine(curpen, 180, 620, 760, 620);
            for (int i = 1; i < 256; i++)
            {
                g.DrawLine(new Pen(this.BackColor, 2), 180 + i * 2, 620, 180 + i * 2, 10);
            }
            g.DrawString("0", new Font("Arial", 8), Brushes.Black, new PointF(172, 630));
            for (int i = 1; i <= 17; i++)
            {
                g.DrawLine(curpen, 180 + 30 * i, 620, 180 + 30 * i, 630);
                g.DrawString(Convert.ToString(i * 15), new Font("Arial", 8), Brushes.Black, new PointF(170 + 30 * i, 630));
            }
            double maxpixel = 0.0;
            for (int i = 0; i < 256; i++) if (count[i] > maxpixel) maxpixel = count[i];
            for (int i = 0; i < 256; i++)
            {
                double tmp = 600.0 * (Convert.ToDouble(count[i]) / maxpixel) + 0.5;
                g.DrawLine(curpen, 180 + i * 2, 620, 180 + i * 2, 620 - (int)tmp);
            }         
            g.DrawString(maxpixel.ToString(), new Font("Arial", 8), Brushes.Black, new PointF(180, 20));
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

        private void btnopencol_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < 300; i++) hashpixel[0,i] = hashpixel[1,i] = hashpixel[2,i] =cntcol[i]= 0;
            for (int i = 0; i < 300; i++) count[i] = 0;
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
            oribitmap = new Bitmap(bitmap);
            int h = oribitmap.Height, w = oribitmap.Width;
            Rectangle rect1 = new Rectangle(0, 0, w, h);
            System.Drawing.Imaging.BitmapData bmpdata1 = oribitmap.LockBits(rect1, System.Drawing.Imaging.ImageLockMode.ReadWrite, oribitmap.PixelFormat);
            IntPtr ptr1 = bmpdata1.Scan0;
            int bytes1 = bmpdata1.Stride * bmpdata1.Height;
            byte[] rgbvalues1 = new byte[bytes1];
            System.Runtime.InteropServices.Marshal.Copy(ptr1, rgbvalues1, 0, bytes1); //copy pixel            

            bitmap = new Bitmap(w, h);
            Rectangle rect2 = new Rectangle(0, 0, w, h);
            System.Drawing.Imaging.BitmapData bmpdata2 = bitmap.LockBits(rect2, System.Drawing.Imaging.ImageLockMode.ReadWrite, bitmap.PixelFormat);
            IntPtr ptr2 = bmpdata2.Scan0;
            int bytes2 = bmpdata2.Stride * bmpdata2.Height;
            byte[] rgbvalues2 = new byte[bytes2];

            for (int x = 0; x < h; x++)
                for (int y = 0; y < w * 4; y += 4)
                {
                    int Al = rgbvalues1[x * bmpdata1.Stride + y + 3];
                    int R = rgbvalues1[x * bmpdata1.Stride + y + 2];
                    int G = rgbvalues1[x * bmpdata1.Stride + y + 1];
                    int B = rgbvalues1[x * bmpdata1.Stride + y + 0];
                    hashpixel[2,R]++;
                    hashpixel[1,G]++;
                    hashpixel[0,B]++;                    
                }
            double N = w * h;
            double[] s = new double[3]; s[0] = s[1] = s[2] = 0.0;
            for (int i = 0; i < 256; i++)
            {
                for (int j = 0; j < 3; j++)
                {
                    s[j] = s[j] + Convert.ToDouble(hashpixel[j, i]);
                    double tmp = 255.0 * s[j] / N + 0.5;
                    map[j,i] = (int)tmp;
                }
            }

            oribitmap.UnlockBits(bmpdata1);

            for (int x = 0; x < h; x++)
                for (int y = 0; y < w * 4; y += 4)
                {
                    for (int i = 0; i < 4; i++)
                    {
                        int tmp = rgbvalues1[x * bmpdata1.Stride + y + i];
                        if (i == 3) { rgbvalues2[x * bmpdata2.Stride + y + 3] = (byte)tmp; break; }
                        tmp = map[i, tmp];
                        rgbvalues2[x * bmpdata2.Stride + y + i] = (byte)tmp;
                    }
                    int gray = Convert.ToInt32(0.299 * rgbvalues2[x * bmpdata2.Stride + y +2] + 0.587 * rgbvalues2[x * bmpdata2.Stride + y + 1] + 0.114 * rgbvalues2[x * bmpdata2.Stride + y + 0]);
                    cntcol[gray]++;
                }
            System.Runtime.InteropServices.Marshal.Copy(rgbvalues2, 0, ptr2, bytes2);
            bitmap.UnlockBits(bmpdata2);           
            //-------------------------------paint the histogram of the equalized histogram
            Graphics g = this.CreateGraphics();
            Pen curpen = new Pen(Brushes.Black, 2);
            g.DrawLine(curpen, 180, 10, 180, 620);
            g.DrawLine(curpen, 180, 620, 760, 620);
            for (int i = 1; i < 256; i++)
            {
                g.DrawLine(new Pen(this.BackColor, 2), 180 + i * 2, 620, 180 + i * 2, 10);
            }
            g.DrawString("0", new Font("Arial", 8), Brushes.Black, new PointF(172, 630));
            for (int i = 1; i <= 17; i++)
            {
                g.DrawLine(curpen, 180 + 30 * i, 620, 180 + 30 * i, 630);
                g.DrawString(Convert.ToString(i * 15), new Font("Arial", 8), Brushes.Black, new PointF(170 + 30 * i, 630));
            }
            double maxpixel = 0.0;
            for (int i = 0; i < 256; i++) if (cntcol[i] > maxpixel) maxpixel = cntcol[i];
            for (int i = 0; i < 256; i++)
            {
                double tmp = 600.0 * (Convert.ToDouble(cntcol[i]) / maxpixel) + 0.5;
                g.DrawLine(curpen, 180 + i * 2, 620, 180 + i * 2, 620 - (int)tmp);
            }
            g.DrawString(maxpixel.ToString(), new Font("Arial", 8), Brushes.Black, new PointF(180, 20));
        }

        private void frmequal_Load(object sender, EventArgs e)
        {

        }
    }
}
