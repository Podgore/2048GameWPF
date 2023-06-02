using System.Collections.Generic;
using System;
using System.ComponentModel;
using System.Collections;
using System.Runtime.CompilerServices;
using Normal2048.Helper;
using System.Linq;

namespace Normal2048.Models
{
    public class Field : INotifyPropertyChanged, IEnumerable<Cell>
    {
        private readonly Random _random = new();

        public Cell[,] _cells;
        private int score;
        public Cell[,] _previous;
        private Field _field;


        public int Size { get; set; }

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
        public Cell[,] Cells => _cells;

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

        public bool Move(Direction direction /*, bool generateNewTiles = true*/)
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
        public bool CanMove(Direction direction)
        {
            var current = CopyFieldState();

            Move(direction);

            bool canMove = !IsFieldStateEqual(current, _cells);

            _cells = current;

            return canMove;
        }

        private bool IsFieldStateEqual(Cell[,] state1, Cell[,] state2)
        {
            for (int row = 0; row < Size; row++)
            {
                for (int column = 0; column < Size; column++)
                {
                    if (state1[row, column].Value != state2[row, column].Value ||
                        state1[row, column].IsOccupied != state2[row, column].IsOccupied)
                    {
                        return false;
                    }
                }
            }

            return true;
        }

        public Cell[,] CopyFieldState()
        {
            Cell[,] copy = new Cell[Size, Size];
            for (int row = 0; row < Size; row++)
            {
                for (int column = 0; column < Size; column++)
                {
                    copy[row, column] = new Cell(row, column)
                    {
                        Value = _cells[row, column].Value,
                        IsOccupied = _cells[row, column].IsOccupied
                    };
                }
            }
            return copy;
        }

        public bool MoveCell(Cell source, Cell target)
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
        public void ReturnLastMove(Cell[,] _previous)
        {
            for (int row = 0; row < Size; row++)
            {
                for (int column = 0; column < Size; column++)
                {
                    _cells[row, column].Value = _previous[row, column].Value;
                    _cells[row, column].IsOccupied = _previous[row, column].IsOccupied;
                }
            }
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
            if (emptyCells == 0)
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
        public Direction FindBestMove(Field field)
        {
            Direction bestMove = Direction.None;
            int bestScore = int.MinValue;
            Cell[,] originalState = field.CopyFieldState();
            int currentScore = field.Score;
            foreach (Direction direction in Enum.GetValues(typeof(Direction)))
            {
                if (direction != Direction.None)
                {
                    if (field.Move(direction))
                    {


                        int recursiveScore = CalculateScore(field, 0); 

                        if (recursiveScore > bestScore)
                        {
                            bestScore = recursiveScore;
                            bestMove = direction;
                        }

                        field.ReturnLastMove(originalState);
                    }
                }
            }
            field.Score = currentScore;

            return bestMove;
        }

        private int CalculateScore(Field field, int depth)
        {
            if (depth == 0 || field.IsGameOver())
            {
                return ScoreFunction(field);
            }

            int bestScore = int.MinValue;

            foreach (Direction direction in Enum.GetValues(typeof(Direction)))
            {
                if (direction != Direction.None)
                {
                    Field clonedField = field;
                    int currentScore = clonedField.Score;
                    if (clonedField.Move(direction))
                    {

                        int recursiveScore = CalculateScore(clonedField, depth - 1);

                        if (recursiveScore > bestScore)
                        {
                            bestScore = recursiveScore;
                        }

                        clonedField.ReturnLastMove(field.Cells);

                    }
                    clonedField.Score = currentScore;
                    clonedField.ReturnLastMove(field.Cells); 
                }
            }

            return bestScore;
        }
        private int ScoreFunction(Field field)
        {
            int score = 0;

            score += ScoreBasedOnAdjacentTiles(field);

            score += field.Score ;

            score += ScoreBasedOnTilePositions(field);

            return score;
        }

        private int ScoreBasedOnAdjacentTiles(Field field)
        {
            int score = 0;

            for (int row = 0; row < field.Size; row++)
            {
                for (int col = 0; col < field.Size; col++)
                {
                    Cell currentTile = _cells[row, col];

                    if (currentTile != null)
                    {
                        Cell? topTile = null;
                        if (row - 1 >= 0)
                        {
                            topTile = _cells[row - 1, col];
                        }
                        else
                        {
                            topTile = _cells[row + 1, col];
                        }

                        Cell? bottomTile = null;
                        if (row + 1 < 4)
                        {
                            bottomTile = _cells[row + 1, col];
                        }
                        else
                        {
                            bottomTile = _cells[row - 1, col];
                        }

                        Cell? leftTile = null;
                        if (col - 1 >= 0)
                        {
                            leftTile = _cells[row, col - 1];
                        }
                        else
                        {
                            leftTile = _cells[row, col + 1];
                        }

                        Cell? rightTile = null;
                        if (col + 1 < 4)
                        {
                            rightTile = _cells[row, col + 1];
                        }
                        else
                        {
                            rightTile = _cells[row, col - 1];
                        }

                        if (topTile != null && topTile.Value == currentTile.Value && currentTile.Value >= 8)
                        {
                            score += currentTile.Value;
                        }

                        if (bottomTile != null && bottomTile.Value == currentTile.Value && currentTile.Value >= 8)
                        {
                            score += currentTile.Value;
                        }

                        if (leftTile != null && leftTile.Value == currentTile.Value && currentTile.Value >= 8)
                        {
                            score += currentTile.Value;
                        }

                        if (rightTile != null && rightTile.Value == currentTile.Value && currentTile.Value >= 8)
                        {
                            score += currentTile.Value;
                        }
                    }
                }
            }

            return score;
        }


        private int ScoreBasedOnTilePositions(Field field)
        {
            int score = 0;

            for (int row = 0; row < field.Size; row++)
            {
                for (int col = 0; col < field.Size; col++)
                {
                    Cell currentTile = _cells[row, col];

                    if (currentTile != null)
                    {
                        bool isLargestInRow = true;
                        for (int i = col ; i < field.Size; i++)
                        {
                            Cell nextTile = _cells[row, i];
                            if (nextTile != null && nextTile.Value > currentTile.Value)
                            {
                                isLargestInRow = false;
                                break;
                            }
                        }
                        bool isLargestInColumn = true;
                        for (int i = row; i < field.Size; i++)
                        {
                            Cell nextTile = _cells[i, col];
                            if (nextTile != null && nextTile.Value > currentTile.Value)
                            {
                                isLargestInColumn = false;
                                break;
                            }
                        }


                        bool isSequentialInRow = true;
                        for (int i = col - 1; i >= 0; i--)
                        {
                            Cell prevTile = _cells[row, i];
                            if (prevTile != null && prevTile.Value != currentTile.Value - 1)
                            {
                                isSequentialInRow = false;
                                break;
                            }
                        }
                        


                        if (isLargestInRow && isSequentialInRow && isLargestInColumn)
                        {
                            score += 8*currentTile.Value;
                        }
                        else if (isLargestInRow && isSequentialInRow || isSequentialInRow && isLargestInColumn)
                        {
                            score += 4 * currentTile.Value;
                        }
                    }
                }
            }

            return score;
        }



        public event PropertyChangedEventHandler? PropertyChanged;

        public void OnPropertyChanged([CallerMemberName] string? propName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propName));
        }

        public IEnumerator<Cell> GetEnumerator()
        {
            for (int i = 0; i < Size; i++)
            {
                for (int j = 0; j < Size; j++)
                {
                    yield return _cells[i, j];
                }
            }
        }

        IEnumerator IEnumerable.GetEnumerator()
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