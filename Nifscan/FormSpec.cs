using System;
using System.Collections.Generic;
using System.Globalization;
using System.Windows.Forms;

namespace NifScan
{
    public partial class FormSpec : Form
    {
        public FormSpec()
        {
            InitializeComponent();
            foreach (string line in FormMain.specTextures)
            {
                textBox1.Text += line + Environment.NewLine;
            }
            textBox1.TextChanged += new System.EventHandler(textBox1_TextChanged);
            fillFromCache(textBox2, FormMain.specGlossiness);
            fillFromCache(textBox3, FormMain.specR);
            fillFromCache(textBox4, FormMain.specG);
            fillFromCache(textBox5, FormMain.specB);
            fillFromCache(textBox6, FormMain.specStrength);
            textBox2.TextChanged += new System.EventHandler(textBox2_TextChanged);
            textBox3.TextChanged += new System.EventHandler(textBox3_TextChanged);
            textBox4.TextChanged += new System.EventHandler(textBox4_TextChanged);
            textBox5.TextChanged += new System.EventHandler(textBox5_TextChanged);
            textBox6.TextChanged += new System.EventHandler(textBox6_TextChanged);
        }

        void fillFromCache(TextBox textbox, List<float> list)
        {
            foreach (float num in list)
            {
                textbox.Text += num + Environment.NewLine;
            }
        }

        void textBox1_TextChanged(object sender, System.EventArgs e)
        {
            FormMain.specTextures.Clear();
            foreach (string line in textBox1.Lines)
            {
                if (!String.IsNullOrEmpty(line))
                {
                    FormMain.specTextures.Add(line);
                }
            }
            changeButton();
        }

        void textBox2_TextChanged(object sender, System.EventArgs e)
        {
            changedText(textBox2, FormMain.specGlossiness);
        }

        void textBox3_TextChanged(object sender, System.EventArgs e)
        {
            changedText(textBox3, FormMain.specR);
        }

        void textBox4_TextChanged(object sender, System.EventArgs e)
        {
            changedText(textBox4, FormMain.specG);
        }

        void textBox5_TextChanged(object sender, System.EventArgs e)
        {
            changedText(textBox5, FormMain.specB);
        }

        void textBox6_TextChanged(object sender, System.EventArgs e)
        {
            changedText(textBox6, FormMain.specStrength);
        }

        void changedText(TextBox textbox, List<float> list)
        {
            list.Clear();
            foreach (string line in textbox.Lines)
            {
                if (!String.IsNullOrEmpty(line))
                {
                    float parse = 0;
                    if (Single.TryParse(line.Replace(',', '.'), NumberStyles.Number, CultureInfo.InvariantCulture, out parse))
                    {
                        list.Add(parse);
                    }
                }
            }
            changeButton();
        }

        void changeButton()
        {
            FormMain.formMain.buttonColor(7, FormMain.specTextures.Count > 0);
        }

        void FormSpec_FormClosing(object sender, FormClosingEventArgs e)
        {
            int count = FormMain.specTextures.Count;
            FormMain.specAvailable = FormMain.specGlossiness.Count == count && FormMain.specR.Count == count && FormMain.specG.Count == count && FormMain.specB.Count == count && FormMain.specStrength.Count == count;
            if (!FormMain.specAvailable)
            {
                MessageBox.Show("Specular parameters count not equal textures count.");
            }
        }
    }
}
