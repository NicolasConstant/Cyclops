using System;
using System.ComponentModel;
using System.Windows;
using System.Windows.Media;
using Prism.Mvvm;

namespace Cyclops.Models
{
    public class ExecutableTaskViewModel : BindableBase
    {
        private readonly ExecutableTask _model;
        private string _name;
        private string _executableFullPath;
        private string _executableArgs;
        private string _executionFolder;
        private bool _isFailed;
        private string _listItemColor;
        private string _listItemLogoColor;
        private Visual _myResource;
        private string _myIcon;
        private ResourceDictionary _oIconResource;

        #region Ctor
        public ExecutableTaskViewModel(ExecutableTask model)
        {
            _oIconResource = new ResourceDictionary() { Source = new Uri(@"Resources\Icons.xaml", UriKind.Relative) };
            
            _model = model;
            _model.PropertyChanged += ModelOnPropertyChanged;

            IsFailed = _model.IsFailed;
            Name = _model.Name;
        }
        
        //public ExecutableTaskViewModel()
        //{
        //    _oIconResource = new ResourceDictionary() { Source = new Uri(@"Resources\Icons.xaml", UriKind.Relative) };
        //    IsFailed = false;
        //}
        //public ExecutableTaskViewModel(string name, string executableFullPath)
        //{
        //    _oIconResource = new ResourceDictionary() { Source = new Uri(@"Resources\Icons.xaml", UriKind.Relative) };
        //    IsFailed = false;
        //    Name = name;
        //    ExecutableFullPathFullPath = executableFullPath;
        //}
        #endregion

        private void ModelOnPropertyChanged(object sender, PropertyChangedEventArgs propertyChangedEventArgs)
        {
            IsFailed = _model.IsFailed;
            Name = _model.Name;
        }

        public string Name
        {
            get { return _name; }
            set { SetProperty(ref _name, value); }
        }

        public string ExecutableFullPathFullPath
        {
            get { return _executableFullPath; }
            set { SetProperty(ref _executableFullPath, value); }
        }

        public string ExecutableArgs
        {
            get { return _executableArgs; }
            set { SetProperty(ref _executableArgs, value); }
        }

        public string ExecutionFolder
        {
            get { return _executionFolder; }
            set { SetProperty(ref _executionFolder, value); }
        }

        public bool IsFailed
        {
            get { return _isFailed; }
            set
            {
                SetProperty(ref _isFailed, value);
                if (_isFailed)
                {
                    ListItemColor = "Crimson";
                    ListItemLogoColor = "WhiteSmoke";
                    MyIcon = "appbar_warning_circle";
                    MyResource = (Visual)_oIconResource[MyIcon];
                }
                else
                {
                    ListItemColor = "WhiteSmoke";
                    ListItemLogoColor = "SteelBlue";
                    MyIcon = "appbar_check";
                    MyResource = (Visual)_oIconResource[MyIcon];
                }
            }
        }

        public string ListItemColor
        {
            get { return _listItemColor; }
            private set { SetProperty(ref _listItemColor, value); }
        }

        public string ListItemLogoColor
        {
            get { return _listItemLogoColor; }
            private set { SetProperty(ref _listItemLogoColor, value); }
        }

        public Visual MyResource
        {
            get { return _myResource; }
            set { SetProperty(ref _myResource, value); }
        }

        public string MyIcon
        {
            get { return _myIcon; }
            set { SetProperty(ref _myIcon, value); }
        }
    }
}