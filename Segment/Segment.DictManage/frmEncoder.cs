using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Segment.DictManage
{
    public partial class frmEncoder : Form
    {
        

        bool m_Ok;

        public frmEncoder()
        {
            InitializeComponent();
        }

        public String Encoding
        {
            get
            {
                return comboBoxEncoder.Text;
            }
        }

        new public DialogResult ShowDialog()
        {
            m_Ok = false;
            base.ShowDialog();

            if (m_Ok)
            {
                return DialogResult.OK;
            }
            else
            {
                return DialogResult.Cancel;
            }
        }

        private void buttonOk_Click(object sender, EventArgs e)
        {
            m_Ok = true;
            Close();
        }
    }
}
