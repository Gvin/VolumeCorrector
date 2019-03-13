using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Windows.Forms;
using VolumeCorrector.Properties;

namespace VolumeCorrector.Views
{
    public partial class FormOptions : Form, IOptionsView
    {
        private readonly List<LanguageData> locales;

        public event EventHandler MaxVolumeChanged;
        public event EventHandler MaxLoudnessChanged;
        public event EventHandler CultureCodeChanged;

        public FormOptions()
        {
            locales = new List<LanguageData>();

            InitializeComponent();
            Icon = System.Drawing.Icon.FromHandle(Resources.VolumeIcon.GetHicon());

            FillLanguageComboBox();

            InitializeMaxValues();
        }

        private void FillLanguageComboBox()
        {
            var englishLanguage = new LanguageData("en", "English");
            locales.Add(englishLanguage);
            comboBoxLanguage.Items.Add(englishLanguage);

            var russianLanguage = new LanguageData("ru", "Русский (Russian)");
            locales.Add(russianLanguage);
            comboBoxLanguage.Items.Add(russianLanguage);

            comboBoxLanguage.SelectedValueChanged += comboBoxLanguage_SelectedValueChanged;
        }

        private void InitializeMaxValues()
        {
            trackBarMaxVolume.Value = Settings.Default.MaxVolume;
            trackBarMaxLoudness.Value = Settings.Default.MaxLoudness;
        }

        public int MaxVolume
        {
            get => trackBarMaxVolume.Value;
            set
            {
                trackBarMaxVolume.Value = value;
                labelMaxVolume.Text = value.ToString();
            }
        }

        public int MaxLoudness
        {
            get => trackBarMaxLoudness.Value;
            set
            {
                trackBarMaxLoudness.Value = value;
                labelMaxLoudness.Text = value.ToString();
            }
        }

        public int Volume
        {
            set
            {
                progressBarVolume.Value = value;
                labelVolume.Text = value.ToString();
            }
        }

        public int Loudness
        {
            set
            {
                progressBarLoudness.Value = value;
                labelLoudness.Text = value.ToString();
            }
        }

        public string CultureCode
        {
            get
            {
                var selectedLanguage = (LanguageData)comboBoxLanguage.SelectedItem;
                return selectedLanguage.Code;
            }
            set
            {
                var matchingCulture = locales.FirstOrDefault(locale =>
                    string.Equals(locale.Code, value, StringComparison.OrdinalIgnoreCase) ||
                    string.Equals(locale.Code, value.Split('-')[0], StringComparison.OrdinalIgnoreCase));

                if (matchingCulture == null)
                {
                    comboBoxLanguage.SelectedIndex = 0;
                }
                else
                {
                    comboBoxLanguage.SelectedItem = matchingCulture;
                }
            }
        }

        public void ForceBringToFront()
        {
            Show();
            WindowState = FormWindowState.Normal;
            Activate();
        }

        private void trackBarMaxVolume_Scroll(object sender, EventArgs e)
        {
            MaxVolumeChanged?.Invoke(this, EventArgs.Empty);
        }

        private void trackBarMaxLoudness_Scroll(object sender, EventArgs e)
        {
            MaxLoudnessChanged?.Invoke(this, EventArgs.Empty);
        }

        private void comboBoxLanguage_SelectedValueChanged(object sender, EventArgs args)
        {
            CultureCodeChanged?.Invoke(this, EventArgs.Empty);
        }

        private class LanguageData
        {
            private readonly string text;

            public LanguageData(string code, string text)
            {
                Code = code;
                this.text = text;
            }

            public string Code { get; }

            public override string ToString()
            {
                return text;
            }
        }
    }
}
