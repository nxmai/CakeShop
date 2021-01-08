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
using System.Windows.Shapes;

namespace CakeShop
{
    /// <summary>
    /// Interaction logic for DetailScreen.xaml
    /// </summary>
    /// 
    class cakeDetail
    {
        public int cakeId { get; set; }
        public string name { get; set; }
        public string price { get; set; }
        public string description { get; set; }
        public Nullable<int> categoryId { get; set; }
        public BitmapImage thumbnailPath { get; set; }
        public string catName { get; set; }

    }
    public partial class DetailScreen : Window
    {
        cakeshopEntities db = new cakeshopEntities();
        cakeDetail detail = new cakeDetail();
        List<image> listImage = new List<image>();
        int cakeDetailID = 1;
        int price = 0;
        public DetailScreen()
        {
            InitializeComponent();
        }

        public DetailScreen(int cakeID)
        {
            cakeDetailID = cakeID;
            InitializeComponent();
            loadDataFromDB(cakeID);
         
            this.DataContext = detail;

            createCarousel();

        }
        private void loadCakeDetailFromDB(int cakeID)
        {
            var tempListCake = db.cakes.Where(p => p.cakeId == cakeID).ToList();
            cake tempcake = new cake();
            if (tempListCake.Count() > 0)
            {
                tempcake = tempListCake[0];

                price = (int) tempcake.price;

                detail.cakeId = cakeID;
                detail.name = tempcake.name;
                detail.price = fomatPrice( (int) tempcake.price);
                detail.description = tempcake.description;
                detail.catName = getCatName((int)tempcake.categoryId);

                var uri = new Uri(getFolder() + $"{tempcake.thumbnailPath}", UriKind.Absolute);
                var bitmap = new BitmapImage(uri);

                detail.thumbnailPath = bitmap;

            }
        }


        private string getCatName (int catID)
        {
            string result = "";
            var cat = new List<category> (db.categories.Where(p => p.catId == catID).ToList());
           
            if (cat.Count() != 0)
            {
                result = cat[0].name;
            }
            return result;
        }

        private string getFolder()
        {
            string folder = "";
            folder = AppDomain.CurrentDomain.BaseDirectory;
            return folder;
        }
        private void loadListImageFromDB(int cakeID)
        {
            listImage = new List<image>(db.images.Where(p => p.cakeId == cakeID).ToList());

        }

        private string fomatPrice (int price)
        {
            string result = "";

            string temp = price.ToString();

            for (int i =1; i<= temp.Length; i++)
            {
                result=result.Insert(0, temp[temp.Length - i].ToString());

                if (i% 3 == 0 && i!= temp.Length)
                {
                    result=result.Insert(0,".");
                }
            }

            result += " VND";
            return result;
        }


        void createCarousel()
        {
            foreach (var tempImage in listImage)
            {

                var border = new Border();
                border.CornerRadius = new CornerRadius(15);

                var temp = new ImageBrush();
                var uri = new Uri(getFolder() + $"{tempImage.path}", UriKind.Absolute);
                var bitmap = new BitmapImage(uri);
                temp.ImageSource = bitmap;


                border.Background = temp;
                border.Width = 120;
                border.Height = 120;

                border.Margin = new Thickness(0, 0, 3, 0);

                carousel.Children.Add(border);
            }

        }

        private void loadDataFromDB(int cakeID)
        {
            loadCakeDetailFromDB(cakeID);
            loadListImageFromDB(cakeID);
        }

        //private void loadAmountInCartFromDB(int cakeID)
        //{
        //    var cardTemp = db.carts.Where(p => p.isCompleted == false).ToList();
        //    if (cardTemp.Count() > 0)
        //    {
        //        var cakeTemp = db.cakeInCarts.Where(p => p.cartId == cardTemp[0].cartId).ToList();
        //        if (cakeTemp.Count() > 0)
        //        {
        //            amountTxt.Text = cakeTemp[0].amount.ToString();
        //        }
        //    }
        //}

        private void decreaseAmountClick(object sender, RoutedEventArgs e)
        {
            int temp = 0;
            bool check = Int32.TryParse(amountTxt.Text, out temp);
            if (check == false)
            {
                temp = 0;
            }
            if (temp > 0)
            {
                amountTxt.Text = (temp - 1).ToString();
            }
        }

        private void increaseAmountClick(object sender, RoutedEventArgs e)
        {
            int temp = 0;
            bool check = Int32.TryParse(amountTxt.Text, out temp);
            if (check == false)
            {
                temp = 0;
            }
            if (temp < Int32.MaxValue)
            {
                amountTxt.Text = (temp + 1).ToString();
            }
        }

        private void addToCartClick(object sender, RoutedEventArgs e)
        {

            int amountNumber = 0;
            bool check = Int32.TryParse(amountTxt.Text, out amountNumber);
            if (check == false)
            {
                MessageBox.Show("Thêm vào giỏ hàng thất bại! Số lượng phải thuộc dạng số nguyên");
            }
            else if (amountNumber <= Int32.MaxValue)
            {
                var listcard = db.carts.ToList();

                //table cart empty
                if (listcard.Count() == 0)
                {

                    //them cart
                    cart cardTemp = new cart();
                    cardTemp.cartId = 1;
                    cardTemp.total = amountNumber * price;
                    cardTemp.isCompleted = false;
                    cardTemp.createAt = DateTime.Now;
                    db.carts.Add(cardTemp);
                    db.SaveChanges();

                    //them chi cake vao cakeincart
                    cakeInCart cicTemp = new cakeInCart();
                    cicTemp.cicId = 1;
                    cicTemp.cakeId = cakeDetailID;
                    cicTemp.amount = amountNumber;
                    cicTemp.cartId = 1;
                    db.cakeInCarts.Add(cicTemp);
                    db.SaveChanges();
                }
                else // teable cart has some row
                {
                    int maxId = 1;
                    int carid = 0;
                    foreach (var cardTemp in listcard)
                    {
                        //truong hop trong cart isfinised = flase // nghia la dang mua hang
                        if (cardTemp.isCompleted == false)
                        {
                            carid = cardTemp.cartId;

                           
                            break;
                        }
                        else
                        {
                            if (cardTemp.cartId > maxId)
                            {
                                maxId = cardTemp.cartId;
                            }
                        }
                    }
                    //truong hop tat ca cart isfinished == true
                    if (carid == 0)
                    {
                        carid = maxId + 1; // tao id moi cho card

                        cart cardTemp = new cart();
                        cardTemp.cartId = carid;
                        cardTemp.total = amountNumber * price;
                        cardTemp.isCompleted = false;
                        cardTemp.createAt = DateTime.Now;
                        db.carts.Add(cardTemp);
                        db.SaveChanges();
                    }
                    else
                    {
                        //update lai cart
                        var cartTemp = db.carts.Where(p => p.cartId == carid).SingleOrDefault();
                        cartTemp.total += amountNumber * price;
                        cartTemp.createAt = DateTime.Now;
                        db.SaveChanges();
                    }


                    //lay maxid trong cakeincart 
                    int maxIDinCiC = 1;
                    int cicID = 0;
                    int amount = 0;
                    foreach (var temp in db.cakeInCarts.ToList())
                    {
                        if (temp.cakeId == cakeDetailID && temp.cartId == carid)
                        {
                            cicID = temp.cicId;
                            amount = (int) temp.amount;
                            break;
                        }
                        if (temp.cicId > maxIDinCiC)
                        {
                            maxIDinCiC = temp.cicId;
                        }
                    }


                    //cake khong co trong cakeincart
                    if (cicID == 0)
                    {
                        cicID = maxIDinCiC + 1;

                        //them cakeincart
                        cakeInCart cicTemp = new cakeInCart();
                        cicTemp.cicId = cicID;
                        cicTemp.cakeId = cakeDetailID;
                        cicTemp.amount = amount+ amountNumber;
                        cicTemp.cartId = carid;

                        db.cakeInCarts.Add(cicTemp);
                        db.SaveChanges();

                    }
                    else
                    {
                        var cicTemp = db.cakeInCarts.Where(p => p.cicId == cicID).SingleOrDefault();
                        cicTemp.amount = amount + amountNumber;
                        db.SaveChanges();
                    }


                    ;
                }
                MessageBox.Show("Thêm thành công !!!");
            }

            
        }
    }
}
