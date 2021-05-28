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
        public ICommand WarehouseCommand { get; set; }
        public ICommand ProductCommand { get; set; }
        public ICommand ReceiptCommand { get; set; }
        public ICommand SettingCommand { get; set; }
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

            WarehouseCommand = new RelayCommand<object>((param) => { return true; }, (param) =>
            {
                ResetPanelColor();
                global.WarehouseColor = Brushes.SaddleBrown.ToString();
                global.WarehouseTextColor = Brushes.White.ToString();
                global.CurrentPageViewModel = WarehouseViewModel.GetInstance();

            });

            ProductCommand = new RelayCommand<object>((param) => { return true; }, (param) =>
            {
                ResetPanelColor();
                global.ProductColor = Brushes.SaddleBrown.ToString();
                global.ProductTextColor = Brushes.White.ToString();
                global.CurrentPageViewModel = ProductViewModel.GetInstance();

            });

            ReceiptCommand = new RelayCommand<object>((param) => { return true; }, (param) =>
            {
                ResetPanelColor();
                global.ReceiptColor = Brushes.SaddleBrown.ToString();
                global.ReceiptTextColor = Brushes.White.ToString();
                global.CurrentPageViewModel = ReceiptViewModel.GetInstance();

            });

            SettingCommand = new RelayCommand<object>((param) => { return true; }, (param) =>
            {
                ResetPanelColor();
                global.SettingColor = Brushes.SaddleBrown.ToString();
                global.SettingTextColor = Brushes.White.ToString();
                global.CurrentPageViewModel = SettingViewModel.GetInstance();

            });
        }

        void ResetPanelColor()
        {
            global.HomeColor = Brushes.White.ToString();
            global.HomeTextColor = Brushes.Gray.ToString();

            global.WarehouseColor = Brushes.White.ToString();
            global.WarehouseTextColor = Brushes.Gray.ToString();

            global.ProductColor = Brushes.White.ToString();
            global.ProductTextColor = Brushes.Gray.ToString();

            global.SettingColor = Brushes.White.ToString();
            global.SettingTextColor = Brushes.Gray.ToString();

            global.ReceiptColor = Brushes.White.ToString();
            global.ReceiptTextColor = Brushes.Gray.ToString();
        }
    }
}
