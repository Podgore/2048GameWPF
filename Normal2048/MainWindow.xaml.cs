using System.Windows;
using System.Windows.Input;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;
using System.Reflection;
using System.Windows.Ink;
using System.Collections.Generic;
using System;
using System.Windows.Media.Animation;
using Normal2048.Models;
using System.Linq;
using System.ComponentModel;
using System.Data.Common;
using System.Xml.Linq;
using System.Collections;
using static System.Net.Mime.MediaTypeNames;

namespace Normal2048
{

    public partial class MainWindow : Window
    {
        private Stack<Field> _undoStack = new Stack<Field>();
        private /*readonly*/ Field _field;
        private /*readonly*/ List<Grid> _grids;
        private /*readonly*/ Dictionary<Cell, TextBlock?> _cellTextBlockMap = new Dictionary<Cell, TextBlock?>();
        private Dictionary<Cell, Rectangle> _cellRectangleMap = new Dictionary<Cell, Rectangle>();

        public MainWindow()
        {
            InitializeComponent();
            _field = new Field(4);
            _grids = GameGrid.Children
                .OfType<Grid>()
                .Cast<Grid>()
                .ToList();
            var cells = _field.ToList();

            int i = 0;

            var grid = _grids[i];
            for (int row = 0; row < _field.Size; row++)
            {
                

                for (int column = 0; column < _field.Size; column++)
                {
                    var cell = _field.GetCell(row, column);
                    grid = _grids[i++];
                    AddTileToGrid(grid, cell);
                }
            }

            _cellTextBlockMap = cells.Zip(_grids).ToDictionary(
           tuple => tuple.First,
           tuple => tuple.Second.Children.OfType<TextBlock>().FirstOrDefault());

            foreach (var cell in cells)
            {
                cell.PropertyChanged += Cell_PropertyChanged!;
            }
            KeyDown += MainWindow_KeyDown;
        }

        private void MainWindow_KeyDown(object sender, KeyEventArgs e)
        {
            var direction = e.Key switch
            {
                Key.Down or Key.S => Direction.Down,
                Key.Up or Key.W => Direction.Up,
                Key.Right or Key.D => Direction.Right,
                Key.Left or Key.A => Direction.Left,
                _ => Direction.None
            };

            if (direction != Direction.None)
            {
                _field.Move(direction);
                if (_field.IsGameOver())
                {
                    
                    MessageBox.Show("You lost( Better luck next time");
                    System.Windows.Application.Current.Shutdown();
                }
            }
        }


        private void Cell_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            var cell = (Cell)sender;
            
            var textBlock = _cellTextBlockMap[cell];

   
            textBlock!.Text = cell.Value == 0 ? "" : cell.Value.ToString();

            var rectangle = _cellRectangleMap[cell];

            rectangle.Fill = GetTileColor(cell.Value);

            CalculateScore();

            if (cell.Value == 2048)
            {
                MessageBox.Show("Congrats!!! You won!");
                System.Windows.Application.Current.Shutdown();
            }
            
            
        }
        public void CalculateScore()
        {
            int score = _field.Score;
            ScoreTextBlock.Text = $"Score: {score}";
        }

        private Brush GetTileColor(int value)
        {
            return value switch
            {
                2 => Brushes.LightYellow,
                4 => Brushes.Yellow,
                8 => Brushes.Orange,
                16 => Brushes.OrangeRed,
                32 => Brushes.Red,
                64 => Brushes.DarkRed,
                128 => Brushes.LightGreen,
                256 => Brushes.Green,
                512 => Brushes.DarkGreen,
                1024 => Brushes.LightBlue,
                2048 => Brushes.Blue,
                0 => Brushes.WhiteSmoke,
                _ => throw new NotImplementedException(),
            };
        }

        private void AddTileToGrid(Grid grid, Cell cell)
        {
            
            Rectangle rectangle = new Rectangle()
            {
                Fill = GetTileColor(cell.Value),         
            };

            Grid.SetRow(rectangle, cell.Row);
            Grid.SetColumn(rectangle, cell.Column);

            grid.Children.Add(rectangle);
            _cellRectangleMap[cell] = rectangle;

            
            TextBlock element = new TextBlock()
            {
                Foreground = Brushes.Black,
                FontSize = 40,
                HorizontalAlignment = HorizontalAlignment.Center,
                VerticalAlignment = VerticalAlignment.Center,
                FontWeight = FontWeights.UltraBlack
            };

            if (cell.Value != 0)
            {
                element.Text = cell.Value.ToString();
            }

            Grid.SetRow(element, cell.Row);
            Grid.SetColumn(element, cell.Column);

            grid.Children.Add(element);

            _cellTextBlockMap[cell] = element;
            
        }
        private void NewGame_Click(object sender, RoutedEventArgs e)
        {
            string exePath = Environment.ProcessPath!;

            System.Diagnostics.Process.Start(exePath);
            System.Windows.Application.Current.Shutdown();
        }

        private Direction GetOppositeDirection(Direction direction)
        {
            return direction switch
            {
                Direction.Up => Direction.Down,
                Direction.Down => Direction.Up,
                Direction.Left => Direction.Right,
                Direction.Right => Direction.Left,
                _ => Direction.None
            };
        }

        private void UndoMove_Click(object sender, RoutedEventArgs e)
        {
            
        }

    } 
}
