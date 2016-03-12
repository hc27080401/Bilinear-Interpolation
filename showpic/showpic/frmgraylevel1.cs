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
    public partial class frmgraylevel1 : Form
    {
        string filename;
        System.Drawing.Bitmap bitmap, oribitmap;       
        public frmgraylevel1()
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

        private byte getvalue(double tmp)
        {
            if (tmp < 0) return 0;
            if (tmp > 255) return 255;
            return (byte)tmp;
        }

        private byte contrast(int r,double r1,double s1,double r2,double s2)
        {
            double tmp;
            if (r < r1) tmp = s1 / r1 * r;
            else if (r1 <= r && r <= r2) tmp = (s2 - s1) / (r2 - r1) * (r - r1) + s1;
            else tmp = (255 - s2) / (255 - r2) * (r - r2) + s2;
            return getvalue(tmp);
        }
        
        private void btnlog_Click(object sender, EventArgs e)
        {
            double c = Convert.ToDouble(textBox1.Text);
            if (oribitmap == null) { MessageBox.Show("请先加载图片", "出错", MessageBoxButtons.OK, MessageBoxIcon.Warning); return; }         
            int h = oribitmap.Height, w = oribitmap.Width;
            bitmap = new Bitmap(w, h);        

            Rectangle rect1 = new Rectangle(0, 0, w, h);
            System.Drawing.Imaging.BitmapData bmpdata1 = oribitmap.LockBits(rect1, System.Drawing.Imaging.ImageLockMode.ReadWrite, oribitmap.PixelFormat);
            IntPtr ptr1 = bmpdata1.Scan0;
            int bytes1 = bmpdata1.Stride * bmpdata1.Height;
            byte[] rgbvalues1 = new byte[bytes1];
            System.Runtime.InteropServices.Marshal.Copy(ptr1, rgbvalues1, 0, bytes1); //copy pixel

            Rectangle rect2 = new Rectangle(0, 0, w, h);
            System.Drawing.Imaging.BitmapData bmpdata2 = bitmap.LockBits(rect2, System.Drawing.Imaging.ImageLockMode.ReadWrite, bitmap.PixelFormat);
            IntPtr ptr2 = bmpdata2.Scan0;
            int bytes2 = bmpdata2.Stride * bmpdata2.Height;
            byte[] rgbvalues2 = new byte[bytes2];
            for (int x = 0; x < h; x++)
                for (int y = 0; y < w * 4; y += 4)
                {
                    int Al = rgbvalues1[x * bmpdata1.Stride + y + 3];
                    int R= rgbvalues1[x * bmpdata1.Stride + y + 2];
                    int G = rgbvalues1[x * bmpdata1.Stride + y + 1];
                    int B = rgbvalues1[x * bmpdata1.Stride + y + 0];
                    rgbvalues2[x * bmpdata2.Stride + y + 3] = (byte)Al;
                    rgbvalues2[x * bmpdata2.Stride + y + 2] = getvalue(c*Math.Log(R+1,2));
                    rgbvalues2[x * bmpdata2.Stride + y + 1] = getvalue(c*Math.Log(G+1,2));
                    rgbvalues2[x * bmpdata2.Stride + y + 0] = getvalue(c*Math.Log(B+1,2));
                }
            //NEW RECTANGLE;
            oribitmap.UnlockBits(bmpdata1);
            System.Runtime.InteropServices.Marshal.Copy(rgbvalues2, 0, ptr2, bytes2);
            bitmap.UnlockBits(bmpdata2);
            pictureBox2.Image =bitmap;       
        }

        private void btngamma_Click(object sender, EventArgs e)
        {
            double c = 255;
            double gamma = Convert.ToDouble(textBox3.Text);
            if (oribitmap == null) { MessageBox.Show("请先加载图片", "出错", MessageBoxButtons.OK, MessageBoxIcon.Warning); return; }
            int h = oribitmap.Height, w = oribitmap.Width;
            bitmap = new Bitmap(w, h);

            Rectangle rect1 = new Rectangle(0, 0, w, h);
            System.Drawing.Imaging.BitmapData bmpdata1 = oribitmap.LockBits(rect1, System.Drawing.Imaging.ImageLockMode.ReadWrite, oribitmap.PixelFormat);
            IntPtr ptr1 = bmpdata1.Scan0;
            int bytes1 = bmpdata1.Stride * bmpdata1.Height;
            byte[] rgbvalues1 = new byte[bytes1];
            System.Runtime.InteropServices.Marshal.Copy(ptr1, rgbvalues1, 0, bytes1); //copy pixel

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
                    rgbvalues2[x * bmpdata2.Stride + y + 3] = (byte)Al;
                    rgbvalues2[x * bmpdata2.Stride + y + 2] = getvalue(c * Math.Pow(R/255.0,gamma));
                    rgbvalues2[x * bmpdata2.Stride + y + 1] = getvalue(c * Math.Pow(G/255.0,gamma));
                    rgbvalues2[x * bmpdata2.Stride + y + 0] = getvalue(c * Math.Pow(B/255.0, gamma));
                }
            //NEW RECTANGLE;
            oribitmap.UnlockBits(bmpdata1);
            System.Runtime.InteropServices.Marshal.Copy(rgbvalues2, 0, ptr2, bytes2);
            bitmap.UnlockBits(bmpdata2);
            pictureBox2.Image = bitmap;       
        }



        private void btnneg_Click(object sender, EventArgs e)
        {
            if (oribitmap == null) { MessageBox.Show("请先加载图片", "出错", MessageBoxButtons.OK, MessageBoxIcon.Warning); return; }
            int h = oribitmap.Height, w = oribitmap.Width;
            bitmap = new Bitmap(w, h);

            Rectangle rect1 = new Rectangle(0, 0, w, h);
            System.Drawing.Imaging.BitmapData bmpdata1 = oribitmap.LockBits(rect1, System.Drawing.Imaging.ImageLockMode.ReadWrite, oribitmap.PixelFormat);
            IntPtr ptr1 = bmpdata1.Scan0;
            int bytes1 = bmpdata1.Stride * bmpdata1.Height;
            byte[] rgbvalues1 = new byte[bytes1];
            System.Runtime.InteropServices.Marshal.Copy(ptr1, rgbvalues1, 0, bytes1); //copy pixel

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
                    //if (x % 10 == 0) MessageBox.Show(Convert.ToString( R));
                    rgbvalues2[x * bmpdata2.Stride + y + 3] =(byte)Al; //Al
                    rgbvalues2[x * bmpdata2.Stride + y + 2] = (byte)(255-R); //R
                    rgbvalues2[x * bmpdata2.Stride + y + 1] = (byte)(255-G); //G
                    rgbvalues2[x * bmpdata2.Stride + y + 0] = (byte)(255-B); //B
                }
            //NEW RECTANGLE;
            oribitmap.UnlockBits(bmpdata1);
            System.Runtime.InteropServices.Marshal.Copy(rgbvalues2, 0, ptr2, bytes2);
            bitmap.UnlockBits(bmpdata2);
            pictureBox2.Image = bitmap;
        }

        private void btncontrast_Click(object sender, EventArgs e)
        {
            if (oribitmap == null) { MessageBox.Show("请先加载图片", "出错", MessageBoxButtons.OK, MessageBoxIcon.Warning); return; }
            double r1, r2, s1, s2;
            r1 = Convert.ToDouble(txtr1.Text);
            r2 = Convert.ToDouble(txtr2.Text);
            s1 = Convert.ToDouble(txts1.Text);
            s2 = Convert.ToDouble(txts2.Text);
            if (r1<0 || r1>255 || r2<0 || r2>255 || s1<0 || s1>255 || s2<0 || s2>255) { MessageBox.Show("请重新输入r1,r2,s1,s2", "出错", MessageBoxButtons.OK, MessageBoxIcon.Exclamation); return; }
            int h = oribitmap.Height, w = oribitmap.Width;
            bitmap = new Bitmap(w, h);

            Rectangle rect1 = new Rectangle(0, 0, w, h);
            System.Drawing.Imaging.BitmapData bmpdata1 = oribitmap.LockBits(rect1, System.Drawing.Imaging.ImageLockMode.ReadWrite, oribitmap.PixelFormat);
            IntPtr ptr1 = bmpdata1.Scan0;
            int bytes1 = bmpdata1.Stride * bmpdata1.Height;
            byte[] rgbvalues1 = new byte[bytes1];
            System.Runtime.InteropServices.Marshal.Copy(ptr1, rgbvalues1, 0, bytes1); //copy pixel

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
                    rgbvalues2[x * bmpdata2.Stride + y + 3] = (byte)Al;
                    rgbvalues2[x * bmpdata2.Stride + y + 2] = contrast(R,r1,s1,r2,s2);
                    rgbvalues2[x * bmpdata2.Stride + y + 1] = contrast(G,r1,s1,r2,s2);
                    rgbvalues2[x * bmpdata2.Stride + y + 0] = contrast(B,r1,s1,r2,s2);
                }
            //NEW RECTANGLE;
            oribitmap.UnlockBits(bmpdata1);
            System.Runtime.InteropServices.Marshal.Copy(rgbvalues2, 0, ptr2, bytes2);
            bitmap.UnlockBits(bmpdata2);
            pictureBox2.Image = bitmap;

        }

    }
}
