namespace VolumeCorrector.Views
{
    partial class FormOptions
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
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormOptions));
            this.toolTip = new System.Windows.Forms.ToolTip(this.components);
            this.buttonAutoDetect = new System.Windows.Forms.Button();
            this.trackBarMaxLoudness = new System.Windows.Forms.TrackBar();
            this.progressBarLoudness = new System.Windows.Forms.ProgressBar();
            this.trackBarMaxVolume = new System.Windows.Forms.TrackBar();
            this.progressBarVolume = new System.Windows.Forms.ProgressBar();
            this.comboBoxLanguage = new System.Windows.Forms.ComboBox();
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabPageVolume = new System.Windows.Forms.TabPage();
            this.groupBoxLoudness = new System.Windows.Forms.GroupBox();
            this.labelMaxLoudness = new System.Windows.Forms.Label();
            this.labelMaxLoudnessText = new System.Windows.Forms.Label();
            this.labelLoudness = new System.Windows.Forms.Label();
            this.groupBoxVolume = new System.Windows.Forms.GroupBox();
            this.labelMaxVolume = new System.Windows.Forms.Label();
            this.labelMaxVolumeText = new System.Windows.Forms.Label();
            this.labelVolume = new System.Windows.Forms.Label();
            this.tabPageCommon = new System.Windows.Forms.TabPage();
            this.labelLanguageText = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.trackBarMaxLoudness)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.trackBarMaxVolume)).BeginInit();
            this.tabControl1.SuspendLayout();
            this.tabPageVolume.SuspendLayout();
            this.groupBoxLoudness.SuspendLayout();
            this.groupBoxVolume.SuspendLayout();
            this.tabPageCommon.SuspendLayout();
            this.SuspendLayout();
            // 
            // buttonAutoDetect
            // 
            resources.ApplyResources(this.buttonAutoDetect, "buttonAutoDetect");
            this.buttonAutoDetect.Name = "buttonAutoDetect";
            this.toolTip.SetToolTip(this.buttonAutoDetect, resources.GetString("buttonAutoDetect.ToolTip"));
            this.buttonAutoDetect.UseVisualStyleBackColor = true;
            this.buttonAutoDetect.Click += new System.EventHandler(this.buttonAutoDetect_Click);
            // 
            // trackBarMaxLoudness
            // 
            resources.ApplyResources(this.trackBarMaxLoudness, "trackBarMaxLoudness");
            this.trackBarMaxLoudness.LargeChange = 10;
            this.trackBarMaxLoudness.Maximum = 100;
            this.trackBarMaxLoudness.Minimum = 10;
            this.trackBarMaxLoudness.Name = "trackBarMaxLoudness";
            this.toolTip.SetToolTip(this.trackBarMaxLoudness, resources.GetString("trackBarMaxLoudness.ToolTip"));
            this.trackBarMaxLoudness.Value = 10;
            this.trackBarMaxLoudness.Scroll += new System.EventHandler(this.trackBarMaxLoudness_Scroll);
            // 
            // progressBarLoudness
            // 
            resources.ApplyResources(this.progressBarLoudness, "progressBarLoudness");
            this.progressBarLoudness.Name = "progressBarLoudness";
            this.toolTip.SetToolTip(this.progressBarLoudness, resources.GetString("progressBarLoudness.ToolTip"));
            // 
            // trackBarMaxVolume
            // 
            resources.ApplyResources(this.trackBarMaxVolume, "trackBarMaxVolume");
            this.trackBarMaxVolume.LargeChange = 10;
            this.trackBarMaxVolume.Maximum = 100;
            this.trackBarMaxVolume.Minimum = 10;
            this.trackBarMaxVolume.Name = "trackBarMaxVolume";
            this.toolTip.SetToolTip(this.trackBarMaxVolume, resources.GetString("trackBarMaxVolume.ToolTip"));
            this.trackBarMaxVolume.Value = 10;
            this.trackBarMaxVolume.Scroll += new System.EventHandler(this.trackBarMaxVolume_Scroll);
            // 
            // progressBarVolume
            // 
            resources.ApplyResources(this.progressBarVolume, "progressBarVolume");
            this.progressBarVolume.Name = "progressBarVolume";
            this.toolTip.SetToolTip(this.progressBarVolume, resources.GetString("progressBarVolume.ToolTip"));
            // 
            // comboBoxLanguage
            // 
            this.comboBoxLanguage.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBoxLanguage.FormattingEnabled = true;
            resources.ApplyResources(this.comboBoxLanguage, "comboBoxLanguage");
            this.comboBoxLanguage.Name = "comboBoxLanguage";
            this.toolTip.SetToolTip(this.comboBoxLanguage, resources.GetString("comboBoxLanguage.ToolTip"));
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.tabPageVolume);
            this.tabControl1.Controls.Add(this.tabPageCommon);
            resources.ApplyResources(this.tabControl1, "tabControl1");
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            // 
            // tabPageVolume
            // 
            this.tabPageVolume.BackColor = System.Drawing.SystemColors.Control;
            this.tabPageVolume.Controls.Add(this.groupBoxLoudness);
            this.tabPageVolume.Controls.Add(this.groupBoxVolume);
            resources.ApplyResources(this.tabPageVolume, "tabPageVolume");
            this.tabPageVolume.Name = "tabPageVolume";
            // 
            // groupBoxLoudness
            // 
            this.groupBoxLoudness.Controls.Add(this.buttonAutoDetect);
            this.groupBoxLoudness.Controls.Add(this.labelMaxLoudness);
            this.groupBoxLoudness.Controls.Add(this.labelMaxLoudnessText);
            this.groupBoxLoudness.Controls.Add(this.trackBarMaxLoudness);
            this.groupBoxLoudness.Controls.Add(this.labelLoudness);
            this.groupBoxLoudness.Controls.Add(this.progressBarLoudness);
            resources.ApplyResources(this.groupBoxLoudness, "groupBoxLoudness");
            this.groupBoxLoudness.Name = "groupBoxLoudness";
            this.groupBoxLoudness.TabStop = false;
            // 
            // labelMaxLoudness
            // 
            resources.ApplyResources(this.labelMaxLoudness, "labelMaxLoudness");
            this.labelMaxLoudness.Name = "labelMaxLoudness";
            // 
            // labelMaxLoudnessText
            // 
            resources.ApplyResources(this.labelMaxLoudnessText, "labelMaxLoudnessText");
            this.labelMaxLoudnessText.Name = "labelMaxLoudnessText";
            // 
            // labelLoudness
            // 
            resources.ApplyResources(this.labelLoudness, "labelLoudness");
            this.labelLoudness.Name = "labelLoudness";
            // 
            // groupBoxVolume
            // 
            this.groupBoxVolume.Controls.Add(this.labelMaxVolume);
            this.groupBoxVolume.Controls.Add(this.labelMaxVolumeText);
            this.groupBoxVolume.Controls.Add(this.trackBarMaxVolume);
            this.groupBoxVolume.Controls.Add(this.labelVolume);
            this.groupBoxVolume.Controls.Add(this.progressBarVolume);
            resources.ApplyResources(this.groupBoxVolume, "groupBoxVolume");
            this.groupBoxVolume.Name = "groupBoxVolume";
            this.groupBoxVolume.TabStop = false;
            // 
            // labelMaxVolume
            // 
            resources.ApplyResources(this.labelMaxVolume, "labelMaxVolume");
            this.labelMaxVolume.Name = "labelMaxVolume";
            // 
            // labelMaxVolumeText
            // 
            resources.ApplyResources(this.labelMaxVolumeText, "labelMaxVolumeText");
            this.labelMaxVolumeText.Name = "labelMaxVolumeText";
            // 
            // labelVolume
            // 
            resources.ApplyResources(this.labelVolume, "labelVolume");
            this.labelVolume.Name = "labelVolume";
            // 
            // tabPageCommon
            // 
            this.tabPageCommon.BackColor = System.Drawing.SystemColors.Control;
            this.tabPageCommon.Controls.Add(this.comboBoxLanguage);
            this.tabPageCommon.Controls.Add(this.labelLanguageText);
            resources.ApplyResources(this.tabPageCommon, "tabPageCommon");
            this.tabPageCommon.Name = "tabPageCommon";
            // 
            // labelLanguageText
            // 
            resources.ApplyResources(this.labelLanguageText, "labelLanguageText");
            this.labelLanguageText.Name = "labelLanguageText";
            // 
            // FormOptions
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.tabControl1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.Name = "FormOptions";
            ((System.ComponentModel.ISupportInitialize)(this.trackBarMaxLoudness)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.trackBarMaxVolume)).EndInit();
            this.tabControl1.ResumeLayout(false);
            this.tabPageVolume.ResumeLayout(false);
            this.groupBoxLoudness.ResumeLayout(false);
            this.groupBoxLoudness.PerformLayout();
            this.groupBoxVolume.ResumeLayout(false);
            this.groupBoxVolume.PerformLayout();
            this.tabPageCommon.ResumeLayout(false);
            this.tabPageCommon.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.ToolTip toolTip;
        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tabPageVolume;
        private System.Windows.Forms.GroupBox groupBoxLoudness;
        private System.Windows.Forms.Button buttonAutoDetect;
        private System.Windows.Forms.Label labelMaxLoudness;
        private System.Windows.Forms.Label labelMaxLoudnessText;
        private System.Windows.Forms.TrackBar trackBarMaxLoudness;
        private System.Windows.Forms.Label labelLoudness;
        private System.Windows.Forms.ProgressBar progressBarLoudness;
        private System.Windows.Forms.GroupBox groupBoxVolume;
        private System.Windows.Forms.Label labelMaxVolume;
        private System.Windows.Forms.Label labelMaxVolumeText;
        private System.Windows.Forms.TrackBar trackBarMaxVolume;
        private System.Windows.Forms.Label labelVolume;
        private System.Windows.Forms.ProgressBar progressBarVolume;
        private System.Windows.Forms.TabPage tabPageCommon;
        private System.Windows.Forms.ComboBox comboBoxLanguage;
        private System.Windows.Forms.Label labelLanguageText;
    }
}

