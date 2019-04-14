using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SearchService.Server
{
    public partial class SBatchInsert : Form
    {

        public int Progress
        {
            get
            {
                return progressBar.Value;
            }

            set
            {
                progressBar.Value = value;
                labelProgress.Text = value.ToString() + "%";
                Application.DoEvents();
            }
        }
        public string Project
        {
            get
            {
                return this.Text;
            }

            set
            {
                this.Text = value;
                Application.DoEvents();
            }
        }
        public SBatchInsert()
        {
            InitializeComponent();
        }
    }
}
