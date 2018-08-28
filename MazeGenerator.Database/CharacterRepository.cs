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
        #if DEBUG
            private const string CharacterFile = @"C:\Users\Step1\Desktop\mazegen\GameFiles\Characters.json";
        #else
                private const string CharacterFile = @"GameFiles\Characters.json";
        #endif
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
            Character character = new Character
            {
                TelegramUserId = telegramUserId,
                State = CharacterState.ChangeName
            };
            res.Add(character);
            File.WriteAllText(CharacterFile, JsonConvert.SerializeObject(res));
        }

        public Character Read(int telegranUserId)
        {
            return ReadAll()?
                .Find(e => e.TelegramUserId == telegranUserId);
        }

        public List<Character> ReadAll()
        {
            if (File.Exists(CharacterFile) == false)
            {
                return null;
            }
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
            var res = JsonConvert.DeserializeObject<List<Character>>(File.ReadAllText(CharacterFile)) ?? new List<Character>();
            var r = res.Find(e => e.TelegramUserId == playerId);
            res.Remove(r);
            File.WriteAllText(CharacterFile, JsonConvert.SerializeObject(res));
        }
    }
}