using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace EraseFileList
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            progressBar1.Style = ProgressBarStyle.Continuous;
            progressBar1.Minimum = 0;
            progressBar1.Step = 1;
            progressBar1.Value = 0;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.InitialDirectory = @"C:\ProgramData\AVAST Software\Avast\GrimeFighter2";
            ofd.Filter = "txt files (*.txt)|*.txt|All files (*.*)|*.*";
            ofd.FilterIndex = 2;
            ofd.RestoreDirectory = true;

            if (ofd.ShowDialog() == DialogResult.OK)
            {
                string openFileName = ofd.FileName;
                textBox2.Text = openFileName;

                string[] filelines = File.ReadAllLines(openFileName);
                progressBar1.Maximum = filelines.Length;

                foreach (string filename in filelines)
                {
                    // 파일
                    if (filename.IndexOf("HKEY") != 0)
                    {
//                        if (filename.IndexOf(@"C:\WINDOWS") == 0)
                            //continue;
                        FileInfo fi = new FileInfo(filename);
                        try
                        {
                            if (fi.Exists == false)
                            {
                                listBox1.Items.Add("Already Deleted File Name : " + filename);
                                listBox1.Refresh();
                                progressBar1.PerformStep();
                                continue;
                            }
                            fi.Delete();
                            listBox1.Items.Add("Deleted File Name : " + filename);
                            listBox1.Refresh();
                            progressBar1.PerformStep();
                        }
                        catch (Exception ex)
                        {
                            listBox2.Items.Add(ex.ToString());
                            listBox2.Refresh();
                            progressBar1.PerformStep();
                            continue;
                        }
                    }
                    else
                    {
                        // 레지스트리
                        try
                        {
                            string reginame = null;
                            if (filename.IndexOf(@"HKEY_USERS\") == 0)
                            {
                                reginame = filename.Remove(0, 11);
                                RegistryKey reg = Registry.Users.OpenSubKey(reginame);
                                if (reg != null)
                                {
                                    Registry.Users.DeleteSubKey(reginame);
                                    progressBar1.PerformStep();
                                }
                                else
                                {
                                    listBox2.Items.Add("No exist Registry Key : " + filename);
                                    listBox2.Refresh();
                                    progressBar1.PerformStep();
                                    continue;
                                }
                            }
                            else if (filename.IndexOf(@"HKEY_CURRENT_USER\") == 0)
                            {
                                reginame = filename.Remove(0, 18);
                                RegistryKey reg = Registry.CurrentUser.OpenSubKey(reginame);
                                if (reg != null)
                                {
                                    Registry.CurrentUser.DeleteSubKey(reginame);
                                    progressBar1.PerformStep();
                                }
                                else
                                {
                                    listBox2.Items.Add("No exist Registry Key : " + filename);
                                    listBox2.Refresh();
                                    progressBar1.PerformStep();
                                    continue;
                                }
                            }
                            else if (filename.IndexOf(@"HKEY_LOCAL_MACHINE\") == 0)
                            {
                                reginame = filename.Remove(0, 19);
                                RegistryKey reg = Registry.LocalMachine.OpenSubKey(reginame);
                                if (reg != null)
                                {
                                    Registry.LocalMachine.DeleteSubKey(reginame);
                                    progressBar1.PerformStep();
                                }
                                else
                                {
                                    listBox2.Items.Add("No exist Registry Key : " + filename);
                                    listBox2.Refresh();
                                    progressBar1.PerformStep();
                                    continue;
                                }
                            }
                            listBox1.Items.Add("Remove Registry Key : " + filename);
                            listBox1.Refresh();
                            progressBar1.PerformStep();
                        }
                        catch(Exception ex)
                        {
                            listBox2.Items.Add(ex.ToString());
                            listBox2.Refresh();
                            progressBar1.PerformStep();
                            continue;
                        }
                    }
                }
            }
        }
    }
}
