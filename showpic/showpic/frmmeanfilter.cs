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
    public partial class frmmeanfilter : Form
    {
        string filename;
        System.Drawing.Bitmap bitmap, oribitmap;
        public frmmeanfilter()
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
        private void btnstart_Click(object sender, EventArgs e)
        {
            int m = Convert.ToInt32(txtm.Text);
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
                    int sumR = 0,sumG=0,sumB=0;
                    for (int a=-(m-1)/2;a<=(m-1)/2;a++)
                        for (int b=-(m-1)/2;b<=(m-1)/2;b++)
                        {
                            int u = x + a, v = y / 4 + b,pR,pG,pB;
                            if (u >= h || u < 0 || v >= w || v < 0) pR=pG=pB= 0;
                            else
                            {                                
                                pR = rgbvalues1[u * bmpdata1.Stride + v*4 + 2];
                                pG = rgbvalues1[u * bmpdata1.Stride + v*4 + 1];
                                pB = rgbvalues1[u * bmpdata1.Stride + v*4 + 0];                                
                            }
                            sumR += pR; sumG += pG; sumB+=pB;
                        }
                    sumR = (int)((double)sumR / (double)(m * m)+0.5);
                    sumG = (int)((double)sumG / (double)(m * m) + 0.5);
                    sumB = (int)((double)sumB / (double)(m * m) + 0.5);
                    int Al = rgbvalues1[x * bmpdata1.Stride + y + 3];                    
                    rgbvalues2[x * bmpdata2.Stride + y + 3] = (byte)Al;
                    rgbvalues2[x * bmpdata2.Stride + y + 2] = (byte)sumR;
                    rgbvalues2[x * bmpdata2.Stride + y + 1] = (byte)sumG;
                    rgbvalues2[x * bmpdata2.Stride + y + 0] = (byte)sumB;
                }
            //NEW RECTANGLE;
            oribitmap.UnlockBits(bmpdata1);
            System.Runtime.InteropServices.Marshal.Copy(rgbvalues2, 0, ptr2, bytes2);
            bitmap.UnlockBits(bmpdata2);
            pictureBox2.Image = bitmap;      
        }

        private void btnmiddle_Click(object sender, EventArgs e)
        {
            int m = Convert.ToInt32(txtm.Text);
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
                    int[] R = new int[m * m];
                    int[] G = new int[m * m];
                    int[] B = new int[m * m];
                    int id = 0;
                    for (int a = -(m - 1) / 2; a <= (m - 1) / 2; a++)
                        for (int b = -(m - 1) / 2; b <= (m - 1) / 2; b++)
                        {                           
                            int u = x + a, v = y / 4 + b;
                            if (u >= h || u < 0 || v >= w || v < 0) R[id] = G[id] = B[id] = 0;
                            else
                            {
                                R[id] = rgbvalues1[u * bmpdata1.Stride + v * 4 + 2];
                                G[id] = rgbvalues1[u * bmpdata1.Stride + v * 4 + 1];
                                B[id] = rgbvalues1[u * bmpdata1.Stride + v * 4 + 0];
                            }                            
                            id++;
                        }
                    Array.Sort(R);
                    Array.Sort(G);
                    Array.Sort(B);                    
                    int Al = rgbvalues1[x * bmpdata1.Stride + y + 3];
                    rgbvalues2[x * bmpdata2.Stride + y + 3] = (byte)Al;
                    rgbvalues2[x * bmpdata2.Stride + y + 2] = (byte)R[(m*m+1)/2];
                    rgbvalues2[x * bmpdata2.Stride + y + 1] = (byte)G[(m*m+1)/2];
                    rgbvalues2[x * bmpdata2.Stride + y + 0] = (byte)B[(m*m+1)/2];
                }
            //NEW RECTANGLE;
            oribitmap.UnlockBits(bmpdata1);
            System.Runtime.InteropServices.Marshal.Copy(rgbvalues2, 0, ptr2, bytes2);
            bitmap.UnlockBits(bmpdata2);
            pictureBox2.Image = bitmap;  
        }
    }
}
