namespace VolumeCorrector.Views
{
    partial class FormAutoDetectLoudness
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormAutoDetectLoudness));
            this.label1 = new System.Windows.Forms.Label();
            this.progressBarLoudness = new System.Windows.Forms.ProgressBar();
            this.labelLoudness = new System.Windows.Forms.Label();
            this.buttonReady = new System.Windows.Forms.Button();
            this.timerUpdateLoudnessValue = new System.Windows.Forms.Timer(this.components);
            this.toolTip = new System.Windows.Forms.ToolTip(this.components);
            this.SuspendLayout();
            // 
            // label1
            // 
            resources.ApplyResources(this.label1, "label1");
            this.label1.Name = "label1";
            // 
            // progressBarLoudness
            // 
            resources.ApplyResources(this.progressBarLoudness, "progressBarLoudness");
            this.progressBarLoudness.Name = "progressBarLoudness";
            this.toolTip.SetToolTip(this.progressBarLoudness, resources.GetString("progressBarLoudness.ToolTip"));
            // 
            // labelLoudness
            // 
            resources.ApplyResources(this.labelLoudness, "labelLoudness");
            this.labelLoudness.Name = "labelLoudness";
            this.toolTip.SetToolTip(this.labelLoudness, resources.GetString("labelLoudness.ToolTip"));
            // 
            // buttonReady
            // 
            resources.ApplyResources(this.buttonReady, "buttonReady");
            this.buttonReady.Name = "buttonReady";
            this.toolTip.SetToolTip(this.buttonReady, resources.GetString("buttonReady.ToolTip"));
            this.buttonReady.UseVisualStyleBackColor = true;
            this.buttonReady.Click += new System.EventHandler(this.buttonReady_Click);
            // 
            // timerUpdateLoudnessValue
            // 
            this.timerUpdateLoudnessValue.Enabled = true;
            this.timerUpdateLoudnessValue.Tick += new System.EventHandler(this.timerUpdateLoudnessValue_Tick);
            // 
            // FormAutoDetectLoudness
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ControlBox = false;
            this.Controls.Add(this.buttonReady);
            this.Controls.Add(this.labelLoudness);
            this.Controls.Add(this.progressBarLoudness);
            this.Controls.Add(this.label1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FormAutoDetectLoudness";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.Shown += new System.EventHandler(this.FormAutoDetectLoudness_Shown);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ProgressBar progressBarLoudness;
        private System.Windows.Forms.Label labelLoudness;
        private System.Windows.Forms.Button buttonReady;
        private System.Windows.Forms.Timer timerUpdateLoudnessValue;
        private System.Windows.Forms.ToolTip toolTip;
    }
}