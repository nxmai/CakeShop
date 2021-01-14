using System;
using System.Configuration;
using System.Windows;
using System.Windows.Media.Imaging;

namespace CakeShop
{
    /// <summary>
    /// Interaction logic for SplashScreen.xaml
    /// </summary>
    public partial class SplashScreen : Window
    {
        public SplashScreen()
        {
            InitializeComponent();
        }

        static Random _rng = new Random();
        private void DisplayImage(Tuple<string, string, string> images)
        {
            String pathRoot = AppDomain.CurrentDomain.BaseDirectory;

            Console.WriteLine($"{pathRoot}SplashImages/{images.Item1}");
            var bitmap = new BitmapImage(
                        new Uri($"{pathRoot}SplashImages/{images.Item1}",
                        UriKind.Absolute)
                        );
            image.Source = bitmap;
        }

        void DisplaySplashScreen()
        {
            Tuple<string, string, string>[] imagesArray =
            {
                Tuple.Create("1.jpg", "BÁNH ALFAJORES", "Bánh Alfajores (Argentina): Alfajores là sự kết hợp hài hòa của lớp bánh xốp mềm, xen giữa là caramel thơm ngậy"),
                Tuple.Create("2.jpg", "BÁNH BABKA", "Bánh Babka (Ba Lan): Babka là loại bánh ngọt truyền thống ở Ba Lan, giống bánh mì brioche nhưng có thêm sô cô la."),
                Tuple.Create("3.jpg", "BÁNH BIZCOCHO", "Bánh Bizcocho Criolla (Cộng hòa Dominica): Bizcocho Criolla hương vani kết hợp với kem dừa thơm ngon"),
                Tuple.Create("4.jpg", "BÁNH BEIGNETS", "Bánh Beignets (New Orleans): Beignets là một loại bánh rán kiểu Pháp, bên trên có thêm lớp bột đường trắng tinh"),
                Tuple.Create("5.jpg", "BÁNH MOCHI", "Bánh Mochi (Nhật Bản): Bánh Mochi với hương vị đa dạng là sự kết hợp tuyệt vời của lớp vỏ làm từ bột gạo nếp và phần kem mát lạnh bên trong."),
                Tuple.Create("6.jpg", "KEM Ý", "Kem Gorgonzola Gelato (Ý): Khó có thể từ chối thưởng thức bất cứ vị kem Gorgonzola Gelato ngon tuyệt nào."),
                Tuple.Create("7.jpg", "TANGYUAN", "Tangyuan (Trung Quốc): Tangyuan được làm từ những viên bột nếp nhiều màu sắc, bên trong có nhân mè, đậu phộng hay đậu đỏ, dùng chung với nước gừng nấu với đường phèn."),
                Tuple.Create("8.jpg", "CHAMPAGNE TRUFLES", "Champagne Truffles (Thụy Sỹ): Champagne Truffles là sự kết hợp của bơ hòa quyện cùng sô cô la sữa, thêm chút sâm panh."),
                Tuple.Create("9.jpg", "BÁNH KNAFEH", "Bánh Knafeh (Israel): Knafeh là loại bánh phô mai xirô có rưới chút nước hoa hồng rất được ưa thích ở Israel."),
            };

            int randomNumber = _rng.Next(6);

            DisplayImage(imagesArray[randomNumber]);
            PlaceName.Text = imagesArray[randomNumber].Item2;
            PlaceContent.Text = imagesArray[randomNumber].Item3;
        }

        /* private void ScreenClosing()
         {
             this.Show();
         }*/
        private void continue_click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void CheckBox_Checked(object sender, RoutedEventArgs e)
        {
            var config = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
            config.AppSettings.Settings["ShowSplashScreen"].Value = "false";
            config.Save(ConfigurationSaveMode.Full);
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            DisplaySplashScreen();
        }
    }
}
