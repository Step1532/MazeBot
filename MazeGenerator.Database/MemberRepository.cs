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
        private string UsersFilePath = @"usersinLobby.json";

        public MemberRepository()
        {
            _connectionString = Config.ConnectionString;
        }

        public void Create(int lobbyId, int userId)
        {
            Member member = new Member
            {
                LobbyId = lobbyId,
                UserId = userId,
                LanguageId = 0
            };
            if (lobbyId == 1)
            {
                Console.WriteLine(JsonConvert.DeserializeObject<List<Member>>(File.ReadAllText(UsersFilePath)));
            }

            var ls = JsonConvert.DeserializeObject<List<Member>>(File.ReadAllText(UsersFilePath));
            ls.Add(member);
            File.WriteAllText(UsersFilePath, JsonConvert.SerializeObject(ls));
        }

        public List<Member> ReadMemberList(int lobbyId)
        {
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
            var res = JsonConvert.DeserializeObject<List<Member>>(File.ReadAllText(UsersFilePath));
            return res;
        }

        public void Delete(int lobbyId)
        {
            //TODO:
//            JsonManager.UpdateJsonList(UsersFilePath, (List<Member> members) => { members.RemoveAll(e => e.LobbyId == lobbyId); });
        }
    }
}