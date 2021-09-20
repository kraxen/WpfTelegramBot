using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
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
using Telegram.Bot;

namespace WpfTelegramBot
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        TelegramBot bot;

        [Obsolete]
        public MainWindow()
        {
            InitializeComponent();

            bot = new TelegramBot(this);

            listView.ItemsSource = bot.messages;
        }

        private void button_Click(object sender, RoutedEventArgs e)
        {
            bot.SendMessage(Convert.ToInt64(userId.Text), textBox.Text);
        }
    }
}
