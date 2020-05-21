using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Flashcards
{
    class TopicMet
    {
        public TopicMet(FileMaster fileMaster, IUserInteractor userInteractor)
        {
            this.fileMaster = fileMaster;
            this.userInteractor = userInteractor;
        }
        IUserInteractor userInteractor;
        private FileMaster fileMaster;
        
        public TopicWithFlashcards Topic { get; set; }
        public List<string> topics;


        public void FindTopics()
        {
            userInteractor.WriteLine("All topics you have:");
            var topicsEn = fileMaster.GetAllTopics();
            topics = new List<string>();
            Console.ReadLine();
            userInteractor.WriteLine("lol");
            if (topicsEn!= null)
            {
                topics.AddRange(topicsEn);
            }
            Console.ReadLine();
        }
        public void WriteTopics()
        {
            foreach (var topic in topics)
            {
                userInteractor.WriteLine(topic);
            }
        }
        public async Task<bool> FindOrCreateTopic()
        {
            var topic = userInteractor.QuestionAnswer("Write a need topic");
            if (!fileMaster.ContainsTopic(topic))
            {
                Topic = await fileMaster.CreateTopic(topic);
            }
            else
            {
                Topic = fileMaster.FindNeedTopic(topic);
            }
            if (topics.Contains(topic))
            {
                return true;
            }
            topics.Add(topic);
            return false;
        }
        public bool FindTopic()
        {
            var topic = userInteractor.QuestionAnswer("Write a need topic");
            var result = fileMaster.ContainsTopic(topic);
            Topic = fileMaster.FindNeedTopic(topic);
            return result;
        }
    }
}
