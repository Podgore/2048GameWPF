using System;
using System.ComponentModel;

namespace Normal2048.Models
{
    public class Cell : INotifyPropertyChanged
    {
        private int _value;
        private bool _isOccupied;
        private int _row;
        private int _column;


        public int Row
        {
            get => _row;
            set
            {
                _row = value;
                OnPropertyChanged(nameof(Row));
            }
        }

        public int Column
        {
            get => _column;
            set
            {
                _column = value;
                OnPropertyChanged(nameof(Column));
            }
        }

        public int Value
        {
            get => _value;
            set
            {
                _value = value;
                OnPropertyChanged(nameof(Value));
            }
        }

        public bool IsOccupied
        {
            get => _isOccupied;
            set
            {
                _isOccupied = value;
                OnPropertyChanged(nameof(IsOccupied));
            }
        }



        public Cell(int row, int column)
        {
            Row = row;
            Column = column;
        }

        public bool IsEmpty()
        {
            return !IsOccupied;
        }

        public event PropertyChangedEventHandler? PropertyChanged;

        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public override bool Equals(object? obj)
        {
            return obj is Cell cell &&
                   Row == cell.Row &&
                   Column == cell.Column &&
                   Value == cell.Value &&
                   IsOccupied == cell.IsOccupied
                   ;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Row, Column);
        }

        public override string ToString()
        {
            return $"{Value} at ({Row}:{Column})";
        }

    }
}
