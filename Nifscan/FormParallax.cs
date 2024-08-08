using System;
using System.Windows.Forms;

namespace NifScan
{
    public partial class FormParallax : Form
    {
        public FormParallax()
        {
            InitializeComponent();
            checkBox1.Checked = FormMain.parallaxInvert;
            checkBox2.Checked = FormMain.parallaxCompare;
            checkBox3.Checked = FormMain.parallaxAdd;
            checkBox4.Checked = FormMain.parallaxRemove;
            if (FormMain.texturesList.Count > 0)
            {
                textBox1.Clear();
                textBox1.AppendText(String.Join(Environment.NewLine, FormMain.texturesList));
            }
        }

        void button1_Click(object sender, EventArgs e)
        {
            FormMain.texturesList.Clear();
            foreach (string line in textBox1.Lines)
            {
                if (!String.IsNullOrEmpty(line))
                {
                    FormMain.texturesList.Add(line);
                }
            }
            Dispose();
        }

        void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            FormMain.parallaxInvert = checkBox1.Checked;
        }

        void checkBox2_CheckedChanged(object sender, EventArgs e)
        {
            FormMain.parallaxCompare = checkBox2.Checked;
        }

        void checkBox3_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox3.Checked)
            {
                checkBox4.Checked = false;
            }
            else
            {
                checkBox1.Checked = false;
            }
            changeState();
        }

        void checkBox4_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox4.Checked)
            {
                checkBox1.Checked = false;
                checkBox3.Checked = false;
            }
            changeState();
        }

        void changeState()
        {
            checkBox1.Enabled = checkBox3.Checked && !checkBox4.Checked;
            checkBox2.Enabled = !checkBox3.Checked && !checkBox4.Checked;
            checkBox2.Checked = false;
            FormMain.parallaxAdd = checkBox3.Checked && !checkBox4.Checked;
            FormMain.parallaxRemove = checkBox4.Checked && !checkBox3.Checked;
            FormMain.parallaxInvert = checkBox1.Checked;
            FormMain.parallaxCompare = checkBox2.Checked;
            FormMain.formMain.buttonColor(3, checkBox3.Checked || checkBox4.Checked);
        }
    }
}
