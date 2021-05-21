using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Configuration;

namespace CoffeeShop.ViewModels
{
    class MainViewModel : BaseViewModel
    {
        #region private variables
        //private BaseViewModel _currentPageViewModel = null;
        //private Visibility _leftPanelVisibility = Visibility.Visible;
        private String _addPlaceColor = Brushes.White.ToString();
        private String _addMemberColor = Brushes.White.ToString();
        private String _settingColor = Brushes.White.ToString();
        private String _aboutColor = Brushes.White.ToString();
        private String _versionTextBlock = Brushes.White.ToString();
        #endregion

        #region Commands

        public ICommand HomeCommand { get; set; }
        #endregion

        #region Panel

        public String VersionTextBlock { get => _versionTextBlock; set { _versionTextBlock = value; OnPropertyChanged(); } }

        public Global global = Global.GetInstance();

        #endregion

        private string GetPublishedVersion()
        {
            var version = System.Reflection.Assembly.GetEntryAssembly().GetName().Version;
            string appVersion = $"{version.Major}.{version.Minor}";
            return appVersion;
        }


        #region static variable

        public static bool IsShowed = false;

        #endregion

        public MainViewModel()
        {
            ResetPanelColor();
            global.HomeColor = Brushes.SaddleBrown.ToString();
            global.HomeTextColor = Brushes.White.ToString();


            VersionTextBlock = GetPublishedVersion();
            if (VersionTextBlock == null || VersionTextBlock == "")
                VersionTextBlock = "not installed";

            HomeCommand = new RelayCommand<object>((param) => { return true; }, (param) =>
            {
                ResetPanelColor();
                global.HomeColor = Brushes.SaddleBrown.ToString();
                global.HomeTextColor = Brushes.White.ToString();
                global.CurrentPageViewModel = HomeViewModel.GetInstance();


            });

        }

        void ResetPanelColor()
        {
            global.HomeColor = Brushes.White.ToString();
            global.HomeTextColor = Brushes.Gray.ToString();

        }
    }
}
