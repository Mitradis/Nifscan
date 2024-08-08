using System;
using System.Windows.Forms;

namespace NifScan
{
    public partial class FormEC : Form
    {
        public FormEC()
        {
            InitializeComponent();
            comboBox1.SelectedIndex = FormMain.ecShaderType;
            checkBox1.Checked = FormMain.ecColor;
            checkBox2.Checked = FormMain.ecMultiple;
            numericUpDown1.Value = (decimal)FormMain.ecR;
            numericUpDown2.Value = (decimal)FormMain.ecG;
            numericUpDown3.Value = (decimal)FormMain.ecB;
            numericUpDown4.Value = (decimal)FormMain.ecM;
        }

        void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            FormMain.ecShaderType = comboBox1.SelectedIndex;
        }

        void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            FormMain.ecColor = checkBox1.Checked;
            numericUpDown1.Enabled = checkBox1.Checked;
            numericUpDown2.Enabled = checkBox1.Checked;
            numericUpDown3.Enabled = checkBox1.Checked;
            changeButton();
        }

        void checkBox2_CheckedChanged(object sender, EventArgs e)
        {
            FormMain.ecMultiple = checkBox2.Checked;
            numericUpDown4.Enabled = checkBox2.Checked;
            changeButton();
        }

        void numericUpDown1_ValueChanged(object sender, EventArgs e)
        {
            FormMain.ecR = (float)numericUpDown1.Value;
        }

        void numericUpDown2_ValueChanged(object sender, EventArgs e)
        {
            FormMain.ecG = (float)numericUpDown2.Value;
        }

        void numericUpDown3_ValueChanged(object sender, EventArgs e)
        {
            FormMain.ecB = (float)numericUpDown3.Value;
        }

        void numericUpDown4_ValueChanged(object sender, EventArgs e)
        {
            FormMain.ecM = (float)numericUpDown4.Value;
        }

        void changeButton()
        {
            FormMain.formMain.buttonColor(4, checkBox1.Checked || checkBox2.Checked);
        }
    }
}
