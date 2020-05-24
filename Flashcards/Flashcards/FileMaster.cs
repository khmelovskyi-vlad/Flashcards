using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Flashcards
{
    class FileMaster
    {
        public async Task WriteData(List<Flashcard> cards)
        {
            using (var context = new UserContext())
            {
                context.Flashcards.AddRange(cards);
                await context.SaveChangesAsync();
            }
        }
        public List<Flashcard> ReadData(TopicWithFlashcards topic)
        {
            using (var context = new UserContext())
            {
                return context.Flashcards.AsNoTracking().ToList().Where(x => x.Topic.Id == topic.Id).ToList();
            }
        }
        public List<string> GetAllTopics()
        {
            using (var context = new UserContext())
            {
                return context.TopicWithFlashcards.Select(x => x.Name).ToList();
            }
        }
        public List<Flashcard> TakeAllFlashcards()
        {
            using (var context = new UserContext())
            {
                return context.Flashcards.ToList();
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
                var needTopics = context.TopicWithFlashcards.Where(x => x.Name == topic).ToList();
                var needTopic = needTopics.First();
                return needTopic;
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
