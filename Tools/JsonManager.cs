using System;
using System.Collections.Generic;
using System.IO;
using MazeGenerator.Models;
using Newtonsoft.Json;

namespace MazeGenerator.Tools
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