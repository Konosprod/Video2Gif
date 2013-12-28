using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using System.Threading;
using System.Diagnostics;

namespace Video2Gif
{
    public partial class mainWindow : Form
    {
        String inputVideo;
        String outputGif;
        Process p;
        Process p2;
        private delegate void progressBarDelegateHandler();

        public mainWindow()
        {
            InitializeComponent();
            dateTimePicker1.Text = "00:00:00";

            p = new Process();
            p.Exited += p_Exited;
        }

        void p_Exited(object sender, EventArgs e)
        {
            p2 = new Process();
            p2.Exited += p2_Exited;
            p2.EnableRaisingEvents = true;
            p2.StartInfo.RedirectStandardOutput = true;
            p2.StartInfo.UseShellExecute = false;
            p2.StartInfo.CreateNoWindow = true;
            p2.StartInfo.Arguments = String.Format(@"{0}", outputGif);
            p2.StartInfo.FileName = "create_gif.bat";

            if (progressBar1.InvokeRequired)
            {
                progressBar1.Invoke(new progressBarDelegateHandler(majProgressBar));
            }
            else
            {
                majProgressBar();
            }

            p2.Start();
        }

        void p2_Exited(object sender, EventArgs e)
        {
            MessageBox.Show("Terminé !");

            if (progressBar1.InvokeRequired)
            {
                progressBar1.Invoke(new progressBarDelegateHandler(majProgressBar));
            }
            else
            {
                majProgressBar();
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (openFileDialog1.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                inputVideo = openFileDialog1.FileName;
            }
            else
            {
                inputVideo = "";
            }

            videoTextBox.Text = inputVideo;
        }

        private void mainWindow_Load(object sender, EventArgs e)
        {

        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void convertButton_Click(object sender, EventArgs e)
        {
            progressBar1.Value = 0;
            label5.Text = "Etape : 0/2";

            p.EnableRaisingEvents = true;
            p.StartInfo.RedirectStandardOutput = true;
            p.StartInfo.UseShellExecute = false;
            p.StartInfo.CreateNoWindow = true;
            p.StartInfo.Arguments = String.Format(@"""{0}"" {1} {2}", inputVideo, numericUpDown1.Value.ToString(), 
                dateTimePicker1.Text);
            p.StartInfo.FileName = "dump_frame.bat";
            p.Start();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            saveFileDialog1.Filter = "(*.gif)|*.gif";
            if (saveFileDialog1.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                outputGif = saveFileDialog1.FileName;
            }
            else
            {
                outputGif = "";
            }

            gifTextBox.Text = outputGif;
        }

        private void majProgressBar()
        {
            progressBar1.Value += 1;
            label5.Text = "Etape : " + progressBar1.Value.ToString() + "/2";
        }
    }
}
