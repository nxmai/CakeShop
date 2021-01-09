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
        int cakeId = -1;
        bool isNewCake = true;
        String oldPath;
        String newPath;
        cake _cake = null;
        List<String> FilePath = new List<string>();
        public EditCake()
        {
            InitializeComponent();
        }
        
        public EditCake(int cakeId)
        {
            InitializeComponent();
            this.cakeId = cakeId;
            isNewCake = false;
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
            if (!isNewCake)
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
            if (isNewCake)
            {
                CakeCategory.ItemsSource = new BindingList<category>(db.categories.ToList());
                cakeId = db.cakes.Max(x => x.cakeId) + 1;
            }
            else
            {
                CakeName.IsEnabled = false;
                CakeName.Foreground = Brushes.Gray;
                CakeCategory.IsEnabled = false;
                CakeCategory.Foreground = Brushes.Gray;
                addThumbail.IsEnabled = false;

                _cake = db.cakes.Find(cakeId);
                CakeCategory.ItemsSource = new BindingList<category>(db.categories.Where(x => x.catId == _cake.categoryId).ToList());
                CakeCategory.SelectedIndex = 0;

                CakeName.Text = _cake.name;
                CakePrice.Text = _cake.price.ToString();
                CakeDescription.Text = _cake.description;

                cakeThumbnail.Source = new BitmapImage(new Uri(AppDomain.CurrentDomain.BaseDirectory + db.cakes.Find(cakeId).thumbnailPath, UriKind.Absolute));
                var listImage = db.images.Where(x => x.cakeId == cakeId).ToList();
                foreach (var tempImage in listImage)
                {

                    var border = new Border();
                    border.CornerRadius = new CornerRadius(15);

                    var temp = new ImageBrush();
                    var uri = new Uri(AppDomain.CurrentDomain.BaseDirectory+ $"{tempImage.path}", UriKind.Absolute);
                    var bitmap = new BitmapImage(uri);
                    temp.ImageSource = bitmap;


                    border.Background = temp;
                    border.Width = 100;
                    border.Height = 90;

                    border.Margin = new Thickness(0, 0, 3, 0);

                    carousel.Children.Add(border);
                }

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

            var _cakePrice = int.Parse(CakePrice.Text);

            if (CakeDescription.Text.Equals(""))
            {
                Notif.Foreground = Brushes.Orange;
                Notif.Text = "Hãy thêm mô tả cho bánh";
                return;
            }

            var _cakeDes = CakeDescription.Text;

            if (!isNewCake)
            {

                var oldCake = db.cakes.Find(cakeId);

                
                if (!_cakePrice.Equals(oldCake.price))
                {
                    oldCake.price = _cakePrice;
                }

                if (!_cakeDes.Equals(oldCake.description))
                {
                    oldCake.description = _cakeDes;
                }

                db.SaveChanges();
                Notif.Foreground = Brushes.Green;
                Notif.Text = "Đã cập nhật thông tin bánh";
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
                _cake.description = _cakeDes;
                _cake.thumbnailPath = _cakeThumbnail;
                _cake.cakeId = cakeId;
                _cake.price = _cakePrice;

                db.cakes.Add(_cake);
                Notif.Foreground = Brushes.Green;
                Notif.Text = "Đã thêm mới món bánh";
                isNewCake = false;
                db.SaveChanges();
                foreach (var path in FilePath)
                {
                    image _image = new image();
                    _image.imgId = db.images.Max(x => x.imgId) + 1;
                    _image.cakeId = cakeId;
                    _image.path = path;
                    db.images.Add(_image);
                    db.SaveChanges();
                }
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

        private void addDesPic_Click(object sender, RoutedEventArgs e)
        {
            //List<String> FilePath = new List<string>();
            var screen = new OpenFileDialog();
            screen.Multiselect = true;
            if (screen.ShowDialog() == true)
            {
                var thumbnailPaths = screen.FileNames;
                foreach (var thumbnailPath in thumbnailPaths)
                {
                    var savePath = "Data/" + Guid.NewGuid() + Path.GetExtension(thumbnailPath);
                    FilePath.Add(savePath);
                    File.Copy(thumbnailPath, AppDomain.CurrentDomain.BaseDirectory + savePath, true);
                    var thumbnail = new BitmapImage(new Uri(AppDomain.CurrentDomain.BaseDirectory + savePath, UriKind.Absolute));
                    var border = new Border();
                    border.CornerRadius = new CornerRadius(15);

                    var temp = new ImageBrush();

                    temp.ImageSource = thumbnail;


                    border.Background = temp;
                    border.Width = 100;
                    border.Height = 90;

                    border.Margin = new Thickness(0, 0, 3, 0);

                    carousel.Children.Add(border);
                }
            }

            if (!isNewCake)
            {
                foreach (var path in FilePath)
                {
                    var db = new cakeshopEntities();
                    image _image = new image();
                    _image.imgId = db.images.Max(x => x.imgId) + 1;
                    _image.cakeId = cakeId;
                    _image.path = path;
                    db.images.Add(_image);
                    db.SaveChanges();
                }
            }
        }
    }
}
