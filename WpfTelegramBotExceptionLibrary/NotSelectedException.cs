using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfTelegramBotExceptionLibrary
{
    public class NotSelectedException : Exception
    {
        /// <summary>
        /// Ошибка, которая говорит, что не выбран элемент
        /// </summary>
        /// <param name="Msg">Имя элемента</param>
        public NotSelectedException(string Msg)
            :base($"Не выбран {Msg}")
        {
        }
    }
}
