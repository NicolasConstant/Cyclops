using System;
using System.Collections.Generic;
using System.IO;
using System.Windows.Media.Media3D;
using Cyclops.Contracts;
using Cyclops.Models;
using Newtonsoft.Json;

namespace Cyclops.Tools
{
    public class JsonConfigFileHandler : IConfigFileHandler
    {
        private readonly string _appDataConfigFile;

        #region Ctor
        public JsonConfigFileHandler()
        {
            var userDataFolder = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);

            var cyclopsFolder = userDataFolder + @"\Cyclops";
            if (!Directory.Exists(cyclopsFolder)) Directory.CreateDirectory(cyclopsFolder);

            _appDataConfigFile = cyclopsFolder + @"\settings.json";
        }
        #endregion
        public IEnumerable<ExecutableTask> GetTasks()
        {
            if (!File.Exists(_appDataConfigFile)) return new List<ExecutableTask>();
            
            var settingsFileContent = File.ReadAllText(_appDataConfigFile);
            return JsonConvert.DeserializeObject<IEnumerable<ExecutableTask>>(settingsFileContent);
        }

        public void SaveTasks(IEnumerable<ExecutableTask> tasks)
        {
            var textToSerialize = JsonConvert.SerializeObject(tasks);
            File.WriteAllText(_appDataConfigFile, textToSerialize);
        }
    }
}