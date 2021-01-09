using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
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

namespace CakeShop
{
    /// <summary>
    /// Interaction logic for EditCake.xaml
    /// </summary>
    public partial class EditCake : Window
    {
        public delegate void DeathHandler();
        public event DeathHandler Dying;
        int cakeId;
        String oldPath;
        String newPath;
        cake _cake = null;
        public EditCake()
        {
            InitializeComponent();
            this.cakeId = -1;
        }
        
        public EditCake(int cakeId)
        {
            InitializeComponent();
            this.cakeId = cakeId;
        }
        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            Dying?.Invoke();
        }

        private void addThumbail_Click(object sender, RoutedEventArgs e)
        {
            if (cakeThumbnail.Source != null)
                oldPath = cakeThumbnail.Source.ToString().Substring(8);
            var screen = new OpenFileDialog();
            if (screen.ShowDialog() == true)
            {
                var thumbnailPath = screen.FileName;
                newPath = "Data/" + Guid.NewGuid() + Path.GetExtension(thumbnailPath);
                var savePath = AppDomain.CurrentDomain.BaseDirectory + newPath;
                File.Copy(thumbnailPath, savePath, true);
                var thumbnail = new BitmapImage(new Uri(savePath, UriKind.Absolute));

                cakeThumbnail.Source = thumbnail;
            }
            if (cakeId != -1)
            {
                var db = new cakeshopEntities();
                var oldTrip = db.cakes.Find(cakeId);
                if (!newPath.Equals(oldTrip.thumbnailPath))
                    oldTrip.thumbnailPath = newPath;

                db.SaveChanges();
                Notif.Foreground = Brushes.Green;
                Notif.Text = "Đã cập nhật hình ảnh";
            }
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            var db = new cakeshopEntities();
            if (cakeId == -1)
            {
                CakeCategory.ItemsSource = new BindingList<category>(db.categories.ToList());
            }
            else
            {
                CakeName.IsEnabled = false;
                CakeName.Foreground = Brushes.Gray;
                CakeCategory.IsEnabled = false;
                CakeCategory.Foreground = Brushes.Gray;

                _cake = db.cakes.Find(cakeId);
                CakeCategory.ItemsSource = new BindingList<category>(db.categories.Where(x => x.catId == _cake.categoryId).ToList());
                CakeCategory.SelectedIndex = 0;

                CakeName.Text = _cake.name;
            }
        }

        private void Save_Click(object sender, RoutedEventArgs e)
        {
            var db = new cakeshopEntities();

         
            if (cakeThumbnail.Source == null)
            {
                Notif.Foreground = Brushes.Orange;
                Notif.Text = "Hãy thêm hình của món bánh";
                return;
            }

            Notif.Text = "";

            var _cakeThumbnail = newPath;

            if (CakePrice.Text.Equals(""))
            {
                Notif.Foreground = Brushes.Orange;
                Notif.Text = "Hãy thêm giá của món bánh";
                return;
            }

            if (cakeId != -1)
            {

                /*var oldCake = db.cakes.Find(cakeId);
*//*
                Notif.Foreground = Brushes.Green;
                Notif.Text = "Đã cập nhật thông tin bánh";*//*


                if (CakeCategory.SelectedIndex != -1)
                {
                    var id = ((route)routeNameEdit.SelectedItem).id;
                    var oldRoute = db.routes.Find(id);
                    oldRoute.cost = int.Parse(routeMoney.Text);
                    db.SaveChanges();
                    Err.Foreground = Brushes.Green;
                    Err.Text = "Da cap nhat thong tin lo trinh";
                }
                else if (routeNameAddNew.Visibility == Visibility.Visible)
                {
                    if (routeNameAddNew.Text.Equals(""))
                    {
                        Err.Foreground = Brushes.Orange;
                        Err.Text = "Hay them ten lo trinh";
                        return;
                    }

                    var maxId = db.cakes.Max(x => x.cakeId);
                    cake newCake = new cake();
                    newCake.cakeId = maxId + 1;
                    newCake.categoryId = ;
                    newCake.price = int.Parse(CakePrice.Text);
                    db.cakes.Add(newCake);
                    Notif.Foreground = Brushes.Green;
                    Notif.Text = "Đã thêm mới bánh";
                    db.SaveChanges();
                }*/
            }
            else
            {

                if (CakeName.Text.Equals(""))
                {
                    Notif.Foreground = Brushes.Orange;
                    Notif.Text = "Hãy thêm tên bánh";
                    return;
                }

                if (CakeCategory.SelectedItem == null)
                {
                    Notif.Foreground = Brushes.Orange;
                    Notif.Text = "Hãy chọn loại bánh";
                    return;
                }

                CakeName.IsEnabled = false;
                CakeCategory.IsEnabled = false;
                addThumbail.IsEnabled = false;

                var _cakeName = CakeName.Text;
                var _cakeCatId = CakeCategory.SelectedItem;
                _cake = new cake();
                _cake.name = _cakeName;
                _cake.categoryId = ((category)_cakeCatId).catId;

                _cake.thumbnailPath = _cakeThumbnail;

                _cake.cakeId = db.cakes.Max(x => x.cakeId) + 1;
                db.cakes.Add(_cake);
                Notif.Foreground = Brushes.Green;
                Notif.Text = "Đã thêm mới món bánh";
                db.SaveChanges();

            }
        }
        private void cakePrice_LostFocus(object sender, RoutedEventArgs e)
        {
            var money = -1;
            try
            {
                money = int.Parse(CakePrice.Text);
                Notif.Text = "";
            }
            catch (Exception err)
            {
                Debug.WriteLine(err.Message);
                Notif.Foreground = Brushes.Orange;
                Notif.Text = "Số tiền phải là số tự nhiên";
                CakePrice.Text = "";
            }
        }

        private void Exit_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
