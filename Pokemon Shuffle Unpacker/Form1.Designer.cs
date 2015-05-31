namespace Pokemon_Shuffle_Unpacker
{
    partial class Form1
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

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.TB_FilePath = new System.Windows.Forms.TextBox();
            this.B_Go = new System.Windows.Forms.Button();
            this.B_Open = new System.Windows.Forms.Button();
            this.RTB_Output = new System.Windows.Forms.RichTextBox();
            this.CHK_UseKnownNames = new System.Windows.Forms.CheckBox();
            this.CHK_RenameArchiveFolders = new System.Windows.Forms.CheckBox();
            this.label1 = new System.Windows.Forms.Label();
            this.CHK_DeleteArchives = new System.Windows.Forms.CheckBox();
            this.SuspendLayout();
            // 
            // TB_FilePath
            // 
            this.TB_FilePath.Location = new System.Drawing.Point(98, 12);
            this.TB_FilePath.Name = "TB_FilePath";
            this.TB_FilePath.ReadOnly = true;
            this.TB_FilePath.Size = new System.Drawing.Size(275, 20);
            this.TB_FilePath.TabIndex = 7;
            this.TB_FilePath.TextChanged += new System.EventHandler(this.TB_FilePath_TextChanged);
            // 
            // B_Go
            // 
            this.B_Go.Enabled = false;
            this.B_Go.ForeColor = System.Drawing.SystemColors.ControlText;
            this.B_Go.Location = new System.Drawing.Point(379, 9);
            this.B_Go.Name = "B_Go";
            this.B_Go.Size = new System.Drawing.Size(80, 25);
            this.B_Go.TabIndex = 6;
            this.B_Go.Text = "Go";
            this.B_Go.UseVisualStyleBackColor = true;
            this.B_Go.Click += new System.EventHandler(this.B_Go_Click);
            // 
            // B_Open
            // 
            this.B_Open.Location = new System.Drawing.Point(9, 9);
            this.B_Open.Name = "B_Open";
            this.B_Open.Size = new System.Drawing.Size(84, 25);
            this.B_Open.TabIndex = 5;
            this.B_Open.Text = "Open";
            this.B_Open.UseVisualStyleBackColor = true;
            this.B_Open.Click += new System.EventHandler(this.B_Open_Click);
            // 
            // RTB_Output
            // 
            this.RTB_Output.BackColor = System.Drawing.SystemColors.Control;
            this.RTB_Output.Location = new System.Drawing.Point(9, 62);
            this.RTB_Output.Name = "RTB_Output";
            this.RTB_Output.ReadOnly = true;
            this.RTB_Output.Size = new System.Drawing.Size(450, 236);
            this.RTB_Output.TabIndex = 8;
            this.RTB_Output.Text = "";
            // 
            // CHK_UseKnownNames
            // 
            this.CHK_UseKnownNames.AutoSize = true;
            this.CHK_UseKnownNames.Checked = true;
            this.CHK_UseKnownNames.CheckState = System.Windows.Forms.CheckState.Checked;
            this.CHK_UseKnownNames.Location = new System.Drawing.Point(156, 40);
            this.CHK_UseKnownNames.Name = "CHK_UseKnownNames";
            this.CHK_UseKnownNames.Size = new System.Drawing.Size(126, 17);
            this.CHK_UseKnownNames.TabIndex = 9;
            this.CHK_UseKnownNames.Text = "Rename Known Files";
            this.CHK_UseKnownNames.UseVisualStyleBackColor = true;
            this.CHK_UseKnownNames.CheckedChanged += new System.EventHandler(this.CHK_UseKnownNames_CheckedChanged);
            // 
            // CHK_RenameArchiveFolders
            // 
            this.CHK_RenameArchiveFolders.AutoSize = true;
            this.CHK_RenameArchiveFolders.Checked = true;
            this.CHK_RenameArchiveFolders.CheckState = System.Windows.Forms.CheckState.Checked;
            this.CHK_RenameArchiveFolders.Location = new System.Drawing.Point(12, 40);
            this.CHK_RenameArchiveFolders.Name = "CHK_RenameArchiveFolders";
            this.CHK_RenameArchiveFolders.Size = new System.Drawing.Size(138, 17);
            this.CHK_RenameArchiveFolders.TabIndex = 10;
            this.CHK_RenameArchiveFolders.Text = "Rename Output Folders";
            this.CHK_RenameArchiveFolders.UseVisualStyleBackColor = true;
            this.CHK_RenameArchiveFolders.CheckedChanged += new System.EventHandler(this.CHK_RenameArchiveFolders_CheckedChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(23, 303);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(422, 13);
            this.label1.TabIndex = 11;
            this.label1.Text = "Made by SciresM (File name/Archive name lists complete as of Pokemon Shuffle v1.2" +
                ".1)";
            // 
            // CHK_DeleteArchives
            // 
            this.CHK_DeleteArchives.AutoSize = true;
            this.CHK_DeleteArchives.Location = new System.Drawing.Point(283, 40);
            this.CHK_DeleteArchives.Name = "CHK_DeleteArchives";
            this.CHK_DeleteArchives.Size = new System.Drawing.Size(176, 17);
            this.CHK_DeleteArchives.TabIndex = 12;
            this.CHK_DeleteArchives.Text = "Delete Archives After Extracting";
            this.CHK_DeleteArchives.UseVisualStyleBackColor = true;
            this.CHK_DeleteArchives.CheckedChanged += new System.EventHandler(this.CHK_DeleteArchives_CheckedChanged);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(469, 321);
            this.Controls.Add(this.CHK_DeleteArchives);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.CHK_RenameArchiveFolders);
            this.Controls.Add(this.CHK_UseKnownNames);
            this.Controls.Add(this.RTB_Output);
            this.Controls.Add(this.TB_FilePath);
            this.Controls.Add(this.B_Go);
            this.Controls.Add(this.B_Open);
            this.MaximumSize = new System.Drawing.Size(485, 360);
            this.MinimumSize = new System.Drawing.Size(485, 360);
            this.Name = "Form1";
            this.Text = "Pokemon Shuffle Unpacker";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox TB_FilePath;
        private System.Windows.Forms.Button B_Go;
        private System.Windows.Forms.Button B_Open;
        private System.Windows.Forms.RichTextBox RTB_Output;
        private System.Windows.Forms.CheckBox CHK_UseKnownNames;
        private System.Windows.Forms.CheckBox CHK_RenameArchiveFolders;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.CheckBox CHK_DeleteArchives;
    }
}

