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
using System.Runtime.CompilerServices;

namespace Normal2048.Models
{
    public class Field : INotifyPropertyChanged, IEnumerable<Cell>
    {
        private readonly Random _random = new();

        private readonly Cell[,] _cells;
        private int score;

        public int Size { get; }

        public int Score
        {
            get => score;
            set
            {
                if (value != score)
                {
                    score = value;
                    OnPropertyChanged();
                }
            }
        }
        public Cell GetCell(int row, int column)
        {
            return _cells[row, column];
        }

        public Field(int size)
        {
            Size = size;
            _cells = new Cell[Size, Size];
            for (int row = 0; row < Size; row++)
            {
                for (int column = 0; column < Size; column++)
                {
                    _cells[row, column] = new Cell(row, column);
                }
            }

            AddRandomValue();
            AddRandomValue();
        }

        private void AddRandomValue()
        {
            int emptyCells = 0;
            foreach (var cell in _cells)
            {
                if (cell.IsEmpty())
                {
                    emptyCells++;
                }
            }

            if (emptyCells == 0)
            {
                return;
            }

            int value = _random.Next(0, 10) == 0 ? 4 : 2;
            int index = _random.Next(0, emptyCells);
            emptyCells = 0;

            foreach (var cell in _cells)
            {
                if (cell.IsEmpty())
                {
                    if (emptyCells == index)
                    {
                        cell.Value = value;
                        cell.IsOccupied = true;
                        return;
                    }

                    emptyCells++;
                }
            }
        }

        public bool Move(Direction direction)
        {
            bool moved = false;

            switch (direction)
            {
                case Direction.Up:
                    for (int column = 0; column < Size; column++)
                    {
                        for (int row = 1; row < Size; row++)
                        {
                            int currentRow = row;
                            while (currentRow > 0 && MoveCell(_cells[currentRow, column], _cells[currentRow - 1, column]))
                            {
                                moved = true;
                                currentRow--;
                            }
                        }
                    }
                    break;

                case Direction.Down:
                    for (int column = 0; column < Size; column++)
                    {
                        for (int row = Size - 2; row >= 0; row--)
                        {
                            int currentRow = row;
                            while (currentRow < Size - 1 && MoveCell(_cells[currentRow, column], _cells[currentRow + 1, column]))
                            {
                                moved = true;
                                currentRow++;
                            }
                        }
                    }
                    break;
                case Direction.Left:
                    for (int row = 0; row < Size; row++)
                    {
                        for (int column = 1; column < Size; column++)
                        {
                            int currentColumn = column;
                            while (currentColumn > 0 && MoveCell(_cells[row, currentColumn], _cells[row, currentColumn - 1]))
                            {
                                moved = true;
                                currentColumn--;
                            }
                        }
                    }
                    break;

                case Direction.Right:
                    for (int row = 0; row < Size; row++)
                    {
                        for (int column = Size - 2; column >= 0; column--)
                        {
                            int currentColumn = column;
                            while (currentColumn < Size - 1 && MoveCell(_cells[row, currentColumn], _cells[row, currentColumn + 1]))
                            {
                                moved = true;
                                currentColumn++;
                            }
                        }
                    }
                    break;
            }

            if (moved)
            {
                AddRandomValue();
            }

            return moved;

        }



        private bool MoveCell(Cell source, Cell target)
        {
            if (source.IsEmpty())
                return false;

            if (target.IsEmpty())
            {    
                MoveFreely(source, target);
                return true;
            }

            if (source.Value == target.Value && !target.IsEmpty())
            {
                
                Merge(source, target);
                return true;
            }

            return false;
        }

        private static bool MoveFreely(Cell source, Cell target)
        {
            target.Value = source.Value;
            source.Value = 0;
            target.IsOccupied = true;
            source.IsOccupied = false;
            return true;
        }

        private void Merge(Cell source, Cell target)
        {
            target.Value *= 2;
            target.IsOccupied = true;
            source.Value = 0;
            source.IsOccupied = false;
            Score += target.Value;
            
        }
        public bool IsGameOver()
        {
            
            int emptyCells = 0;
            foreach (var cell in _cells)
            {
                if (cell.IsEmpty())
                {
                    emptyCells++;
                    if (emptyCells >= 1)
                    {
                        return false;
                    }
                }
            }
            if(emptyCells == 0)
            {
                for (int row = 0; row < Size; row++)
                {
                    for (int column = 0; column < Size; column++)
                    {
                        var currentCell = _cells[row, column];

                        if (row > 0 && currentCell.Value == _cells[row - 1, column].Value)
                        {
                            return false;
                        }

                        if (row < Size - 1 && currentCell.Value == _cells[row + 1, column].Value)
                        {
                            return false;
                        }

                        if (column > 0 && currentCell.Value == _cells[row, column - 1].Value)
                        {
                            return false;
                        }

                        if (column < Size - 1 && currentCell.Value == _cells[row, column + 1].Value)
                        {
                            return false;
                        }
                    }
                }
            }
            return true;
        }

        public event PropertyChangedEventHandler? PropertyChanged;

        public void OnPropertyChanged([CallerMemberName] string? propName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propName));
        }

        IEnumerator<Cell> IEnumerable<Cell>.GetEnumerator()
        {
            for (int i = 0; i < Size; i++)
            {
                for (int j = 0; j < Size; j++)
                {
                    yield return _cells[i, j];
                }
            }
        }

        public IEnumerator GetEnumerator()
        {
            return GetEnumerator();
        }

    }
}


public enum Direction
{
    Up,
    Down,
    Left,
    Right,
    None
}