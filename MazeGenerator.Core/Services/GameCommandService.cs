using System;
using System.Collections.Generic;
using System.Linq;
using MazeGenerator.Database;
using MazeGenerator.Models;
using MazeGenerator.Models.ActionStatus;
using MazeGenerator.Models.Enums;

namespace MazeGenerator.Core.Services
{
    public static class GameCommandService
    {
        private static readonly LobbyRepository LobbyRepository = new LobbyRepository();
        private static readonly MemberRepository MemberRepository = new MemberRepository();
        private static readonly CharacterRepository CharacterRepository = new CharacterRepository();

        public static MoveStatus MoveCommand(int userId, Direction direction)
        {
            Lobby lobby = LobbyRepository.Read(MemberRepository.ReadLobbyId(userId));
            if (CanMakeTurn(lobby, userId) == false)
            {
                return new MoveStatus()
                {
                    IsOtherTurn = true
                };

            }

            var currentPlayer = lobby.Players[lobby.CurrentTurn];
            var username = currentPlayer.HeroName;
            var actionList = PlayerLogic.TryMove(lobby, currentPlayer, direction);
            
            //TODO: Вывод для дебага
            //FormatAnswers.ConsoleApp(lobby);

            if (actionList.Contains(PlayerAction.GameEnd))
            {
                lobby.IsActive = false;
                MemberRepository.Delete(lobby.GameId);

                EndTurn(lobby);
                return new MoveStatus
                {
                    IsGameEnd = true,
                    CurrentPlayer = currentPlayer
                };
            }

            MoveStatus status = new MoveStatus
            {
                IsOtherTurn = false,
                IsGameEnd = false,
                CurrentPlayer = currentPlayer
            };
            if (actionList.Contains(PlayerAction.MeetPlayer))
            {
                status.PlayersOnSameCell = MazeLogic.PlayersOnCell(currentPlayer, lobby);
            }
            return status;
        }

        public static AttackStatus ShootCommand(int userId, Direction direction)
        {
            Lobby lobby = LobbyRepository.Read(MemberRepository.ReadLobbyId(userId));
            if (CanMakeTurn(lobby, userId) == false)
            {
                return new AttackStatus
                {
                    IsOtherTurn = true
                };
            }

            var currentPlayer = lobby.Players[lobby.CurrentTurn];
            var shootResult = PlayerLogic.TryShoot(lobby, currentPlayer, direction);

            EndTurn(lobby);
            return shootResult;
        }

        public static AttackStatus StabCommand(int userId)
        {
            Lobby lobby = LobbyRepository.Read(MemberRepository.ReadLobbyId(userId));
            if (CanMakeTurn(lobby, userId) == false)
            {
                return new AttackStatus
                {
                    IsOtherTurn = true
                };
            }

            var currentPlayer = lobby.Players[lobby.CurrentTurn];
            var stabResult = PlayerLogic.Stab(lobby, currentPlayer);

            EndTurn(lobby);
            return stabResult;
        }

        public static BombStatus BombCommand(int userId, Direction direction)
        {
            Lobby lobby = LobbyRepository.Read(MemberRepository.ReadLobbyId(userId));
            if (CanMakeTurn(lobby, userId) == false)
            {
                return new BombStatus
                {
                    IsOtherTurn = true
                };
            }

            var currentPlayer = lobby.Players[lobby.CurrentTurn];
            var username = currentPlayer.HeroName;

            var bombResult = PlayerLogic.Bomb(lobby, lobby.Players[lobby.CurrentTurn], direction);
            EndTurn(lobby);

            if (currentPlayer.Bombs == 0)
            {
                //TODO: correct keyboard
                //msg.KeyBoardId = KeyboardType.Bomb;
            }

            return new BombStatus
            {
                CurrentPlayer = currentPlayer,
                Result = bombResult
            };
        }

        public static bool SkipTurn(int userId)
        {
            Lobby lobby = LobbyRepository.Read(MemberRepository.ReadLobbyId(userId));
            if (CanMakeTurn(lobby, userId) == false)
            {
                //TODO:
                return false;
                //return new BombStatus
                //{
                //    IsOtherTurn = true
                //};
            }

            var currentPlayer = lobby.Players[lobby.CurrentTurn];
            var username = currentPlayer.HeroName;
            EndTurn(lobby);

            //TODO:
            return true;
            //return new MessageConfig
            //{
            //    Answer = string.Format(Answers.SkipTurn.RandomAnswer(), res.HeroName),
            //    AnswerForOther = null,
            //};
        }

        private static void EndTurn(Lobby lobby)
        {
            lobby.CurrentTurn = (lobby.CurrentTurn + 1) % lobby.Players.Count;
            lobby.TimeLastMsg = DateTime.Now;
            LobbyRepository.Update(lobby);
        }

        private static bool CanMakeTurn(Lobby lobby, int userId)
        {
            return lobby.Players.FindIndex(e => e.TelegramUserId == userId) == lobby.CurrentTurn;
        }
    }
}
