using System;
using System.Collections.Generic;
using System.Text;
using MazeGenerator.Database;
using MazeGenerator.Test.Tools;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace MazeGenerator.Test.LobbyRepositoryTest
{
    [TestClass]
    public class LobbyRepositoryTest
    {
        [TestMethod]
        public void ReadTest()
        {
            int lobbyId = 1;
            InstanceGenerator.Cleaner(lobbyId);
            var lobby = InstanceGenerator.GenerateTestingLobby(lobbyId);

            LobbyRepository lobbyRepository = new LobbyRepository();
            lobbyRepository.Create(lobby);

            var newLobby = lobbyRepository.Read(lobbyId);
            Assert.AreEqual(lobby.GameId, newLobby.GameId);
            Assert.AreEqual(lobby.Maze[0, 3], newLobby.Maze[0, 3]);
            Assert.AreEqual(lobby.Players.Count, newLobby.Players.Count);
        }

        [TestMethod]
        public void UpdateTest()
        {
            int lobbyId = 1;
            InstanceGenerator.Cleaner(lobbyId);
            var lobby = InstanceGenerator.GenerateTestingLobby(lobbyId);

            LobbyRepository lobbyRepository = new LobbyRepository();
            lobbyRepository.Create(lobby);
            lobby.Maze[0, 3] = 0;
            lobbyRepository.Update(lobby);
            var newLobby = lobbyRepository.Read(lobbyId);

            Assert.AreEqual(lobby.GameId, newLobby.GameId);
            Assert.AreEqual(lobby.Maze[0, 3], newLobby.Maze[0, 3]);
            Assert.AreEqual(lobby.Players.Count, newLobby.Players.Count);
        }
    }
}
