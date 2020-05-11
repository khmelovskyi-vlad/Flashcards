using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Flashcards
{
    class Input
    {
        public Input(FileMaster fileMaster)
        {
            this.fileMaster = fileMaster;
        }
        private FileMaster fileMaster;
        private const string PathAllTopics = "D:\\temp\\Flashcards";
        private string PathTopic;
        private string Topic { get; set; }
        private List<string> topics;

        public async Task Run()
        {
            FindTopics();
            await FindData();
        }
        private async Task FindData()
        {
            if (FindTopic())
            {
                var key = QuestionAnswerKey("If you want to watch your cards, press 'Enter', if not - press else");
                if (key.Key == ConsoleKey.Enter)
                {
                    var flashcards = await fileMaster.ReadData(PathTopic);
                    if (flashcards == null)
                    {
                        Console.WriteLine("Don't have cards");
                    }
                    else
                    {
                        foreach (var flashcard in flashcards)
                        {
                            Console.WriteLine($"{flashcard.Topic} - " +
                                $"{flashcard.FrontOrForeignTranslation} - " +
                                $"{flashcard.Transcription} - " +
                                $"{flashcard.BackOrOriginalWord}");
                        }
                    }
                }
            }
            await EnterData();
        }
        private async Task EnterData()
        {
            while (true)
            {
                var key = QuestionAnswerKey($"If you want enter flashcards in {Topic}, press 'Enter', if not - press else");
                if (key.Key == ConsoleKey.Enter)
                {
                    var front = QuestionAnswerInUkr("Write front or foreign translation");
                    var transcription = QuestionAnswerInUkr("Write transcription");
                    var back = QuestionAnswerInUkr("Write back or original word");
                    var newCard = new Flashcard(Topic, front, transcription, back);
                    await fileMaster.WriteData(PathTopic, newCard);
                    Console.WriteLine("Your card has been added!");
                }
                else
                {
                    return;
                }
            }
        }
        private bool FindTopic()
        {
            Topic = QuestionAnswer("Write a need topic");
            PathTopic = $"{PathAllTopics}\\{Topic}\\Flashcards.json";
            fileMaster.CreateDirectory($"{PathAllTopics}\\{Topic}");
            foreach (var topic in topics)
            {
                if (topic == Topic)
                {
                    return true;
                }
            }
            topics.Add(Topic);
            return false;
        }
        private void FindTopics()
        {
            Console.WriteLine("All topics you have:");
            topics = fileMaster.GetTopics(PathAllTopics);
            foreach (var topic in topics)
            {
                Console.WriteLine(topic);
            }
        }
        private string QuestionAnswerInUkr(string question)
        {
            Console.WriteLine(question);
            var line = Console.ReadLine();
            var stringBuilder = new StringBuilder();
            foreach (var sign in line)
            {
                if (sign == '?')
                {
                    stringBuilder.Append('i');
                    continue;
                }
                stringBuilder.Append(sign);
            }
            return stringBuilder.ToString();
        }
        private string QuestionAnswer(string question)
        {
            Console.WriteLine(question);
            return Console.ReadLine();
        }
        private ConsoleKeyInfo QuestionAnswerKey(string question)
        {
            Console.WriteLine(question);
            return Console.ReadKey(true);
        }
    }
}
