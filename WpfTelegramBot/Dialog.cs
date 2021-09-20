using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;
using System.Collections.ObjectModel;

namespace WpfTelegramBot
{
    /// <summary>
    /// Диалог с пользователем
    /// </summary>
    public class Dialog: INotifyPropertyChanged
    {
        /// <summary>
        /// Коллекция сообщений
        /// </summary>
        ObservableCollection<TelegramMessage> messages;
        /// <summary>
        /// Коллекция сообщений
        /// </summary>
        public ObservableCollection<TelegramMessage> Messages 
        { 
            get => this.messages;
            set 
            { 
                this.messages = value; 
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(this.Messages)));
            }
        }
        /// <summary>
        /// id диалога
        /// </summary>
        long id;

        public event PropertyChangedEventHandler PropertyChanged;

        public long Id { get { return this.id; } }
        /// <summary>
        /// Имя пользователя
        /// </summary>
        public string UserName { get; set; }
        /// <summary>
        /// Добавление диалога
        /// </summary>
        /// <param name="id">Id диалога</param>
        public Dialog(long id, string userName)
        {
            this.id = id;
            this.UserName = userName;
            Messages = new ObservableCollection<TelegramMessage>();
        }
    }
}
