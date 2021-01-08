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
       
        
        public MainWindow()
        {
            InitializeComponent();
            cats = db.categories.ToList();
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



    }
}
