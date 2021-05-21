using CoffeeShop.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoffeeShop.ViewModels
{
    public class HomeViewModel:BaseViewModel
    {
        #region variables
        private static HomeViewModel _instance = null;
        private static object m_lock = new object();

        private AsyncObservableCollection<dynamic> _categories;
        private dynamic _selectedCategory;
        #endregion

        #region properties
        public AsyncObservableCollection<dynamic> Categories { get => _categories; set { _categories = value; OnPropertyChanged(); } }
        public dynamic SelectedCategory { get => _selectedCategory; set { _selectedCategory = value; OnPropertyChanged(); } }

        #endregion

        public static HomeViewModel GetInstance()
        {
            // DoubleLock
            if (_instance == null)
            {
                lock (m_lock)
                {
                    if (_instance == null)
                    {
                        _instance = new HomeViewModel();
                    }
                }
            }
            return _instance;
        }

        public HomeViewModel()
        {
            // khởi tạo dữ liệu

            // Danh sách loại sản phẩm
            Categories = new AsyncObservableCollection<dynamic>();
            Categories.Add(new
            {
                Ma = 0,
                Ten = "Tất cả"
            });
            foreach (var item in DataProvider.Ins.DB.LoaiSanPham)
            {
                Categories.Add(new
                {
                    Ma = item.Ma,
                    Ten = item.Ten
                });
            }
            SelectedCategory = Categories.ElementAt(0);
        }

    }
}
