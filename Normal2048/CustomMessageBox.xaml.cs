using System.ComponentModel;
using System.Windows;

namespace Normal2048
{
    public partial class CustomMessageBox : Window, INotifyPropertyChanged
    {
        private string _message;

        public event PropertyChangedEventHandler? PropertyChanged;

        public string Message
        {
            get { return _message; }
            set
            {
                _message = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("Message"));
            }
        }

        public CustomMessageBox(string message)
        {
            InitializeComponent();
            DataContext = this;
            Message = message;
        }

        private void NewGameButton_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = true;
        }

        private void ExitButton_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
        }
    }
}
