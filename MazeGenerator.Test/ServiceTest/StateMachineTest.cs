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
    public class StateMachineTest
    {
        private static MazeBot bot = new MazeBot("");
        private static CharacterRepository _characterRepository = new CharacterRepository();
        private static LobbyRepository _lobbyRepository = new LobbyRepository();
        private static MemberRepository _memberRepository = new MemberRepository();
        private int CharacteId1 = 123;
        private int CharacteId2 = 124;

        [TestMethod]
        public void Registration()
        {
            InstanceGenerator.Cleaner(CharacteId1);
            bot.StateMachine(CharacterState.NewCharacter, "/start", CharacteId1);
            Assert.AreEqual(_characterRepository.Read(CharacteId1).State, CharacterState.ChangeName);
        }
        [TestMethod]
        public void TwoStarts()
        {
            InstanceGenerator.Cleaner(CharacteId1);
            bot.StateMachine(CharacterState.NewCharacter, "/start", CharacteId1);
            bot.StateMachine(CharacterState.NewCharacter, "/start", CharacteId1);
            Assert.AreEqual(_characterRepository.Read(CharacteId1).State, CharacterState.ChangeName);
        }
        [TestMethod]
        public void SetUserName()
        {
            InstanceGenerator.Cleaner(CharacteId1);
            bot.StateMachine(CharacterState.NewCharacter, "/start", CharacteId1);
            bot.StateMachine(_characterRepository.Read(CharacteId1).State, "Step1", CharacteId1);
            Assert.AreEqual(_characterRepository.Read(CharacteId1).CharacterName, "Step1");
            Assert.AreEqual(_characterRepository.Read(CharacteId1).State, CharacterState.ChangeGameMode);
        }
        [TestMethod]
        public void FindGame()
        {
            InstanceGenerator.Cleaner(CharacteId1);
            bot.StateMachine(CharacterState.NewCharacter, "/start", CharacteId1);
            bot.StateMachine(_characterRepository.Read(CharacteId1).State, "Step1", CharacteId1);
            bot.StateMachine(_characterRepository.Read(CharacteId1).State, "/game", CharacteId1);
            Assert.AreEqual(_characterRepository.Read(CharacteId1).State, CharacterState.FindGame);
        }
        [TestMethod]
        public void StopFindGame()
        {
            InstanceGenerator.Cleaner(CharacteId1);
            bot.StateMachine(CharacterState.NewCharacter, "/start", CharacteId1);
            bot.StateMachine(_characterRepository.Read(CharacteId1).State, "Step1", CharacteId1);
            bot.StateMachine(_characterRepository.Read(CharacteId1).State, "/game", CharacteId1);
            bot.StateMachine(_characterRepository.Read(CharacteId1).State, "/stop", CharacteId1);
            Assert.AreEqual(_characterRepository.Read(CharacteId1).State, CharacterState.ChangeGameMode);
        }
        [TestMethod]
        public void CheckLobby()
        {
            InstanceGenerator.Cleaner(CharacteId1);
            bot.StateMachine(CharacterState.NewCharacter, "/start", CharacteId1);
            bot.StateMachine(_characterRepository.Read(CharacteId1).State, "Step1", CharacteId1);
            bot.StateMachine(_characterRepository.Read(CharacteId1).State, "/game", CharacteId1);
            bot.StateMachine(CharacterState.NewCharacter, "/start", CharacteId2);
            bot.StateMachine(_characterRepository.Read(CharacteId2).State, "Step2", CharacteId2);
            bot.StateMachine(_characterRepository.Read(CharacteId2).State, "/game", CharacteId2);
            int lobbyId = _memberRepository.ReadLobbyId(CharacteId1);
            Lobby lobby  = _lobbyRepository.Read(lobbyId);
            Assert.AreEqual(lobby.Players.Count, 2);
            Assert.AreEqual(lobby.Players[0].TelegramUserId, CharacteId1);
            Assert.AreEqual(lobby.Players[1].TelegramUserId, CharacteId2);
            Assert.AreEqual(lobby.IsActive, true);
        }
    }
}
