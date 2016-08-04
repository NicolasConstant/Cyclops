using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Windows.Forms;

namespace Cyclops.Tools
{
    class SysTrayWrapper : IDisposable
    {
        private readonly SysTray _trayIcon;

        public event Action LeftCLickOnTrayIconOccured;

        #region Ctor
        public SysTrayWrapper()
        {
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

            var stopWorkingMenuItem = new MenuItem();
            stopWorkingMenuItem.Index = 1;
            stopWorkingMenuItem.Text = "Stop Working";
            stopWorkingMenuItem.Click += StopWorkingAnimation;

            var startWorkingMenuItem = new MenuItem();
            startWorkingMenuItem.Index = 0;
            startWorkingMenuItem.Text = "Start Working";
            startWorkingMenuItem.Click += StartWorkingAnimation;

            var stopWarningMenuItem = new MenuItem();
            stopWarningMenuItem.Index = 1;
            stopWarningMenuItem.Text = "Stop Warning";
            stopWarningMenuItem.Click += StopWarningAnimation;

            var startWarningMenuItem = new MenuItem();
            startWarningMenuItem.Index = 0;
            startWarningMenuItem.Text = "Start Warning";
            startWarningMenuItem.Click += StartWarningAnimation;

            contextMenu.MenuItems.AddRange(new[] { startWarningMenuItem, stopWarningMenuItem, startWorkingMenuItem, stopWorkingMenuItem });
            #endregion

            _trayIcon = new SysTray("Cyclops", neutralIcon, contextMenu);
            _trayIcon.OnLeftClick += LeftCLickOnTrayIconOccured;

            _trayIcon.SetWarningAnimationClip(warningIconList.ToArray());
            _trayIcon.SetWorkingAnimationClip(workingIconList.ToArray());
        }

        private void StopWorkingAnimation(object sender, EventArgs e)
        {
            StopWorkingAnimation();
        }

        private void StartWorkingAnimation(object sender, EventArgs e)
        {
            StartWorkingAnimation();
        }

        private void StopWarningAnimation(object sender, EventArgs e)
        {
            StopWarningAnimation();
        }

        private void StartWarningAnimation(object sender, EventArgs e)
        {
            StartWarningAnimation();
        }
        #endregion

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
