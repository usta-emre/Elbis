using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Elbis
{
    public partial class ShowDataForm : Form
    {
        public ShowDataForm()
        {
            InitializeComponent();
        }
        public ShowDataForm(string data)
        {
            InitializeComponent();

            // RichTextBox içeriğini belirtilen veriyle doldur
            richTextBox1.Text = data;
        }

    }
}
