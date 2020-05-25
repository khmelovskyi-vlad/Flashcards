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
        public async Task WriteFlashcards(List<Flashcard> cards)
        {
            using (var context = new UserContext())
            {
                context.Flashcards.AddRange(cards);
                await context.SaveChangesAsync();
            }
        }
        public async Task<List<Flashcard>> ReadData(TopicWithFlashcards topic)
        {
            using (var context = new UserContext())
            {
                var needTopic = await context.TopicWithFlashcards.FirstAsync(x => x.Id == topic.Id);
                return needTopic.Flashcards.ToList();
            }
        }
        public async Task<List<string>> GetAllTopics()
        {
            using (var context = new UserContext())
            {
                return await context.TopicWithFlashcards.Select(x => x.Name).ToListAsync();
            }
        }
        public async Task<List<Flashcard>> TakeAllFlashcards()
        {
            using (var context = new UserContext())
            {
                var needTopics = await context.TopicWithFlashcards.ToListAsync();
                return needTopics.SelectMany(x => x.Flashcards).ToList();
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
        public async Task<bool> ContainsTopic(string topic)
        {
            using (var context = new UserContext())
            {
                return await context.TopicWithFlashcards.Select(x => x.Name).ContainsAsync(topic);
            }
        }
        public async Task<TopicWithFlashcards> FindNeedTopic(string topic)
        {
            using (var context = new UserContext())
            {
                return await context.TopicWithFlashcards.Where(x => x.Name == topic).FirstAsync();
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
