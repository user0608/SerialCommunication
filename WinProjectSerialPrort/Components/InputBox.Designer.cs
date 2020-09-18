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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(InputBox));
            this.btnSend = new System.Windows.Forms.Button();
            this.btnFiles = new System.Windows.Forms.Button();
            this.txtMessage = new System.Windows.Forms.TextBox();
            this.myDataGridView = new System.Windows.Forms.DataGridView();
            ((System.ComponentModel.ISupportInitialize)(this.myDataGridView)).BeginInit();
            this.SuspendLayout();
            // 
            // btnSend
            // 
            this.btnSend.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(244)))), ((int)(((byte)(244)))), ((int)(((byte)(244)))));
            this.btnSend.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("btnSend.BackgroundImage")));
            this.btnSend.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.btnSend.FlatAppearance.BorderColor = System.Drawing.Color.Teal;
            this.btnSend.FlatAppearance.BorderSize = 0;
            this.btnSend.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnSend.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnSend.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(8)))), ((int)(((byte)(8)))), ((int)(((byte)(8)))));
            this.btnSend.Location = new System.Drawing.Point(470, 9);
            this.btnSend.Margin = new System.Windows.Forms.Padding(0);
            this.btnSend.Name = "btnSend";
            this.btnSend.Size = new System.Drawing.Size(75, 86);
            this.btnSend.TabIndex = 2;
            this.btnSend.Text = "Send";
            this.btnSend.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            this.btnSend.UseVisualStyleBackColor = false;
            this.btnSend.Click += new System.EventHandler(this.btnSend_Click);
            // 
            // btnFiles
            // 
            this.btnFiles.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(244)))), ((int)(((byte)(244)))), ((int)(((byte)(244)))));
            this.btnFiles.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("btnFiles.BackgroundImage")));
            this.btnFiles.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.btnFiles.FlatAppearance.BorderSize = 0;
            this.btnFiles.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnFiles.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnFiles.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(8)))), ((int)(((byte)(8)))), ((int)(((byte)(8)))));
            this.btnFiles.Location = new System.Drawing.Point(387, 9);
            this.btnFiles.Margin = new System.Windows.Forms.Padding(0);
            this.btnFiles.Name = "btnFiles";
            this.btnFiles.Size = new System.Drawing.Size(75, 86);
            this.btnFiles.TabIndex = 1;
            this.btnFiles.Text = "Files";
            this.btnFiles.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            this.btnFiles.UseVisualStyleBackColor = false;
            this.btnFiles.Click += new System.EventHandler(this.btnFiles_Click);
            // 
            // txtMessage
            // 
            this.txtMessage.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(233)))), ((int)(((byte)(233)))), ((int)(((byte)(233)))));
            this.txtMessage.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.txtMessage.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtMessage.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(8)))), ((int)(((byte)(8)))), ((int)(((byte)(8)))));
            this.txtMessage.Location = new System.Drawing.Point(0, 8);
            this.txtMessage.Margin = new System.Windows.Forms.Padding(8);
            this.txtMessage.Multiline = true;
            this.txtMessage.Name = "txtMessage";
            this.txtMessage.Size = new System.Drawing.Size(379, 88);
            this.txtMessage.TabIndex = 0;
            this.txtMessage.Text = "Write a message here :D";
            this.txtMessage.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.txtMessage_KeyPress);
            this.txtMessage.KeyUp += new System.Windows.Forms.KeyEventHandler(this.txtMessage_KeyUp);
            // 
            // myDataGridView
            // 
            this.myDataGridView.AllowUserToResizeColumns = false;
            this.myDataGridView.AllowUserToResizeRows = false;
            this.myDataGridView.BackgroundColor = System.Drawing.Color.FromArgb(((int)(((byte)(233)))), ((int)(((byte)(233)))), ((int)(((byte)(233)))));
            this.myDataGridView.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.myDataGridView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.myDataGridView.GridColor = System.Drawing.Color.FromArgb(((int)(((byte)(244)))), ((int)(((byte)(244)))), ((int)(((byte)(244)))));
            this.myDataGridView.Location = new System.Drawing.Point(0, 10);
            this.myDataGridView.Margin = new System.Windows.Forms.Padding(0);
            this.myDataGridView.Name = "myDataGridView";
            this.myDataGridView.Size = new System.Drawing.Size(379, 86);
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
        private System.Windows.Forms.DataGridView myDataGridView;
        private System.Windows.Forms.TextBox txtMessage;
    }
}
