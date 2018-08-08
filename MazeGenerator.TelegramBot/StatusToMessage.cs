using System;
using MazeGenerator.Core.Services;
using MazeGenerator.Core.Tools;
using MazeGenerator.Models;
using MazeGenerator.Models.ActionStatus;
using MazeGenerator.Models.Enums;
using MazeGenerator.TelegramBot.Models;

namespace MazeGenerator.TelegramBot
{
    //TODO: Реализовать ввиде конвертора Enum => String
    public static class StatusToMessage
    {
        public static string MessageOnMoveAction(PlayerAction action, string username)
        {
            switch (action)
            {
                case PlayerAction.FakeChest:
                    return string.Format(Answers.ExitFalseChest.RandomAnswer(), username);
                case PlayerAction.OnArsenal:
                    return string.Format(Answers.MoveArs.RandomAnswer(), username);
                case PlayerAction.OnChest:
                    return string.Format(Answers.MoveChest.RandomAnswer(), username);
                case PlayerAction.OnHospital:
                    return string.Format(Answers.MoveHosp.RandomAnswer(), username);
                default:
                    return null;
            }
        }

        public static MessageConfig MessageOnStab(AttackType status, string username)
        {
            switch (status)
            {
                case AttackType.NoTarget:
                    return new MessageConfig
                    {
                        Answer = string.Format(Answers.ShootWall.RandomAnswer(), username),
                    };
                case AttackType.Kill:
                    return new MessageConfig
                    {
                        Answer = string.Format(Answers.ShootKill.RandomAnswer(), username),
                    };
                case AttackType.Hit:
                    return new MessageConfig
                    {
                        Answer = string.Format(Answers.ShootHit.RandomAnswer(), username),
                    };
            }

            throw new ArgumentException(status.ToString());
        }

        public static (string, string) MessageOnShoot(AttackType status, string username, string username2, string direction)
        {
            switch (status)
            {
                case AttackType.NoTarget:
                    return (string.Format(Answers.ShootWall.RandomAnswer(), username, username2, direction),
                            string.Format(AnswersForOther.ShootWall.RandomAnswer(), username, username2, direction)) ;
                case AttackType.Kill:
                    return (string.Format(Answers.ShootKill.RandomAnswer(), username, username2, direction),
                        string.Format(AnswersForOther.ShootKill.RandomAnswer(), username, username2, direction));
                case AttackType.Hit:
                    return (string.Format(Answers.ShootHit.RandomAnswer(), username, username2, direction),
                        string.Format(AnswersForOther.ShootHit.RandomAnswer(), username, username2, direction));
            }

            throw new ArgumentException(status.ToString());
        }
    }
}