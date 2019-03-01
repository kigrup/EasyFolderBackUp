using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace BackUpX
{
    using System;
    using System.IO;
    
    public partial class Form1 : Form
    {
        string folder_path, backup_path, dated_backup_path;
        bool auto_backup = false;
        int auto_interval = 10000;

        bool folder_selected = false;
        bool backup_selected = false;

        private void DirectoryCopy(string sourceDirName, string destDirName, bool copySubDirs)
        {
            // Get the subdirectories for the specified directory.
            DirectoryInfo dir = new DirectoryInfo(sourceDirName);

            if (!dir.Exists)
            {
                throw new DirectoryNotFoundException(
                    "Source directory does not exist or could not be found: "
                    + sourceDirName);
            }

            DirectoryInfo[] dirs = dir.GetDirectories();
            // If the destination directory doesn't exist, create it.
            if (!Directory.Exists(destDirName))
            {
                Directory.CreateDirectory(destDirName);
            }

            // Get the files in the directory and copy them to the new location.
            FileInfo[] files = dir.GetFiles();
            foreach (FileInfo file in files)
            {
                string temppath = Path.Combine(destDirName, file.Name);
                file.CopyTo(temppath, false);
            }

            // If copying subdirectories, copy them and their contents to new location.
            if (copySubDirs)
            {
                foreach (DirectoryInfo subdir in dirs)
                {
                    string temppath = Path.Combine(destDirName, subdir.Name);
                    DirectoryCopy(subdir.FullName, temppath, copySubDirs);
                }
            }
        }

        void makeBackupVisible()
        {
            checkBox1.Visible = true;
            label2.Visible = true;
            textBox1.Visible = true;
            label3.Visible = true;
            button2.Visible = true;

        }

        void Backup()
        {
            try
            {
                dated_backup_path = Path.Combine(backup_path, DateTime.Now.ToString("yyyy-MM-dd_HH-mm-ss"));
                DirectoryCopy(folder_path, dated_backup_path, true);
            } catch (Exception e)
            {
                MessageBox.Show(e.ToString());
            }
        }

        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void button3_Click(object sender, EventArgs e)
        {
            if (folderBrowserDialog2.ShowDialog() == DialogResult.OK)
            {
                backup_selected = true;
                backup_path = folderBrowserDialog2.SelectedPath;

                if (backup_selected && folder_selected)
                {
                    makeBackupVisible();
                }
            }
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            auto_backup = !auto_backup;
            timer1.Interval = auto_interval;
            timer1.Enabled = auto_backup;
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            Backup();
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            try
            {
                auto_interval = 1000 * int.Parse(textBox1.Text);
                timer1.Interval = auto_interval;
            } catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Backup();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (folderBrowserDialog1.ShowDialog() == DialogResult.OK)
            {
                folder_selected = true;
                folder_path = folderBrowserDialog1.SelectedPath;

                if (backup_selected && folder_selected)
                {
                    makeBackupVisible();
                }
            }
        }
    }
}
