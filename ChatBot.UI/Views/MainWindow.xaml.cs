using System.Windows;
using ChatBot.UI.ViewModels;

namespace ChatBot.UI.Views
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            DataContext = new MainViewModel();
        }

        private void SendMessage_Click(object sender, RoutedEventArgs e)
        {
            if (DataContext is MainViewModel vm)
            {
                vm.SendMessage();
            }
        }
    }
}