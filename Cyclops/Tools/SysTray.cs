using System;
using System.Drawing;
using System.Windows.Forms;

namespace Cyclops.Tools
{
	/// <summary>
	/// SysTray class that can be used to display animated icons or text in the system tray
	/// </summary>
	public class SysTray : IDisposable
	{
        private readonly NotifyIcon _mNotifyIcon;
        private readonly Font _mFont;
        private readonly Color _mCol = Color.Black;
        private readonly Timer _workingTimer;
        private readonly Timer _warningTimer;
        private readonly Icon _mDefaultIcon;

        private Icon[] _workingAnimationIcons;
        private Icon[] _warningAnimationIcons;
        private int _mCurrIndex = 0;

        #region Ctor
        /// <summary>
        /// The constructor
        /// </summary>
        /// <param name="text">The toolip text</param>
        /// <param name="icon">The icon that will be shown by default, can be null</param>
        /// <param name="menu">The context menu to be opened on right clicking on the 
        ///                    icon in the tray. This can be null.</param>
        public SysTray(string text, Icon icon, ContextMenu menu)
		{
			_mNotifyIcon = new NotifyIcon();
            _mNotifyIcon.MouseClick += m_notifyIcon_MouseClick;
            _mNotifyIcon.Text = text;
            _mNotifyIcon.Visible = true;
            _mNotifyIcon.Icon = icon;
            _mDefaultIcon = icon;
            _mNotifyIcon.ContextMenu = menu;
            _mFont = new Font("Helvetica", 8);

            _workingTimer = new Timer();
            _workingTimer.Interval = 100;
            _workingTimer.Tick += WorkingTimerTick;

            _warningTimer = new Timer();
            _warningTimer.Interval = 100;
            _warningTimer.Tick += WarningTimerTick;
        }
        #endregion 

        public event Action OnLeftClick;

        private void m_notifyIcon_MouseClick(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
                OnLeftClick?.Invoke();
        }

        /// <summary>
        /// Shows text instead of icon in the tray
        /// </summary>
        /// <param name="text">The text to be displayed on the tray. 
        ///                    Make this only 1 or 2 characters. E.g. "23"</param>
        public void ShowText(string text)
        {
            ShowText(text, _mFont, _mCol);
        }

        /// <summary>
        /// Shows text instead of icon in the tray
        /// </summary>
        /// <param name="text">Same as above</param>
        /// <param name="col">Color to be used to display the text in the tray</param>
        public void ShowText(string text, Color col)
        {
            ShowText(text, _mFont, col);
        }

        /// <summary>
        /// Shows text instead of icon in the tray
        /// </summary>
        /// <param name="text">Same as above</param>
        /// <param name="font">The default color will be used but in user given font</param>
        public void ShowText(string text, Font font)
        {
            ShowText(text, font, _mCol);
        }

        /// <summary>
        /// Shows text instead of icon in the tray
        /// </summary>
        /// <param name="text">the text to be displayed</param>
        /// <param name="font">The font to be used</param>
        /// <param name="col">The color to be used</param>
        public void ShowText(string text, Font font, Color col)
        {
            var bitmap = new Bitmap(16, 16);

            Brush brush = new SolidBrush(col);

            var graphics = Graphics.FromImage(bitmap);
            graphics.DrawString(text, _mFont,brush, 0, 0);

            var hIcon = bitmap.GetHicon();
            var icon = Icon.FromHandle(hIcon);
            _mNotifyIcon.Icon = icon;

        }

        /// <summary>
        /// Sets the working animation clip that will be displayed in the system tray
        /// </summary>
        /// <param name="icons">The array of icons which forms each frame of the animation
        ///                     This'll work by showing one icon after another in the array.
        ///                     Each of the icons must be 16x16 pixels </param>
        public void SetWorkingAnimationClip (Icon [] icons)
        {
            _workingAnimationIcons = icons;
        }

        /// <summary>
        /// Sets the warning animation clip that will be displayed in the system tray
        /// </summary>
        /// <param name="icons">The array of icons which forms each frame of the animation
        ///                     This'll work by showing one icon after another in the array.
        ///                     Each of the icons must be 16x16 pixels </param>
        public void SetWarningAnimationClip(Icon[] icons)
        {
            _warningAnimationIcons = icons;
        }

        public void StartWarningAnimation(int interval)
        {
            if(_warningAnimationIcons == null) throw new ApplicationException("Animation clip not set with SetWarningAnimationClip");

            _warningTimer.Interval = interval;
            _warningTimer.Start();
        }

        public void StartWorkingAnimation(int interval)
        {
            if (_workingAnimationIcons == null) throw new ApplicationException("Animation clip not set with SetWorkingAnimationClip");

            _workingTimer.Interval = interval;
            _workingTimer.Start();
        }

        public void StopWarningAnimation()
        {
            _warningTimer.Stop();
            _mNotifyIcon.Icon = _mDefaultIcon;
        }

        public void StopWorkingAnimation()
        {
            _workingTimer.Stop();
            _mNotifyIcon.Icon = _mDefaultIcon;
        }
        
        #region Dispose
        public void Dispose()
        {
            _mNotifyIcon.Dispose();
            if(_mFont != null)
                _mFont.Dispose();
        }
        #endregion
   
        #region Event handlers
        private void WorkingTimerTick(object sender, EventArgs e)
        {
            if(_mCurrIndex < _workingAnimationIcons.Length)
            {
                _mNotifyIcon.Icon = _workingAnimationIcons[_mCurrIndex];
                _mCurrIndex++;
            }
            else
            {
                _mCurrIndex = 0;
            }
        }

        private void WarningTimerTick(object sender, EventArgs e)
        {
            if (_mCurrIndex < _warningAnimationIcons.Length)
            {
                _mNotifyIcon.Icon = _warningAnimationIcons[_mCurrIndex];
                _mCurrIndex++;
            }
            else
            {
                _mCurrIndex = 0;
            }
        }
        #endregion


    }
}
