using System;
using System.Collections.Generic;
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
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Collections.ObjectModel;
using System.Data.SqlClient;
using System.Configuration;

namespace CakeShop
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    /// 

    public class treeViewProduct
    {
        public treeViewProduct()
        {
            this.item = new ObservableCollection<cake>();
        }

        public string Name { get; set; }

        public int? Amount { get; set; }

        public ObservableCollection<cake> item { get; set; }
    }


    public partial class MainWindow : Window
    {
        public cakeshopEntities db = new cakeshopEntities();

        public string connectionString = "Server=.;Database=cakeshop;Trusted_Connection=True;";

        public List<category> cats = new List<category>();

        public List<treeViewProduct> products = new List<treeViewProduct>();

        public List<cake> Cake = new List<cake>();
        
        public MainWindow()
        {
            InitializeComponent();
            cats = db.categories.ToList();
            Cake = db.cakes.ToList();
            foreach (var cake in Cake)
            {
                cake.thumbnailPath = AppDomain.CurrentDomain.BaseDirectory + cake.thumbnailPath;
            }
            //test(connectionString, 1);
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {

            //MessageBox.Show($"{CakeByCatId(connectionString, 1)}");

            foreach (category cat in cats)
            {
                treeViewProduct tmp = new treeViewProduct();
                tmp.Name = cat.name;
                tmp.Amount = cat.amount;

                tmp.item = CakeByCatId(connectionString, cat.catId);

                products.Add(tmp);
            }

            dataListview.ItemsSource = Cake;
            dataTreeview.ItemsSource = products;
        }

        public ObservableCollection<cake>CakeByCatId(string connectionString, int id)
        {
            string queryString = $"select * from cake join category on cake.categoryId = category.catId where category.catId = {id};";
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                ObservableCollection<cake > res = new ObservableCollection<cake>();
                
               
                SqlCommand command = new SqlCommand(queryString, connection);
                connection.Open();
                using (SqlDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        cake tmp = new cake ();
                        tmp.cakeId = Int32.Parse(reader[0].ToString());
                        tmp.name = reader[1].ToString();
                        tmp.price = Int32.Parse(reader[2].ToString());
                        tmp.description = reader[3].ToString();
                        tmp.categoryId = Int32.Parse(reader[4].ToString());
                        tmp.thumbnailPath = reader[5].ToString();
                        
                        res.Add(tmp);
                    }
                }
                connection.Close();
                return res;
            }
        }

        public List<cake> CakeByCatName(string connectionString, string name)
        {
            string queryString = $"select * from cake join category on cake.categoryId = category.catId where category.name = '{name}';";
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                List<cake> res = new List<cake>();


                SqlCommand command = new SqlCommand(queryString, connection);
                connection.Open();
                using (SqlDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        cake tmp = new cake();
                        tmp.cakeId = Int32.Parse(reader[0].ToString());
                        tmp.name = reader[1].ToString();
                        tmp.price = Int32.Parse(reader[2].ToString());
                        tmp.description = reader[3].ToString();
                        tmp.categoryId = Int32.Parse(reader[4].ToString());
                        tmp.thumbnailPath = reader[5].ToString();

                        res.Add(tmp);
                    }
                }
                connection.Close();
                return res;
            }
        }


        private void dataTreeView_selectedItemChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
        {
            List<category> catSelect = new List<category>();

            var item = (sender as TreeView).SelectedItem as treeViewProduct;

            //MessageBox.Show($"{item.Name}");

            List<cake> cakeSelect = CakeByCatName(connectionString, item.Name);

            dataListview.ItemsSource = cakeSelect;

        }

        private void addCake_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            var addCake = new EditCake();
            addCake.Dying += ScreenClosing;
            this.Hide();
            addCake.Show();
        }

        private void ScreenClosing()
        {
            this.Show();
        }

        private void seeDetail_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            var item = (sender as FrameworkElement).DataContext;
            var id = (item as cake).cakeId;

            DetailScreen detail = new DetailScreen(id);
            detail.Dying += ScreenClosing;
            this.Hide();
            detail.Show();
        }

        private void ShoppingCart_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            var s = new ShoppingCart();
            s.Dying += ScreenClosing;
            this.Hide();
            s.Show();
        }

        private void Window_SourceInitialized(object sender, EventArgs e)
        {
            bool showSplash;
            var value = ConfigurationManager.AppSettings["ShowSplashScreen"];
            showSplash = bool.Parse(value);
            if (showSplash == false)
            {
                return;
            }
            else
            {
                var sc = new SplashScreen();
                sc.ShowDialog();
            }
        }
    }
}
