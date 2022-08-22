
namespace OutwardSaveTransfer
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form1));
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.check_save_location_button = new System.Windows.Forms.Button();
            this.file_browser_button = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F);
            this.label1.Location = new System.Drawing.Point(12, 95);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(194, 20);
            this.label1.TabIndex = 0;
            this.label1.Text = "Type in your save directory";
            this.label1.Click += new System.EventHandler(this.label1_Click);
            // 
            // label2
            // 
            this.label2.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 15F);
            this.label2.Location = new System.Drawing.Point(172, 34);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(232, 25);
            this.label2.TabIndex = 1;
            this.label2.Text = "Outward save file transfer";
            // 
            // textBox1
            // 
            this.textBox1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.textBox1.Location = new System.Drawing.Point(16, 118);
            this.textBox1.Name = "textBox1";
            this.textBox1.Size = new System.Drawing.Size(519, 20);
            this.textBox1.TabIndex = 2;
            this.textBox1.TextChanged += new System.EventHandler(this.textBox1_TextChanged);
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
            this.check_save_location_button.Location = new System.Drawing.Point(16, 154);
            this.check_save_location_button.Margin = new System.Windows.Forms.Padding(0);
            this.check_save_location_button.Name = "check_save_location_button";
            this.check_save_location_button.Size = new System.Drawing.Size(565, 26);
            this.check_save_location_button.TabIndex = 3;
            this.check_save_location_button.Text = "Start";
            this.check_save_location_button.UseVisualStyleBackColor = false;
            this.check_save_location_button.Click += new System.EventHandler(this.check_save_location_button_Click);
            // 
            // file_browser_button
            // 
            this.file_browser_button.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.file_browser_button.BackColor = System.Drawing.Color.White;
            this.file_browser_button.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.file_browser_button.Font = new System.Drawing.Font("Microsoft Sans Serif", 6F);
            this.file_browser_button.ForeColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.file_browser_button.Location = new System.Drawing.Point(553, 118);
            this.file_browser_button.Margin = new System.Windows.Forms.Padding(0);
            this.file_browser_button.Name = "file_browser_button";
            this.file_browser_button.Size = new System.Drawing.Size(28, 20);
            this.file_browser_button.TabIndex = 4;
            this.file_browser_button.Text = ". . .";
            this.file_browser_button.UseVisualStyleBackColor = false;
            this.file_browser_button.Click += new System.EventHandler(this.button1_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.ControlLight;
            this.ClientSize = new System.Drawing.Size(593, 197);
            this.Controls.Add(this.file_browser_button);
            this.Controls.Add(this.check_save_location_button);
            this.Controls.Add(this.textBox1);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "Form1";
            this.Text = "Outward save transfer";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox textBox1;
        private System.Windows.Forms.Button check_save_location_button;
        private System.Windows.Forms.Button file_browser_button;
    }
}

