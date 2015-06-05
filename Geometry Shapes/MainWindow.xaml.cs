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
using Microsoft.Win32;

namespace Geometry_Shapes
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            string name = "Menu_Color_1";   //Postavlja defaultnu odabranu boju olovke na ("Menu_Color_1")
            object obj = FindName(name);
            if (obj != null)
            {
                var menu_item = obj as MenuItem;
                menu_item.IsChecked = true;
            }


            string name1 = "Debljina_1";    //Postavlja defaultnu odabranu debljinu olovke na ("Debljina_1")
            object obj1 = FindName(name1);
            if (obj1 != null)
            {
                var menu_item1 = obj1 as MenuItem;
                menu_item1.IsChecked = true;
            }

            string name2 = "Menu_Fill_1";   //Postavlja defaultnu odabranu boju ispune likova na ("Menu_Fill_1")
            object obj2 = FindName(name2);
            if (obj2 != null)
            {
                var menu_item2 = obj2 as MenuItem;
                menu_item2.IsChecked = true;
            }
            
        }

        string[] colors = { "Black", "Red", "Blue", "Green", "Yellow" };                        //varijabla koja sadrži nazive boja olovke
        string[] fills = { "Gray", "LightCoral", "LightBlue", "LightGreen", "LightYellow", "Transparent" };    // varijabla koja sadrži nazive boja za ispunu 
        double[] debljina = { 1.0F, 4.0F, 8.0F };                                               // varijabla koja sadrži debljinu olovke

        bool IsRectangleClicked = false;        //ako je postavljeno na true crta se pravokutnik
        bool IsEllipseClicked = false;          //ako je postavljeno na true crta se elipsa
        bool IsLineClicked = false;             //ako je postavljeno na true crta je linija
        bool IsSecondClick = false;             // Ako je drugi klik na Canvasu onda crtaj a ako nije onda je prvi klik i zapamti ga




        Point prviClick;                                //pozicija prvog klika u canvasu
        Pen olovka = new Pen(Brushes.Black, 1.0F);      //definicija olovke
        Brush pozadina = Brushes.Gray;                  //definicija pozadine


        public static class util
        {
            public static void SaveCanvas(Window window, Canvas canvas, int dpi, string filename)
            {
                Size size = new Size(window.Width, window.Height);
                canvas.Measure(size);

                var rtb = new RenderTargetBitmap(
                    (int)window.Width, //width
                    (int)window.Height, //height
                    dpi, //dpi x
                    dpi, //dpi y
                    PixelFormats.Pbgra32 // pixelformat
                    );
                rtb.Render(canvas);

                SaveRTBAsPNG(rtb, filename);
            } 

            private static void SaveRTBAsPNG(RenderTargetBitmap bmp, string filename)
            {
                 var enc = new System.Windows.Media.Imaging.PngBitmapEncoder();
                 try
                 {
                     enc.Frames.Add(System.Windows.Media.Imaging.BitmapFrame.Create(bmp));
                     using (var stm = System.IO.File.Create(filename))
                     {
                         enc.Save(stm);
                     }
                 }
                 catch
                 {
                     MessageBoxResult result = MessageBox.Show("Došlo je do greške!"); 
                     return;
                 }
            }
        }


        private void Postavi_Debljinu(object sender, RoutedEventArgs e)
        {
            var menu_item = sender as MenuItem;
            for (int i = 0; i < 3; i++)
            {
                string broj = Convert.ToString(i + 1);
                if (menu_item.Name.Contains(broj))
                {
                    menu_item.IsChecked = true;
                    olovka.Thickness = debljina[i];
                }
                else
                {
                    string name = "Debljina_" + Convert.ToString(i + 1);
                    object obj = FindName(name);
                    if (obj != null)
                    {
                        var menu_item2 = obj as MenuItem;
                        menu_item2.IsChecked = false;
                    }
                }
            }
        }




        private void Postavi_Boju(object sender, RoutedEventArgs e)
        {
            var menu_item = sender as MenuItem;
            for (int i = 0; i < 5; i++)
            {
                string broj = Convert.ToString(i + 1);
                if (menu_item.Name.Contains(broj))
                {
                    menu_item.IsChecked = true;
                    SolidColorBrush color = (SolidColorBrush)new BrushConverter().ConvertFromString(colors[i]);
                    olovka.Brush = color;
                }
                else
                {
                    string name = "Menu_Color_" + Convert.ToString(i + 1);
                    object obj = FindName(name);
                    if (obj != null)
                    {
                        var menu_item2 = obj as MenuItem;
                        menu_item2.IsChecked = false;
                    }
                }
            }
        }


        private void Postavi_Ispunu(object sender, RoutedEventArgs e)
        {
            var menu_item = sender as MenuItem;
            for (int i = 0; i < 6; i++)
            {
                string broj = Convert.ToString(i + 1);
                if (menu_item.Name.Contains(broj))
                {
                    menu_item.IsChecked = true;
                    SolidColorBrush color = (SolidColorBrush)new BrushConverter().ConvertFromString(fills[i]);
                    pozadina = color;
                }
                else
                {
                    string name = "Menu_Fill_" + Convert.ToString(i + 1);
                    object obj = FindName(name);
                    if (obj != null)
                    {
                        var menu_item2 = obj as MenuItem;
                        menu_item2.IsChecked = false;
                    }
                }
            }
        }



        private void RectangleMenuItem_Click(object sender, RoutedEventArgs e)
        {
            IsRectangleClicked = true;
            IsEllipseClicked = false;
            IsLineClicked = false;
            IsSecondClick = false;
        }

        private void ElipseMenuItem_Click(object sender, RoutedEventArgs e)
        {
            IsRectangleClicked = false;
            IsEllipseClicked = true;
            IsLineClicked = false;
            IsSecondClick = false;
        }


        private void LineMenuItem_Click(object sender, RoutedEventArgs e)
        {
            IsRectangleClicked = false;
            IsEllipseClicked = false;
            IsLineClicked = true;
            IsSecondClick = false;
        }


        private void ClearScreenItem_Click(object sender, RoutedEventArgs e)
        {
            myCanvas.Children.Clear();
        }

        private void SaveMenuItem_Click(object sender, RoutedEventArgs e)
        {
            SaveFileDialog open = new SaveFileDialog();
            if (open.ShowDialog() == true)
            {
                string file_name = open.FileName;
                file_name += ".bmp";
                util.SaveCanvas(this, this.myCanvas, 96, file_name);
            }


        }

        private void myCanvas_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {

            if (IsRectangleClicked)
            {
                if (!IsSecondClick)
                {
                    prviClick = Mouse.GetPosition(myCanvas);
                    IsSecondClick = true;
                }
                else
                {
                    Point drugiClick = Mouse.GetPosition(myCanvas);
                    Rectangle novi = new Rectangle();
                    novi.Width = Math.Abs(prviClick.X - drugiClick.X);
                    novi.Height = Math.Abs(prviClick.Y - drugiClick.Y);
                    novi.Stroke = olovka.Brush;
                    novi.StrokeThickness = olovka.Thickness;
                    novi.Fill = pozadina;
                    if (prviClick.X > drugiClick.X || prviClick.Y > drugiClick.Y)
                    {
                        Point temp = prviClick;
                        prviClick = drugiClick;
                        drugiClick = temp;
                    }
                    myCanvas.Children.Add(novi);
                    Canvas.SetLeft(novi, prviClick.X);
                    Canvas.SetTop(novi, prviClick.Y);

                    IsRectangleClicked = false;
                    IsSecondClick = false;
                }
            }

            else if(IsEllipseClicked)
            {
                if (!IsSecondClick)
                {
                    prviClick = Mouse.GetPosition(myCanvas);
                    IsSecondClick = true;
                }
                else
                {
                    Point drugiClick = Mouse.GetPosition(myCanvas);

                    Ellipse nova = new Ellipse();
                    nova.Width = Math.Abs(prviClick.X - drugiClick.X);
                    nova.Height = Math.Abs(prviClick.Y - drugiClick.Y);
                    nova.Stroke = olovka.Brush;
                    nova.Fill = pozadina;
                    nova.StrokeThickness = olovka.Thickness;
                    if (prviClick.X > drugiClick.X || prviClick.Y > drugiClick.Y)
                    {
                        Point temp = prviClick;
                        prviClick = drugiClick;
                        drugiClick = temp;
                    }
                    myCanvas.Children.Add(nova);
                    Canvas.SetLeft(nova, prviClick.X);
                    Canvas.SetTop(nova, prviClick.Y);

                    IsEllipseClicked = false;
                    IsSecondClick = false;
                }
            
            
            }

            else if (IsLineClicked)
            {
                if (!IsSecondClick)
                {
                    prviClick = Mouse.GetPosition(myCanvas);
                    IsSecondClick = true;
                }
                else
                {
                    Point drugiClick = Mouse.GetPosition(myCanvas);

                    Line myLine = new Line();
                    myLine.Stroke = olovka.Brush;
                    myLine.StrokeThickness = olovka.Thickness;
                    myLine.X1 = prviClick.X;
                    myLine.X2 = drugiClick.X;
                    myLine.Y1 = prviClick.Y;
                    myLine.Y2 = drugiClick.Y;
                    myCanvas.Children.Add(myLine);

                    IsLineClicked = false;
                    IsSecondClick = false;
                }
            }
        } 
    }
}
