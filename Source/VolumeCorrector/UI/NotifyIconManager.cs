using System;
using System.Drawing;
using System.Windows.Forms;
using VolumeCorrector.Properties;
using VolumeCorrector.VolumeCorrection;

namespace VolumeCorrector.UI
{
    /// <summary>
    /// Handles notify icon of the application.
    /// Processes clicks and context menu.
    /// </summary>
    public class NotifyIconManager : IDisposable
    {
        private FormOptions formOptions;

        private readonly NotifyIcon icon;
        private readonly VolumeMonitor volumeMonitor;

        /// <summary>
        /// Creates new instance of <c>NotifyIconManager</c>.
        /// </summary>
        /// <param name="volumeMonitor">Volume monitor to track status.</param>
        public NotifyIconManager(VolumeMonitor volumeMonitor)
        {
            this.volumeMonitor = volumeMonitor;
            this.volumeMonitor.StatusChanged += volumeMonitor_StatusChanged;

            icon = new NotifyIcon
            {
                Icon = volumeMonitor.Enabled ? LoadAutoIcon() : LoadManualIcon(),
                Text = Resources.ApplicationName,
                Visible = true,
                ContextMenuStrip = CreateContextMenu()
            };
            icon.DoubleClick += icon_DoubleClick;
        }

        private Icon LoadManualIcon()
        {
            return Icon.FromHandle(Resources.VolumeIcon.GetHicon());
        }

        private Icon LoadAutoIcon()
        {
            return Icon.FromHandle(Resources.VolumeIconAuto.GetHicon());
        }

        private ContextMenuStrip CreateContextMenu()
        {
            var menu = new ContextMenuStrip();

            {
                var startStopItem = new ToolStripMenuItem(Resources.Menu_EnableDisable, null, startStopItem_Click);
                startStopItem.CheckOnClick = true;
                startStopItem.Checked = false;
                menu.Items.Add(startStopItem);

                var optionsItem = new ToolStripMenuItem(Resources.Menu_Options, Resources.Options, optionsItem_Click);
                menu.Items.Add(optionsItem);

                menu.Items.Add(new ToolStripSeparator());

                var exitItem = new ToolStripMenuItem(Resources.Menu_Exit, Resources.Exit, exitItem_Click);
                menu.Items.Add(exitItem);
            }

            return menu;
        }

        private void volumeMonitor_StatusChanged(object sender, EventArgs args)
        {
            icon.Icon = volumeMonitor.Enabled ? LoadAutoIcon() : LoadManualIcon();
        }

        private void optionsItem_Click(object sender, EventArgs args)
        {
            ShowOptionsForm();
        }

        private void exitItem_Click(object sender, EventArgs args)
        {
            Application.Exit();
        }

        private void icon_DoubleClick(object sender, EventArgs args)
        {
            ShowOptionsForm();
        }

        private void startStopItem_Click(object sender, EventArgs args)
        {
            var item = (ToolStripMenuItem) sender;

            if (volumeMonitor.Enabled)
            {
                volumeMonitor.Stop();
            }
            else
            {
                volumeMonitor.Start();
            }

            item.Checked = volumeMonitor.Enabled;
        }

        /// <summary>
        /// Opens or brings to front options form.
        /// </summary>
        public void ShowOptionsForm()
        {
            if (formOptions != null)
            {
                PutFormToFront();
            }
            else
            {
                formOptions = new FormOptions(volumeMonitor);
                formOptions.Closed += FormOptionsClosed;
                formOptions.Show();

            }
        }

        private void FormOptionsClosed(object sender, EventArgs args)
        {
            CloseMainForm();
        }

        private void PutFormToFront()
        {
            formOptions.Show();
            formOptions.WindowState = FormWindowState.Normal;
            formOptions.Activate();
        }

        private void CloseMainForm()
        {
            if (formOptions == null)
                return;

            formOptions.Closed -= FormOptionsClosed;
            formOptions = null;
        }

        /// <summary>
        /// Implements <see cref="IDisposable"/> interface.
        /// Unsubscribes from all events and disposes notify icon.
        /// </summary>
        public void Dispose()
        {
            volumeMonitor.StatusChanged -= volumeMonitor_StatusChanged;

            CloseMainForm();

            icon.DoubleClick -= icon_DoubleClick;
            icon.Dispose();
        }
    }
}