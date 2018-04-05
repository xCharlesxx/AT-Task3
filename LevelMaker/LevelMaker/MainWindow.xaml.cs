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
using System.IO;
using System.Text.RegularExpressions;
using System.Windows.Threading; 

namespace LevelMaker
{
    public partial class MainWindow : Window
    {
        static int totalSize = 75;
        static int minHeight = 475;
        static int minWidth = 150;
        static int primStartX;
        static int primStartY;
        int gridHeight = 50;
        int gridWidth = 50;
        bool leftButtonDown = false;
        bool rightButtonDown = false;
        int tileSize = 10;
        static string green = Brushes.Green.ToString();
        Rectangle[,] tileArray = new Rectangle[totalSize, totalSize];
        float[,] heightMapArray = new float[totalSize, totalSize];
        List<Rectangle> wallList = new List<Rectangle>();
        SolidColorBrush wallColour = Brushes.Black;
        SolidColorBrush corridorColour = Brushes.White;
        SolidColorBrush Lava = Brushes.OrangeRed;
        SolidColorBrush Tresure = Brushes.Yellow;
        SolidColorBrush Doors = Brushes.Brown;
        SolidColorBrush FillColour = Brushes.Blue;
        public MainWindow()
        {
            primStartX = gridWidth / 2;
            primStartY = gridHeight / 2;
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
                        tileArray[column, row].IsEnabled = false;
                        tileArray[column, row].Visibility = Visibility.Hidden;
                    }
                    else
                    {
                        tileArray[column, row].IsEnabled = true;
                        tileArray[column, row].Visibility = Visibility.Visible;
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
            primStartX = gridWidth / 2;
            primStartY = gridHeight / 2;
        }

        private void GenerateGrid()
        {
            for (int row = 0; row < totalSize; row++)
            {
                for (int column = 0; column < totalSize; column++)
                {
                    tileArray[column, row] = new Rectangle()
                    {
                        Width = tileSize,
                        Height = tileSize,
                        Fill = wallColour,
                        Stroke = Brushes.Gray,
                        StrokeThickness = 0,
                    };
                    tileArray[column, row].MouseEnter += rect_MouseButtonHeldDown;
                    tileArray[column, row].MouseLeftButtonDown += rect_MouseLeftButtonDown;
                    tileArray[column, row].MouseRightButtonDown += rect_MouseRightButtonDown;
                    canvas.Children.Add(tileArray[column, row]);
                    Canvas.SetLeft(tileArray[column, row], tileSize * column);
                    Canvas.SetTop(tileArray[column, row], tileSize * row);
                }
            }
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
            }
            MessageBox.Show("File Saved Successfully");
        }

        private char[] ConvertToTxt()
        {
            char[] array = new char[gridHeight * (gridWidth)];
            for (int row = 0; row < gridHeight; row++)
            {
                for (int column = 0; column < gridWidth; column++)
                {
                    if (tileArray[column, row].Fill == wallColour)
                        array[column + (row * gridWidth)] = '0';

                    else if (tileArray[column, row].Fill == corridorColour || tileArray[column, row].Fill == Brushes.Blue)
                        array[column + (row * gridWidth)] = '1';

                    else if (tileArray[column, row].Fill == Lava)
                        array[column + (row * gridWidth)] = 'L';

                    else if (tileArray[column, row].Fill == Doors)
                        array[column + (row * gridWidth)] = 'D';

                    else if (tileArray[column, row].Fill == Tresure)
                        array[column + (row * gridWidth)] = 'T';

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

            //Get height and width of file 
            string[] checkSize = File.ReadAllLines(filename);
            gridWidth = checkSize[0].Length;
            gridHeight = checkSize.Length;
            XValue.Text = gridWidth.ToString();
            YValue.Text = gridHeight.ToString();

            ResizeGrid();
            string readText = File.ReadAllText(filename);
            char[] charArray = readText.ToCharArray();

            int i = 0;
            for (int row = 0; row < gridHeight; row++)
            {
                for (int column = 0; column < gridWidth; column++)
                {
                    switch (charArray[i])
                    {
                        case '0':
                            tileArray[column, row].Fill = wallColour;
                            break;
                        case '1':
                        case '2':
                        case '3':
                        case '4':
                        case '5':
                        case '6':
                        case '7':
                        case '8':
                        case '9':
                            tileArray[column, row].Fill = corridorColour;
                            break;
                        case 'L':
                            tileArray[column, row].Fill = Lava;
                            break;
                        case 'D':
                            tileArray[column, row].Fill = Doors;
                            break;
                        case 'T':
                            tileArray[column, row].Fill = Tresure;
                            break;
                        case '\r':
                            i += 2;
                            break;
                    }
                    i++;
                    if (row == gridHeight - 1 || column == gridWidth - 1 || column == 0 || row == 0)
                    {
                        tileArray[column, row].Fill = wallColour;
                    }
                }
            }
        }

        private static bool IsTextAllowed(string text)
        {
            Regex regex = new Regex("[^0-9]+");
            return !regex.IsMatch(text);
        }

        //Generate Height Map
        //ClearHeightMap();
        //Random rnd = new Random();
        //int x = rnd.Next(4, 5); 
        //    for (int i = 0; i<x; i++)
        //        ProcedurallyGenerate();

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

        //Create heightMap
        private void ProcedurallyGenerate()
        {
            Random rnd = new Random();
            for (int row = 0; row < gridHeight; row++)
            {
                for (int column = 0; column < gridWidth; column++)
                {
                    //If not on boundries of map
                    if (column != 0 && row != 0 && column != totalSize - 1 && row != totalSize - 1)
                    {
                        heightMapArray[column, row] += AverageSurroundingHeight(column, row);
                    }
                    //Get random float between 0.001 and 1
                    float random = ((float)rnd.Next(1, 1000) / 1000);
                    //If float not in 99th percentile
                    if (random < 0.99)
                        random = 0;
                    else
                        random = 10;

                    heightMapArray[column, row] += random;
                    HeightToType(column, row);
                }
            }
        }

        private void HeightToType(int column, int row)
        {
            //Depending on height change tile type
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
    

        private float AverageSurroundingHeight(int column, int row)
        {
            //Average surrounding tiles height to create smooth inclines
            float result = 0;
            for (int dRow = -1; dRow <= 1; ++dRow)
                for (int dCol = -1; dCol <= 1; ++dCol)
                    if (dCol != 0 || dRow != 0)
                        result += heightMapArray[column + dCol, row + dRow];

            return (result / 8); 
        }

        private void Prims()
        {
            AllWalls();
            wallList.Clear(); 
            tileArray[primStartX, primStartY].Fill = corridorColour;
            GetWalls(primStartX, primStartY);
            Random rand = new Random();
            while (wallList.Count != 0)
            {
                int rndm = rand.Next(0, wallList.Count);
                Rectangle currentWall = wallList[rndm];
                wallList.RemoveAt(rndm);
                currentWall.Fill = Brushes.Red;
                ProcessUITasks();
                for (int row = 0; row < gridHeight; row++)
                {
                    for (int column = 0; column < gridWidth; column++)
                    {
                        if (tileArray[column, row] == currentWall)
                        {
                            if (row != 0 && row != gridHeight - 1 && column != gridWidth - 1 && column != 0)
                                if (numWalls(column, row) >= 6)
                                {
                                    tileArray[column, row].Fill = corridorColour;
                                    GetWalls(column, row);
                                    break;
                                }
                            currentWall.Fill = wallColour;
                        }
                    }
                }
            }
        }

        private int numWalls(int column, int row)
        {
            int num = 0;
            for (int dRow = -1; dRow <= 1; ++dRow)
                for (int dCol = -1; dCol <= 1; ++dCol)
                    if (dRow != 0 || dCol != 0)
                        if (row + dRow < gridHeight && row + dRow > -1 && column + dCol < gridWidth && column + dCol > -1)
                            if (tileArray[column + dCol, row + dRow].Fill == wallColour)
                                num++;
            return num; 
        }

        private int numConnections(int column, int row)
        {
            int num = 0;
            for (int dRow = -1; dRow <= 1; ++dRow)
                for (int dCol = -1; dCol <= 1; ++dCol)
                    if (dRow == 0 ^ dCol == 0)
                        if (row + dRow < gridHeight && row + dRow > -1 && column + dCol < gridWidth && column + dCol > -1)
                            if (tileArray[column + dCol, row + dRow].Fill != wallColour)
                                num++;
            return num;
        }

        private void GetWalls(int column, int row)
        {
            for (int dRow = -1; dRow <= 1; ++dRow)
                for (int dCol = -1; dCol <= 1; ++dCol)
                    if (dRow == 0 || dCol == 0)
                        if (row + dRow < gridHeight && row + dRow > -1 && column + dCol < gridWidth && column + dCol > -1)
                            if (tileArray[column + dCol, row + dRow].Fill == wallColour)
                                if (!wallList.Contains<Rectangle>(tileArray[column + dCol, row + dRow]))
                                    wallList.Add(tileArray[column + dCol, row + dRow]);
        }

        private void AllWalls()
        {
            for (int row = 0; row < gridHeight; row++)
            {
                for (int column = 0; column < gridWidth; column++)
                {
                    tileArray[column, row].Fill = wallColour;
                }
            }
        }

        private void FloodFiller()
        {
            ReFlood(); 
            tileArray[primStartX, primStartY].Fill = corridorColour; 
            for (int row = 0; row < gridHeight; row++)
            {
                for (int column = 0; column < gridWidth; column++)
                {
                    if (row == gridHeight - 1 || column == gridWidth - 1 || column == 0 || row == 0)
                    {
                        tileArray[column, row].Fill = wallColour;
                    }
                }
            }
            Flood(primStartX, primStartY, corridorColour, FillColour);
        }

        private void Flood(int col, int row, SolidColorBrush targetColour, SolidColorBrush replaceColour)
        {
            ProcessUITasks();
            if (targetColour == replaceColour)
                return;
            if (tileArray[col, row].Fill != targetColour)
                return;
            tileArray[col, row].Fill = replaceColour;

            Flood(col, row + 1, targetColour, replaceColour);
            Flood(col - 1, row, targetColour, replaceColour);
            Flood(col + 1, row, targetColour, replaceColour);
            Flood(col, row - 1, targetColour, replaceColour);

            return; 
        }

        private void ReFlood()
        {
            for (int row = 0; row < gridHeight; row++)
                for (int column = 0; column < gridWidth; column++)
                    if (tileArray[column, row].Fill == Brushes.Blue)
                        tileArray[column, row].Fill = corridorColour;
        }

        private void Detail()
        {
            ReFlood(); 
            //PreProcessing
            for (int row = 0; row < gridHeight; row++)
            {
                for (int column = 0; column < gridWidth; column++)
                {
                    //Tresure 
                    if (tileArray[column, row].Fill == corridorColour && numWalls(column, row) == 7)
                        tileArray[column, row].Fill = Tresure;
                    //Lava 
                    if (tileArray[column, row].Fill == wallColour && numWalls(column, row) > 6
                        && column > 2 && column < gridWidth - 3 && row > 2 && row < gridHeight - 3)
                        tileArray[column, row].Fill = Brushes.Red;
                    //Doors 
                    if (tileArray[column, row].Fill == corridorColour && numWalls(column, row) == 2)
                        tileArray[column, row].Fill = Doors;

                }
            }
            //PostProcessing
            for (int row = 0; row < gridHeight; row++)
            {
                for (int column = 0; column < gridWidth; column++)
                {
                    if (tileArray[column, row].Fill == Brushes.Red)
                        for (int dRow = -1; dRow <= 1; ++dRow)
                            for (int dCol = -1; dCol <= 1; ++dCol)
                                if (tileArray[column + dCol, row + dRow].Fill == wallColour || tileArray[column + dCol, row + dRow].Fill == Brushes.Red)
                                    tileArray[column + dCol, row + dRow].Fill = Lava;
                }
            }
        }

        private void Rooms()
        {
            FloodFiller();
            for (int row = 0; row < gridHeight; row++)
            {
                for (int column = 0; column < gridWidth; column++)
                {
                    if ((tileArray[column, row].Fill == wallColour || tileArray[column, row].Fill == Doors)
                        && row != gridHeight - 1 && column != gridWidth - 1 && column != 0 && row != 0)
                    {
                        bool del = true;
                        for (int dRow = -1; dRow <= 1; ++dRow)
                            for (int dCol = -1; dCol <= 1; ++dCol)
                                if (tileArray[column + dCol, row + dRow].Fill == FillColour
                                    && row + dRow != gridHeight - 1 && column + dCol != gridWidth - 1 && column + dCol != 0 && row + dRow != 0)
                                    del = false;

                        if (del)
                            tileArray[column, row].Fill = corridorColour;
                    }
                }
            }
        }

        //Buttons & other UI tasks

        public static void ProcessUITasks()
        {
            DispatcherFrame frame = new DispatcherFrame();
            Dispatcher.CurrentDispatcher.BeginInvoke(DispatcherPriority.Background, new DispatcherOperationCallback(delegate (object parameter) {
                frame.Continue = false;
                return null;
            }), null);
            Dispatcher.PushFrame(frame);
        }

        private void MakeRooms_Click(object sender, RoutedEventArgs e)
        {
            Rooms();
        }

        private void CheckBox_Checked(object sender, RoutedEventArgs e)
        {
            for (int row = 0; row < totalSize; row++)
            {
                for (int column = 0; column < totalSize; column++)
                {
                    if (CheckLines.IsChecked == true)
                        tileArray[column, row].StrokeThickness = 1;
                    else
                        tileArray[column, row].StrokeThickness = 0;
                }
            }
        }

        private void AddDetail_Click(object sender, RoutedEventArgs e)
        {
            Detail();
        }

        private void FloodFill_Click(object sender, RoutedEventArgs e)
        {
            FloodFiller();
        }

        private void PrimsButton_Click(object sender, RoutedEventArgs e)
        {
            Prims();
        }

        private void Generate_Click(object sender, RoutedEventArgs e)
        {
            Prims();
            Detail();
            Rooms();
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

        private void rect_MouseButtonHeldDown(object sender, MouseEventArgs e)
        {
            if (System.Windows.Input.Mouse.LeftButton == MouseButtonState.Released)
                leftButtonDown = false;
            if (System.Windows.Input.Mouse.RightButton == MouseButtonState.Released)
                rightButtonDown = false;

            if (leftButtonDown)
                rect_MouseLeftButtonDown(sender, e);
            else if (rightButtonDown)
                ((Rectangle)sender).Fill = corridorColour;
        }

        private void rect_MouseRightButtonDown(object sender, MouseEventArgs e)
        {
            ((Rectangle)sender).Fill = corridorColour;
        }

        private void rect_MouseLeftButtonDown(object sender, MouseEventArgs e)
        {
            switch (TileType.SelectionBoxItem.ToString())
            {
                case "Treasure":
                    ((Rectangle)sender).Fill = Tresure;
                    break;
                case "Wall":
                    ((Rectangle)sender).Fill = wallColour;
                    break;
                case "Lava":
                    ((Rectangle)sender).Fill = Lava;
                    break;
                case "Door":
                    ((Rectangle)sender).Fill = Doors;
                    break;
                case "Empty":
                    ((Rectangle)sender).Fill = corridorColour;
                    break;
            }
        }

        private void button_Click(object sender, RoutedEventArgs e)
        {
            PrintToTxt(ConvertToTxt());
        }
    }
};

