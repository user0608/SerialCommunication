using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Windows.Forms;

namespace WinProjectSerialPrort
{
    public partial class Options : Form
    {
        public delegate void HandelrtextBoxPath(string path);
        public HandelrtextBoxPath handlerTxtPath;

        FolderBrowserDialog folderBrowser;
        private string folderPath;
        public Options()
        {
            InitializeComponent();
            this.folderBrowser = new FolderBrowserDialog();          
            this.StartPosition = FormStartPosition.CenterParent;
            string path = Path.GetFullPath(@"./");
            this.folderPath = path;
        }

        private void btnChange_Click(object sender, EventArgs e)
        {
            DialogResult result = folderBrowser.ShowDialog();

            if (result == DialogResult.OK)
            {
                if (!string.IsNullOrWhiteSpace(folderBrowser.SelectedPath))
                {
                    this.txtPath.Text = this.folderBrowser.SelectedPath;                   
                }
                else
                {
                    MessageBox.Show(this, "Invalid selected path ", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }
      
       
        public string getFolderPath()
        {
            return this.folderPath;
        }
        private void Options_Load(object sender, EventArgs e)
        {
            if (this.txtPath.Text == "")
            {
                this.txtPath.Text = this.folderPath;

            }
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            if (Directory.Exists(this.txtPath.Text))
            {
                this.folderPath = this.txtPath.Text;
                if (this.handlerTxtPath != null) this.handlerTxtPath(this.folderPath);
                this.Close();
            }
            else
            {
                MessageBox.Show(this,"Invalid selected path","Error", MessageBoxButtons.OK, MessageBoxIcon.Error);             
            }
        }

        private void btnTry_Click(object sender, EventArgs e)
        {
            string message = "Invalid selected path";
            string title = "Fail";
            if (Directory.Exists(this.txtPath.Text))
            {
                message = "The select folder path is correct";
                title = "Success";
            }
            MessageBox.Show(message,title, MessageBoxButtons.OK, ((title== "Fail")?MessageBoxIcon.Error:MessageBoxIcon.Information));
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.txtPath.Text = this.folderPath;
            this.Close();
        }
    }
}
