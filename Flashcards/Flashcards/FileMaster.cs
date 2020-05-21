using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Flashcards
{
    class FileMaster
    {
        public async Task WriteData(Flashcard card)
        {
            using (var context = new UserContext())
            {
                context.Flashcards.Add(card);
                await context.SaveChangesAsync();
            }
        }
        public IEnumerable<Flashcard> ReadData(TopicWithFlashcards topic)
        {
            using (var context = new UserContext())
            {
                return context.Flashcards.Where(x => x.Topic == topic);
            }
        }
        public IEnumerable<string> GetAllTopics()
        {
            using (var context = new UserContext())
            {
                return context.TopicWithFlashcards.Select(x => x.Name).ToList();
            }
        }
        public IEnumerable<Flashcard> TakeAllFlashcards()
        {
            using (var context = new UserContext())
            {
                return context.Flashcards;
            }
        }
        public async Task<TopicWithFlashcards> CreateTopic(string Topic)
        {
            using (var context = new UserContext())
            {
                var newTopic = new TopicWithFlashcards() { Name = Topic };
                context.TopicWithFlashcards.Add(newTopic);
                await context.SaveChangesAsync();
                return newTopic;
            }
        }
        public bool ContainsTopic(string topic)
        {
            using (var context = new UserContext())
            {
                return context.TopicWithFlashcards.Select(x => x.Name).Contains(topic);
            }
        }
        public TopicWithFlashcards FindNeedTopic(string topic)
        {
            using (var context = new UserContext())
            {
                return context.TopicWithFlashcards.Where(x => x.Name == topic).First();
            }
        }
        //private async Task<string> ReadData(string path, FileStream stream)
        //{
        //    var sb = new StringBuilder();
        //    var count = 256;
        //    var buffer = new byte[count];
        //    while (true)
        //    {
        //        var realCount = await stream.ReadAsync(buffer, 0, count);
        //        sb.Append(Encoding.Default.GetString(buffer, 0, realCount));
        //        if (realCount < count)
        //        {
        //            break;
        //        }
        //    }
        //    return sb.ToString();
        //}
        //private List<Flashcard> DesToLFlashcard(string dataJson)
        //{
        //    return JsonConvert.DeserializeObject<List<Flashcard>>(dataJson);
        //}
        //private async Task<List<Flashcard>> ReadAndDesToLString(string path, FileStream stream)
        //{
        //    var dataJson = await ReadData(path, stream);
        //    return DesToLFlashcard(dataJson);
        //}
    }
}
