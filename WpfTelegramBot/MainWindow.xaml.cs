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
using WpfTelegramBotExceptionLibrary;

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

            //bot = new TelegramBot(this, new ObservableCollection<Dialog>());  // Создание нового экзепляра бота
            bot = new TelegramBot(this);                                        // Создание бота по сохранененой истории сообщений

            lvDialigues.ItemsSource = bot.dialogues;
        }
        /// <summary>
        /// Кнопка отправки сообщения
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="a"></param>
        public void button_Click(object sender, RoutedEventArgs a)
        {
            try
            {
                if (lvDialigues.SelectedItem == null) throw new NotSelectedException("Диалог");
                bot.SendMessage(TelegramBot.thisDialog.Id, tbSendMessage.Text);
                tbSendMessage.Text = "";
            }
            catch(Exception e) 
            {
                MessageBox.Show($"{e.Message}\n\n{e.GetType()}","Произошла ошибка",MessageBoxButton.OK,MessageBoxImage.Error);
            }
            
        }
        /// <summary>
        /// Действие при выделении диалога
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void lvDialigues_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            TelegramBot.thisDialog = lvDialigues.SelectedItem as Dialog;
            lvMesseges.ItemsSource = TelegramBot.thisDialog.Messages;
        }
    }
}
