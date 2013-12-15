namespace Beanulator
{
    partial class FormBrowse
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormBrowse));
            this.menuStrip = new System.Windows.Forms.MenuStrip();
            this.fileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.openToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.recentToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.buttonRecent1 = new System.Windows.Forms.ToolStripMenuItem();
            this.buttonRecent2 = new System.Windows.Forms.ToolStripMenuItem();
            this.buttonRecent3 = new System.Windows.Forms.ToolStripMenuItem();
            this.buttonRecent4 = new System.Windows.Forms.ToolStripMenuItem();
            this.buttonRecent5 = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator = new System.Windows.Forms.ToolStripSeparator();
            this.exitToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.optionsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.openFileDialog = new System.Windows.Forms.OpenFileDialog();
            this.menuStrip.SuspendLayout();
            this.SuspendLayout();
            // 
            // menuStrip
            // 
            this.menuStrip.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.menuStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.fileToolStripMenuItem,
            this.toolsToolStripMenuItem});
            this.menuStrip.Location = new System.Drawing.Point(0, 0);
            this.menuStrip.Name = "menuStrip";
            this.menuStrip.Size = new System.Drawing.Size(624, 24);
            this.menuStrip.TabIndex = 0;
            this.menuStrip.Text = "menuStrip1";
            // 
            // fileToolStripMenuItem
            // 
            this.fileToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.openToolStripMenuItem,
            this.recentToolStripMenuItem,
            this.toolStripSeparator,
            this.exitToolStripMenuItem});
            this.fileToolStripMenuItem.Name = "fileToolStripMenuItem";
            this.fileToolStripMenuItem.Size = new System.Drawing.Size(37, 20);
            this.fileToolStripMenuItem.Text = "File";
            // 
            // openToolStripMenuItem
            // 
            this.openToolStripMenuItem.Image = ((System.Drawing.Image)(resources.GetObject("openToolStripMenuItem.Image")));
            this.openToolStripMenuItem.Name = "openToolStripMenuItem";
            this.openToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.O)));
            this.openToolStripMenuItem.Size = new System.Drawing.Size(154, 22);
            this.openToolStripMenuItem.Text = "&Open...";
            this.openToolStripMenuItem.Click += new System.EventHandler(this.openToolStripMenuItem_Click);
            // 
            // recentToolStripMenuItem
            // 
            this.recentToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.buttonRecent1,
            this.buttonRecent2,
            this.buttonRecent3,
            this.buttonRecent4,
            this.buttonRecent5});
            this.recentToolStripMenuItem.Image = ((System.Drawing.Image)(resources.GetObject("recentToolStripMenuItem.Image")));
            this.recentToolStripMenuItem.Name = "recentToolStripMenuItem";
            this.recentToolStripMenuItem.Size = new System.Drawing.Size(154, 22);
            this.recentToolStripMenuItem.Text = "Recent";
            // 
            // buttonRecent1
            // 
            this.buttonRecent1.Enabled = false;
            this.buttonRecent1.Name = "buttonRecent1";
            this.buttonRecent1.Size = new System.Drawing.Size(83, 22);
            this.buttonRecent1.Text = "1.";
            this.buttonRecent1.Click += new System.EventHandler(this.buttonRecent1_Click);
            // 
            // buttonRecent2
            // 
            this.buttonRecent2.Enabled = false;
            this.buttonRecent2.Name = "buttonRecent2";
            this.buttonRecent2.Size = new System.Drawing.Size(83, 22);
            this.buttonRecent2.Text = "2.";
            this.buttonRecent2.Click += new System.EventHandler(this.buttonRecent2_Click);
            // 
            // buttonRecent3
            // 
            this.buttonRecent3.Enabled = false;
            this.buttonRecent3.Name = "buttonRecent3";
            this.buttonRecent3.Size = new System.Drawing.Size(83, 22);
            this.buttonRecent3.Text = "3.";
            this.buttonRecent3.Click += new System.EventHandler(this.buttonRecent3_Click);
            // 
            // buttonRecent4
            // 
            this.buttonRecent4.Enabled = false;
            this.buttonRecent4.Name = "buttonRecent4";
            this.buttonRecent4.Size = new System.Drawing.Size(83, 22);
            this.buttonRecent4.Text = "4.";
            this.buttonRecent4.Click += new System.EventHandler(this.buttonRecent4_Click);
            // 
            // buttonRecent5
            // 
            this.buttonRecent5.Enabled = false;
            this.buttonRecent5.Name = "buttonRecent5";
            this.buttonRecent5.Size = new System.Drawing.Size(83, 22);
            this.buttonRecent5.Text = "5.";
            this.buttonRecent5.Click += new System.EventHandler(this.buttonRecent5_Click);
            // 
            // toolStripSeparator
            // 
            this.toolStripSeparator.Name = "toolStripSeparator";
            this.toolStripSeparator.Size = new System.Drawing.Size(151, 6);
            // 
            // exitToolStripMenuItem
            // 
            this.exitToolStripMenuItem.Name = "exitToolStripMenuItem";
            this.exitToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Alt | System.Windows.Forms.Keys.F4)));
            this.exitToolStripMenuItem.Size = new System.Drawing.Size(154, 22);
            this.exitToolStripMenuItem.Text = "Exit";
            this.exitToolStripMenuItem.Click += new System.EventHandler(this.exitToolStripMenuItem_Click);
            // 
            // toolsToolStripMenuItem
            // 
            this.toolsToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.optionsToolStripMenuItem});
            this.toolsToolStripMenuItem.Name = "toolsToolStripMenuItem";
            this.toolsToolStripMenuItem.Size = new System.Drawing.Size(46, 20);
            this.toolsToolStripMenuItem.Text = "Tools";
            // 
            // optionsToolStripMenuItem
            // 
            this.optionsToolStripMenuItem.Image = ((System.Drawing.Image)(resources.GetObject("optionsToolStripMenuItem.Image")));
            this.optionsToolStripMenuItem.Name = "optionsToolStripMenuItem";
            this.optionsToolStripMenuItem.Size = new System.Drawing.Size(125, 22);
            this.optionsToolStripMenuItem.Text = "Options...";
            this.optionsToolStripMenuItem.Click += new System.EventHandler(this.optionsToolStripMenuItem_Click);
            // 
            // FormBrowse
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(624, 441);
            this.Controls.Add(this.menuStrip);
            this.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.MainMenuStrip = this.menuStrip;
            this.Name = "FormBrowse";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Beanulator";
            this.menuStrip.ResumeLayout(false);
            this.menuStrip.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.MenuStrip menuStrip;
        private System.Windows.Forms.ToolStripMenuItem fileToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem openToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem toolsToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem optionsToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator;
        private System.Windows.Forms.ToolStripMenuItem exitToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem recentToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem buttonRecent1;
        private System.Windows.Forms.ToolStripMenuItem buttonRecent2;
        private System.Windows.Forms.ToolStripMenuItem buttonRecent3;
        private System.Windows.Forms.ToolStripMenuItem buttonRecent4;
        private System.Windows.Forms.ToolStripMenuItem buttonRecent5;
        private System.Windows.Forms.OpenFileDialog openFileDialog;

    }
}

