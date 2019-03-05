using BytesRoad.Net.Ftp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace ftp_client
{
    class Program
    {
       public static int TimeoutFTP=1000;
        public static void init()
        {
            client = new FtpClient();
            client.UsedEncoding = Encoding.UTF8;
            Console.Write("Введите адресс сервера (по умолчанию localhost): ");
            string Host = Console.ReadLine();
            Console.Write("Введите логин (по умолчанию anonymous): ");
            string UserName = Console.ReadLine();
            Console.Write("Введите пароль (по умолчанию anonymous): ");
            string Password = Console.ReadLine();
            Console.Write("Введите порт (по умолчанию 21): ");
            int port = 21;
            if (!int.TryParse(Console.ReadLine(), out port)) port = 21;   
            client.Connect(TimeoutFTP, Host, port);
            client.Login(TimeoutFTP, UserName, Password);
        }
        static FtpClient client = null;
        static void Main(string[] args)
        {
            init();
            ConsoleTable consoleTable;
            while (true)
            {
               
                Console.Write("/>");
                string[] command = Console.ReadLine().Split(' ');
                try
                {
                    switch (command[0])
                    {
                        case "ls":
                            var ls = client.GetDirectoryList(TimeoutFTP);
                             consoleTable = new ConsoleTable("Name", "Size", "Date","Type");
                            foreach (var ftpItem in ls)
                            {
                                
                                consoleTable.AddRow(ftpItem.Name,ftpItem.Size,ftpItem.Date,ftpItem.ItemType);
                            }
                            consoleTable.Write();
                            break;
                        case "cd":
                            client.ChangeDirectory(TimeoutFTP, command[1]);
                            break;
                        case "upl":
                            client.PutFile(TimeoutFTP,command[1], command[2]);
                            break;
                        case "down":
                            client.GetFile(TimeoutFTP, command[1], command[2]);
                            break;
                        case "rm":
                            client.DeleteFile(TimeoutFTP, command[1]);
                            break;
                        case "rmdir":
                            client.DeleteDirectory(TimeoutFTP, command[1]);
                            break;
                        case "mkdir":
                            client.CreateDirectory(TimeoutFTP, command[1]);
                            break;
                        case "help":
                             consoleTable = new ConsoleTable("Синтаксис", "Описание");
                            consoleTable.AddRow("ls","Просмотр содержимого директории");
                            consoleTable.AddRow("сd <Название папки> (Поддерживается ../)", "Переход в папку");
                            consoleTable.AddRow("upl <имя файла на сервере> <что грузим - имя файла на компьютере>", "Загрузка файла на удаленный компьютер");
                            consoleTable.AddRow("down <куда принимаем - путь на диске> <Что принимаем - файл на сервере> ", "Загрузка файла на локальный компьютер");
                            consoleTable.AddRow("rm <имя файла>", "Удаление файла");
                            consoleTable.AddRow("rmdir <имя директории>", "Просмотр содержимого директории");
                            consoleTable.AddRow("clear", "Очистка экрана");
                            consoleTable.AddRow("help", "Просмотр справки");
                            consoleTable.Write();
                            break;
                        case "clear":
                            Console.Clear();
                            break;
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine(String.Format("Произошла ошибка: {0}", ex.Message));
                }
            }
        }
    }
}