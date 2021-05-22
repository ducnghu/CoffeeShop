using CoffeeShop.Model;
using CoffeeShop.Models;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Windows.Media.Imaging;

namespace CoffeeShop.ViewModels
{
    public class ProductViewModel:BaseViewModel
    {
        #region variables
        private static ProductViewModel _instance = null;
        private static object m_lock = new object();

        private AsyncObservableCollection<dynamic> _categories;
        private dynamic _selectedCategory;
        private AsyncObservableCollection<dynamic> _products;
        private dynamic _selectedProduct;
        private bool _isOpenAddProductDialog;
        private bool _isOpenChangeAmountProductDialog;
        private bool _isOpenDeleteProductDialog;
        private string _newProductName;
        private int _newProductPrice;
        private byte[] _newProductThumbnail;
        private dynamic _selectedNewProductCategory;
        private AsyncObservableCollection<dynamic> _newProductCategories;
        private dynamic _selectedAddNewProductRawMaterial;
        private string _selectedAddNewProductRawMaterialUnit;
        private int _newProductRawMaterialAmount;
        private AsyncObservableCollection<dynamic> _addNewProductRawMaterials;
        private dynamic _selectedNewProductRawMaterial;
        private AsyncObservableCollection<dynamic> _newProductRawMaterials;
        #endregion

        #region properties
        public AsyncObservableCollection<dynamic> Categories { get => _categories; set { _categories = value; OnPropertyChanged(); } }
        public dynamic SelectedCategory { get => _selectedCategory; set { _selectedCategory = value;
                if (value != null)
                {
                    int maloai = value.Ma;
                    Products = new AsyncObservableCollection<dynamic>();
                    foreach (var item in DataProvider.Ins.DB.SanPham.Where(x=>x.MaLoai == maloai))
                    {
                        Products.Add(new { 
                            Ma = item.Ma,
                            Ten = item.Ten,
                            Gia = item.Gia,
                            Anh = item.Anh,
                        });
                    }
                }
                OnPropertyChanged(); } }
        public AsyncObservableCollection<dynamic> Products { get => _products; set { _products = value; OnPropertyChanged(); } }
        public dynamic SelectedProduct { get => _selectedProduct; set { _selectedProduct = value; OnPropertyChanged(); } }
        public bool IsOpenAddProductDialog
        {
            get => _isOpenAddProductDialog; set
            {
                _isOpenAddProductDialog = value;
                if (value == true)
                {
                    NewProductName = "";
                    NewProductPrice = 0;
                    SelectedNewProductCategory = null;
                    NewProductCategories = new AsyncObservableCollection<dynamic>();
                    foreach (var item in DataProvider.Ins.DB.LoaiSanPham)
                    {
                        NewProductCategories.Add(new
                        {
                            Ma = item.Ma,
                            Ten = item.Ten
                        });
                    }
                    NewProductThumbnail = null;
                    SelectedAddNewProductRawMaterial = null;
                    AddNewProductRawMaterials = new AsyncObservableCollection<dynamic>();
                    foreach (var item in DataProvider.Ins.DB.KhoNguyenLieu)
                    {
                        AddNewProductRawMaterials.Add(new
                        {
                            MaNL = item.MaNL,
                            Ten = item.Ten,
                            DonVi = item.DonVi,
                        });
                    }
                    SelectedNewProductRawMaterial = null;
                    NewProductRawMaterials = new AsyncObservableCollection<dynamic>();
                }
                OnPropertyChanged();
            }
        }
        public bool IsOpenChangeAmountProductDialog
        {
            get => _isOpenChangeAmountProductDialog; set
            {
                _isOpenChangeAmountProductDialog = value;
                if (value == true)
                {
                }
                OnPropertyChanged();
            }
        }
        public bool IsOpenDeleteProductDialog { get => _isOpenDeleteProductDialog; set { _isOpenDeleteProductDialog = value; OnPropertyChanged(); } }
        public string NewProductName { get => _newProductName; set { value = value.ToUpper(); _newProductName = value; OnPropertyChanged(); } }
        public int NewProductPrice { get => _newProductPrice; set { _newProductPrice = value; OnPropertyChanged(); } }
        public byte[] NewProductThumbnail { get => _newProductThumbnail; set { _newProductThumbnail = value; OnPropertyChanged(); } }
        public dynamic SelectedNewProductCategory { get => _selectedNewProductCategory; set { _selectedNewProductCategory = value; OnPropertyChanged(); } }
        public AsyncObservableCollection<dynamic> NewProductCategories { get => _newProductCategories; set { _newProductCategories = value; OnPropertyChanged(); } }
        public dynamic SelectedAddNewProductRawMaterial { get => _selectedAddNewProductRawMaterial; set { _selectedAddNewProductRawMaterial = value; SelectedAddNewProductRawMaterialUnit = (value == null) ?  "" : value.DonVi; OnPropertyChanged(); } }
        public string SelectedAddNewProductRawMaterialUnit { get => _selectedAddNewProductRawMaterialUnit; set { _selectedAddNewProductRawMaterialUnit = value; OnPropertyChanged(); } }
        public int NewProductRawMaterialAmount { get => _newProductRawMaterialAmount; set { _newProductRawMaterialAmount = value; OnPropertyChanged(); } }
        public AsyncObservableCollection<dynamic> AddNewProductRawMaterials { get => _addNewProductRawMaterials; set { _addNewProductRawMaterials = value; OnPropertyChanged(); } }
        public dynamic SelectedNewProductRawMaterial { get => _selectedNewProductRawMaterial; set { _selectedNewProductRawMaterial = value; OnPropertyChanged(); } }
        public AsyncObservableCollection<dynamic> NewProductRawMaterials { get => _newProductRawMaterials; set { _newProductRawMaterials = value; OnPropertyChanged(); } }

        #endregion

        #region command
        public ICommand ClickAddProductButtonCommand { get; set; }
        public ICommand AddProductCommand { get; set; }
        public ICommand AddThumbnailCommand { get; set; }
        public ICommand AddNewProductRawMaterialCommand { get; set; }
        public ICommand DeleteNewProductRawMaterialCommand { get; set; }
        public ICommand ChangeCategoryCommand { get; set; }

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
            // khởi tạo dữ liệu

            // Danh sách loại sản phẩm
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
            ClickAddProductButtonCommand = new RelayCommand<object>((param) => { return true; }, (param) => {
                IsOpenAddProductDialog = true;
            });

            AddProductCommand = new RelayCommand<object>((param) => { return true; }, (param) => {
                if (bool.Parse(param.ToString()) == true)
                {
                    SanPham newsanpham = new SanPham();
                    newsanpham.Ma = (DataProvider.Ins.DB.SanPham.Count() == 0) ? 1 : DataProvider.Ins.DB.SanPham.Max(x => x.Ma) + 1;
                    newsanpham.Ten = NewProductName;
                    newsanpham.MaLoai = SelectedNewProductCategory.Ma;
                    newsanpham.Gia = NewProductPrice;
                    newsanpham.Anh = NewProductThumbnail;
                    DataProvider.Ins.DB.SanPham.Add(newsanpham);
                    DataProvider.Ins.DB.SaveChanges();
                    foreach (var item in NewProductRawMaterials)
                    {
                        DataProvider.Ins.DB.NguyenLieu.Add(new NguyenLieu { 
                            MaSP = newsanpham.Ma,
                            MaNL = item.MaNL,
                            SoLuong = item.SoLuong,
                        });
                    }
                    DataProvider.Ins.DB.SaveChanges();
                }
                IsOpenAddProductDialog = false;
            });

            AddThumbnailCommand = new RelayCommand<object>((param) => { return true; }, (param) => {
                OpenFileDialog ofd = new OpenFileDialog();
                ofd.Title = "Open a Image File";
                ofd.Filter = "All Files(*.*) | *.*"; //Here you can filter which all files you wanted allow to open  

                if (ofd.ShowDialog() == true)
                {
                    BitmapImage image = new BitmapImage(
                    new Uri(ofd.FileName,
                    UriKind.Absolute));
                    MemoryStream memStream = new MemoryStream();
                    JpegBitmapEncoder encoder = new JpegBitmapEncoder();
                    encoder.Frames.Add(BitmapFrame.Create(image));
                    encoder.Save(memStream);
                    NewProductThumbnail = memStream.ToArray();
                }
            });

            AddNewProductRawMaterialCommand = new RelayCommand<object>((param) => { return true; }, (param) => {
                NewProductRawMaterials.Add(new { 
                    MaNL = SelectedAddNewProductRawMaterial.MaNL,
                    Ten = SelectedAddNewProductRawMaterial.Ten,
                    SoLuong = NewProductRawMaterialAmount,
                    DonVi = SelectedAddNewProductRawMaterialUnit,
                });
                AddNewProductRawMaterials.Remove(SelectedAddNewProductRawMaterial);
                SelectedAddNewProductRawMaterial = null;
                NewProductRawMaterialAmount = 0;
            });

            DeleteNewProductRawMaterialCommand = new RelayCommand<dynamic>((param) => { return true; }, (param) => {
                AddNewProductRawMaterials.Add(new
                {
                    MaNL = param.MaNL,
                    Ten = param.Ten,
                    DonVi = param.DonVi,
                });
                NewProductRawMaterials.Remove(param);
            });

            ChangeCategoryCommand = new RelayCommand<dynamic>((param) => { return true; }, (param) => {
                SelectedCategory = param;
            });
        }

    }
}
