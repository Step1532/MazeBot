using System;
using System.Collections.Generic;
using System.Text;
using MazeGenerator.Database;
using MazeGenerator.Models;
using MazeGenerator.Models.Enums;
using MazeGenerator.TelegramBot;
using MazeGenerator.Test.Tools;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace MazeGenerator.Test.ServiceTest
{
    [TestClass]
    public class ShootTest
    {
        private static MazeBot bot = new MazeBot("");
        private static CharacterRepository _characterRepository = new CharacterRepository();
        private static LobbyRepository _lobbyRepository = new LobbyRepository();
        private static MemberRepository _memberRepository = new MemberRepository();
        private int CharacteId1 = 123;
        private int CharacteId2 = 124;
        private int lobbyId = 1;

        [TestMethod]
        public void ShootInPlayer()
        {
            InstanceGenerator.Cleaner(CharacteId1);
            CreateChar(CharacteId1, lobbyId, "Step1");
            CreateChar(CharacteId2, lobbyId, "Step2");

            Lobby newlobby = InstanceGenerator.GenerateTestingLobby(lobbyId);
            _lobbyRepository.Create(newlobby);
            bot.SComm(CharacteId1, Direction.North);
            newlobby = _lobbyRepository.Read(lobbyId);
            Assert.AreEqual(newlobby.Players[1].Health == LobbyRules.Test().PlayerMaxHealth-1, true);
            Assert.AreEqual(newlobby.Players[0].Guns == LobbyRules.Test().PlayerMaxGuns-1, true);
        }
        [TestMethod]
        public void BombinWall()
        {
            InstanceGenerator.Cleaner(CharacteId1);
            CreateChar(CharacteId1, lobbyId, "Step1");
            CreateChar(CharacteId2, lobbyId, "Step2");

            Lobby newlobby = InstanceGenerator.GenerateTestingLobby(lobbyId);
            _lobbyRepository.Create(newlobby);
            bot.StateMachine(_characterRepository.Read(CharacteId1).State, "Вверх", CharacteId1);
            bot.StateMachine(_characterRepository.Read(CharacteId2).State, "Вверх", CharacteId2);
            bot.BComm(CharacteId1, Direction.North);
            newlobby = _lobbyRepository.Read(lobbyId);
            Assert.AreEqual(newlobby.Maze[2, 0] == 0, true);
            Assert.AreEqual(newlobby.Players[0].Bombs == LobbyRules.Test().PlayerMaxBombs - 1, true);
        }

        [TestMethod]
        public void BombinVoid()
        {
            InstanceGenerator.Cleaner(CharacteId1);
            CreateChar(CharacteId1, lobbyId, "Step1");
            CreateChar(CharacteId2, lobbyId, "Step2");

            Lobby newlobby = InstanceGenerator.GenerateTestingLobby(lobbyId);
            _lobbyRepository.Create(newlobby);
            bot.BComm(CharacteId1, Direction.North);
            newlobby = _lobbyRepository.Read(lobbyId);
            Assert.AreEqual(newlobby.Maze[2, 1] == 0, true);
            Assert.AreEqual(newlobby.Players[0].Bombs == LobbyRules.Test().PlayerMaxBombs - 1, true);
        }
        [TestMethod]
        public void StabInPlayer()
        {
            InstanceGenerator.Cleaner(CharacteId1);
            CreateChar(CharacteId1, lobbyId, "Step1");
            CreateChar(CharacteId2, lobbyId, "Step2");

            Lobby newlobby = InstanceGenerator.GenerateTestingLobby(lobbyId);
            _lobbyRepository.Create(newlobby);
            bot.StateMachine(_characterRepository.Read(CharacteId1).State, "Пропуск хода", CharacteId1);
            bot.StateMachine(_characterRepository.Read(CharacteId2).State, "Вниз", CharacteId2);
            bot.StateMachine(_characterRepository.Read(CharacteId1).State, "Удар кинжалом", CharacteId1);
            newlobby = _lobbyRepository.Read(lobbyId);
            Assert.AreEqual(newlobby.Players[1].Health == LobbyRules.Test().PlayerMaxHealth-1, true);
        }
        private static void CreateChar(int id, int lobbyId, string username)
        {
            _characterRepository.Create(id);
            _memberRepository.Create(lobbyId, id);
            _characterRepository.Update(new Character
            {
                CharacterName = username,
                State = CharacterState.InGame,
                TelegramUserId = id
            });
        }
    }
}
