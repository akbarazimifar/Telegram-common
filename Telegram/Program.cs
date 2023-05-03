using System;
using System.Threading;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Exceptions;
using Telegram.Bot.Extensions.Polling;
using Telegram.Bot.Polling;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.InputFiles;
using Telegram.Bot.Args;


namespace TelegramBotExperiments
{
    class Program
    {
        static ITelegramBotClient bot = new TelegramBotClient("6073096280:AAGjLrQdWF0j3phbnOMIip0g_8aYSVu_Vf0");

        public static async Task HandleUpdateAsync(ITelegramBotClient botClient, Update update, CancellationToken cancellationToken)
        {
            Console.WriteLine(Newtonsoft.Json.JsonConvert.SerializeObject(update));

            if (update.Type == UpdateType.Message)
            {
                var message = update.Message;

                if (message.Text.ToLower() == "/start")
                {
                    await botClient.SendTextMessageAsync(message.Chat, "Привет! Я бот, который может помочь тебе в решении проблем в области психологии. Отправь /help, чтобы узнать, что я могу для тебя сделать.");
                }
                else if (message.Text.ToLower() == "/help")
                {
                    await botClient.SendTextMessageAsync(
                        chatId: message.Chat.Id,
                        text: "Список доступных команд: /start - начать диалог со мной, /help - показать список команд."
                    );
                }
                else
                {
                    await botClient.SendTextMessageAsync(
                        chatId: message.Chat.Id,
                        text: "Извините, я не понимаю вашего сообщения. Отправьте /help, чтобы узнать, какие команды я понимаю."
                    );
                }
            }
        }

        public static async Task HandleErrorAsync(ITelegramBotClient botClient, Exception exception, CancellationToken cancellationToken)
        {
            Console.WriteLine(Newtonsoft.Json.JsonConvert.SerializeObject(exception));
        }

        static async Task Main(string[] args)
        {
            Console.WriteLine("Запущен бот " + (await bot.GetMeAsync()).FirstName);

            var cts = new CancellationTokenSource();
            var cancellationToken = cts.Token;

            var receiverOptions = new ReceiverOptions
            {
                AllowedUpdates = Array.Empty<UpdateType>()
            };

           await bot.StartReceivingAsync(
                new DefaultUpdateHandler(HandleUpdateAsync, HandleErrorAsync),
                cancellationToken
            );

            Console.ReadLine();
            cts.Cancel();
        }
        private static async void Bot_OnMessage(object sender, Telegram.Bot.Args.MessageEventArgs e)
        {
            var message = e.Message;

            if (message.Type == MessageType.Text)
            {
                if (message.Text.ToLower() == "/sendimage")
                {
                    // Путь к изображению на вашем компьютере
                    string imagePath = @"C:\example\image.jpg";

                    // Открываем файл изображения
                    using (var stream = System.IO.File.Open(imagePath, System.IO.FileMode.Open))
                    {
                        // Создаем объект InputOnlineFile, который позволит отправить изображение
                        var file = new InputOnlineFile(stream);

                        // Создаем объект отправляемого сообщения
                        var photo = new InputMediaPhoto(file);

                        // Отправляем сообщение
                        await bot.SendPhotoAsync(chatId: message.Chat.Id, photo: photo, caption: "Привет, это картинка!");
                    }
                }
            }
        }
    }
}

/*using System;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Args;
using Telegram.Bot.Types.Enums;

namespace TelegramBotExample
{
    class Program
    {
        private static TelegramBotClient botClient;

        static void Main(string[] args)
        {
            botClient = new TelegramBotClient("6073096280:AAGjLrQdWF0j3phbnOMIip0g_8aYSVu_Vf0");

            botClient.OnMessage += Bot_OnMessage;

            botClient.StartReceiving(UpdateType.Message);

            Console.WriteLine("Bot started");
            Console.ReadLine();

            
        }

        private static async void Bot_OnMessage(object sender, MessageEventArgs e)
        {
            if (e.Message.Type == Telegram.Bot.Types.Enums.MessageType.Text)
            {
                if (e.Message.Text == "/start")
                {
                    await botClient.SendTextMessageAsync(
                        chatId: e.Message.Chat.Id,
                        text: "Привет! Я бот, который может помочь тебе в решении проблем в области психологии. Отправь /help, чтобы узнать, что я могу для тебя сделать."
                    );
                }
                else if (e.Message.Text == "/help")
                {
                    await botClient.SendTextMessageAsync(
                        chatId: e.Message.Chat.Id,
                        text: "Список доступных команд: /start - начать диалог со мной, /help - показать список команд."
                    );
                }
                else
                {
                    await botClient.SendTextMessageAsync(
                        chatId: e.Message.Chat.Id,
                        text: "Извините, я не понимаю вашего сообщения. Отправьте /help, чтобы узнать, какие команды я понимаю."
                    );
                }
            }
        }
    }
}
*/




/*using System;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Args;


ITelegramBotClient botClient = null;

botClient = new TelegramBotClient("6073096280:AAGjLrQdWF0j3phbnOMIip0g_8aYSVu_Vf0");
private static async void Bot_OnMessage(object sender, MessageEventArgs e)
{
    if (e.Message.Type == Telegram.Bot.Types.Enums.MessageType.Text)
    {
        if (e.Message.Text == "/start")
        {
            await botClient.SendTextMessageAsync(
                chatId: e.Message.Chat.Id,
                text: "Привет! Я бот, который может помочь тебе в решении проблем в области психологии. Отправь /help, чтобы узнать, что я могу для тебя сделать."
            );
        }
        else if (e.Message.Text == "/help")
        {
            await botClient.SendTextMessageAsync(
                chatId: e.Message.Chat.Id,
                text: "Список доступных команд: /start - начать диалог со мной, /help - показать список команд."
            );
        }
        else
        {
            await botClient.SendTextMessageAsync(
                chatId: e.Message.Chat.Id,
                text: "Извините, я не понимаю вашего сообщения. Отправьте /help, чтобы узнать, какие команды я понимаю."
            );
        }
    }
}

botClient.OnMessage += Bot_OnMessage;

botClient.StartReceiving();

Console.WriteLine("Bot has been started. Press any key to exit.");
Console.ReadKey();

botClient.StopReceiving();

async void Bot_OnMessage(object sender, MessageEventArgs e)
{
    if (e.Message.Type == Telegram.Bot.Types.Enums.MessageType.Text)
    {
        if (e.Message.Text == "/start")
        {
            await botClient.SendTextMessageAsync(
                chatId: e.Message.Chat.Id,
                text: "Добро пожаловать в нашего бота! Я здесь, чтобы помочь вам расслабиться, справиться со стрессом и научиться жить в настоящем моменте. Начнем?"
            );
        }
    }
}
Console.ReadLine();

var client = new TelegramBotClient("6073096280:AAGjLrQdWF0j3phbnOMIip0g_8aYSVu_Vf0");
client.StartReceiving(Update,Error);

Task Error(ITelegramBotClient arg1, Exception arg2, CancellationToken arg3)
{
    throw new NotImplementedException();
}

async static Task Update(ITelegramBotClient botClient, Update update, CancellationToken token)
{
    var message = update.Message;
    if (message.Text != null)
    {
        if (message.Text.ToLower().Contains("привет"))
        {
            await botClient.SendTextMessageAsync(message.Chat.Id, "Рад приветствовать вас в моем боте! Я создан, чтобы помочь вам в путешествии к более здоровой и счастливой жизни. Давайте начнем этот путь вместе!");
            return;
        }    
    }
}*/







