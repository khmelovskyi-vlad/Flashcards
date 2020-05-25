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


        public async Task FindTopics()
        {
            topics = await fileMaster.GetAllTopics();
        }
        public void WriteTopics()
        {
            userInteractor.WriteLine("All topics you have:");
            foreach (var topic in topics)
            {
                userInteractor.WriteLine(topic);
            }
        }
        public async Task<bool> FindOrCreateTopic()
        {
            var topic = userInteractor.QuestionAnswer("Write a need topic");
            if (!await fileMaster.ContainsTopic(topic))
            {
                Topic = await fileMaster.CreateTopic(topic);
                topics.Add(topic);
                return false;
            }
            else
            {
                Topic = await fileMaster.FindNeedTopic(topic);
                return true;
            }
        }
        public async Task<bool> FindTopic()
        {
            var topic = userInteractor.QuestionAnswer("Write a need topic");
            var result = await fileMaster.ContainsTopic(topic);
            Topic = await fileMaster.FindNeedTopic(topic);
            return result;
        }
    }
}
