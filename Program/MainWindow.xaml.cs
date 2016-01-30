﻿using System;
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

namespace Program
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        Player player = new Player("Giel", new Point(1, 1), AssetManager.readMobilePlayer());
        BitmapImage carBitmap = new BitmapImage(new Uri(@"C:\Users\Titaantje\Documents\GitHub\Program\TileTest\Program\bin\Debug\Tuscan_Idle_60000.png", UriKind.Absolute));
        //Image[] carImg = new Image[5];
        //Random rnd = new Random();



        public MainWindow()
        {
            InitializeComponent();

            // inisialize map
            Map map = new Map();

            //load a reagion into the map.
            map.addMap(ReadRegion.Read(".//Region 1.xml"));

            
            Region toLoadRegion = map.getmap(0);

            List<List<Tile>> drawList = toLoadRegion.getRegionValue();

            RevealRegion(drawList);
            
            
            
            

            drawCharacter(player.getPointposition(), player.getIdleImage("Tuscan_Idle_60000.png"));

        }

        private void drawCharacter(Point cord, BitmapImage bitImage)
        {
            Image tekening = new Image();
            tekening.Source = bitImage;
            tekening.Width = 128;
            tekening.Height = 64;
            drawingCanvas.Children.Add(tekening);
            Canvas.SetTop(tekening, getPointTilePoint(Convert.ToInt32(cord.X), Convert.ToInt32(cord.Y), 64, 32).Y);
            Canvas.SetLeft(tekening, getPointTilePoint(Convert.ToInt32(cord.X), Convert.ToInt32(cord.Y), 64, 32).X);
        }

        private void RevealRegion(List<List<Tile>> drawList)
        {
            int i = 0;

            int width = 64;
            int height = 32;

            //plits the matrix in mutiple rows
            foreach (var value in drawList)
            {
                //ge the value of each row
                for (int j = 0; j < value.Count; j++)
                {
                    DrawingTile.DrawTile(i, j, width, height, value[j]);


                    


                    // get the value of the rectangle
                    //Rectangle tile = CreateVisualMap.draw(value[j].getValue(), width, height);
                    //RotateTransform rotation = new RotateTransform();
                    ////tile.LayoutTransform = new RotateTransform(45, 0, 0);
                    //drawingCanvas.Children.Add(tile);

                    //Canvas.SetTop(tile, j + width);
                    //Canvas.SetLeft(tile, i + height);

                }
                i++;
            }
            
        }

        private Point getPointTilePoint(int x, int y, int with, int height)
        {
            var mainWindowInstant = (MainWindow)App.Current.MainWindow;
            Point point = new Point();
            int screenX = (x - y) * with / 2 + Convert.ToInt32(drawingCanvas.Width / 2);
            int screenY = (x + y) * height / 2;
            point = new Point(screenX, screenY);

            return point;
        }
            

        private void button_Click_1(object sender, RoutedEventArgs e)
        {
        }
        private Point getCords(double x, double y, double tileWidth, double tileHeight)
       {
           double test = ((x / (tileWidth / 2) + y / (tileHeight / 2)) / 2);
           //take the lowest cord -x
           int mapX = Convert.ToInt32(Math.Floor(test));
      
           test = (y / (tileHeight / 2) - (x / (tileWidth / 2))) / 2;
           //take the lowest cord -Y
            int mapY = Convert.ToInt32(Math.Floor(test));

            Console.WriteLine("Cord x {0} and cord y {1} ", mapX, mapY);
            Point cord = new Point(mapX, mapY);
            return cord;
          }

        private void drawingCanvas_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            Point cordClick = new Point();
            // get the clicked cordinations
            var pos = this.PointToScreen(Mouse.GetPosition(this));
            //convert to isometric cord values
            cordClick = getCords(Convert.ToInt32(pos.X - (drawingCanvas.ActualWidth / 2)), Convert.ToInt32(pos.Y), 64, 32);
            player.setPosition(cordClick);
            //drawingCanvas.Children.Clear();
            
            //Clear all images from the canvas
            var images = drawingCanvas.Children.OfType<Image>().ToList();
            foreach (var image in images)
            {
                drawingCanvas.Children.Remove(image);
            }
            
            drawCharacter(player.getPointposition(), player.getIdleImage("Tuscan_Idle_60000.png"));
        }
    }  
}
