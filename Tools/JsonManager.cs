using System;
using System.Collections.Generic;
using System.IO;
using MazeGenerator.Models;
using Newtonsoft.Json;

namespace MazeGenerator.Tools
{
    //TODO: refactoring
    public static class JsonManager
    {
        public static void UpdateJson<T>(string fileName, Action<T> update)
        {
            var data = JsonConvert.DeserializeObject<T>(fileName);
            update(data);
            File.WriteAllText(fileName, JsonConvert.SerializeObject(data));
        }

        //TODO: remove (see Lobby.Save)

        //public static void WriteMazeToJson(byte[,] maze, int gameId, List<Coordinate> events)
        //{
        //    var serialized = JsonConvert.SerializeObject(maze);
        //    Directory.CreateDirectory($@"\Game{gameId}");
        //    File.WriteAllText(string.Format($@"\Game{gameId}\maze.json"), serialized);
        //    serialized = JsonConvert.SerializeObject(events);
        //    Directory.CreateDirectory($@"\Game{gameId}");
        //    File.WriteAllText(string.Format($@"\Game{gameId}\CoordinateEvents.json"), serialized);
        //}

        //public static void WritePlayersToJson(Player players, int Gameid)
        //{
        //    var PlayersList = new List<Player>();
        //    var json = File.ReadAllText(string.Format($@"\Game{Gameid}\Players.json"));
        //    PlayersList = JsonConvert.DeserializeObject<List<Player>>(json);
        //    if (PlayersList == null)
        //    {
        //        PlayersList = new List<Player>();
        //        PlayersList.Add(players);
        //        var serialized = JsonConvert.SerializeObject(PlayersList);
        //        Directory.CreateDirectory($@"\Game{Gameid}");
        //        File.WriteAllText(string.Format($@"\Game{Gameid}\Players.json"), serialized);
        //    }
        //    else
        //    {
        //        PlayersList.Add(players);
        //        var serialized = JsonConvert.SerializeObject(PlayersList);
        //        Directory.CreateDirectory($@"\Game{Gameid}");
        //        File.WriteAllText(string.Format($@"\Game{Gameid}\Players.json"), serialized);
        //    }
        //}

        //public static void WriteCoordinateEventsToJson(List<Coordinate> eventses, int Gameid)
        //{
        //    var serialized = JsonConvert.SerializeObject(eventses);
        //    Directory.CreateDirectory($@"\Game{Gameid}");
        //    File.WriteAllText(string.Format($@"\Game{Gameid}\CoordinateEvents.json"), serialized);
        //}

        //public static void WriteLobbiesPlayerCountToJson(List<int> playerscount)
        //{
        //    var serialized = JsonConvert.SerializeObject(playerscount);
        //    File.WriteAllText("lobbiesPlayerCount.json", serialized);
        //}
    }
}