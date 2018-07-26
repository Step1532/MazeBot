using System;
using System.IO;
using Newtonsoft.Json;

namespace MazeGenerator.Database
{
    public static class JsonManager
    {
        public static void UpdateJson<T>(string fileName, Action<T> update)
        {
            var data = JsonConvert.DeserializeObject<T>(fileName);
            update(data);
            File.WriteAllText(fileName, JsonConvert.SerializeObject(data));
        }
    }
}