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

            //TODO: Вывод для дебага
            FormatAnswers.ConsoleApp(lobby);

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
            if (currentPlayer.Guns == 0)
                shootResult.KeyboardType = KeyboardType.Bomb;
            if (currentPlayer.Guns == 0 && currentPlayer.Bombs == 0)
                shootResult.KeyboardType  = KeyboardType.Move;
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
            var bombResult = PlayerLogic.Bomb(lobby, currentPlayer, direction);
            LobbyService.EndTurn(lobby);

            if (currentPlayer.Bombs == 0 &&  currentPlayer.Guns == 0)
            {
                return new BombStatus
                {
                    CurrentPlayer = currentPlayer,
                    Result = bombResult,
                    KeyboardId = KeyboardType.Move
                };
            }
            if (currentPlayer.Bombs == 0)
            {
                return new BombStatus
                {
                    CurrentPlayer = currentPlayer,
                    Result = bombResult,
                    KeyboardId = KeyboardType.Shoot
                };
            }

            return new BombStatus
            {
                CurrentPlayer = currentPlayer,
                Result = bombResult,
            };
        }

        public static SkipTurnResult SkipTurn(int userId)
        {
            Lobby lobby = LobbyRepository.Read(MemberRepository.ReadLobbyId(userId));
            var result = new SkipTurnResult();

            if (LobbyService.CanMakeTurn(lobby, userId) == false)
            {
                //TODO:
                result.CanMakeTurn = false;
                return result;
               
            }
            result.PickChest = PlayerLogic.LootChest(lobby, lobby.Players[lobby.CurrentTurn]);
            LobbyService.EndTurn(lobby);

            //TODO:
            result.CanMakeTurn = true;
            return result;
        }
        
    }
}
