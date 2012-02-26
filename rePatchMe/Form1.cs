using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace WindowsFormsApplication1
{
    public partial class Form1 : Form
    {
        public string filepath;

        public Form1()
        {
            InitializeComponent();
            Patcher_Version.encrypt_1(8);
        }

        private void Load_VDT(string path)
        {
            numericUpDown1.Enabled = true;
            numericUpDown2.Enabled = true;
            numericUpDown3.Enabled = true;
            button1.Enabled = true;

            string hex_dump = Patcher_Version.load_hex(path);

            string[] words = hex_dump.Split(' ');

            numericUpDown1.Value = Patcher_Version.decrypt_1(words[2]);
            numericUpDown2.Value = Patcher_Version.decrypt_1(words[4]);
            numericUpDown3.Value = Patcher_Version.decrypt_1(words[6]);

        }

        private void button2_Click(object sender, EventArgs e)
        {
            OpenFileDialog dlg = new OpenFileDialog();
            if (filepath == null)
            {
                dlg.InitialDirectory = Application.StartupPath;
            }
            else
            {
                dlg.InitialDirectory = System.IO.Path.GetDirectoryName(filepath);
            }
            dlg.Filter = "Fiesta Patcher|FiestaOnline.exe";
   
            if (dlg.ShowDialog() == DialogResult.OK)
            {
                string path = System.IO.Path.GetDirectoryName(dlg.FileName);
                filepath = path + "\\Fiesta.vdt";
                if (System.IO.File.Exists(filepath))
                {
                    Load_VDT(filepath);
                }
                else
                {
                    MessageBox.Show("Invalid patcher");
                }
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (Patcher_Version.save_ver(this.filepath, (int)numericUpDown1.Value, (int)numericUpDown2.Value, (int)numericUpDown3.Value) == true)
            {
                MessageBox.Show("Save was successful! Changed to: " + numericUpDown1.Value + "." + numericUpDown2.Value + "." + numericUpDown3.Value);
            }
            else
            {
                MessageBox.Show("I think there was an error there somewhere...\n\nTry running me as administrator ;).");
            }
        }
    }
}
