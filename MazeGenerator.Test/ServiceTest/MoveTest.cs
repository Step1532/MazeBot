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
    public class MoveTest
    {
        private static MazeBot bot = new MazeBot("");
        private static CharacterRepository _characterRepository = new CharacterRepository();
        private static LobbyRepository _lobbyRepository = new LobbyRepository();
        private static MemberRepository _memberRepository = new MemberRepository();
        private int CharacteId1 = 123;
        private int CharacteId2 = 124;
        private int lobbyId = 1;

        [TestMethod]
        public void MoveUp()
        {
            InstanceGenerator.Cleaner(CharacteId1);
            CreateChar(CharacteId1, lobbyId, "Step1");
            CreateChar(CharacteId2, lobbyId, "Step2");

            Lobby newlobby = InstanceGenerator.GenerateTestingLobby(lobbyId);
            _lobbyRepository.Create(newlobby);
            bot.StateMachine(_characterRepository.Read(CharacteId1).State, "Вверх", CharacteId1);
            newlobby = _lobbyRepository.Read(lobbyId);
            Assert.AreEqual(newlobby.Players[0].UserCoordinate.X == 2, true);
            Assert.AreEqual(newlobby.Players[0].UserCoordinate.Y == 1, true);
        }
        [TestMethod]
        public void MoveUpUp()
        {
            InstanceGenerator.Cleaner(CharacteId1);
            CreateChar(CharacteId1, lobbyId, "Step1");
            CreateChar(CharacteId2, lobbyId, "Step2");
            Lobby newlobby = InstanceGenerator.GenerateTestingLobby(lobbyId);
            _lobbyRepository.Create(newlobby);
            bot.StateMachine(_characterRepository.Read(CharacteId1).State, "Вверх", CharacteId1);
            bot.StateMachine(_characterRepository.Read(CharacteId2).State, "Вверх", CharacteId2);
            bot.StateMachine(_characterRepository.Read(CharacteId1).State, "Вверх", CharacteId1);
            newlobby = _lobbyRepository.Read(lobbyId);
            Assert.AreEqual(newlobby.Players[0].UserCoordinate.X == 2, true);
            Assert.AreEqual(newlobby.Players[0].UserCoordinate.Y == 1, true);
        }
        [TestMethod]
        public void MoveToArsenal()
        {
            InstanceGenerator.Cleaner(CharacteId1);
            CreateChar(CharacteId1, lobbyId, "Step1");
            CreateChar(CharacteId2, lobbyId, "Step2");
            Lobby newlobby = InstanceGenerator.GenerateTestingLobby(lobbyId);
            _lobbyRepository.Create(newlobby);

            bot.StateMachine(_characterRepository.Read(CharacteId1).State, "Вправо", CharacteId1);
            newlobby = _lobbyRepository.Read(lobbyId);
            Assert.AreEqual(newlobby.Players[0].Guns == LobbyRules.Test().PlayerMaxGuns, true);
            Assert.AreEqual(newlobby.Players[0].Bombs == LobbyRules.Test().PlayerMaxBombs, true);
        }
        [TestMethod]
        public void MoveToHospital()
        {
            InstanceGenerator.Cleaner(CharacteId1);
            CreateChar(CharacteId1, lobbyId, "Step1");
            CreateChar(CharacteId2, lobbyId, "Step2");
            Lobby newlobby = InstanceGenerator.GenerateTestingLobby(lobbyId);
            _lobbyRepository.Create(newlobby);

            bot.StateMachine(_characterRepository.Read(CharacteId1).State, "Влево", CharacteId1);
            newlobby = _lobbyRepository.Read(lobbyId);
            Assert.AreEqual(newlobby.Players[0].Health == LobbyRules.Test().PlayerMaxHealth, true);
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
