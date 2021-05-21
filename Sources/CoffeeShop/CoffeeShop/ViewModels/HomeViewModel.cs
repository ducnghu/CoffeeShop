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
        private List<string> _listSort = new List<string>() {
            "Bán chạy",
            "Hàng mới",
            "A-Z",
            "Z-A",
            "Giá tăng dần",
            "Giá giảm dần",
        };
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

        }

    }
}
