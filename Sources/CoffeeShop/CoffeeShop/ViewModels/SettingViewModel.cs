using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoffeeShop.ViewModels
{
    public class SettingViewModel:BaseViewModel
    {
        #region variables
        private static SettingViewModel _instance = null;
        private static object m_lock = new object();


        #endregion

        #region properties

        #endregion

        public static SettingViewModel GetInstance()
        {
            // DoubleLock
            if (_instance == null)
            {
                lock (m_lock)
                {
                    if (_instance == null)
                    {
                        _instance = new SettingViewModel();
                    }
                }
            }
            return _instance;
        }

        public SettingViewModel()
        {

        }
    }
}
