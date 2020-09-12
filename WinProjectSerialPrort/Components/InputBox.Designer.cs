namespace WinProjectSerialPrort.Components
{
    partial class InputBox
    {
        /// <summary> 
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.btnSend = new System.Windows.Forms.Button();
            this.btnFiles = new System.Windows.Forms.Button();
            this.txtMessage = new System.Windows.Forms.TextBox();
            this.myDataGridView = new System.Windows.Forms.DataGridView();
            ((System.ComponentModel.ISupportInitialize)(this.myDataGridView)).BeginInit();
            this.SuspendLayout();
            // 
            // btnSend
            // 
            this.btnSend.BackColor = System.Drawing.Color.Teal;
            this.btnSend.FlatAppearance.BorderColor = System.Drawing.Color.Teal;
            this.btnSend.ForeColor = System.Drawing.SystemColors.ButtonFace;
            this.btnSend.Location = new System.Drawing.Point(467, 10);
            this.btnSend.Name = "btnSend";
            this.btnSend.Size = new System.Drawing.Size(75, 40);
            this.btnSend.TabIndex = 0;
            this.btnSend.Text = "Send";
            this.btnSend.UseVisualStyleBackColor = false;
            this.btnSend.Click += new System.EventHandler(this.btnSend_Click);
            // 
            // btnFiles
            // 
            this.btnFiles.ForeColor = System.Drawing.Color.DarkBlue;
            this.btnFiles.Location = new System.Drawing.Point(467, 56);
            this.btnFiles.Name = "btnFiles";
            this.btnFiles.Size = new System.Drawing.Size(75, 23);
            this.btnFiles.TabIndex = 1;
            this.btnFiles.Text = "Files";
            this.btnFiles.UseVisualStyleBackColor = true;
            this.btnFiles.Click += new System.EventHandler(this.btnFiles_Click);
            // 
            // txtMessage
            // 
            this.txtMessage.Location = new System.Drawing.Point(4, 10);
            this.txtMessage.Multiline = true;
            this.txtMessage.Name = "txtMessage";
            this.txtMessage.Size = new System.Drawing.Size(439, 69);
            this.txtMessage.TabIndex = 2;
            this.txtMessage.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.txtMessage_KeyPress);
            this.txtMessage.KeyUp += new System.Windows.Forms.KeyEventHandler(this.txtMessage_KeyUp);
            // 
            // myDataGridView
            // 
            this.myDataGridView.AllowUserToResizeColumns = false;
            this.myDataGridView.AllowUserToResizeRows = false;
            this.myDataGridView.BackgroundColor = System.Drawing.SystemColors.Control;
            this.myDataGridView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.myDataGridView.Location = new System.Drawing.Point(3, 10);
            this.myDataGridView.Name = "myDataGridView";
            this.myDataGridView.Size = new System.Drawing.Size(440, 91);
            this.myDataGridView.TabIndex = 3;
            this.myDataGridView.CellContentClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.myDataGridView_CellContentClick);
            this.myDataGridView.DragDrop += new System.Windows.Forms.DragEventHandler(this.InputBox_DragDrop);
            this.myDataGridView.DragEnter += new System.Windows.Forms.DragEventHandler(this.InputBox_DragEnter);
            this.myDataGridView.DragOver += new System.Windows.Forms.DragEventHandler(this.InputBox_DragOver);
            this.myDataGridView.DragLeave += new System.EventHandler(this.InputBox_DragLeave);
            // 
            // InputBox
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.Controls.Add(this.myDataGridView);
            this.Controls.Add(this.txtMessage);
            this.Controls.Add(this.btnFiles);
            this.Controls.Add(this.btnSend);
            this.Name = "InputBox";
            this.Size = new System.Drawing.Size(545, 104);
            this.Load += new System.EventHandler(this.InputBox_Load);
            this.DragDrop += new System.Windows.Forms.DragEventHandler(this.InputBox_DragDrop);
            this.DragEnter += new System.Windows.Forms.DragEventHandler(this.InputBox_DragEnter);
            this.DragOver += new System.Windows.Forms.DragEventHandler(this.InputBox_DragOver);
            this.DragLeave += new System.EventHandler(this.InputBox_DragLeave);
            ((System.ComponentModel.ISupportInitialize)(this.myDataGridView)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnSend;
        private System.Windows.Forms.Button btnFiles;
        private System.Windows.Forms.TextBox txtMessage;
        private System.Windows.Forms.DataGridView myDataGridView;
    }
}
