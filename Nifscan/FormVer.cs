using System;
using System.Windows.Forms;

namespace Nifscan
{
    public partial class FormVer : Form
    {
        public FormVer()
        {
            InitializeComponent();
            checkBox1.Checked = FormMain.verX;
            checkBox3.Checked = FormMain.verY;
            checkBox5.Checked = FormMain.verZ;
            checkBox2.Checked = FormMain.verXHard;
            checkBox4.Checked = FormMain.verYHard;
            checkBox6.Checked = FormMain.verZHard;
            checkBox7.Checked = FormMain.verTrim;
            numericUpDown1.Value = (decimal)FormMain.verXval;
            numericUpDown2.Value = (decimal)FormMain.verYval;
            numericUpDown3.Value = (decimal)FormMain.verZval;
            numericUpDown4.Value = (decimal)FormMain.verTrimNum;
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            FormMain.verX = checkBox1.Checked;
            numericUpDown1.Enabled = checkBox1.Checked;
            changeButton();
        }

        private void checkBox2_CheckedChanged(object sender, EventArgs e)
        {
            FormMain.verXHard = checkBox2.Checked;
        }

        private void numericUpDown4_ValueChanged(object sender, EventArgs e)
        {
            FormMain.verXval = (double)numericUpDown1.Value;
        }

        private void checkBox3_CheckedChanged(object sender, EventArgs e)
        {
            FormMain.verY = checkBox3.Checked;
            numericUpDown2.Enabled = checkBox3.Checked;
            changeButton();
        }

        private void checkBox4_CheckedChanged(object sender, EventArgs e)
        {
            FormMain.verYHard = checkBox4.Checked;
        }

        private void numericUpDown1_ValueChanged(object sender, EventArgs e)
        {
            FormMain.verYval = (double)numericUpDown2.Value;
        }

        private void checkBox5_CheckedChanged(object sender, EventArgs e)
        {
            FormMain.verZ = checkBox5.Checked;
            numericUpDown3.Enabled = checkBox5.Checked;
            changeButton();
        }

        private void checkBox6_CheckedChanged(object sender, EventArgs e)
        {
            FormMain.verZHard = checkBox6.Checked;
        }

        private void numericUpDown2_ValueChanged(object sender, EventArgs e)
        {
            FormMain.verZval = (double)numericUpDown3.Value;
        }

        private void checkBox7_CheckedChanged(object sender, EventArgs e)
        {
            FormMain.verTrim = checkBox7.Checked;
            numericUpDown4.Enabled = checkBox7.Checked;
            changeButton();
        }

        private void numericUpDown4_ValueChanged_1(object sender, EventArgs e)
        {
            FormMain.verTrimNum = (int)numericUpDown4.Value;
        }

        private void changeButton()
        {
            FormMain.formMain.buttonColor(6, checkBox1.Checked || checkBox3.Checked || checkBox5.Checked || checkBox7.Checked);
        }
    }
}
