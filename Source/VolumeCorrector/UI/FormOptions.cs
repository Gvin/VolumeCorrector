using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading;
using System.Windows.Forms;
using VolumeCorrector.Properties;
using VolumeCorrector.VolumeCorrection;

namespace VolumeCorrector.UI
{
    public partial class FormOptions : Form
    {
        private readonly VolumeMonitor volumeMonitor;
        private readonly List<LanguageData> locales;

        public FormOptions(VolumeMonitor volumeMonitor)
        {
            locales = new List<LanguageData>();

            this.volumeMonitor = volumeMonitor;

            InitializeComponent();
            Icon = System.Drawing.Icon.FromHandle(Resources.VolumeIcon.GetHicon());

            FillLanguageComboBox();

            InitializeMaxValues();
            UpdateMaxVolume();
            UpdateMaxLoudness();
        }

        private void FillLanguageComboBox()
        {
            var englishLanguage = new LanguageData("en", "English");
            locales.Add(englishLanguage);
            comboBoxLanguage.Items.Add(englishLanguage);

            var russianLanguage = new LanguageData("ru", "Русский (Russian)");
            locales.Add(russianLanguage);
            comboBoxLanguage.Items.Add(russianLanguage);

            var matchingCulture = locales.FirstOrDefault(locale => locale.IsLanguageFor(CultureInfo.CurrentUICulture));

            if (matchingCulture == null)
            {
                comboBoxLanguage.SelectedIndex = 0;
            }
            else
            {
                comboBoxLanguage.SelectedItem = matchingCulture;
            }

            comboBoxLanguage.SelectedValueChanged += comboBoxLanguage_SelectedValueChanged;
        }

        private void InitializeMaxValues()
        {
            trackBarMaxVolume.Value = Settings.Default.MaxVolume;
            trackBarMaxLoudness.Value = Settings.Default.MaxLoudness;
        }

        private void UpdateMaxVolume()
        {
            var value = trackBarMaxVolume.Value;

            labelMaxVolume.Text = value.ToString();
            volumeMonitor.MaxVolume = value;
            Settings.Default.MaxVolume = value;
            Settings.Default.Save();
        }

        private void UpdateMaxLoudness()
        {
            var value = trackBarMaxLoudness.Value;

            labelMaxLoudness.Text = value.ToString();
            volumeMonitor.MaxLoudness = value;
            Settings.Default.MaxLoudness = value;
            Settings.Default.Save();
        }

        private void UpdateBars()
        {
            progressBarVolume.Value = volumeMonitor.Volume;
            labelVolume.Text = volumeMonitor.Volume.ToString();

            progressBarLoudness.Value = volumeMonitor.Loudness;
            labelLoudness.Text = volumeMonitor.Loudness.ToString();
        }

        private void timerUpdateStatus_Tick(object sender, EventArgs e)
        {
            UpdateBars();
        }

        private void trackBarMaxVolume_Scroll(object sender, EventArgs e)
        {
            UpdateMaxVolume();
        }

        private void trackBarMaxLoudness_Scroll(object sender, EventArgs e)
        {
            UpdateMaxLoudness();
        }

        private void buttonAutoDetect_Click(object sender, EventArgs e)
        {
            using (var autoDetectForm = new FormAutoDetectLoudness())
            {
                autoDetectForm.ShowDialog();
                if (autoDetectForm.MaxLoudness >= trackBarMaxLoudness.Minimum)
                {
                    trackBarMaxLoudness.Value = autoDetectForm.MaxLoudness;
                }
                else
                {
                    trackBarMaxLoudness.Value = trackBarMaxLoudness.Minimum;
                }
                
                UpdateMaxLoudness();
            }
        }

        private void comboBoxLanguage_SelectedValueChanged(object sender, EventArgs args)
        {
            var selectedLanguage = (LanguageData) comboBoxLanguage.SelectedItem;

            Settings.Default.LanguageCode = selectedLanguage.Code;
            Settings.Default.Save();

            Thread.CurrentThread.CurrentUICulture = new CultureInfo(selectedLanguage.Code);

            MessageBox.Show(Resources.LanguageChange_Text, Resources.LanguageChange_Caption, MessageBoxButtons.OK,
                MessageBoxIcon.Warning);
        }

        private class LanguageData
        {
            private readonly string code;
            private readonly string text;

            public LanguageData(string code, string text)
            {
                this.code = code;
                this.text = text;
            }

            public string Code => code;

            public string Text => text;

            public bool IsLanguageFor(CultureInfo culture)
            {
                if (string.Equals(code, culture.Name, StringComparison.OrdinalIgnoreCase))
                    return true;

                var shortCode = culture.Name.Split('-')[0];
                return string.Equals(code, shortCode, StringComparison.OrdinalIgnoreCase);
            }

            public override string ToString()
            {
                return text;
            }
        }
    }
}
