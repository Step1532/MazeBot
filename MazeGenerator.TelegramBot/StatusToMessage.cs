using System;
using MazeGenerator.Core.Tools;
using MazeGenerator.Models;
using MazeGenerator.Models.ActionStatus;
using MazeGenerator.Models.Enums;
using MazeGenerator.TelegramBot.Models;

namespace MazeGenerator.TelegramBot
{
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

        public static MessageConfig MessageOnShoot(AttackType status, string username)
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
    }
}