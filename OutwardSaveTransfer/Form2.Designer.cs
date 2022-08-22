
namespace OutwardSaveTransfer
{
    partial class Form2
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form2));
            this.file_browser_button = new System.Windows.Forms.Button();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.check_save_location_button = new System.Windows.Forms.Button();
            this.label2 = new System.Windows.Forms.Label();
            this.consoleLabel = new System.Windows.Forms.Label();
            this.consoleWindow = new System.Windows.Forms.RichTextBox();
            this.SuspendLayout();
            // 
            // file_browser_button
            // 
            this.file_browser_button.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.file_browser_button.BackColor = System.Drawing.Color.White;
            this.file_browser_button.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.file_browser_button.Font = new System.Drawing.Font("Microsoft Sans Serif", 6F);
            this.file_browser_button.ForeColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.file_browser_button.Location = new System.Drawing.Point(549, 378);
            this.file_browser_button.Margin = new System.Windows.Forms.Padding(0);
            this.file_browser_button.Name = "file_browser_button";
            this.file_browser_button.Size = new System.Drawing.Size(28, 20);
            this.file_browser_button.TabIndex = 7;
            this.file_browser_button.Text = ". . .";
            this.file_browser_button.UseVisualStyleBackColor = false;
            this.file_browser_button.Click += new System.EventHandler(this.file_browser_button_Click);
            // 
            // textBox1
            // 
            this.textBox1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.textBox1.Location = new System.Drawing.Point(12, 378);
            this.textBox1.Name = "textBox1";
            this.textBox1.Size = new System.Drawing.Size(519, 20);
            this.textBox1.TabIndex = 6;
            // 
            // label1
            // 
            this.label1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F);
            this.label1.Location = new System.Drawing.Point(8, 355);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(190, 17);
            this.label1.TabIndex = 5;
            this.label1.Text = "Type in your output directory";
            // 
            // check_save_location_button
            // 
            this.check_save_location_button.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.check_save_location_button.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(30)))), ((int)(((byte)(219)))), ((int)(((byte)(6)))));
            this.check_save_location_button.FlatAppearance.BorderSize = 0;
            this.check_save_location_button.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.check_save_location_button.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(254)));
            this.check_save_location_button.ForeColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.check_save_location_button.Location = new System.Drawing.Point(12, 413);
            this.check_save_location_button.Margin = new System.Windows.Forms.Padding(0);
            this.check_save_location_button.Name = "check_save_location_button";
            this.check_save_location_button.Size = new System.Drawing.Size(565, 26);
            this.check_save_location_button.TabIndex = 8;
            this.check_save_location_button.Text = "Start";
            this.check_save_location_button.UseVisualStyleBackColor = false;
            this.check_save_location_button.Click += new System.EventHandler(this.check_save_location_button_Click);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F);
            this.label2.Location = new System.Drawing.Point(12, 13);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(214, 20);
            this.label2.TabIndex = 9;
            this.label2.Text = "Total saved characters found";
            // 
            // consoleLabel
            // 
            this.consoleLabel.AutoSize = true;
            this.consoleLabel.Location = new System.Drawing.Point(11, 58);
            this.consoleLabel.Name = "consoleLabel";
            this.consoleLabel.Size = new System.Drawing.Size(87, 13);
            this.consoleLabel.TabIndex = 11;
            this.consoleLabel.Text = "Console Window";
            // 
            // consoleWindow
            // 
            this.consoleWindow.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.consoleWindow.Location = new System.Drawing.Point(14, 74);
            this.consoleWindow.Name = "consoleWindow";
            this.consoleWindow.ReadOnly = true;
            this.consoleWindow.Size = new System.Drawing.Size(564, 262);
            this.consoleWindow.TabIndex = 12;
            this.consoleWindow.Text = "";
            this.consoleWindow.TextChanged += new System.EventHandler(this.consoleWindow_TextChanged);
            // 
            // Form2
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(593, 450);
            this.Controls.Add(this.consoleWindow);
            this.Controls.Add(this.consoleLabel);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.check_save_location_button);
            this.Controls.Add(this.file_browser_button);
            this.Controls.Add(this.textBox1);
            this.Controls.Add(this.label1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MinimumSize = new System.Drawing.Size(609, 489);
            this.Name = "Form2";
            this.Text = "Options";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Form2_Closing);
            this.Load += new System.EventHandler(this.Form2_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button file_browser_button;
        private System.Windows.Forms.TextBox textBox1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button check_save_location_button;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label consoleLabel;
        private System.Windows.Forms.RichTextBox consoleWindow;
    }
}