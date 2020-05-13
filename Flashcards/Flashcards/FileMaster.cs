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
        public async Task WriteData(string path, Flashcard card)
        {
            using (var stream = File.Open(path, FileMode.OpenOrCreate, FileAccess.ReadWrite, FileShare.Write))
            {
                var data = await ReadAndDesToLString(path, stream);
                if (data == null)
                {
                    data = new List<Flashcard>();
                }
                data.Add(card);
                stream.SetLength(0);
                var dataJson = JsonConvert.SerializeObject(data);
                var buffer = Encoding.Default.GetBytes(dataJson);
                await stream.WriteAsync(buffer, 0, buffer.Length);
            }
        }
        public async Task<List<Flashcard>> ReadData(string path)
        {
            using (var stream = File.Open(path, FileMode.OpenOrCreate, FileAccess.ReadWrite, FileShare.Write))
            {
                var dataJson = await ReadData(path, stream);
                return DesToLFlashcard(dataJson);
            }
        }
        public List<string> GetDirectoriesName(string path)
        {
            var topicsPaths = GetDirectoriesPath(path);
            List<string> topics = new List<string>();
            foreach (var topicPath in topicsPaths)
            {
                topics.Add(Path.GetFileName(topicPath));
            }
            return topics;
        }
        public string[] GetDirectoriesPath(string path)
        {
            return Directory.GetDirectories(path);
        }
        public void CreateDirectory(string path)
        {
            Directory.CreateDirectory(path);
        }
        public bool FileExists(string path)
        {
            return File.Exists(path);
        }
        private async Task<string> ReadData(string path, FileStream stream)
        {
            var sb = new StringBuilder();
            var count = 256;
            var buffer = new byte[count];
            while (true)
            {
                var realCount = await stream.ReadAsync(buffer, 0, count);
                sb.Append(Encoding.Default.GetString(buffer, 0, realCount));
                if (realCount < count)
                {
                    break;
                }
            }
            return sb.ToString();
        }
        private List<Flashcard> DesToLFlashcard(string dataJson)
        {
            return JsonConvert.DeserializeObject<List<Flashcard>>(dataJson);
        }
        private async Task<List<Flashcard>> ReadAndDesToLString(string path, FileStream stream)
        {
            var dataJson = await ReadData(path, stream);
            return DesToLFlashcard(dataJson);
        }
    }
}
