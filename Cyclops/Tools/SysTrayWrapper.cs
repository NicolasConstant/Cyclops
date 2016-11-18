using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Windows.Forms;
using Cyclops.Contracts;

namespace Cyclops.Tools
{
    class SysTrayWrapper : IDisposable
    {
        private readonly IAutoStartManager _autostartManager;
        private readonly SysTray _trayIcon;

        private const string AutoStartOffLabel = "Autostart: OFF";
        private const string AutoStartOnLabel = "Autostart: ON";
        private MenuItem _autoStartMenuItem;

        public event Action LeftCLickOnTrayIconOccured;
        public event Action CloseAction;

        #region Ctor
        public SysTrayWrapper(IAutoStartManager autostartManager)
        {
            _autostartManager = autostartManager;

            #region Init Common Icon
            Icon neutralIcon;
            using (var stream = Assembly.GetExecutingAssembly().GetManifestResourceStream("Cyclops.Icons.neutral.ico"))
            {
                neutralIcon = new Icon(stream);
            }
            #endregion

            #region Init Warning Animation List
            var warningIconList = new List<Icon>();
            for (var i = 1; i <= 11; i++)
            {
                using (var stream = Assembly.GetExecutingAssembly().GetManifestResourceStream($"Cyclops.Icons.warning_{i}.ico"))
                {
                    var icon = new Icon(stream);
                    warningIconList.Add(icon);
                }
            }
            var warningIconListCopy = warningIconList.ToList();
            warningIconListCopy.Reverse();
            warningIconList.AddRange(warningIconListCopy);
            #endregion

            #region Init Working Animation List
            var workingIconList = new List<Icon>();
            workingIconList.Add(neutralIcon);
            using (var stream = Assembly.GetExecutingAssembly().GetManifestResourceStream("Cyclops.Icons.neutral_loading.ico"))
            {
                var icon = new Icon(stream);
                workingIconList.Add(icon);
            }
            #endregion

            #region Init Context Menu
            var contextMenu = new ContextMenu();

            //var stopWorkingMenuItem = new MenuItem
            //{
            //    Index = 1,
            //    Text = "Stop Working"
            //};
            //stopWorkingMenuItem.Click += StopWorkingAnimation;

            //var startWorkingMenuItem = new MenuItem
            //{
            //    Index = 0,
            //    Text = "Start Working"
            //};
            //startWorkingMenuItem.Click += StartWorkingAnimation;

            //var stopWarningMenuItem = new MenuItem
            //{
            //    Index = 1,
            //    Text = "Stop Warning"
            //};
            //stopWarningMenuItem.Click += StopWarningAnimation;

            //var startWarningMenuItem = new MenuItem
            //{
            //    Index = 0,
            //    Text = "Start Warning"
            //};
            //startWarningMenuItem.Click += StartWarningAnimation;

            _autoStartMenuItem = new MenuItem
            {
                Index = 1,
                Text = _autostartManager.IsAutoStartSet() ? AutoStartOnLabel : AutoStartOffLabel
            };

            _autoStartMenuItem.Click += AutostartClick;

            var closeAppMenuItem = new MenuItem
            {
                Index = 0,
                Text = "Close"
            };
            closeAppMenuItem.Click += CloseAppClick;

            contextMenu.MenuItems.AddRange(new[] { _autoStartMenuItem, closeAppMenuItem });
            #endregion

            _trayIcon = new SysTray("Cyclops", neutralIcon, contextMenu);
            _trayIcon.OnLeftClick += OnLeftClick;

            _trayIcon.SetWarningAnimationClip(warningIconList.ToArray());
            _trayIcon.SetWorkingAnimationClip(workingIconList.ToArray());
        }
        #endregion

        private void AutostartClick(object sender, EventArgs e)
        {
            var autoStartNewStatus = !_autostartManager.IsAutoStartSet();

            _autostartManager.SetAutoStart(autoStartNewStatus);
            _autoStartMenuItem.Text = autoStartNewStatus ? AutoStartOnLabel : AutoStartOffLabel;
        }

        private void CloseAppClick(object sender, EventArgs e)
        {
            CloseAction?.Invoke();
        }

        private void OnLeftClick()
        {
            LeftCLickOnTrayIconOccured?.Invoke();
        }

        //private void StopWorkingAnimation(object sender, EventArgs e)
        //{
        //    StopWorkingAnimation();
        //}

        //private void StartWorkingAnimation(object sender, EventArgs e)
        //{
        //    StartWorkingAnimation();
        //}

        //private void StopWarningAnimation(object sender, EventArgs e)
        //{
        //    StopWarningAnimation();
        //}

        //private void StartWarningAnimation(object sender, EventArgs e)
        //{
        //    StartWarningAnimation();
        //}

        public void StartWarningAnimation()
        {
            _trayIcon.StartWarningAnimation(50);
        }

        public void StopWarningAnimation()
        {
            _trayIcon.StopWarningAnimation();
        }

        public void StartWorkingAnimation()
        {
            _trayIcon.StartWorkingAnimation(200);
        }

        public void StopWorkingAnimation()
        {
            _trayIcon.StopWorkingAnimation();
        }

        public void Dispose()
        {
            _trayIcon.OnLeftClick -= LeftCLickOnTrayIconOccured;
            _trayIcon.Dispose();
        }
    }
}
