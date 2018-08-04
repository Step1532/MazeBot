using System;
using System.IO;
using MazeGenerator.Models;
using MazeGenerator.Models.Enums;

namespace MazeGenerator.Test.Tools
{
    public static class InstanceGenerator
    {
        private static  string CharacterFile = @"C:\Users\Step1\Desktop\mazegen\GameFiles\Characters.json";
        private static string LobbyFile(int id) => $@"C:\Users\Step1\Desktop\mazegen\GameFiles\Game{id}\lobby.json";
        private static string LobbyDirectory(int id) => $@"C:\Users\Step1\Desktop\mazegen\GameFiles\Game{id}";
        private static string UsersFilePath = @"C:\Users\Step1\Desktop\mazegen\GameFiles\usersinLobby.json";

        public static Lobby GenerateTestingLobby(int lobbyId)
        {
            Lobby lobby = new Lobby(lobbyId);
            lobby.Maze = new byte[,]
            {
                {1, 1, 1, 1, 1 },
                {1, 0, 0, 0, 1 },
                {1, 0, 0, 0, 1 },
                {1, 0, 0, 0, 1 },
                {1, 1, 0, 1, 1 }
            };
            lobby.Players.Add(new Player
            {
                Health = 3,
                Guns = 2,
                Bombs = 3,
                Rotate = Direction.North,
                TelegramUserId = 123,
                UserCoordinate = new Coordinate(2, 2)
            });
            lobby.Players.Add(new Player
            {
                Health = 3,
                Rotate = Direction.North,
                TelegramUserId = 124,
                UserCoordinate = new Coordinate(2, 1)
            });
            lobby.Events.Add(new GameEvent(EventTypeEnum.Arsenal, new Coordinate(3, 2)));
            lobby.Events.Add(new GameEvent(EventTypeEnum.Hospital, new Coordinate(1, 2)));
            return lobby;
        }
        public static void Cleaner(int id)
        {
            File.Delete(CharacterFile);
            if(Directory.Exists(LobbyDirectory(id)))
                File.Delete(LobbyFile(id));
            File.Delete(UsersFilePath);
        }

    }
    
}