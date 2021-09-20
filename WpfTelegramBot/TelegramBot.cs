using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using Telegram.Bot;
using static WpfTelegramBot.AsyncMethods;

namespace WpfTelegramBot
{
    public class TelegramBot
    {
        private TelegramBotClient bot;
        private string token;
        private MainWindow w;
        //public ObservableCollection<TelegramMessage> messages { get; set; }
        public ObservableCollection<Dialog> dialogues { get; set; }
        public static Dialog thisDialog;

        static string FAQstr = "Данный бот представляет из себя подобие облачного хранилища.\n " +
            "Вы можете отправлять файлы и они будут загружены на сервер.\n" +
            "Командой: \"Скаченные файлы\" можно получить список сохраненных файлов\n" +
            "Командой \"Скачать файл: имя_файла\"  необходимого файла можно скачать файл";
        /// <summary>
        /// Создание телеграм бота на основе окна и коллекции сообщений
        /// </summary>
        /// <param name="w"></param>
        /// <param name="dialogues"></param>
        [Obsolete]
        public TelegramBot(MainWindow w, ObservableCollection<Dialog> dialogues)
        {
            try
            {
                token = File.ReadAllText($@"token.txt");
                this.dialogues = dialogues;
                this.w = w;
                bot = new TelegramBotClient(token);
                bot.OnMessage += Bot_OnMessage;
                bot.StartReceiving();
            }
            catch(Exception e)
            {
                MessageBox.Show($"{e.Message}\n\n{ e.GetType()}");
            }
            
        }
        /// <summary>
        /// Создание телеграм бота на основе предыдущих сообщений
        /// </summary>
        /// <param name="w"></param>
        [Obsolete]
        public TelegramBot(MainWindow w)
            :this(w, new ObservableCollection<Dialog>())
        {
            SaveAsFile.GetObservableCollectionDialogIntoFile(this);
            w.lvDialigues.Items.Refresh();
        }

        [Obsolete]
        private void Bot_OnMessage(object sender, Telegram.Bot.Args.MessageEventArgs e)
        {
            // Показать сообщение пользователя или его тип
            string date = DateTime.Now.ToLongTimeString();
            long id = e.Message.Chat.Id;
            string name = e.Message.From.Username;
            if(!this.dialogues.Any(d => d.Id == e.Message.Chat.Id))
            {
                thisDialog = new Dialog(e.Message.Chat.Id, e.Message.From.Username);
                w.Dispatcher.Invoke(() => this.dialogues.Add(thisDialog));
            }
            else
            {
                thisDialog = this.dialogues.First(d => d.Id == e.Message.Chat.Id);
            }

            if (e.Message.Type == Telegram.Bot.Types.Enums.MessageType.Text)
            {
                w.Dispatcher.Invoke(() =>
                thisDialog.Messages.Add(new TelegramMessage(id, name, e.Message.Text, date))
                );
                DoIfTextMessage(e);
            }
            else
            // Сохранение голосовых сообщений
            if (e.Message.Type == Telegram.Bot.Types.Enums.MessageType.Voice)
            {
                SaveVoiceMessage(e, bot);
            }
            else
            // Сохранение картинок
            if (e.Message.Type == Telegram.Bot.Types.Enums.MessageType.Photo)
            {
                SavePhotoMessage(e, bot);
            }
            else
            // Сохранение документов
            if (e.Message.Type == Telegram.Bot.Types.Enums.MessageType.Document)
            {
                SaveDocumentMessage(e, bot);
            }
            SaveAsFile.SaveToJson(dialogues);
        }
        /// <summary>
        /// Метод получения списка сохраненных файлов
        /// </summary>
        /// <returns></returns>
        string GetSaveFilesNames()
        {
            string str = "";

            if (!File.Exists(@"teken.txt")) File.Create(@"teken.txt");
            FileInfo fI = new FileInfo(@"teken.txt");
            if (!File.Exists($@"{fI.DirectoryName}\\files")) Directory.CreateDirectory($@"{fI.DirectoryName}\\files");
            DirectoryInfo filesDI = new DirectoryInfo($@"{fI.DirectoryName}\\files");
            var directoryes = filesDI.GetDirectories();
            foreach (var e in directoryes)
            {
                var files = e.GetFiles();
                foreach (var f in files)
                {
                    str += $"{f.Name}\n";
                }
            }

            return str;
        }
        /// <summary>
        /// Метод обработки текстровой строки
        /// </summary>
        /// <param name="e"></param>
        [Obsolete]
        void DoIfTextMessage(Telegram.Bot.Args.MessageEventArgs e)
        {
            if (e.Message.Text == "/start")
            {
                bot.SendTextMessageAsync(e.Message.Chat.Id, FAQstr);
            }
            else
            if (e.Message.Text == "Скаченные файлы")
            {
                bot.SendTextMessageAsync(e.Message.Chat.Id, GetSaveFilesNames());
            }
            else if (e.Message.Text.Contains("Скачать файл:"))
            {
                string filePath = ""; // Строка для хранения пути файла
                var fileName = e.Message.Text.Split(':'); // Получение имени файла из сообщения пользователя
                fileName[1] = fileName[1].Trim(); // Удаление пробелов в пути файла
                // Проверка на наличие файла в системе
                if (!GetSaveFilesNames().Contains($"{fileName[1]}"))
                {
                    bot.SendTextMessageAsync(e.Message.Chat.Id, "Выбранный вами файл не найден");
                }
                else
                // Проверка папки, в которой будет лежать файл
                if (fileName[1].Contains("mp3")) { filePath = $"files\\audio\\{fileName[1]}"; }
                else if (fileName[1].Contains("jpg")) { filePath = $"files\\photo\\{fileName[1]}"; }
                else { filePath = $"files\\documents\\{fileName[1]}"; }

                if (filePath != "") SendDocumentAsync(e, bot, filePath, fileName[1]);
            }
        }
        /// <summary>
        /// Метод отправки сообщений
        /// </summary>
        /// <param name="userId">Id диалога, кому отправлять сообщение</param>
        /// <param name="messageText">Текст сообщения</param>
        public async void SendMessage(long userId, string messageText)
        {
            try
            {
                await this.bot.SendTextMessageAsync(userId, messageText);
                thisDialog.Messages.Add(new TelegramMessage(-1, "BotAdmin", messageText, DateTime.Now.ToLongTimeString()));
                SaveAsFile.SaveToJson(dialogues);
            }
            catch (System.Net.Http.HttpRequestException e)
            {
                MessageBox.Show($"{e.Message}\n{e.GetType()}, бот не был создан на основе токена");
            }
            
        }
    }


}
