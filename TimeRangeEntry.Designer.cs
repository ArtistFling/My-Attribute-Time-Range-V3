namespace OSIsoft.AF.Asset.DataReference
{
    partial class TimeRangeEntry
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
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.txtRelativeTime = new System.Windows.Forms.TextBox();
            this.txtMinPercentGood = new System.Windows.Forms.TextBox();
            this.lblMinPercentGood = new System.Windows.Forms.Label();
            this.cmbCalculationBasis = new OSIsoft.AF.UI.AFComboBox();
            this.lblCalculationBasis = new System.Windows.Forms.Label();
            this.cmbByTimeRange = new OSIsoft.AF.UI.AFComboBox();
            this.lblByTimeRange = new System.Windows.Forms.Label();
            this.cmbByTime = new OSIsoft.AF.UI.AFComboBox();
            this.lblRelativeTime = new System.Windows.Forms.Label();
            this.lblByTime = new System.Windows.Forms.Label();
            this.lblSourceUnit = new System.Windows.Forms.Label();
            this.gbCalculationOpts = new System.Windows.Forms.GroupBox();
            this.cmbDay = new System.Windows.Forms.ComboBox();
            this.label1 = new System.Windows.Forms.Label();
            this.btnAttributeSearch = new System.Windows.Forms.Button();
            this.txtTargetAttribute = new System.Windows.Forms.TextBox();
            this.lblTargetAttribute = new System.Windows.Forms.Label();
            this.cmbSourceUnit = new OSIsoft.AF.UI.AFUOMDropDown();
            this.btnCancel = new System.Windows.Forms.Button();
            this.btnOK = new System.Windows.Forms.Button();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.groupBox1.SuspendLayout();
            this.gbCalculationOpts.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.txtRelativeTime);
            this.groupBox1.Controls.Add(this.txtMinPercentGood);
            this.groupBox1.Controls.Add(this.lblMinPercentGood);
            this.groupBox1.Controls.Add(this.cmbCalculationBasis);
            this.groupBox1.Controls.Add(this.lblCalculationBasis);
            this.groupBox1.Controls.Add(this.cmbByTimeRange);
            this.groupBox1.Controls.Add(this.lblByTimeRange);
            this.groupBox1.Controls.Add(this.cmbByTime);
            this.groupBox1.Controls.Add(this.lblRelativeTime);
            this.groupBox1.Controls.Add(this.lblByTime);
            this.groupBox1.Location = new System.Drawing.Point(31, 184);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(408, 207);
            this.groupBox1.TabIndex = 7;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Value retrieval methods";
            // 
            // txtRelativeTime
            // 
            this.txtRelativeTime.Location = new System.Drawing.Point(135, 53);
            this.txtRelativeTime.Name = "txtRelativeTime";
            this.txtRelativeTime.Size = new System.Drawing.Size(264, 20);
            this.txtRelativeTime.TabIndex = 21;
            // 
            // txtMinPercentGood
            // 
            this.txtMinPercentGood.Enabled = false;
            this.txtMinPercentGood.Location = new System.Drawing.Point(135, 160);
            this.txtMinPercentGood.Name = "txtMinPercentGood";
            this.txtMinPercentGood.Size = new System.Drawing.Size(264, 20);
            this.txtMinPercentGood.TabIndex = 20;
            // 
            // lblMinPercentGood
            // 
            this.lblMinPercentGood.AutoSize = true;
            this.lblMinPercentGood.Location = new System.Drawing.Point(39, 167);
            this.lblMinPercentGood.Name = "lblMinPercentGood";
            this.lblMinPercentGood.Size = new System.Drawing.Size(93, 13);
            this.lblMinPercentGood.TabIndex = 19;
            this.lblMinPercentGood.Text = "Min percent good:";
            // 
            // cmbCalculationBasis
            // 
            this.cmbCalculationBasis.Enabled = false;
            this.cmbCalculationBasis.FormattingEnabled = true;
            this.cmbCalculationBasis.Location = new System.Drawing.Point(135, 133);
            this.cmbCalculationBasis.Name = "cmbCalculationBasis";
            this.cmbCalculationBasis.Size = new System.Drawing.Size(264, 21);
            this.cmbCalculationBasis.TabIndex = 18;
            // 
            // lblCalculationBasis
            // 
            this.lblCalculationBasis.AutoSize = true;
            this.lblCalculationBasis.Location = new System.Drawing.Point(39, 141);
            this.lblCalculationBasis.Name = "lblCalculationBasis";
            this.lblCalculationBasis.Size = new System.Drawing.Size(89, 13);
            this.lblCalculationBasis.TabIndex = 17;
            this.lblCalculationBasis.Text = "Calculation basis:";
            // 
            // cmbByTimeRange
            // 
            this.cmbByTimeRange.FormattingEnabled = true;
            this.cmbByTimeRange.Location = new System.Drawing.Point(135, 93);
            this.cmbByTimeRange.Name = "cmbByTimeRange";
            this.cmbByTimeRange.Size = new System.Drawing.Size(264, 21);
            this.cmbByTimeRange.TabIndex = 16;
            this.cmbByTimeRange.TextChanged += new System.EventHandler(this.cmbByTimeRange_TextChanged);
            // 
            // lblByTimeRange
            // 
            this.lblByTimeRange.AutoSize = true;
            this.lblByTimeRange.Location = new System.Drawing.Point(8, 101);
            this.lblByTimeRange.Name = "lblByTimeRange";
            this.lblByTimeRange.Size = new System.Drawing.Size(83, 13);
            this.lblByTimeRange.TabIndex = 15;
            this.lblByTimeRange.Text = "By Time Range:";
            // 
            // cmbByTime
            // 
            this.cmbByTime.FormattingEnabled = true;
            this.cmbByTime.Location = new System.Drawing.Point(135, 19);
            this.cmbByTime.Name = "cmbByTime";
            this.cmbByTime.Size = new System.Drawing.Size(264, 21);
            this.cmbByTime.TabIndex = 14;
            this.cmbByTime.TextChanged += new System.EventHandler(this.cmbByTime_TextChanged);
            // 
            // lblRelativeTime
            // 
            this.lblRelativeTime.AutoSize = true;
            this.lblRelativeTime.Location = new System.Drawing.Point(39, 60);
            this.lblRelativeTime.Name = "lblRelativeTime";
            this.lblRelativeTime.Size = new System.Drawing.Size(72, 13);
            this.lblRelativeTime.TabIndex = 12;
            this.lblRelativeTime.Text = "Relative Time";
            // 
            // lblByTime
            // 
            this.lblByTime.AutoSize = true;
            this.lblByTime.Location = new System.Drawing.Point(6, 27);
            this.lblByTime.Name = "lblByTime";
            this.lblByTime.Size = new System.Drawing.Size(45, 13);
            this.lblByTime.TabIndex = 4;
            this.lblByTime.Text = "By Time";
            // 
            // lblSourceUnit
            // 
            this.lblSourceUnit.AutoSize = true;
            this.lblSourceUnit.Location = new System.Drawing.Point(4, 53);
            this.lblSourceUnit.Name = "lblSourceUnit";
            this.lblSourceUnit.Size = new System.Drawing.Size(69, 13);
            this.lblSourceUnit.TabIndex = 13;
            this.lblSourceUnit.Text = "Source units:";
            // 
            // gbCalculationOpts
            // 
            this.gbCalculationOpts.Controls.Add(this.cmbDay);
            this.gbCalculationOpts.Controls.Add(this.label1);
            this.gbCalculationOpts.Controls.Add(this.btnAttributeSearch);
            this.gbCalculationOpts.Controls.Add(this.txtTargetAttribute);
            this.gbCalculationOpts.Controls.Add(this.lblTargetAttribute);
            this.gbCalculationOpts.Controls.Add(this.cmbSourceUnit);
            this.gbCalculationOpts.Controls.Add(this.lblSourceUnit);
            this.gbCalculationOpts.Location = new System.Drawing.Point(11, 19);
            this.gbCalculationOpts.Name = "gbCalculationOpts";
            this.gbCalculationOpts.Size = new System.Drawing.Size(391, 88);
            this.gbCalculationOpts.TabIndex = 2;
            this.gbCalculationOpts.TabStop = false;
            // 
            // cmbDay
            // 
            this.cmbDay.FormattingEnabled = true;
            this.cmbDay.Location = new System.Drawing.Point(245, 45);
            this.cmbDay.Name = "cmbDay";
            this.cmbDay.Size = new System.Drawing.Size(101, 21);
            this.cmbDay.TabIndex = 19;
            this.cmbDay.Visible = false;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(226, 45);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(13, 20);
            this.label1.TabIndex = 18;
            this.label1.Text = "/";
            this.label1.Visible = false;
            // 
            // btnAttributeSearch
            // 
            this.btnAttributeSearch.Location = new System.Drawing.Point(359, 17);
            this.btnAttributeSearch.Name = "btnAttributeSearch";
            this.btnAttributeSearch.Size = new System.Drawing.Size(26, 23);
            this.btnAttributeSearch.TabIndex = 17;
            this.btnAttributeSearch.Text = "...";
            this.btnAttributeSearch.UseVisualStyleBackColor = true;
            this.btnAttributeSearch.Click += new System.EventHandler(this.btnAttributeSearch_Click);
            // 
            // txtTargetAttribute
            // 
            this.txtTargetAttribute.Location = new System.Drawing.Point(96, 19);
            this.txtTargetAttribute.Name = "txtTargetAttribute";
            this.txtTargetAttribute.Size = new System.Drawing.Size(250, 20);
            this.txtTargetAttribute.TabIndex = 16;
            // 
            // lblTargetAttribute
            // 
            this.lblTargetAttribute.AutoSize = true;
            this.lblTargetAttribute.Location = new System.Drawing.Point(6, 26);
            this.lblTargetAttribute.Name = "lblTargetAttribute";
            this.lblTargetAttribute.Size = new System.Drawing.Size(49, 13);
            this.lblTargetAttribute.TabIndex = 15;
            this.lblTargetAttribute.Text = "Attribute:";
            // 
            // cmbSourceUnit
            // 
            this.cmbSourceUnit.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDown;
            this.cmbSourceUnit.FormattingEnabled = true;
            this.cmbSourceUnit.Location = new System.Drawing.Point(96, 45);
            this.cmbSourceUnit.Name = "cmbSourceUnit";
            this.cmbSourceUnit.Size = new System.Drawing.Size(124, 21);
            this.cmbSourceUnit.TabIndex = 14;
            // 
            // btnCancel
            // 
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.Location = new System.Drawing.Point(363, 407);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(75, 23);
            this.btnCancel.TabIndex = 9;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            // 
            // btnOK
            // 
            this.btnOK.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.btnOK.Location = new System.Drawing.Point(282, 407);
            this.btnOK.Name = "btnOK";
            this.btnOK.Size = new System.Drawing.Size(75, 23);
            this.btnOK.TabIndex = 8;
            this.btnOK.Text = "OK";
            this.btnOK.UseVisualStyleBackColor = true;
            this.btnOK.Click += new System.EventHandler(this.btnOK_Click);
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.gbCalculationOpts);
            this.groupBox2.Location = new System.Drawing.Point(31, 42);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(408, 121);
            this.groupBox2.TabIndex = 10;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Calculation Configuration";
            // 
            // TimeRangeEntry
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(470, 452);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnOK);
            this.Controls.Add(this.groupBox2);
            this.Cursor = System.Windows.Forms.Cursors.Default;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Name = "TimeRangeEntry";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "TimeRange";
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.gbCalculationOpts.ResumeLayout(false);
            this.gbCalculationOpts.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Label lblSourceUnit;
        private System.Windows.Forms.Label lblByTime;
        private System.Windows.Forms.GroupBox gbCalculationOpts;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Button btnOK;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.Label lblMinPercentGood;
        private System.Windows.Forms.Label lblCalculationBasis;
        private UI.AFComboBox cmbByTimeRange;
        private System.Windows.Forms.Label lblByTimeRange;
        private UI.AFComboBox cmbByTime;
        private System.Windows.Forms.Label lblRelativeTime;
        private UI.AFUOMDropDown cmbSourceUnit;
        private System.Windows.Forms.Label lblTargetAttribute;
        private System.Windows.Forms.Button btnAttributeSearch;
        private System.Windows.Forms.TextBox txtTargetAttribute;
        private System.Windows.Forms.TextBox txtMinPercentGood;
        private System.Windows.Forms.TextBox txtRelativeTime;
        private System.Windows.Forms.ComboBox cmbDay;
        private System.Windows.Forms.Label label1;
        public UI.AFComboBox cmbCalculationBasis;
    }
}