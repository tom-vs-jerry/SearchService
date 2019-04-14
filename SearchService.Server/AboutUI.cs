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
    public partial class AboutUI : Form
    {
        #region  构造函数
        /// <summary>
        /// 构造函数
        /// </summary>
        public AboutUI()
        {
            InitializeComponent();
        }
        #endregion

        #region  确定按钮相应事件btnOK_Click
        /// <summary>
        /// 确定按钮相应事件btnOK_Click
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnOK_Click(object sender, EventArgs e)
        {
            this.Close();
        }
        #endregion
    }
}
