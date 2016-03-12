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
    public partial class frmmatch : Form
    {
        string filename;
        System.Drawing.Bitmap bitmap1, oribitmap1,bitmap2,oribitmap2,bitmap,bitmapsave; //bitmapsave is used for save picture
        int[] cntpixel1 = new int[300];
        int[] cntpixel2 = new int[300];
        int[] cntgray = new int[300];
        int[] cntcolorgray = new int[300];
        int[,] hashpixel1 = new int[3, 300];
        int[,] hashpixel2 = new int[3, 300];    
        int[,] mapT = new int[3, 300];
        int[,] mapG = new int[3, 300];
        int[,] appear = new int[3, 300];  //judge if s exists
        int[] maprz = new int[300];
        int[,] maprzRGB = new int[3, 300];
        int[] hash = new int[300];
        int[] T = new int[300];
        int[] G = new int[300];
        int w, h;
        System.Drawing.Imaging.BitmapData bmpdata1;
        byte[] rgbvalues1;
        public frmmatch()
        {
            InitializeComponent();
        }

        private void btnopen1_Click(object sender, EventArgs e) //bitmap1 oribitmap1
        {
            for (int i = 0; i < 300; i++) cntpixel1[i] = T[i]=hash[i]=0;
            for (int i = 0; i < 3; i++) for (int j = 0; j < 300; j++) hashpixel1[i, j] = 0;
            OpenFileDialog opndlg = new OpenFileDialog();
            opndlg.InitialDirectory = "E:\\";
            opndlg.Filter = "所有图像文件 | *.bmp; *.png; *.jpg; *.gif; *.tif; *.ico; *.emf|" + "位图(*.bmp; *.png; *.jpg; *.gif; ...) | *.bmp; *.png; *.jpg; *.gif; *.ico|" + "矢量图(*.emf) | *.emf";
            opndlg.Title = "打开图像文件";
            if (opndlg.ShowDialog() == DialogResult.OK)
            {
                filename = opndlg.FileName;
                try
                {
                    bitmap1 = (Bitmap)Image.FromFile(filename);
                }
                catch (Exception exp)
                {
                    MessageBox.Show(exp.Message);
                }
            }
            lbl1.Text = filename;
            if (bitmap1 == null) return;
            oribitmap1 = new Bitmap(bitmap1);
            h = oribitmap1.Height;
            w = oribitmap1.Width;            
            Rectangle rect1 = new Rectangle(0, 0, w, h);
            bmpdata1 = oribitmap1.LockBits(rect1, System.Drawing.Imaging.ImageLockMode.ReadWrite, oribitmap1.PixelFormat);
            IntPtr ptr1 = bmpdata1.Scan0;
            int bytes1 = bmpdata1.Stride * bmpdata1.Height;
            rgbvalues1 = new byte[bytes1];
            System.Runtime.InteropServices.Marshal.Copy(ptr1, rgbvalues1, 0, bytes1); //copy pixel               
            for (int x = 0; x < h; x++)
                for (int y = 0; y < w * 4; y += 4)
                {
                    int Al = rgbvalues1[x * bmpdata1.Stride + y + 3];
                    int R = rgbvalues1[x * bmpdata1.Stride + y + 2];
                    int G = rgbvalues1[x * bmpdata1.Stride + y + 1];
                    int B = rgbvalues1[x * bmpdata1.Stride + y + 0];
                    int gray = Convert.ToInt32(0.299 * R + 0.587 * G + 0.114 * B);
                    cntpixel1[gray]++; //prepare for calculate pr(rk)
                    hashpixel1[2, R]++;
                    hashpixel1[1, G]++;
                    hashpixel1[0, B]++;
                }
            double N = w * h;
            double[] s = new double[3];
            s[0] = s[1] = s[2] = 0.0;
            double sum = 0.0;
            for (int i = 0; i < 256; i++)
            {
                for (int j = 0; j < 3; j++)
                {
                    s[j] = s[j] + Convert.ToDouble(hashpixel1[j,i]);
                    double tmp = 255.0 * s [j]/ N + 0.5;                    
                    mapT[j, i] = (int)tmp;
                    appear[j, mapT[j,i]] = 1;
                }
                sum = sum + Convert.ToDouble(cntpixel1[i]);
                double res=255.0*sum/N+0.5;
                T[i] = (int)res;
                hash[T[i]] = 1;
            }
            oribitmap1.UnlockBits(bmpdata1);           
        }

        private void btnopen2_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < 300; i++) cntpixel2[i]=G[i]=0;
            for (int i = 0; i < 3; i++) for (int j = 0; j < 300; j++) hashpixel2[i, j] = 0;
            OpenFileDialog opndlg = new OpenFileDialog();
            opndlg.InitialDirectory = "E:\\";
            opndlg.Filter = "所有图像文件 | *.bmp; *.png; *.jpg; *.gif; *.tif; *.ico; *.emf|" + "位图(*.bmp; *.png; *.jpg; *.gif; ...) | *.bmp; *.png; *.jpg; *.gif; *.ico|" + "矢量图(*.emf) | *.emf";
            opndlg.Title = "打开图像文件";
            if (opndlg.ShowDialog() == DialogResult.OK)
            {
                filename = opndlg.FileName;
                try
                {
                    bitmap2 = (Bitmap)Image.FromFile(filename);
                }
                catch (Exception exp)
                {
                    MessageBox.Show(exp.Message);
                }
            }
            if (bitmap2 == null) return;
            lbl2.Text = filename;
            oribitmap2 = new Bitmap(bitmap2);
            int h2 = oribitmap2.Height, w2 = oribitmap2.Width;
            Rectangle rect2 = new Rectangle(0, 0, w2, h2);
            System.Drawing.Imaging.BitmapData bmpdata2 = oribitmap2.LockBits(rect2, System.Drawing.Imaging.ImageLockMode.ReadWrite, oribitmap2.PixelFormat);
            IntPtr ptr2 = bmpdata2.Scan0;
            int bytes2 = bmpdata2.Stride * bmpdata2.Height;
            byte[] rgbvalues2 = new byte[bytes2];
            System.Runtime.InteropServices.Marshal.Copy(ptr2, rgbvalues2, 0, bytes2); //copy pixel               
            for (int x = 0; x < h2; x++)
                for (int y = 0; y < w2 * 4; y += 4)
                {
                    int Al = rgbvalues2[x * bmpdata2.Stride + y + 3];
                    int R = rgbvalues2[x * bmpdata2.Stride + y + 2];
                    int G = rgbvalues2[x * bmpdata2.Stride + y + 1];
                    int B = rgbvalues2[x * bmpdata2.Stride + y + 0];
                    int gray = Convert.ToInt32(0.299 * R + 0.587 * G + 0.114 * B);
                    cntpixel2[gray]++; //prepare for calculate pr(rk)
                    hashpixel2[2, R]++;
                    hashpixel2[1, G]++;
                    hashpixel2[0, B]++;
                }
            double N = w * h;           
            double[] s = new double[3];
            s[0] = s[1] = s[2] = 0.0;
            double sum = 0.0;
            for (int i = 0; i < 256; i++)
            {
                for (int j = 0; j < 3; j++)
                {
                    s[j] = s[j] + Convert.ToDouble(hashpixel2[j, i]);
                    double tmp = 255.0 * s[j] / N + 0.5;
                    mapG[j, i] = (int)tmp;                    
                }
                sum = sum + Convert.ToDouble(cntpixel2[i]);
                double res = 255.0 * sum / N + 0.5;
                G[i] = (int)res;                
            }
            oribitmap2.UnlockBits(bmpdata2);

            //paint the origin histogram
            Graphics g = this.CreateGraphics();
            Pen curpen = new Pen(Brushes.Black, 2);
            g.DrawLine(curpen, 150, 10, 150, 500);
            g.DrawLine(curpen, 150, 500, 700, 500);
            for (int i = 1; i < 256; i++)
            {
                g.DrawLine(new Pen(this.BackColor, 2), 150 + i * 2, 500, 150 + i * 2, 10);
            }
            g.DrawString("0", new Font("Arial", 8), Brushes.Black, new PointF(140, 510));
            for (int i = 1; i <= 17; i++)
            {
                g.DrawLine(curpen, 150 + 30 * i, 500, 150 + 30 * i, 510);
                g.DrawString(Convert.ToString(i * 15), new Font("Arial", 8), Brushes.Black, new PointF(140 + 30 * i, 510));
            }
            double maxpixel = 0.0;
            for (int i = 0; i < 256; i++) if (cntpixel2[i] > maxpixel) maxpixel = cntpixel2[i];
            for (int i = 0; i < 256; i++)
            {
                double tmp = 480.0 * (Convert.ToDouble(cntpixel2[i]) / maxpixel) + 0.5;
                g.DrawLine(curpen, 150 + i * 2, 500, 150 + i * 2, 500 - (int)tmp);
            }
            g.DrawString(maxpixel.ToString(), new Font("Arial", 8), Brushes.Black, new PointF(150, 20));            
        }

        private void btnmatch_Click(object sender, EventArgs e)
        {
            if (bitmap1 == null || bitmap2 == null) return;
            for (int i = 0; i < 300; i++) maprz[i] =cntgray[i]= 0;
            for (int i = 0; i < 256; i++)
            {
                if (hash[i] != 0) //s=T(r) exists   for each s find the min z such that G(z) is closet to s
                {
                   double cmp = 10000.0;
                   int num = -1;
                   for (int j = 0; j < 256; j++)
                   {
                       double res = Math.Abs(G[j] - i);
                       if (cmp - res > 1e-8)
                       {
                          cmp = res;
                          num = j;
                       }
                   } // map s -> z    (i -> num)
                   for (int j = 0; j < 256; j++)
                   {
                       if (T[j] == i) maprz[j] = num;
                   }
                }
            }
            bitmap = new Bitmap(bitmap1);
            Rectangle rect = new Rectangle(0, 0, w, h);
            System.Drawing.Imaging.BitmapData bmpdata = bitmap.LockBits(rect, System.Drawing.Imaging.ImageLockMode.ReadWrite, bitmap.PixelFormat);
            IntPtr ptr = bmpdata.Scan0;
            int bytes = bmpdata.Stride * bmpdata.Height;
            byte[] rgbvalues = new byte[bytes];
            System.Runtime.InteropServices.Marshal.Copy(ptr, rgbvalues, 0, bytes); //copy pixel    

            bitmapsave = new Bitmap(w, h);
            Rectangle rect1 = new Rectangle(0, 0, w, h);
            System.Drawing.Imaging.BitmapData bmpdata1 = bitmapsave.LockBits(rect1, System.Drawing.Imaging.ImageLockMode.ReadWrite, bitmapsave.PixelFormat);
            IntPtr ptr1 = bmpdata1.Scan0;
            int bytes1 = bmpdata1.Stride * bmpdata1.Height;
            byte[] rgbvalues1 = new byte[bytes1];
            for (int x = 0; x < h; x++)
                for (int y = 0; y < w * 4; y += 4)
                {
                    int Al = rgbvalues[x * bmpdata.Stride + y + 3];
                    int R = rgbvalues[x * bmpdata.Stride + y + 2];
                    int G = rgbvalues[x * bmpdata.Stride + y + 1];
                    int B = rgbvalues[x * bmpdata.Stride + y + 0];
                    int gray = Convert.ToInt32(0.299 * R + 0.587 * G + 0.114 * B);
                    gray = maprz[gray];
                    cntgray[gray]++;
                    rgbvalues1[x * bmpdata1.Stride + y + 3] = (byte)Al;
                    rgbvalues1[x * bmpdata1.Stride + y + 2] = (byte)gray;
                    rgbvalues1[x * bmpdata1.Stride + y + 1] = (byte)gray;
                    rgbvalues1[x * bmpdata1.Stride + y + 0] = (byte)gray;                    
                }
            bitmap.UnlockBits(bmpdata);
            System.Runtime.InteropServices.Marshal.Copy(rgbvalues1, 0, ptr1, bytes1);
            bitmapsave.UnlockBits(bmpdata1);

            //paint the processed histogram
            Graphics g = this.CreateGraphics();
            Pen curpen = new Pen(Brushes.Black, 2);
            g.DrawLine(curpen, 750, 10, 750, 500);
            g.DrawLine(curpen, 750, 500, 1300, 500);
            for (int i = 1; i < 256; i++)
            {
                g.DrawLine(new Pen(this.BackColor, 2), 750 + i * 2, 500, 750 + i * 2, 10);
            }
            g.DrawString("0", new Font("Arial", 8), Brushes.Black, new PointF(740, 510));
            for (int i = 1; i <= 17; i++)
            {
                g.DrawLine(curpen, 750 + 30 * i, 500, 750 + 30 * i, 510);
                g.DrawString(Convert.ToString(i * 15), new Font("Arial", 8), Brushes.Black, new PointF(740 + 30 * i, 510));
            }
            double maxpixel = 0.0;
            for (int i = 0; i < 256; i++) if (cntgray[i] > maxpixel) maxpixel = cntgray[i];
            for (int i = 0; i < 256; i++)
            {
                double tmp = 480.0 * (Convert.ToDouble(cntgray[i]) / maxpixel) + 0.5;
                g.DrawLine(curpen, 750 + i * 2, 500, 750 + i * 2, 500 - (int)tmp);
            }
            g.DrawString(maxpixel.ToString(), new Font("Arial", 8), Brushes.Black, new PointF(750, 20));      

            MessageBox.Show("已完成灰度匹配");
        }

        private void btnsave_Click(object sender, EventArgs e)
        {
            if (bitmapsave == null) return;
            SaveFileDialog savedlg = new SaveFileDialog();
            savedlg.Title = "另存为";
            savedlg.OverwritePrompt = true;
            savedlg.Filter = "BMP文件(*.bmp) | *.bmp|" + "GIF文件(*.gif) | *.gif|" + "JPEG文件(*.jpg) | *.jpg|" + "PNG文件(*.png) | *.png";
            if (savedlg.ShowDialog() == DialogResult.OK)
            {
                string filename = savedlg.FileName;
                string tmp = filename.Remove(0, filename.Length - 3);
                lbl3.Text = filename;
                switch (tmp)
                {
                    case "bmp":
                        bitmapsave.Save(filename, System.Drawing.Imaging.ImageFormat.Bmp);
                        break;
                    case "jpg":
                        bitmapsave.Save(filename, System.Drawing.Imaging.ImageFormat.Jpeg);
                        break;
                    case "gif":
                        bitmapsave.Save(filename, System.Drawing.Imaging.ImageFormat.Gif);
                        break;
                    case "png":
                        bitmapsave.Save(filename, System.Drawing.Imaging.ImageFormat.Png);
                        break;
                    default:
                        break;
                }
            }
        }

        private void btncolor_Click(object sender, EventArgs e)
        {
            if (bitmap1 == null || bitmap2 == null) return;
            for (int i = 0; i < 300; i++) for (int j = 0; j < 3; j++) maprzRGB[j, i] = 0;
            for (int i = 0; i < 300; i++) cntcolorgray[i] = 0;
            for (int i = 0; i < 256; i++)
            {
                for (int j = 0; j < 3; j++)
                {
                    if (appear[j, i] != 0) //s=T(r) exists (RGB fenliang) for each s find the min z such that G(z) is closet to s
                    {
                        double cmp = 10000.0;
                        int num = -1;
                        for (int u = 0; u < 256; u++)
                        {
                            double res = Math.Abs(mapG[j, u] - i);
                            if (cmp - res > 1e-8)
                            {
                                cmp = res;
                                num = u;
                            }
                        } // map s -> z    (i -> num)
                        for (int u = 0; u < 256; u++)
                        {
                            if (mapT[j, u] == i) maprzRGB[j, u] = num;
                        }
                    }
                }
            }
            bitmap = new Bitmap(bitmap1);
            Rectangle rect = new Rectangle(0, 0, w, h);
            System.Drawing.Imaging.BitmapData bmpdata = bitmap.LockBits(rect, System.Drawing.Imaging.ImageLockMode.ReadWrite, bitmap.PixelFormat);
            IntPtr ptr = bmpdata.Scan0;
            int bytes = bmpdata.Stride * bmpdata.Height;
            byte[] rgbvalues = new byte[bytes];
            System.Runtime.InteropServices.Marshal.Copy(ptr, rgbvalues, 0, bytes); //copy pixel    

            bitmapsave = new Bitmap(w, h);
            Rectangle rect1 = new Rectangle(0, 0, w, h);
            System.Drawing.Imaging.BitmapData bmpdata1 = bitmapsave.LockBits(rect1, System.Drawing.Imaging.ImageLockMode.ReadWrite, bitmapsave.PixelFormat);
            IntPtr ptr1 = bmpdata1.Scan0;
            int bytes1 = bmpdata1.Stride * bmpdata1.Height;
            byte[] rgbvalues1 = new byte[bytes1];
            for (int x = 0; x < h; x++)
                for (int y = 0; y < w * 4; y += 4)
                {
                    int R=0, G=0, B=0;
                    for (int i = 0; i < 3; i++)
                    {
                        int RGB = rgbvalues[x * bmpdata.Stride + y + i];
                        RGB = maprzRGB[i, RGB];
                        rgbvalues1[x * bmpdata1.Stride + y + i] = (byte)RGB;
                        switch (i)
                        {
                            case 2: R = RGB; break;
                            case 1: G = RGB; break;
                            case 0: B = RGB; break;
                        }
                    }
                    int Al = rgbvalues[x * bmpdata.Stride + y + 3];
                    rgbvalues1[x * bmpdata1.Stride + y + 3] = (byte)Al;

                    int gray = Convert.ToInt32(0.299 *R + 0.587 * G + 0.114 * B);                    
                    cntcolorgray[gray]++;
                }
            bitmap.UnlockBits(bmpdata);
            System.Runtime.InteropServices.Marshal.Copy(rgbvalues1, 0, ptr1, bytes1);
            bitmapsave.UnlockBits(bmpdata1);

            //paint the processed histogram
            Graphics g = this.CreateGraphics();
            Pen curpen = new Pen(Brushes.Black, 2);
            g.DrawLine(curpen, 750, 10, 750, 500);
            g.DrawLine(curpen, 750, 500, 1300, 500);
            for (int i = 1; i < 256; i++)
            {
                g.DrawLine(new Pen(this.BackColor, 2), 750 + i * 2, 500, 750 + i * 2, 10);
            }
            g.DrawString("0", new Font("Arial", 8), Brushes.Black, new PointF(740, 510));
            for (int i = 1; i <= 17; i++)
            {
                g.DrawLine(curpen, 750 + 30 * i, 500, 750 + 30 * i, 510);
                g.DrawString(Convert.ToString(i * 15), new Font("Arial", 8), Brushes.Black, new PointF(740 + 30 * i, 510));
            }
            double maxpixel = 0.0;
            for (int i = 0; i < 256; i++) if (cntcolorgray[i] > maxpixel) maxpixel = cntcolorgray[i];
            for (int i = 0; i < 256; i++)
            {
                double tmp = 480.0 * (Convert.ToDouble(cntcolorgray[i]) / maxpixel) + 0.5;
                g.DrawLine(curpen, 750 + i * 2, 500, 750 + i * 2, 500 - (int)tmp);
            }
            g.DrawString(maxpixel.ToString(), new Font("Arial", 8), Brushes.Black, new PointF(750, 20));      
            MessageBox.Show("已完成彩色匹配");
        }
    }
}
