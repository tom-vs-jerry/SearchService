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
    public partial class frmBatchInsert : Form
    {
        

        WordAttribute m_Word = new WordAttribute();
        bool m_Ok;

        public WordAttribute Word
        {
            get
            {
                return m_Word;
            }

            set
            {
                m_Word = value;
            }
        }

        public bool AllUse
        {
            get
            {
                return checkBoxAllUse.Checked;
            }
        }

        public frmBatchInsert()
        {
            InitializeComponent();
        }

        new public DialogResult ShowDialog()
        {
            m_Ok = false;
            textBoxWord.Text = m_Word.Word;
            numericUpDownFrequency.Value = (decimal)m_Word.Frequency;
            posCtrl.Pos = (int)m_Word.Pos;

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
            m_Word.Frequency = (int)numericUpDownFrequency.Value;
            m_Word.Pos = (POS)posCtrl.Pos;

            Close();
        }

        private void buttonCancel_Click(object sender, EventArgs e)
        {
            Close();
        }
    }
}
