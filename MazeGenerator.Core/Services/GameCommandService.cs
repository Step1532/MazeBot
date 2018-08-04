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
     
            if (LobbyService.CanMakeTurn(lobby, userId) == false)
            {
                return new MoveStatus()
                {
                    IsOtherTurn = true
                };

            }

            var currentPlayer = lobby.Players[lobby.CurrentTurn];
            var actionList = PlayerLogic.TryMove(lobby, currentPlayer, direction);
            LobbyService.EndTurn(lobby);
            FormatAnswers.ConsoleApp(lobby);
            //TODO: Вывод для дебага
            //FormatAnswers.ConsoleApp(lobby);

            if (actionList.Contains(PlayerAction.GameEnd))
            {
                lobby.IsActive = false;
                MemberRepository.Delete(lobby.GameId);

                LobbyService.EndTurn(lobby);
                return new MoveStatus
                {
                    IsGameEnd = true,
                    CurrentPlayer = currentPlayer,
                    PlayerActions = actionList
                };
            }

            MoveStatus status = new MoveStatus
            {
                IsOtherTurn = false,
                IsGameEnd = false,
                CurrentPlayer = currentPlayer,
                    PlayerActions = actionList
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
            if (LobbyService.CanMakeTurn(lobby, userId) == false)
            {
                return new AttackStatus
                {
                    IsOtherTurn = true
                };
            }

            var currentPlayer = lobby.Players[lobby.CurrentTurn];
            var shootResult = PlayerLogic.TryShoot(lobby, currentPlayer, direction);

            LobbyService.EndTurn(lobby);
            return shootResult;
        }

        public static AttackStatus StabCommand(int userId)
        {
            Lobby lobby = LobbyRepository.Read(MemberRepository.ReadLobbyId(userId));
            if (LobbyService.CanMakeTurn(lobby, userId) == false)
            {
                return new AttackStatus
                {
                    IsOtherTurn = true
                };
            }

            var currentPlayer = lobby.Players[lobby.CurrentTurn];
            var stabResult = PlayerLogic.Stab(lobby, currentPlayer);

            LobbyService.EndTurn(lobby);
            return stabResult;
        }

        public static BombStatus BombCommand(int userId, Direction direction)
        {
            Lobby lobby = LobbyRepository.Read(MemberRepository.ReadLobbyId(userId));
            if (LobbyService.CanMakeTurn(lobby, userId) == false)
            {
                return new BombStatus
                {
                    IsOtherTurn = true
                };
            }

            var currentPlayer = lobby.Players[lobby.CurrentTurn];
            var username = currentPlayer.HeroName;

            var bombResult = PlayerLogic.Bomb(lobby, lobby.Players[lobby.CurrentTurn], direction);
            LobbyService.EndTurn(lobby);

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
            if (LobbyService.CanMakeTurn(lobby, userId) == false)
            {
                //TODO:
                return false;
                //return new BombStatus
                //{
                //    IsOtherTurn = true
                //};
            }

            var currentPlayer = lobby.Players[lobby.CurrentTurn];
            LobbyService.EndTurn(lobby);

            //TODO:
            return true;
            //return new MessageConfig
            //{
            //    Answer = string.Format(Answers.SkipTurn.RandomAnswer(), res.HeroName),
            //    AnswerForOther = null,
            //};
        }


        
    }
}
