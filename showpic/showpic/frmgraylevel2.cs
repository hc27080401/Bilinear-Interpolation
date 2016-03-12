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
    public partial class frmgraylevel2 : Form
    {
        string filename;
        System.Drawing.Bitmap bitmap, oribitmap;
        public frmgraylevel2()
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

        private byte slice1(int r,int A,int B,int num1,int num2)
        {
            if (r < A || r>B) return (byte)num1;
            return (byte)num2;
        }

        private byte slice2(int r, int A, int B, int num)
        {
            if (r < A || r > B) return (byte)r;
            return (byte)num;
        }

        private void btnslice1_Click(object sender, EventArgs e)
        {
            int a = Convert.ToInt32(txta.Text);
            int b = Convert.ToInt32(txtb.Text);
            int num1 = Convert.ToInt32(txtnum1.Text);
            int num2 = Convert.ToInt32(txtnum2.Text);
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
                    rgbvalues2[x * bmpdata2.Stride + y + 2] = slice1(R,a,b,num1,num2);
                    rgbvalues2[x * bmpdata2.Stride + y + 1] = slice1(G,a,b,num1,num2);
                    rgbvalues2[x * bmpdata2.Stride + y + 0] = slice1(B,a,b,num1,num2);
                }
            //NEW RECTANGLE;
            oribitmap.UnlockBits(bmpdata1);
            System.Runtime.InteropServices.Marshal.Copy(rgbvalues2, 0, ptr2, bytes2);
            bitmap.UnlockBits(bmpdata2);
            pictureBox2.Image = bitmap;
        }

        private void btnslice2_Click(object sender, EventArgs e)
        {
            int a = Convert.ToInt32(txta1.Text);
            int b = Convert.ToInt32(txtb1.Text);            
            int num = Convert.ToInt32(txtnum.Text);
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
                    rgbvalues2[x * bmpdata2.Stride + y + 2] = slice2(R, a, b, num);
                    rgbvalues2[x * bmpdata2.Stride + y + 1] = slice2(G, a, b, num);
                    rgbvalues2[x * bmpdata2.Stride + y + 0] = slice2(B, a, b, num);
                }
            //NEW RECTANGLE;
            oribitmap.UnlockBits(bmpdata1);
            System.Runtime.InteropServices.Marshal.Copy(rgbvalues2, 0, ptr2, bytes2);
            bitmap.UnlockBits(bmpdata2);
            pictureBox2.Image = bitmap;
        }
    }
}
