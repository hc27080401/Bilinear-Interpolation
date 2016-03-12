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
    public partial class frmhistogram : Form
    {
       
        public frmhistogram()
        {
            InitializeComponent();
        }
       
        private void btngray_Click(object sender, EventArgs e)
        {
            frmgrayhistogram f = new frmgrayhistogram();
            f.Show();                
        }

        private void btnequal_Click(object sender, EventArgs e)
        {
            frmequal f = new frmequal();
            f.Show();
        }

        private void btnmatch_Click(object sender, EventArgs e)
        {
            frmmatch f = new frmmatch();
            f.Show();
        }

        private void btnlocal_Click(object sender, EventArgs e)
        {
            frmlocal f = new frmlocal();
            f.Show();
        }
    }
}
