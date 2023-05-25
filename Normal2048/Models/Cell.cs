using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data.Common;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace Normal2048.Models
{
    public class Cell : INotifyPropertyChanged
    {
        private int _value;
        private bool isOccupied;
        private int row;
        private int column;


        public int Row
        {
            get => row;
            set
            {
                row = value;
                OnPropertyChanged(nameof(Row));
            }
        }

        public int Column
        {
            get => column;
            set
            {
                column = value;
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
            get => isOccupied;
            set
            {
                isOccupied = value;
                OnPropertyChanged(nameof(IsOccupied));
            }
        }
        public bool IsMerged 
        { 
            get; 
            set; 
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
