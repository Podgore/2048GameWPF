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

namespace Normal2048
{

    public partial class MainWindow : Window
    {
        private readonly Field _field;
        private readonly List<Grid> _grids;
        private readonly Dictionary<Cell, TextBlock> _cellTextBlockMap;

        public MainWindow()
        {
            InitializeComponent();
            _field = new Field(4);
            _grids = GameGrid.Children
                .OfType<Grid>()
                .Cast<Grid>()
                .ToList();
            var cells = _field.ToList();

            foreach (var grid in _grids)
            {
                grid.KeyDown += MainWindow_KeyDown;
                TextBlock element = new TextBlock()
                {
                    Foreground = Brushes.Black,
                    FontSize = 15
                };
                element.Text = (_grids.IndexOf(grid) + 1).ToString();
                grid.Children.Add(element);
            }
            foreach (var cell in cells)
            {
                cell.PropertyChanged += Cell_PropertyChanged;
            }

            _cellTextBlockMap = cells.Zip(_grids).ToDictionary(
                tuple => tuple.First,
                tuple => tuple.Second.Children.Cast<TextBlock>().First());
        }

        private void MainWindow_KeyDown(object sender, KeyEventArgs e)
        {
            var direction = e.Key switch
            {
                Key.Down or Key.S => Direction.Down,
                Key.Up or Key.W => Direction.Up,
                Key.Right or Key.D => Direction.Right,
                Key.Left or Key.A => Direction.Left
            };
            _field.Move(direction);
        }


        private void Cell_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            var cell = (Cell)sender;
            var textBlock = _cellTextBlockMap[cell];
            textBlock.Text = cell.Value.ToString();
        }
    }
}
