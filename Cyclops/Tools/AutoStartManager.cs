using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Cyclops.Contracts;
using Microsoft.Win32;

namespace Cyclops.Tools
{
    public class AutoStartManager : IAutoStartManager
    {
        private readonly RegistryKey _rkApp = Registry.CurrentUser.OpenSubKey("SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\Run", true);
        private readonly string _appName;
        private readonly string _exeFullPath;

        public AutoStartManager(string appName, string exeFullPath)
        {
            _appName = appName;
            _exeFullPath = exeFullPath;
        }

        public bool IsAutoStartSet()
        {
            var returnValue = _rkApp.GetValue(_appName) != null;
            return returnValue;
        }

        public void SetAutoStart(bool enabled)
        {
            if (enabled)
            {
                // Add the value in the registry so that the application runs at startup
                _rkApp.SetValue(_appName, _exeFullPath);
            }
            else
            {
                // Remove the value from the registry so that the application doesn't start
                _rkApp.DeleteValue(_appName, false);
            }
        }
    }
}
