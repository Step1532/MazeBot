using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using MazeGenerator.Models;
using Newtonsoft.Json;

namespace MazeGenerator.Database
{
    public class MemberRepository
    {
        private readonly string _connectionString;
        private string UsersFilePath = @"C:\Users\Step1\Desktop\mazegen\GameFiles\usersinLobby.json";

        public MemberRepository()
        {
            _connectionString = Config.ConnectionString;
        }

        public void Create(int lobbyId, int userId)
        {
            List<Member> ls = new List<Member>();
            if (File.Exists(UsersFilePath))
            {
                ls = JsonConvert.DeserializeObject<List<Member>>(File.ReadAllText(UsersFilePath));
            }
            Member member = new Member
            {
                LobbyId = lobbyId,
                UserId = userId,
                LanguageId = 0
            };
            ls.Add(member);

            File.WriteAllText(UsersFilePath, JsonConvert.SerializeObject(ls));
        }

        public List<Member> ReadMemberList(int lobbyId)
        {
            if (File.Exists(UsersFilePath) == false)
            {
                return new List<Member>();
            }
            var res = JsonConvert.DeserializeObject<List<Member>>(File.ReadAllText(UsersFilePath))
                .Where(e => e.LobbyId == lobbyId)
                .ToList();
            return res;
        }

        public int ReadLobbyId(int userId)
        {
            var res = JsonConvert.DeserializeObject<List<Member>>(File.ReadAllText(UsersFilePath)).Find(e => e.UserId == userId);
            return res.LobbyId;
        }
        public List<Member> ReadLobbyAll()
        {
            if(File.Exists(UsersFilePath) == false)
                return  new List<Member>();
            var res = JsonConvert.DeserializeObject<List<Member>>(File.ReadAllText(UsersFilePath));
            return res;
        }
        public void DeleteOne(int userId)
        {
            var res = JsonConvert.DeserializeObject<List<Member>>(File.ReadAllText(UsersFilePath));
            var r = res.Where(e => e.UserId != userId);
            File.WriteAllText(UsersFilePath, JsonConvert.SerializeObject(r));
        }
        public void Delete(int lobbyId)
        {
            var res = JsonConvert.DeserializeObject<List<Member>>(File.ReadAllText(UsersFilePath));
            var r = res.Where(e => e.LobbyId != lobbyId);
            File.WriteAllText(UsersFilePath, JsonConvert.SerializeObject(r));
        }
    }
}