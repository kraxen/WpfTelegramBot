using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfTelegramBot
{
    /// <summary>
    /// Сообщение пользователя в телеграме
    /// </summary>
    public class TelegramMessage
    {
        /// <summary>
        /// Id пользователя
        /// </summary>
        public long id { get; }
        /// <summary>
        /// Имя пользователя
        /// </summary>
        public string userName { get; }
        /// <summary>
        /// Сообщение пользователя
        /// </summary>
        public string messageText { get; }
        /// <summary>
        /// Время отправки сообщения
        /// </summary>
        public string messageTime { get; }
        /// <summary>
        /// Создание сообщения пользователю
        /// </summary>
        /// <param name="id">Id пользователя</param>
        /// <param name="userName">Имя пользователя</param>
        /// <param name="messageText">Сообщение пользователя</param>
        /// <param name="messageTime">Время отправки сообщения</param>
        public TelegramMessage(long id, string userName, string messageText, string messageTime)
        {
            this.id = id;
            this.messageTime = messageTime;
            this.userName = userName;
            this.messageText = messageText;
        }
    }
}
