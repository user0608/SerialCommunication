using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;

namespace WinProjectSerialPrort.Components
{
    public partial class InputBox : UserControl
    {
        public delegate bool HandlerSentMessageClick(string message);
        public HandlerSentMessageClick onMessageSend;
        public delegate bool HandlerSentFileClick(List<string> paths);
        public HandlerSentFileClick onFilesSend;

        private bool fileMode;
        private List<string> filePaths;
        private string message;
        private OpenFileDialog openFileDialog;

        private bool flag;

        public InputBox()
        {
            InitializeComponent();
            this.init();
        }
        public void init()
        {
            this.myDataGridView.ColumnCount = 1;
            DataGridViewButtonColumn btn = new DataGridViewButtonColumn();
            this.myDataGridView.Columns.Add(btn);
            btn.Text = " X ";
            this.myDataGridView.CellClick += this.dataGridView1_CellClick;
            btn.Name = "btn";
            this.myDataGridView.Columns[0].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            this.myDataGridView.Columns[1].AutoSizeMode = DataGridViewAutoSizeColumnMode.DisplayedCells;           
            this.myDataGridView.RowHeadersVisible = false;

            btn.UseColumnTextForButtonValue = true;
            this.myDataGridView.ColumnHeadersVisible = false;
            this.myDataGridView.ReadOnly = true;
            this.myDataGridView.AllowUserToAddRows = false;

            this.openFileDialog = new OpenFileDialog();
            this.openFileDialog.InitialDirectory = @".\";
            this.openFileDialog.Multiselect = true;
            this.openFileDialog.RestoreDirectory = true;
            this.openFileDialog.Filter ="All files (*.*)|*.*";        
            
            this.openFileDialog.Title = "Select some files";


            this.fileMode = false;
            this.filePaths = new List<string>();
            this.message = "";
            this.ComponentesState(false);
            
            this.myDataGridView.AllowDrop = true;
            this.txtMessage.Visible = true;
            this.myDataGridView.Visible = false;
        }
        public void ComponentesState(bool state)
        {
            this.btnFiles.Enabled = state;
            this.btnSend.Enabled = state;
            this.txtMessage.Enabled = state;
            this.txtMessage.Text = "";
            this.AllowDrop = state;
        }
        private void addFilePads(string[] paths)
        {
            foreach (string path in paths)
            {
                string[] row = new string[] { path };
                this.filePaths.Add(path);
                this.myDataGridView.Rows.Add(row);
            }
            this.myDataGridView.FirstDisplayedScrollingRowIndex = this.myDataGridView.RowCount - 1;
            this.fileMode = true;

        }
        private void btnFiles_Click(object sender, EventArgs e)
        {
            DialogResult result = this.openFileDialog.ShowDialog();
            if (result == DialogResult.OK)
            {
                if(this.filePaths.Count==0) dragOverComponent(true);
                this.addFilePads(this.openFileDialog.FileNames);
            }

        }
        private void resetDefault()
        {
            this.fileMode = false;
            this.filePaths.Clear();
            this.myDataGridView.Rows.Clear();
            this.message = "";
            this.txtMessage.Text = "";
            this.txtMessage.Visible=true;
            this.myDataGridView.Visible = false;
        }

        private void btnSend_Click(object sender, EventArgs e)
        {
            if (this.txtMessage.Text!="" || this.filePaths.Count>0)
            {
                if (this.fileMode)
                {
                    if (this.onFilesSend != null)
                        this.onFilesSend(this.filePaths);
                }
                else
                {
                    this.message = this.txtMessage.Text.Trim();
                    if (this.onMessageSend != null)
                        this.onMessageSend(this.message);
                }
            }
            this.resetDefault();
        }

        private void txtMessage_KeyPress(object sender, KeyPressEventArgs e)
        {
            if ((int)e.KeyChar == (int)Keys.Delete){if(this.txtMessage.Text=="")this.resetDefault();}
        }

        private void txtMessage_KeyUp(object sender, KeyEventArgs e)
        {
            if ((int)e.KeyCode == (int)Keys.Enter)
            {
                if (this.txtMessage.Text != "") this.btnSend_Click(sender, e);                
            }

        }

        private void InputBox_Load(object sender, EventArgs e)
        {          

           

        }
        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.ColumnIndex == 1)
            {
                this.myDataGridView.Rows.RemoveAt(e.RowIndex);
                this.filePaths.RemoveAt(e.RowIndex);
            }
            if (this.filePaths.Count == 0)
            {
                this.resetDefault();
                this.fileMode = false;
            }
        }
        private void dragOverComponent(bool status)
        {
            this.txtMessage.Visible = !status;
            this.myDataGridView.Visible = status;
        }
        

        private void InputBox_DragDrop(object sender, DragEventArgs e)
        {
            string[] filesList = (string[])e.Data.GetData(DataFormats.FileDrop, false);
            List<string> validPathList = new List<string>();
            try
            {
                foreach (string path in filesList)
                {
                    if (!File.Exists(path))
                    {
                        MessageBox.Show(this, path + " is not a valid file", "Error");                        
                        return;
                    }
                    validPathList.Add(path);
                }
            }
            catch
            {
                MessageBox.Show(this, "Something wrong happened with the file paths check", "Error");
            }                      
            this.addFilePads(validPathList.ToArray());            
        }

        private void InputBox_DragEnter(object sender, DragEventArgs e)
        {
            if (((Control)sender).Name == this.Name) flag = true;
            if (!flag) { 
                if (e.Data.GetDataPresent(DataFormats.FileDrop))            
                    e.Effect = DragDropEffects.Copy;            
                else
                    e.Effect = DragDropEffects.None;
            }
        }

        private void InputBox_DragLeave(object sender, EventArgs e)
        {
            if (((Control)sender).Name == this.Name) flag = false;
            if (!flag)
            {
                if (filePaths.Count==0) dragOverComponent(false);
            }
        }

        private void InputBox_DragOver(object sender, DragEventArgs e)
        {
           if(filePaths.Count==0) dragOverComponent(true);
        }

        private void myDataGridView_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }
    }
}
