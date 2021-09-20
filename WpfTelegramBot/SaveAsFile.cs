using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.ObjectModel;
using System.IO;
using Newtonsoft.Json;

namespace WpfTelegramBot
{
    public static class SaveAsFile
    {
        /// <summary>
        /// Сохранение истории диалогов в файл dialogues.json
        /// </summary>
        /// <param name="dialogues"></param>
        public static async void SaveToJson(ObservableCollection<Dialog> dialogues)
        {
            string path = "dialogues.json";
            using (StreamWriter sw = File.CreateText(path))
            {
                string json = JsonConvert.SerializeObject(dialogues,Formatting.Indented);
                await sw.WriteAsync(json);
            }
        }
        /// <summary>
        /// Сохранение истории диалогов в файл dialogues.json
        /// </summary>
        /// <param name="dialogues"></param>
        public static async void GetObservableCollectionDialogIntoFile(object o)
        {
            TelegramBot bot = o as TelegramBot;
            string path = "dialogues.json";
            if (File.Exists(path))
            {
                using (StreamReader sr = File.OpenText(path))
                {
                    string json = "";
                    json = await sr.ReadToEndAsync();
                    var dialogsTemp = JsonConvert.DeserializeObject<ObservableCollection<Dialog>>(json);
                    foreach(var d in dialogsTemp)
                    {
                        bot.dialogues.Add(d);
                    }
                }
            }
            else
            {
                bot.dialogues = new ObservableCollection<Dialog>();
            }
            
            
        }
    }
}
