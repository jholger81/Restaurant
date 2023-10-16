using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace RestaurantClient
{
    public partial class index : Form
    {
        public index()
        {
            InitializeComponent();
            this.FormBorderStyle = FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
        }

    

        private void btnadd_Click(object sender, EventArgs e)
        {
            Form addOrder = new addOrder();

            // Show the settings form
            addOrder.Show();
        }

        private void btnpay_Click(object sender, EventArgs e)
        {
            Form payMenu = new payMenu();

            // Show the settings form
            payMenu.Show();
        }
    }
}
