using System;
using System.Windows.Forms;

namespace NifScan
{
    public partial class FormVC : Form
    {
        public FormVC()
        {
            InitializeComponent();
            checkBox1.Enabled = !FormMain.fixesOn;
            checkBox2.Enabled = !FormMain.fixesOn;
            checkBox1.Checked = FormMain.vcColor;
            checkBox2.Checked = FormMain.vcAlpha;
            checkBox3.Checked = FormMain.vcRemove;
            numericUpDown1.Value = (decimal)FormMain.vcR;
            numericUpDown2.Value = (decimal)FormMain.vcG;
            numericUpDown3.Value = (decimal)FormMain.vcB;
            numericUpDown4.Value = (decimal)FormMain.vcA;
        }

        void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            FormMain.vcColor = checkBox1.Checked;
            numericUpDown1.Enabled = checkBox1.Checked;
            numericUpDown2.Enabled = checkBox1.Checked;
            numericUpDown3.Enabled = checkBox1.Checked;
            changeButton();
        }

        void checkBox2_CheckedChanged(object sender, EventArgs e)
        {
            FormMain.vcAlpha = checkBox2.Checked;
            numericUpDown4.Enabled = checkBox2.Checked;
            changeButton();
        }

        void numericUpDown1_ValueChanged(object sender, EventArgs e)
        {
            FormMain.vcR = (float)numericUpDown1.Value;
        }

        void numericUpDown2_ValueChanged(object sender, EventArgs e)
        {
            FormMain.vcG = (float)numericUpDown2.Value;
        }

        void numericUpDown3_ValueChanged(object sender, EventArgs e)
        {
            FormMain.vcB = (float)numericUpDown3.Value;
        }

        void numericUpDown4_ValueChanged(object sender, EventArgs e)
        {
            FormMain.vcA = (float)numericUpDown4.Value;
        }

        void checkBox3_CheckedChanged(object sender, EventArgs e)
        {
            FormMain.vcRemove = checkBox3.Checked;
            if (checkBox3.Checked)
            {
                checkBox1.Checked = false;
                checkBox2.Checked = false;
                checkBox1.Enabled = false;
                checkBox2.Enabled = false;
                checkBox1_CheckedChanged(this, new EventArgs());
                checkBox2_CheckedChanged(this, new EventArgs());
            }
            else if (!FormMain.fixesOn)
            {
                checkBox1.Enabled = true;
                checkBox2.Enabled = true;
            }
            changeButton();
        }

        void changeButton()
        {
            FormMain.formMain.buttonColor(5, checkBox1.Checked || checkBox2.Checked || checkBox3.Checked);
        }
    }
}
