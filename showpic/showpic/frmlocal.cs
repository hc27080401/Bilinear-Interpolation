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
    public partial class frmlocal : Form
    {
        string filename;
        System.Drawing.Bitmap bitmap, oribitmap;
        int[,] graylevel;
        double [,] msxy,devxy;        
        public frmlocal()
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

        private void btnwork_Click(object sender, EventArgs e)
        {
            double E = Convert.ToDouble(txtE.Text);
            double k0 = Convert.ToDouble(txtk0.Text);
            double k1 = Convert.ToDouble(txtk1.Text);
            double k2 = Convert.ToDouble(txtk2.Text);            
            int M=Convert.ToInt32(txtM.Text);
            if (oribitmap == null) { MessageBox.Show("请先加载图片", "出错", MessageBoxButtons.OK, MessageBoxIcon.Warning); return; }
            if (M % 2 != 1) { MessageBox.Show("请输入偶数的M（N）", "出错", MessageBoxButtons.OK, MessageBoxIcon.Warning); return; }
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
            //calculate MG

            graylevel = new int[h,w];
            double MG = 0.0;
            for (int x = 0; x < h; x++)
                for (int y = 0; y < w * 4; y += 4)
                {
                    int Al = rgbvalues1[x * bmpdata1.Stride + y + 3];
                    int R = rgbvalues1[x * bmpdata1.Stride + y + 2];
                    int G = rgbvalues1[x * bmpdata1.Stride + y + 1];
                    int B = rgbvalues1[x * bmpdata1.Stride + y + 0];
                    int gray = Convert.ToInt32(0.299 * R + 0.587 * G + 0.114 * B);
                    graylevel[x, y/4] = gray;
                    MG += Convert.ToDouble(gray); //global gray level;                   
                }
            MG/=Convert.ToDouble(h*w);
            //calculate msxy devxy
            msxy=new double[h,w];
            devxy=new double[h,w];
            for (int x = 0; x < h;x++)
            {
                for (int y=0;y<w;y++)
                {
                    int LL = y, RR = w - 1 - y, UP = x, DOWN = h - 1 - x;
                    int ll = 0, up = 0;
                    if (LL < M / 2) ll = LL;
                    else if (RR < M / 2) ll = M - 1 - RR; else ll = M / 2;
                    if (UP < M / 2) up = UP;
                    else if (DOWN < M / 2) up = M - 1 - DOWN; else up = M / 2;
                    double sum = 0.0,res=0.0;
                    for (int i=-ll;i<=M-1-ll;i++)                    
                        for (int j=-up;j<=M-1-up;j++)                                                  
                            sum+=graylevel[x+j,y+i];                     
                    msxy[x,y]=sum/Convert.ToDouble(M*M);
                    for (int i=-ll;i<=M-1-ll; i++)
                        for (int j = -up; j <= M - 1 - up; j++)
                            res += (graylevel[x + j, y + i] - msxy[x, y]) * (graylevel[x + j, y + i] - msxy[x, y]);
                    res/=Convert.ToDouble(M*M);
                    res=Math.Sqrt(res); //standard deviation
                    devxy[x,y]=res;
                }                
            }

            //calculate global standard deviation
            Double DG=0.0;            
            for (int x=0;x<h;x++)    
                for (int y=0;y<w;y++)           
                    DG+=(graylevel[x,y]-MG)*(graylevel[x,y]-MG);                                           
            DG/=Convert.ToDouble(h*w);
            DG=Math.Sqrt(DG); //global standard deviation

            //modify gray level
            for (int x = 0; x < h; x++)
                for (int y = 0; y < w * 4; y += 4)
                {
                    double nowE=1.0;
                    if (msxy[x,y/4]<=k0*MG && k1*DG<=devxy[x,y/4] && devxy[x,y/4]<=k2*DG) nowE=E;
                    double nowgray=graylevel[x,y/4]*nowE;                    
                    rgbvalues2[x*bmpdata2.Stride+y+3]=rgbvalues1[x * bmpdata1.Stride + y + 3];
                    rgbvalues2[x*bmpdata2.Stride+y+2]=getvalue(nowgray);
                    rgbvalues2[x*bmpdata2.Stride+y+1]=getvalue(nowgray);
                    rgbvalues2[x*bmpdata2.Stride+y+0]=getvalue(nowgray);                    
                }

            //NEW RECTANGLE;
            oribitmap.UnlockBits(bmpdata1);
            System.Runtime.InteropServices.Marshal.Copy(rgbvalues2, 0, ptr2, bytes2);
            bitmap.UnlockBits(bmpdata2);
            pictureBox2.Image = bitmap;       
        }
   }
}
