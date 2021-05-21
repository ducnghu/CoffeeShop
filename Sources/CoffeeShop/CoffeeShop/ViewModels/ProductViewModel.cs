using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoffeeShop.ViewModels
{
    public class ProductViewModel:BaseViewModel
    {
        #region variables
        private static ProductViewModel _instance = null;
        private static object m_lock = new object();

        private AsyncObservableCollection<dynamic> _categories;
        private dynamic _selectedCategory;
        #endregion

        #region properties
        public AsyncObservableCollection<dynamic> Categories { get => _categories; set { _categories = value; OnPropertyChanged(); } }
        public dynamic SelectedCategory { get => _selectedCategory; set { _selectedCategory = value; OnPropertyChanged(); } }

        #endregion

        public static ProductViewModel GetInstance()
        {
            // DoubleLock
            if (_instance == null)
            {
                lock (m_lock)
                {
                    if (_instance == null)
                    {
                        _instance = new ProductViewModel();
                    }
                }
            }
            return _instance;
        }

        public ProductViewModel()
        {

        }
    }
}
