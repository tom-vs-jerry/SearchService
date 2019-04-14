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

namespace SearchService.Server.SSystemSet
{
    public partial class SSystemSetUI : Form
    {
        //=============================== 系统代码 ===============================
        #region 系统代码
        public SSystemSetUI()
        {
            InitializeComponent();
        }
        #endregion


        //=============================== 变量区 ===============================
        #region 配置实体
        /// <summary>配置实体
        /// 配置实体
        /// </summary>
        //private SSystemSetData m_SSystemSetData = new SSystemSetData();
        #endregion

        //=============================== 事件方法 ===============================
        #region 窗体加载事件
        /// <summary>窗体加载事件
        /// 窗体加载事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void SSystemSetUI_Load(object sender, EventArgs e)
        {
            //this.tbxIP.Text = ConfigFile.GetKeyValue("");
            this.tbxPort.Text = ConfigurationManager.AppSettings["WebSocketPort"];
            this.tbxDicPath.Text = Application.StartupPath + "KTDictSeg.xml";

        }
        #endregion

        #region 确定事件
        /// <summary>确定事件
        /// 确定事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnConfirm_Click(object sender, EventArgs e)
        {

        }
        #endregion

        #region 取消事件
        /// <summary>取消事件
        /// 取消事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }
        #endregion

        #region 日志保存路径选择 事件
        /// <summary>
        /// 日志保存路径选择 事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnSave_Click(object sender, EventArgs e)
        {
            ////选择多个文件
            //this.fbdLog.RootFolder = Environment.SpecialFolder.MyComputer;

            //this.fbdLog.ShowNewFolderButton = true;

            //this.fbdLog.Description = "选择日志的保存路径";

            //System.Windows.Forms.DialogResult dialogResult = this.fbdLog.ShowDialog();

            //if (dialogResult == System.Windows.Forms.DialogResult.OK)
            //{
            //    //return null;
            //    this.tbxLog.Text = this.fbdLog.SelectedPath;
            //}

        }
        #endregion

        //=============================== 私有方法 ===============================
        #region 设置数据
        /// <summary>设置数据
        /// 设置数据
        /// </summary>
        private void SetData()
        {

        }
        #endregion

        #region 得到数据
        /// <summary>得到数据
        /// 得到数据
        /// </summary>
        private void GetConfigData()
        {

            try
            {

            }
            catch
            {
            }



            //设置控件的默认路径
            this.fbdLog.SelectedPath = Application.StartupPath + @"\log";
        }
        #endregion

        #region 得到窗体数据
        /// <summary>得到窗体数据
        /// 得到窗体数据
        /// </summary>
        private void GetFormData()
        {
            //this.m_SSystemSetData.Interval = this.NumUpDownSpace.Value.ToString();
            //this.m_SSystemSetData.MarketOpenTime = this.DTOpen.Text;
            //this.m_SSystemSetData.MarketCloseTime = this.DTClose.Text;
            //this.m_SSystemSetData.LogSavePath = this.tbxLog.Text;
        }
        #endregion




    }
}
