using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace CakeShop
{
    /// <summary>
    /// Interaction logic for ShoppingCart.xaml
    /// </summary>
    class CakeInCart : INotifyPropertyChanged  
    {
        public int cakeId { get; set; }
        public int cic { get; set; }
        public string name { get; set; }
        public int price { get; set; }
        public Nullable<int> categoryId { get; set; }
        public String thumbnailPath { get; set; }
        public int amount { get; set; }
        public int total { get; set; }

        public event PropertyChangedEventHandler PropertyChanged;
    }
    public partial class ShoppingCart : Window
    {
        public delegate void DeathHandler();
        public event DeathHandler Dying;
        BindingList<CakeInCart> ShoppingCartList = new BindingList<CakeInCart>();
        int totalCost = 0;
        public ShoppingCart()
        {
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            Cart.Visibility = Visibility.Visible;
            Order.Visibility = Visibility.Collapsed;
            var db = new cakeshopEntities();
            var cakeInCart = db.carts.Where(x => x.isCompleted == false).Select(y=>y.cakeInCarts).FirstOrDefault();
            if (cakeInCart == null)
            {
                Title.Text = "GIỎ HÀNG TRỐNG";
                Payment.IsEnabled = false;
                return;
            }
            foreach (var c in cakeInCart)
            {
                var _c = new CakeInCart();
                _c.name = c.cake.name;
                _c.cic = c.cicId;
                _c.price = (int)c.cake.price;
                _c.amount = (int)c.amount;
                _c.thumbnailPath = AppDomain.CurrentDomain.BaseDirectory + c.cake.thumbnailPath;
                _c.total = _c.price * _c.amount;
                totalCost += _c.total;
                ShoppingCartList.Add(_c);
            }
            data.ItemsSource = ShoppingCartList;
            data1.ItemsSource = ShoppingCartList;
            TotalCost.Text = totalCost.ToString();
            total.Text = totalCost.ToString();
            Title.Text = "SHOPPING CART";
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            Dying?.Invoke();
        }

        private void Delete_Click(object sender, RoutedEventArgs e)
        {
            var db = new cakeshopEntities();
            var item = (sender as FrameworkElement).DataContext;
            db.cakeInCarts.Remove(db.cakeInCarts.Find((item as CakeInCart).cic));
            ShoppingCartList.Remove((item as CakeInCart));
            db.SaveChanges();
        }

        private void Payment_Click(object sender, RoutedEventArgs e)
        {
            var db = new cakeshopEntities();
            int id = db.carts.Where(x => x.isCompleted == false).Select(y => y.cartId).FirstOrDefault();
            db.carts.Find(id).isCompleted = true;
            db.SaveChanges();
            Cart.Visibility = Visibility.Collapsed;
            Order.Visibility = Visibility.Visible;
            Title.Text= "ORDER COMPLETE";
            OrderID.Text = db.carts.Find(id).cartId.ToString();
            DateCreated.Text = db.carts.Find(id).createAt.ToString();
        }

        private void finish_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
