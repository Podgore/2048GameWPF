using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Navigation;

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
                            if (MoveCell(_cells[row, column], _cells[row - 1, column]))
                            {
                                moved = true;
                            }
                        }
                    }
                    break;

                case Direction.Down:
                    for (int column = 0; column < Size; column++)
                    {
                        for (int row = Size - 2; row >= 0; row--)
                        {
                            if (MoveCell(_cells[row, column], _cells[row + 1, column]))
                            {
                                moved = true;
                            }
                        }
                    }
                    break;
                case Direction.Left:
                    for (int row = 0; row < Size; row++)
                    {
                        for (int column = 1; column < Size; column++)
                        {
                            if (MoveCell(_cells[row, column], _cells[row, column - 1]))
                            {
                                moved = true;
                            }
                        }
                    }
                    break;

                case Direction.Right:
                    for (int row = 0; row < Size; row++)
                    {
                        for (int column = Size - 2; column >= 0; column--)
                        {
                            if (MoveCell(_cells[row, column], _cells[row, column + 1]))
                            {
                                moved = true;
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
                return MoveFreely(source, target);

            if (source.Value == target.Value && !target.IsEmpty())
                return Merge(source, target);

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

        private bool Merge(Cell source, Cell target)
        {
            target.Value *= 2;
            target.IsOccupied = true;
            source.Value = 0;
            source.IsOccupied = false;
            Score += target.Value;
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
    Right
}