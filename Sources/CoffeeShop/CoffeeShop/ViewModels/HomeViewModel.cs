using CoffeeShop.Model;
using CoffeeShop.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace CoffeeShop.ViewModels
{
    public class HomeViewModel:BaseViewModel
    {
        #region variables
        private static HomeViewModel _instance = null;
        private static object m_lock = new object();

        private AsyncObservableCollection<dynamic> _categories;
        private dynamic _selectedCategory;
        private AsyncObservableCollection<dynamic> _products;
        private dynamic _selectedProduct;
        private AsyncObservableCollection<dynamic> _spendMaterial;
        // Customer
        private bool _isOpenMessageNewCustomerDialog;
        private bool _isCustomer;
        private string _customerName;
        private string _customerPhone;

        // Cart
        private bool _hasCustomer;
        private dynamic _selectedCartProduct;
        private AsyncObservableCollection<dynamic> _cartProducts;

        // mesage out of raw material
        private bool _isOpenMessageOutOfMaterialDialog;

        #endregion

        #region properties
        public AsyncObservableCollection<dynamic> Categories { get => _categories; set { _categories = value; OnPropertyChanged(); } }
        public dynamic SelectedCategory
        {
            get => _selectedCategory; set
            {
                _selectedCategory = value;
                if (value != null)
                {
                    int maloai = value.Ma;
                    Products = new AsyncObservableCollection<dynamic>();
                    foreach (var item in DataProvider.Ins.DB.SanPham.Where(x => x.MaLoai == maloai))
                    {
                        if (CartProducts.Where(x=>x.MaSP==item.Ma).Count() == 0)
                        {
                            Products.Add(new
                            {
                                Ma = item.Ma,
                                Ten = item.Ten,
                                Gia = item.Gia,
                                Anh = item.Anh,
                            });
                        }
                    }
                }
                OnPropertyChanged();
            }
        }
        public AsyncObservableCollection<dynamic> Products { get => _products; set { _products = value; OnPropertyChanged(); } }
        public dynamic SelectedProduct { get => _selectedProduct; set { _selectedProduct = value; OnPropertyChanged(); } }

        // Customer
        public bool IsOpenMessageNewCustomerDialog { get => _isOpenMessageNewCustomerDialog; set { _isOpenMessageNewCustomerDialog = value; OnPropertyChanged(); } }
        public bool IsCustomer { get => _isCustomer; set { _isCustomer = value; OnPropertyChanged(); } }
        public string CustomerName { get => _customerName; set { _customerName = value.ToUpper(); OnPropertyChanged(); } }
        public string CustomerPhone { get => _customerPhone; set { _customerPhone = value; IsCustomer = true; HasCustomer = false; OnPropertyChanged(); } }

        // Cart
        public bool HasCustomer { get => _hasCustomer; set { _hasCustomer = value; OnPropertyChanged(); } }
        public dynamic SelectedCartProduct { get => _selectedCartProduct; set { _selectedCartProduct = value; OnPropertyChanged(); } }
        public AsyncObservableCollection<dynamic> CartProducts { get => _cartProducts; set { _cartProducts = value; OnPropertyChanged(); } }

        // mesage out of raw material
        public bool IsOpenMessageOutOfMaterialDialog { get => _isOpenMessageOutOfMaterialDialog; set { _isOpenMessageOutOfMaterialDialog = value; OnPropertyChanged(); } }
        #endregion

        #region command
        public ICommand ChangeCategoryCommand { get; set; }
        public ICommand ClickCheckCustommerButtonCommand { get; set; }
        public ICommand CloseMessageNewCustomer { get; set; }
        public ICommand AddToCartCommand { get; set; }
        public ICommand ClickDecreaseCartProductButtonCommand { get; set; }
        public ICommand ClickIncreaseCartProductButtonCommand { get; set; }
        public ICommand CloseOutOfMaterialDialog { get; set; }

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
            CartProducts = new AsyncObservableCollection<dynamic>();
            _spendMaterial = new AsyncObservableCollection<dynamic>();
            Categories = new AsyncObservableCollection<dynamic>();
            foreach (var item in DataProvider.Ins.DB.LoaiSanPham)
            {
                Categories.Add(new
                {
                    Ma = item.Ma,
                    Ten = item.Ten
                });
            }
            SelectedCategory = Categories.ElementAt(0);

            // Command

            ChangeCategoryCommand = new RelayCommand<dynamic>((param) => { return true; }, (param) => {
                SelectedCategory = param;
            });

            ClickCheckCustommerButtonCommand = new RelayCommand<dynamic>((param) => { return true; }, (param) => {
                if (HasCustomer == false)
                {
                    if (IsCustomer == true)
                    {
                        KhachHang khachhang = (DataProvider.Ins.DB.KhachHang.Where(x => x.SDT == CustomerPhone).Count() == 0) ? null : DataProvider.Ins.DB.KhachHang.First(x => x.SDT == CustomerPhone);
                        if (khachhang == null)
                        {
                            IsOpenMessageNewCustomerDialog = true;
                            IsCustomer = false;
                        }
                        else
                        {
                            CustomerName = khachhang.Ten;
                            HasCustomer = true;
                        }
                    }
                    else
                    {
                        DataProvider.Ins.DB.KhachHang.Add(new KhachHang
                        {
                            Ma = (DataProvider.Ins.DB.KhachHang.Count() == 0) ? 1 : DataProvider.Ins.DB.KhachHang.Max(x => x.Ma) + 1,
                            Ten = CustomerName,
                            SDT = CustomerPhone,
                            DiemTichLuy = 0,
                            TongChiTieu = 0,
                        });
                        DataProvider.Ins.DB.SaveChanges();
                        HasCustomer = true;
                        IsCustomer = true;
                    }
                }
            });

            CloseMessageNewCustomer = new RelayCommand<dynamic>((param) => { return true; }, (param) => {
                IsOpenMessageNewCustomerDialog = false;
            });

            AddToCartCommand = new RelayCommand<dynamic>((param) => { return true; }, (param) => {
                if (CheckKho(param.Ma, 1) == true)
                {
                    CartProducts.Add(new
                    {
                        MaSP = param.Ma,
                        Ten = param.Ten,
                        Gia = param.Gia,
                        SoLuong = 1,
                        TongGia = param.Gia,
                    });
                    Products.Remove(param);
                }
            });

            ClickIncreaseCartProductButtonCommand = new RelayCommand<dynamic>((param) => { return true; }, (param) => {
                if (CheckKho(param.MaSP, param.SoLuong + 1) == true)
                {
                    AsyncObservableCollection<dynamic> newcartproducts = new AsyncObservableCollection<dynamic>();
                    foreach (var item in CartProducts)
                    {
                        if (item.MaSP != param.MaSP)
                        {
                            newcartproducts.Add(item);
                        }
                        else
                        {
                            newcartproducts.Add(new
                            {
                                MaSP = param.MaSP,
                                Ten = param.Ten,
                                Gia = param.Gia,
                                SoLuong = param.SoLuong + 1,
                                TongGia = param.Gia * (param.SoLuong + 1),
                            });
                        }
                        CartProducts = newcartproducts;
                    }
                }
            });

            ClickDecreaseCartProductButtonCommand = new RelayCommand<dynamic>((param) => { return true; }, (param) => {
                int maSP = param.MaSP;
                if (param.SoLuong == 1)
                {
                    SanPham sanpham = DataProvider.Ins.DB.SanPham.First(x => x.Ma == maSP);
                    if (SelectedCategory.Ma == sanpham.MaLoai)
                    {
                        Products.Add(new
                        {
                            Ma = sanpham.Ma,
                            Ten = sanpham.Ten,
                            Gia = sanpham.Gia,
                            Anh = sanpham.Anh,
                        });
                    }
                    CartProducts.Remove(param);
                }
                else
                {
                    AsyncObservableCollection<dynamic> newcartproducts = new AsyncObservableCollection<dynamic>();
                    foreach (var item in CartProducts)
                    {
                        if (item.MaSP != param.MaSP)
                        {
                            newcartproducts.Add(item);
                        }
                        else
                        {
                            newcartproducts.Add(new
                            {
                                MaSP = param.MaSP,
                                Ten = param.Ten,
                                Gia = param.Gia,
                                SoLuong = param.SoLuong - 1,
                                TongGia = param.Gia * (param.SoLuong - 1),
                            });
                        }
                        CartProducts = newcartproducts;
                    }
                }
                // trả lại số nguyên liệu hao tổn
                foreach (var item in DataProvider.Ins.DB.NguyenLieu.Where(x => x.MaSP == maSP))
                {
                    if (_spendMaterial.Where(x => x.Ma == item.MaNL).Count() != 0)
                    {
                        int soluongmoi = _spendMaterial.First(x => x.Ma == item.MaNL).SoLuong - item.SoLuong;
                        _spendMaterial.Remove(_spendMaterial.First(x => x.Ma == item.MaNL));
                        _spendMaterial.Add(new
                        {
                            Ma = item.MaNL,
                            SoLuong = soluongmoi,
                        });
                    }
                }
            });

            CloseOutOfMaterialDialog = new RelayCommand<dynamic>((param) => { return true; }, (param) => {
                IsOpenMessageOutOfMaterialDialog = false;
            });
        }

        private bool CheckKho(int maSP, int soluong)
        {
            bool result = true;
            // cập nhật số nguyên liệu hao tổn
            foreach (var item in DataProvider.Ins.DB.NguyenLieu.Where(x=>x.MaSP==maSP))
            {
                if (_spendMaterial.Where(x=>x.Ma==item.MaNL).Count() != 0)
                {
                    int soluongmoi = _spendMaterial.First(x => x.Ma == item.MaNL).SoLuong + item.SoLuong;
                    _spendMaterial.Remove(_spendMaterial.First(x => x.Ma == item.MaNL));
                    _spendMaterial.Add(new { 
                        Ma = item.MaNL,
                        SoLuong = soluongmoi,
                    });
                }
                else
                {
                    _spendMaterial.Add(new
                    {
                        Ma = item.MaNL,
                        SoLuong = item.SoLuong,
                    });
                }
            }
            // kiểm tra kho nguyên liệu
            foreach (var item in _spendMaterial)
            {
                int maNL = item.Ma;
                if (DataProvider.Ins.DB.KhoNguyenLieu.First(x=>x.MaNL== maNL).SoLuong < item.SoLuong)
                {
                    result = false;
                    break;
                }
            }

            if (result == false)
            {
                // trả lại số nguyên liệu hao tổn
                foreach (var item in DataProvider.Ins.DB.NguyenLieu.Where(x => x.MaSP == maSP))
                {
                    if (_spendMaterial.Where(x => x.Ma == item.MaNL).Count() != 0)
                    {
                        int soluongmoi = _spendMaterial.First(x => x.Ma == item.MaNL).SoLuong - item.SoLuong;
                        _spendMaterial.Remove(_spendMaterial.First(x => x.Ma == item.MaNL));
                        _spendMaterial.Add(new
                        {
                            Ma = item.MaNL,
                            SoLuong = soluongmoi,
                        });
                    }
                }

                IsOpenMessageOutOfMaterialDialog = true;
            }
            return result;
        }
    }
}
