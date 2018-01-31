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
using System.IO;
using System.Text.RegularExpressions;

namespace LevelMaker
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>

    public partial class MainWindow : Window
    {
        //30 / 100
        static int totalSize = 75;
        static int minHeight = 450;
        static int minWidth = 150;
        int gridHeight = 50;
        int gridWidth = 50;
        bool leftButtonDown = false;
        bool rightButtonDown = false;
        int tileSize = 10;
        static string green = Brushes.Green.ToString();
        Rectangle[,] tileArray = new Rectangle[totalSize, totalSize];
        float[,] heightMapArray = new float[totalSize, totalSize];

        public MainWindow()
        {
            InitializeComponent();
            //form button
            canvas.MouseLeftButtonDown += Window_MouseLeftButtonDown;
            canvas.MouseLeftButtonUp += Window_MouseLeftButtonUp;
            canvas.MouseRightButtonDown += Window_MouseRightButtonDown;
            canvas.MouseRightButtonUp += Window_MouseRightButtonUp;
            GenerateGrid();
            ResizeGrid();
            XValue.Text = gridWidth.ToString();
            YValue.Text = gridHeight.ToString();
        }

        private void ResizeGrid()
        {
            for (int row = 0; row < totalSize; row++)
            {
                for (int column = 0; column < totalSize; column++)
                {
                    if (row > gridHeight - 1 || column > gridWidth - 1)
                    {
                        tileArray[row, column].IsEnabled = false;
                        tileArray[row, column].Visibility = Visibility.Hidden;
                    }
                    else
                    {
                        tileArray[row, column].IsEnabled = true;
                        tileArray[row, column].Visibility = Visibility.Visible;
                    }
                }
            }
            if (gridHeight != 0 && gridWidth != 0)
            {
                int width = (gridWidth * tileSize) + minWidth;
                int height = (gridHeight * tileSize) + 40;
                if (width < minWidth)
                    this.Width = minWidth;
                else
                    this.Width = width;
                if (height < minHeight)
                    this.Height = minHeight;
                else
                    this.Height = height;
            }
            else
            {
                this.Width = minWidth;
                this.Height = minHeight;
            }
        }

        private void GenerateGrid()
        {
            for (int row = 0; row < totalSize; row++)
            {
                for (int column = 0; column < totalSize; column++)
                {
                    tileArray[row, column] = new Rectangle()
                    {
                        Width = tileSize,
                        Height = tileSize,
                        Fill = Brushes.LightGray,
                        Stroke = Brushes.Gray,
                        StrokeThickness = 1,
                    };
                    tileArray[row, column].MouseEnter += rect_MouseButtonHeldDown;
                    tileArray[row, column].MouseLeftButtonDown += rect_MouseLeftButtonDown;
                    tileArray[row, column].MouseRightButtonDown += rect_MouseRightButtonDown;
                    canvas.Children.Add(tileArray[row, column]);
                    Canvas.SetLeft(tileArray[row, column], tileSize * column);
                    Canvas.SetTop(tileArray[row, column], tileSize * row);
                    //if (row == totalSize - 1 || column == totalSize - 1 || column == 0 || row == 0)
                    //{
                    //    tileArray[row, column].Fill = Brushes.Purple;
                    //}
                }
            }
        }

        private void rect_MouseButtonHeldDown(object sender, MouseEventArgs e)
        {
            if (System.Windows.Input.Mouse.LeftButton == MouseButtonState.Released)
                leftButtonDown = false;
            if (System.Windows.Input.Mouse.RightButton == MouseButtonState.Released)
                rightButtonDown = false;

            if (leftButtonDown)
                rect_MouseLeftButtonDown(sender, e);
            else if (rightButtonDown)
                ((Rectangle)sender).Fill = Brushes.LightGray;
        }

        private void rect_MouseRightButtonDown(object sender, MouseEventArgs e)
        {
            ((Rectangle)sender).Fill = Brushes.LightGray;
        }

        private void rect_MouseLeftButtonDown(object sender, MouseEventArgs e)
        {
            switch (TileType.SelectionBoxItem.ToString())
            {
                case "Platform":
                    ((Rectangle)sender).Fill = Brushes.Green;
                    break;
                case "Wall":
                    ((Rectangle)sender).Fill = Brushes.Purple;
                    break;
                case "Enemy Path":
                    ((Rectangle)sender).Fill = Brushes.Red;
                    break;
                case "Door Right":
                    ((Rectangle)sender).Fill = Brushes.Brown;
                    break;
                case "Door Left":
                    ((Rectangle)sender).Fill = Brushes.RosyBrown;
                    break;
                case "Door Up":
                    ((Rectangle)sender).Fill = Brushes.SaddleBrown;
                    break;
                case "Door Down":
                    ((Rectangle)sender).Fill = Brushes.SandyBrown;
                    break;
                case "Collectable":
                    ((Rectangle)sender).Fill = Brushes.Blue;
                    break;
                case "Ladder":
                    ((Rectangle)sender).Fill = Brushes.Yellow;
                    break;
                case "Rope":
                    ((Rectangle)sender).Fill = Brushes.Black;
                    break;
                case "Stairs up from Left":
                    ((Rectangle)sender).Fill = Brushes.Aqua;
                    break;
                case "Stairs up from Right":
                    ((Rectangle)sender).Fill = Brushes.Aquamarine;
                    break;
                case "Conveyor Left":
                    ((Rectangle)sender).Fill = Brushes.DarkBlue;
                    break;
                case "Conveyor Right":
                    ((Rectangle)sender).Fill = Brushes.DarkCyan;
                    break;
                case "Empty":
                    ((Rectangle)sender).Fill = Brushes.LightGray;
                    break;
            }
        }

        private void button_Click(object sender, RoutedEventArgs e)
        {
            PrintToTxt(ConvertToTxt());
        }

        private void PrintToTxt(char[] array)
        {
            //Create File
            Directory.CreateDirectory("../../Levels/" + textBox.Text.ToString());
            //Set file location
            var fileName = LevelNumber.SelectionBoxItem.ToString();
            var rootPath = "../../Levels/" + textBox.Text.ToString() + "/";
            var filePath = System.IO.Path.Combine(rootPath, fileName);
            if (File.Exists(filePath))
            {
                var result = MessageBox.Show("This file already exists, would you like to overwrite?", "File Overwrite Detected", MessageBoxButton.YesNo);
                if (result != MessageBoxResult.Yes)
                    return;
            }
            using (StreamWriter sw = new StreamWriter(filePath))
            {
                int i = 0;
                foreach (char item in array)
                {
                    if (i % gridWidth == 0 && i != 0)
                    {
                        sw.Write(Environment.NewLine);
                    }
                    sw.Write(item);
                    i++;
                }
                //sw.Write(Environment.NewLine);
                //sw.Write(LevelName.Text.ToString());
                //sw.Write(Environment.NewLine);
                //sw.Write(LevelType.SelectionBoxItem.ToString());
                //sw.Write(Environment.NewLine);
                //sw.Write(comboRoomUp.SelectionBoxItem.ToString());
                //sw.Write(Environment.NewLine);
                //sw.Write(comboRoomDown.SelectionBoxItem.ToString());
                //sw.Write(Environment.NewLine);
                //sw.Write(comboRoomLeft.SelectionBoxItem.ToString());
                //sw.Write(Environment.NewLine);
                //sw.Write(comboRoomRight.SelectionBoxItem.ToString());
            }
            MessageBox.Show("File Saved Successfully");
        }

        private char[] ConvertToTxt()
        {
            bool Ladder = false;
            char[] array = new char[gridHeight * (gridWidth)];
            for (int row = 0; row < gridHeight; row++)
            {
                for (int column = 0; column < gridWidth; column++)
                {
                    if (tileArray[row, column].Fill == Brushes.LightGray)
                    {
                        //Empty Space
                        array[column + (row * gridWidth)] = '0';
                    }
                    else if (tileArray[row, column].Fill == Brushes.Green)
                    {
                        //Platform
                        array[column + (row * gridWidth)] = '1';
                    }
                    else if (tileArray[row, column].Fill == Brushes.Purple)
                    {
                        //Wall
                        array[column + (row * gridWidth)] = '8';
                    }
                    else if (tileArray[row, column].Fill == Brushes.Yellow)
                    {
                        //Ladder
                        if (Ladder == false)
                        {
                            array[column + (row * gridWidth)] = '4';
                            Ladder = true;
                        }
                        else
                        {
                            array[column + (row * gridWidth)] = '6';
                            Ladder = false;
                        }
                    }
                    else if (tileArray[row, column].Fill == Brushes.Red)
                    {
                        //Enemy Path
                        array[column + (row * gridWidth)] = '3';
                    }
                    else if (tileArray[row, column].Fill == Brushes.Brown)
                    {
                        //Door Right
                        array[column + (row * gridWidth)] = 'R';
                    }
                    else if (tileArray[row, column].Fill == Brushes.RosyBrown)
                    {
                        //Door Left
                        array[column + (row * gridWidth)] = 'L';
                    }
                    else if (tileArray[row, column].Fill == Brushes.SaddleBrown)
                    {
                        //Door Up
                        array[column + (row * gridWidth)] = 'U';
                    }
                    else if (tileArray[row, column].Fill == Brushes.SandyBrown)
                    {
                        //Door Down
                        array[column + (row * gridWidth)] = 'D';
                    }
                    else if (tileArray[row, column].Fill == Brushes.Blue)
                    {
                        //Collectable
                        array[column + (row * gridWidth)] = '2';
                    }
                    else if (tileArray[row, column].Fill == Brushes.Black)
                    {
                        //Rope
                        array[column + (row * gridWidth)] = 'I';
                    }
                    else if (tileArray[row, column].Fill == Brushes.Aqua)
                    {
                        //Stairs up from left
                        array[column + (row * gridWidth)] = '<';
                    }
                    else if (tileArray[row, column].Fill == Brushes.Aquamarine)
                    {
                        //Stairs up from right
                        array[column + (row * gridWidth)] = '>';
                    }
                    else if (tileArray[row, column].Fill == Brushes.DarkBlue)
                    {
                        //Conveyor Left
                        array[column + (row * gridWidth)] = 'C';
                    }
                    else if (tileArray[row, column].Fill == Brushes.DarkCyan)
                    {
                        //Conveyor Right
                        array[column + (row * gridWidth)] = 'K';
                    }
                }
            }

            return array;
        }

        void LoadPreviousLevel()
        {
            Microsoft.Win32.OpenFileDialog dlg = new Microsoft.Win32.OpenFileDialog();
            string CombinedPath = System.IO.Path.Combine(Directory.GetCurrentDirectory(), "..\\..\\Levels");
            dlg.InitialDirectory = System.IO.Path.GetFullPath(CombinedPath);

            // Display OpenFileDialog by calling ShowDialog method 
            Nullable<bool> result = dlg.ShowDialog();
            string filename = "";
            // Get the selected file name and display in a TextBox 
            if (result == true)
            {
                // Open document 
                filename = dlg.FileName;
                string[] files = filename.Split('\\');
                textBox.Text = files[files.Length - 2];
                int index = filename[filename.Length - 1] - '0';
                LevelNumber.SelectedIndex = index - 1;
            }
            else
            {
                MessageBox.Show("Invalid File");
                return;
            }
            //Remove current level
            for (int row = 0; row < gridHeight; row++)
            {
                for (int column = 0; column < gridWidth; column++)
                {
                    canvas.Children.Remove(tileArray[row, column]);
                }
            }

            string readText = File.ReadAllText(filename);
            char[] charArray = readText.ToCharArray();

            int i = 0;
            for (int row = 0; row < gridHeight; row++)
            {
                for (int column = 0; column < gridWidth; column++)
                {
                    tileArray[row, column] = new Rectangle()
                    {
                        Width = tileSize,
                        Height = tileSize,
                        Fill = Brushes.LightGray,
                        Stroke = Brushes.Gray,
                        StrokeThickness = 1,
                    };

                    switch (charArray[i])
                    {
                        case '1':
                            tileArray[row, column].Fill = Brushes.Green;
                            break;
                        case '2':
                            tileArray[row, column].Fill = Brushes.Blue;
                            break;
                        case '3':
                            tileArray[row, column].Fill = Brushes.Red;
                            break;
                        case '4':
                            tileArray[row, column].Fill = Brushes.Yellow;
                            break;
                        case '6':
                            tileArray[row, column].Fill = Brushes.Yellow;
                            break;
                        case '8':
                            tileArray[row, column].Fill = Brushes.Purple;
                            break;
                        case 'R':
                            tileArray[row, column].Fill = Brushes.Brown;
                            break;
                        case 'L':
                            tileArray[row, column].Fill = Brushes.RosyBrown;
                            break;
                        case 'U':
                            tileArray[row, column].Fill = Brushes.SaddleBrown;
                            break;
                        case 'D':
                            tileArray[row, column].Fill = Brushes.SandyBrown;
                            break;
                        case 'I':
                            tileArray[row, column].Fill = Brushes.Black;
                            break;
                        case '<':
                            tileArray[row, column].Fill = Brushes.Aqua;
                            break;
                        case '>':
                            tileArray[row, column].Fill = Brushes.Aquamarine;
                            break;
                        case 'K':
                            tileArray[row, column].Fill = Brushes.DarkCyan;
                            break;
                        case 'C':
                            tileArray[row, column].Fill = Brushes.DarkBlue;
                            break;
                        case '\r':
                            i += 2;
                            break;
                    }
                    i++;
                    tileArray[row, column].MouseEnter += rect_MouseButtonHeldDown;
                    tileArray[row, column].MouseLeftButtonDown += rect_MouseLeftButtonDown;
                    tileArray[row, column].MouseRightButtonDown += rect_MouseRightButtonDown;
                    canvas.Children.Add(tileArray[row, column]);
                    Canvas.SetLeft(tileArray[row, column], tileSize * column);
                    Canvas.SetTop(tileArray[row, column], tileSize * row);
                    //if (row == gridHeight - 1 || column == gridWidth - 1 || column == 0 || row == 0)
                    //{
                    //    tileArray[row, column].Fill = Brushes.Purple;
                    //}
                }
            }
            //string[] lines = System.IO.File.ReadAllLines(filename);
            ////Delete all but the extra elements
            //for (int x = 0; x < gridHeight; x++)
            //{
            //    lines = lines.Skip(x).ToArray();
            //}
            //LevelName.Text = lines[0];
            //foreach (ComboBoxItem item in LevelType.Items)
            //    if (item.Content.ToString() == lines[1])
            //    {
            //        LevelType.SelectedValue = item;
            //        break;
            //    }
            //foreach (ComboBoxItem item in comboRoomUp.Items)
            //    if (item.Content.ToString() == lines[2])
            //    {
            //        comboRoomUp.SelectedValue = item;
            //        break;
            //    }
            //foreach (ComboBoxItem item in comboRoomDown.Items)
            //    if (item.Content.ToString() == lines[3])
            //    {
            //        comboRoomDown.SelectedValue = item;
            //        break;
            //    }
            //foreach (ComboBoxItem item in comboRoomLeft.Items)
            //    if (item.Content.ToString() == lines[4])
            //    {
            //        comboRoomLeft.SelectedValue = item;
            //        break;
            //    }
            //foreach (ComboBoxItem item in comboRoomRight.Items)
            //    if (item.Content.ToString() == lines[5])
            //    {
            //        comboRoomRight.SelectedValue = item;
            //        break;
            //    }
        }


        private void Load_Click(object sender, RoutedEventArgs e)
        {
            LoadPreviousLevel();
        }

        private void Window_MouseRightButtonDown(object sender, MouseButtonEventArgs e)
        {
            rightButtonDown = true;
        }

        private void Window_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            leftButtonDown = true;
        }

        private void Window_MouseRightButtonUp(object sender, MouseButtonEventArgs e)
        {
            rightButtonDown = false;
        }

        private void Window_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            leftButtonDown = false;
        }

        private void SliderX_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            XValue.Text = e.NewValue.ToString();
            gridWidth = (int)e.NewValue;
            ResizeGrid();
        }

        private void SliderY_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            YValue.Text = e.NewValue.ToString();
            gridHeight = (int)e.NewValue;
            ResizeGrid();
        }

        private void XValue_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (XValue.Text != "")
                SliderX.Value = Convert.ToDouble(XValue.Text);
        }

        private void YValue_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (YValue.Text != "")
                SliderY.Value = Convert.ToDouble(YValue.Text);
        }

        private void PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            e.Handled = !IsTextAllowed(e.Text);
        }
        private static bool IsTextAllowed(string text)
        {
            Regex regex = new Regex("[^0-9]+");
            return !regex.IsMatch(text);
        }

        private void Generate_Click(object sender, RoutedEventArgs e)
        {
            ClearHeightMap(); 
            Random rnd = new Random();
            int x = rnd.Next(4, 5); 
            for (int i = 0; i < x; i++)
            {
                ProcedurallyGenerate(); 
            }
        }

        private void ClearHeightMap()
        {
            for (int row = 0; row < totalSize; row++)
            {
                for (int column = 0; column < totalSize; column++)
                {
                    heightMapArray[column, row] = 0; 
                }
            }
        }
        private void ProcedurallyGenerate()
        {
            Random rnd = new Random();
            for (int row = 0; row < gridHeight; row++)
            {
                for (int column = 0; column < gridWidth; column++)
                {
                    if (column != 0 && row != 0)
                    {
                        heightMapArray[column, row] += AverageSurroundingHeight(row, column);
                    }
                    float random = ((float)rnd.Next(1, 1000) / 250);
                    if (random < 3.9)
                        random = 0;
                    else
                        random = 10;
                    heightMapArray[column, row] += random;
                    HeightToType(row, column);
                }
            }
        }

        private void HeightToType(int row, int column)
        {
            switch (heightMapArray[column, row])
            {
                case float n when (n <= 10):
                    tileArray[column, row].Fill = Brushes.Blue; 
                    break;
                case float n when (n <= 25 && n > 10):
                    tileArray[column, row].Fill = Brushes.Yellow;
                    break;
                case float n when (n <= 50 && n > 25):
                    tileArray[column, row].Fill = Brushes.Green;
                    break;
                case float n when (n <= 75 && n > 50):
                    tileArray[column, row].Fill = Brushes.DarkGreen;
                    break;
                case float n when (n <= 100 && n > 75):
                    tileArray[column, row].Fill = Brushes.DarkGray;
                    break;
                case float n when (n > 101):
                    tileArray[column, row].Fill = Brushes.OrangeRed;
                    break;
            }
        }
        private float AverageSurroundingHeight(int row, int column)
        {
            float result = 0;

            result += heightMapArray[column + 1, row - 1];
            result += heightMapArray[column + 1, row + 1];
            result += heightMapArray[column - 1, row - 1];
            result += heightMapArray[column - 1, row + 1];
            result += heightMapArray[column - 1, row];
            result += heightMapArray[column + 1, row];
            result += heightMapArray[column, row + 1];
            result += heightMapArray[column, row - 1];

            return (result / 8); 
        }
    }
};

