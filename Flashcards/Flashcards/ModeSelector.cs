using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Flashcards
{
    class ModeSelector
    {
        private FileMaster fileMaster = new FileMaster();
        public async Task Run()
        {
            while (true)
            {
                Console.WriteLine("What do you want to do?\n\r" +
                    "If you want to add cards, press 'a'\n\r" +
                    "If you want to learn cards, press 'l'\n\r" +
                    "If you want to exit the application, press 'Esc'");
                var key = Console.ReadKey(true);
                switch (key.Key)
                {
                    case ConsoleKey.A:
                        Input input = new Input(fileMaster);
                        await input.Run();
                        break;
                    case ConsoleKey.L:
                        Console.WriteLine();
                        break;
                    case ConsoleKey.Escape:
                        Console.WriteLine("bye");
                        return;
                    default:
                        Console.WriteLine();
                        break;
                }
            }
        }
    }
}
