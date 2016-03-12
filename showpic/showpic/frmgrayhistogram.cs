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
    public partial class frmgrayhistogram : Form
    {
        string filename;
        System.Drawing.Bitmap bitmap,oribitmap;
        int[] cntpixel = new int[300];
        double[] pk = new double[300];
        public frmgrayhistogram()
        {
            InitializeComponent();
        }
    

        private void btnopen_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < 300; i++) cntpixel[i] = 0;
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
            Graphics g = this.CreateGraphics();
            Pen curpen = new Pen(Brushes.Black, 2);
            g.DrawLine(curpen, 180, 10, 180, 620);
            g.DrawLine(curpen, 180, 620, 760, 620);
            for (int i = 1; i < 256;i++ )
            {
                g.DrawLine(new Pen(this.BackColor,2), 180 + i * 2, 620, 180 + i * 2, 10);
            }
            g.DrawString("0", new Font("Arial", 8), Brushes.Black, new PointF(160, 630));
            for (int i = 1; i <= 17; i++)
            {
                g.DrawLine(curpen, 180 + 30 * i, 620, 180 + 30 * i, 630);
                g.DrawString(Convert.ToString(i * 15), new Font("Arial", 8), Brushes.Black, new PointF(170 + 30 * i, 630));
            }

            int h = oribitmap.Height, w = oribitmap.Width;            
            Rectangle rect1 = new Rectangle(0, 0, w, h);
            System.Drawing.Imaging.BitmapData bmpdata1 = oribitmap.LockBits(rect1, System.Drawing.Imaging.ImageLockMode.ReadWrite, oribitmap.PixelFormat);
            IntPtr ptr1 = bmpdata1.Scan0;
            int bytes1 = bmpdata1.Stride * bmpdata1.Height;
            byte[] rgbvalues1 = new byte[bytes1];
            System.Runtime.InteropServices.Marshal.Copy(ptr1, rgbvalues1, 0, bytes1); //copy pixel            
            
            for (int x = 0; x < h; x++)
                for (int y = 0; y < w * 4; y += 4)
                {
                    int Al = rgbvalues1[x * bmpdata1.Stride + y + 3];
                    int R = rgbvalues1[x * bmpdata1.Stride + y + 2];
                    int G = rgbvalues1[x * bmpdata1.Stride + y + 1];
                    int B = rgbvalues1[x * bmpdata1.Stride + y + 0];
                    int gray=Convert.ToInt32(0.299*R+0.587*G+0.114*B);
                    cntpixel[gray]++;
                }
            double maxpixel=0.0;
            double N=w*h;
            for (int i = 0; i < 256; i++) if (cntpixel[i] > maxpixel) maxpixel = cntpixel[i];
            for (int i = 0; i < 256; i++)
            {
                double tmp = 600.0 * (Convert.ToDouble(cntpixel[i]) / maxpixel)+0.5;
                g.DrawLine(curpen, 180 + i * 2, 620, 180 + i * 2, 620 - (int)tmp);
            }
            g.DrawString(maxpixel.ToString(),new Font("Arial", 8), Brushes.Black, new PointF(180, 20));
            oribitmap.UnlockBits(bmpdata1);            
        }
    }
}
