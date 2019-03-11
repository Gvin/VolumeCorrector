using System;
using System.Drawing;
using System.Windows.Forms;
using VolumeCorrector.Model.VolumeCorrection;
using VolumeCorrector.Properties;
using VolumeCorrector.VolumeCorrection;

namespace VolumeCorrector.Views
{
    /// <summary>
    /// Handles notify icon of the application.
    /// Processes clicks and context menu.
    /// </summary>
    public class NotifyIconView : INotifyIconView
    {
        private readonly NotifyIcon notifyIcon;

        private ToolStripMenuItem enableDisableButton;

        public event EventHandler ExitClick;
        public event EventHandler StartStopClick;
        public event EventHandler OptionsClick;

        /// <summary>
        /// Creates new instance of <c>NotifyIconView</c>.
        /// </summary>
        public NotifyIconView()
        {
            notifyIcon = new NotifyIcon
            {
                Text = Resources.ApplicationName,
                Visible = true,
                ContextMenuStrip = CreateContextMenu()
            };
            notifyIcon.Click += notifyIcon_Click;
        }

        private Icon LoadManualIcon()
        {
            return Icon.FromHandle(Resources.VolumeIcon.GetHicon());
        }

        private Icon LoadAutoIcon()
        {
            return Icon.FromHandle(Resources.VolumeIconAuto.GetHicon());
        }

        public void UpdateStatus(bool enabled)
        {
            notifyIcon.Icon = enabled ? LoadAutoIcon() : LoadManualIcon();
            enableDisableButton.Checked = enabled;
        }

        private void notifyIcon_Click(object sender, EventArgs args)
        {
            if (args is MouseEventArgs mouseArgs && mouseArgs.Button == MouseButtons.Left)
            {
                StartStopClick?.Invoke(this, EventArgs.Empty);
            }
        }

        private ContextMenuStrip CreateContextMenu()
        {
            var menu = new ContextMenuStrip();

            {
                enableDisableButton =
                    new ToolStripMenuItem(Resources.Menu_EnableDisable, null, enableDisableButton_Click)
                    {
                        CheckOnClick = true,
                        Checked = false
                    };
                menu.Items.Add(enableDisableButton);

                var optionsItem = new ToolStripMenuItem(Resources.Menu_Options, Resources.Options, optionsItem_Click);
                menu.Items.Add(optionsItem);

                menu.Items.Add(new ToolStripSeparator());

                var exitItem = new ToolStripMenuItem(Resources.Menu_Exit, Resources.Exit, exitItem_Click);
                menu.Items.Add(exitItem);
            }

            return menu;
        }

        private void enableDisableButton_Click(object sender, EventArgs args)
        {
            StartStopClick?.Invoke(this, EventArgs.Empty);
        }

        private void optionsItem_Click(object sender, EventArgs args)
        {
            OptionsClick?.Invoke(this, EventArgs.Empty);
        }

        private void exitItem_Click(object sender, EventArgs args)
        {
            ExitClick?.Invoke(this, EventArgs.Empty);
        }

//        private void PutFormToFront()
//        {
//            formOptions.Show();
//            formOptions.WindowState = FormWindowState.Normal;
//            formOptions.Activate();
//        }

        /// <summary>
        /// Implements <see cref="IDisposable"/> interface.
        /// Unsubscribes from all events and disposes notify icon.
        /// </summary>
        public void Dispose()
        {
            notifyIcon.Click -= notifyIcon_Click;
            notifyIcon.Dispose();
        }
    }
}