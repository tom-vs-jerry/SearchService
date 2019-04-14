using System;
using System.Configuration;
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
    public partial class LockedUI : Form
    {


        //=============================== 系统代码 ===============================
        #region 窗体构造函数
        /// <summary>
        /// 窗体构造函数
        /// </summary>
        public LockedUI()
        {
            InitializeComponent();
        }

        #endregion



        //=============================== 事件方法 ===============================
        #region 解锁按钮Click事件
        /// <summary>
        /// 解锁按钮Click事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnUnlocked_Click(object sender, EventArgs e)
        {
            if (this.CheckUnlockedUser(this.txtUserName.Text, this.txtPassword.Text))
            {
                this.notifyIcon1.Visible = false;
                this.Visible = false;
                this.Owner.Show();
            }
            else
            {
                MessageBox.Show("解锁帐号或密码不正确，请重新输入。", "提示信息", MessageBoxButtons.OK, MessageBoxIcon.Information);
                this.txtPassword.Text = "";
            }
        }
        #endregion

        #region 隐藏按钮Click事件
        /// <summary>
        /// 隐藏按钮Click事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnHidden_Click(object sender, EventArgs e)
        {
            this.Visible = false;
            this.notifyIcon1.Visible = true;
        }
        #endregion

        #region 托盘图标MouseDoubleClick事件
        /// <summary>
        /// 托盘图标MouseDoubleClick事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void notifyIcon1_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            MenuItemShowLockedUI_Click(null, null);
        }
        #endregion

        #region 显示锁定窗口菜单的Click事件
        /// <summary>
        /// 显示锁定窗口菜单的Click事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MenuItemShowLockedUI_Click(object sender, EventArgs e)
        {

            this.Visible = true;
            this.Activate();
            this.notifyIcon1.Visible = false;
        }
        #endregion



        //=============================== 私有方法 ===============================
        #region 验证解锁帐号
        /// <summary>
        /// 验证解锁帐号
        /// </summary>
        /// <returns>是否正确</returns>
        private bool CheckUnlockedUser(string _UserName, string _Password)
        {
            if (_UserName == ConfigurationManager.AppSettings["UnlockUserName"] && _Password == ConfigurationManager.AppSettings["UnlockUserName"])
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        #endregion
    }

}
