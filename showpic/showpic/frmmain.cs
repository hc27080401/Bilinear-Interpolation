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
    public partial class frmmain : Form
    {
        
        public frmmain()
        {
           
            InitializeComponent();
        }    
        private void btn1(object sender, EventArgs e)
        {
            frmsamplequan f = new frmsamplequan();
            f.Show();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            frmzoom f = new frmzoom();
            f.Show();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            frmgraylevel1 f = new frmgraylevel1();
            f.Show();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            frmgraylevel2 f = new frmgraylevel2();
            f.Show();
        }

        private void button5_Click(object sender, EventArgs e)
        {
            frmhistogram f = new frmhistogram();
            f.Show();
        }

        private void btnmean_Click(object sender, EventArgs e)
        {
            frmmeanfilter f = new frmmeanfilter();
            f.Show();
        }      
    }
}