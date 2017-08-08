using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Threading;
using System.IO;

namespace Directory_Cloner
{
    public partial class Form1 : Form
    {

        Thread mainThread = null;
        public Form1()
        {
            InitializeComponent();
           
        }

        private void button1_Click(object sender, EventArgs e)
        {
            string startupPath = Application.StartupPath;
            using (FolderBrowserDialog dialog = new FolderBrowserDialog())
            {
                dialog.Description = "Select the Target Folder";
                dialog.ShowNewFolderButton = false;
                dialog.RootFolder = Environment.SpecialFolder.MyComputer;

                if (dialog.ShowDialog() == DialogResult.OK)
                {
                    string folder = dialog.SelectedPath;
                    textBox1.Text = folder;
                }
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            string startupPath = Application.StartupPath;
            using (FolderBrowserDialog dialog = new FolderBrowserDialog())
            {
                dialog.Description = "Select the Target Folder";
                dialog.ShowNewFolderButton = false;
                dialog.RootFolder = Environment.SpecialFolder.MyComputer;

                if (dialog.ShowDialog() == DialogResult.OK)
                {
                    string folder = dialog.SelectedPath;
                    textBox2.Text = folder;
                }
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            mainThread = new Thread(delegate ()
              {
                  button3.Enabled = false;
                  button4.Enabled = true;
                  if(Directory.Exists(textBox1.Text)==false || Directory.Exists(textBox2.Text))
                  {
                      MessageBox.Show("Invalid source or destination path");
                      goto end;
                  }
                  string[] subdirectoryEntries = Directory.GetDirectories(textBox1.Text,"*",SearchOption.AllDirectories);
                  label5.Text = subdirectoryEntries.Length.ToString();
                  progressBar1.Maximum = subdirectoryEntries.Length;
                  progressBar1.Value = 0;
                  progressBar1.Step = 1;
                  int count = Convert.ToInt32(numericUpDown1.Value);
                  foreach (string d in subdirectoryEntries)
                  {
                      string[] files = Directory.GetFiles(d);
                      Directory.CreateDirectory(d.Replace(textBox1.Text, textBox2.Text));
                      if (files.Length < count)
                      {
                          foreach (string f in files)
                          {
                              File.Copy(f, f.Replace(textBox1.Text, textBox2.Text), true);
                          }
                      }
                      else
                      {
                          for(int i=0; i<count; i++)
                          {
                              File.Copy(files[i], files[i].Replace(textBox1.Text, textBox2.Text), true);
                          }
                      }

                      progressBar1.PerformStep();
                  }
                  System.Threading.Thread.Sleep(2000);
                  MessageBox.Show("Clonning complete..!");
                  progressBar1.Value = 0;
                  end:
                  button3.Enabled = true;
                  button4.Enabled = false;
                  
              });
            mainThread.Start();
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void Form1_Load(object sender, EventArgs e)
        {

            textBox1.Enabled = false;
            textBox2.Enabled = false;
            button4.Enabled = false;
            TextBox.CheckForIllegalCrossThreadCalls = false;
        }

        private void button4_Click(object sender, EventArgs e)
        {
            mainThread.Abort();
            button3.Enabled = true;
            button4.Enabled = false;
        }
    }
}
