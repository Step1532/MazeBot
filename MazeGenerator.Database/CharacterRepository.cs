using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using MazeGenerator.Models;
using MazeGenerator.Models.Enums;
using Newtonsoft.Json;

namespace MazeGenerator.Database
{
    public class CharacterRepository
    {
        private string _connectionString;
        private string CharacterFile = $@"Characters.json";


        public CharacterRepository()
        {
            _connectionString = Config.ConnectionString;
        }

        public void Create(int telegramUserId)
        {
            if (File.Exists(CharacterFile) == false)
            {
                File.WriteAllText(CharacterFile, JsonConvert.SerializeObject(new List<Character>()));
            }
            var res = JsonConvert.DeserializeObject<List<Character>>(File.ReadAllText(CharacterFile)) ?? new List<Character>();
            Character character = new Character();
            character.TelegramUserId = telegramUserId;
            character.State = CharacterState.ChangeName;
            res.Add(character);
            File.WriteAllText(CharacterFile, JsonConvert.SerializeObject(res));
        }

        public Character Read(int telegranUserId)
        {
            if (File.Exists(CharacterFile) == false)
            {
                return null;
            }
            var res = JsonConvert.DeserializeObject<List<Character>>(File.ReadAllText(CharacterFile)) ?? new List<Character>();
            return res.Find(e => e.TelegramUserId == telegranUserId);
        }

        public List<Character> ReadAll()
        {
            return JsonConvert.DeserializeObject<List<Character>>(File.ReadAllText(CharacterFile)) ?? new List<Character>();
        }

        public void Update(Character character)
        {
            var res = JsonConvert.DeserializeObject<List<Character>>(File.ReadAllText(CharacterFile)) ?? new List<Character>();
            var r = res.Find(e => e.TelegramUserId == character.TelegramUserId);
            res.Remove(r);
            res.Add(character);
            File.WriteAllText(CharacterFile, JsonConvert.SerializeObject(res));
        }

        public void Delete(int playerId)
        {
            //TODO: сброс прогресса
        }
    }
}