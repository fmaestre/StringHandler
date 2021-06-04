namespace String_Handling
{
    partial class frmStringsHandling
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
            this.txtEntries = new System.Windows.Forms.TextBox();
            this.txtResults = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.btnIn = new System.Windows.Forms.Button();
            this.cmbLis = new System.Windows.Forms.ComboBox();
            this.btnDualUnion = new System.Windows.Forms.Button();
            this.button1 = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // txtEntries
            // 
            this.txtEntries.Location = new System.Drawing.Point(12, 23);
            this.txtEntries.Multiline = true;
            this.txtEntries.Name = "txtEntries";
            this.txtEntries.Size = new System.Drawing.Size(193, 363);
            this.txtEntries.TabIndex = 0;
            // 
            // txtResults
            // 
            this.txtResults.Location = new System.Drawing.Point(395, 23);
            this.txtResults.Multiline = true;
            this.txtResults.Name = "txtResults";
            this.txtResults.Size = new System.Drawing.Size(203, 363);
            this.txtResults.TabIndex = 1;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 7);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(39, 13);
            this.label1.TabIndex = 2;
            this.label1.Text = "Entries";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(392, 9);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(42, 13);
            this.label2.TabIndex = 3;
            this.label2.Text = "Results";
            // 
            // btnIn
            // 
            this.btnIn.Location = new System.Drawing.Point(227, 22);
            this.btnIn.Name = "btnIn";
            this.btnIn.Size = new System.Drawing.Size(51, 22);
            this.btnIn.TabIndex = 4;
            this.btnIn.Text = "IN";
            this.btnIn.UseVisualStyleBackColor = true;
            this.btnIn.Click += new System.EventHandler(this.btnIn_Click);
            // 
            // cmbLis
            // 
            this.cmbLis.FormattingEnabled = true;
            this.cmbLis.Items.AddRange(new object[] {
            "\'",
            " "});
            this.cmbLis.Location = new System.Drawing.Point(284, 23);
            this.cmbLis.Name = "cmbLis";
            this.cmbLis.Size = new System.Drawing.Size(70, 21);
            this.cmbLis.TabIndex = 5;
            this.cmbLis.SelectedIndexChanged += new System.EventHandler(this.comboBox1_SelectedIndexChanged);
            // 
            // btnDualUnion
            // 
            this.btnDualUnion.Location = new System.Drawing.Point(228, 50);
            this.btnDualUnion.Name = "btnDualUnion";
            this.btnDualUnion.Size = new System.Drawing.Size(84, 22);
            this.btnDualUnion.TabIndex = 6;
            this.btnDualUnion.Text = "Dual Union";
            this.btnDualUnion.UseVisualStyleBackColor = true;
            this.btnDualUnion.Click += new System.EventHandler(this.btnDualUnion_Click);
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(228, 78);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(84, 22);
            this.button1.TabIndex = 7;
            this.button1.Text = "Load Excel";
            this.button1.UseVisualStyleBackColor = true;
            // 
            // frmStringsHandling
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(610, 398);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.btnDualUnion);
            this.Controls.Add(this.cmbLis);
            this.Controls.Add(this.btnIn);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.txtResults);
            this.Controls.Add(this.txtEntries);
            this.Name = "frmStringsHandling";
            this.Text = "Strings";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox txtEntries;
        private System.Windows.Forms.TextBox txtResults;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button btnIn;
        private System.Windows.Forms.ComboBox cmbLis;
        private System.Windows.Forms.Button btnDualUnion;
        private System.Windows.Forms.Button button1;
    }
}

