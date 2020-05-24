using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Flashcards
{
    class Program
    {
        static async Task<int> Main(string[] args)
        {
            //Console.ReadLine();
            //using (var context = new UserContext())
            //{
            //    context.TopicWithFlashcards.Add(new TopicWithFlashcards() { Name = "new"});
            //    context.SaveChanges();
            //}
            //Console.ReadLine();
            ModeSelector modeSelector = new ModeSelector(new ConsoleUserInteractor());
            await modeSelector.Run();
            var paths = Directory.GetDirectories("D:\\temp");
            List<string> results = new List<string>();
            foreach (var path in paths)
            {
                results.Add(Path.GetFileName(path));
            }
            foreach (var result in results)
            {
                Console.WriteLine(result);
            }
            Console.ReadKey();
            return 5;
        }
    }
}
