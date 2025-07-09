using BepInEx;
using BepInEx.IL2CPP.Utils;
using CustomGameModes;
using HarmonyLib;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace FreeForAllGameMode
{
    public sealed class CustomGameModeFFA : CustomGameModes.CustomGameMode
    {
        internal static CustomGameModeFFA Instance;
        //internal static GameModeSnowBrawl SnowBrawl;
        internal Harmony patches;

        internal static bool isGameStarted = false;

        public CustomGameModeFFA() : base
        (
            name: "Free For All",
            description: "• Weapon Given\n\n• You must kill everyone\n\n• Pickup guns of those you killed to reload",
            gameModeType: GameModeType.SnowBrawl,
            vanillaGameModeType: GameModeType.SnowBrawl,
            waitForRoundOverToDeclareSoloWinner: true,

            shortModeTime: 60,
            mediumModeTime: 90,
            longModeTime: 120,

            compatibleMapNames: [
                "Bitter Beach",
                "Blueline",
                "Cocky Containers",
                "Color Climb",
                "Funky Field",
                "Hasty Hill",
                "Icy Crack",
                "Karlson",
                "Lanky Lava",
                "Playground",
                "Playground2",
                "Return to Monke",
                "Sandstorm",
                "Snowtop",
                "Splat",
                "Splot",
                "Sunny Saloon",
                "Toxic Train",
            ]
        )
            => Instance = this;

        public override void PreInit()
            => patches = Harmony.CreateAndPatchAll(GetType());
        public override void PostEnd()
            => patches?.UnpatchSelf();

        [HarmonyPatch(typeof(GameModeSnowBrawl), nameof(GameModeSnowBrawl.OnFreezeOver))]
        [HarmonyPrefix]
        internal static bool PreOnFreezeOver(GameModeSnowBrawl __instance)
        {
            if (!SteamManager.Instance.IsLobbyOwner())
                return false;
            isGameStarted = true;

            GameServer.ForceGiveAllWeapon(0);
            GameServer.ForceGiveAllWeapon(1);

            return false;
        }

        [HarmonyPatch(typeof(GameModeSnowBrawl), nameof(GameModeSnowBrawl.PlayerDied))]
        [HarmonyPostfix]
        public static void OnPlayerDied(GameModeSnowBrawl __instance)
        {
            if (!SteamManager.Instance.IsLobbyOwner())
                return;
            if (!isGameStarted) return;
            int alivePlayers = GetAlivePlayers().Count;

            if (alivePlayers == 1)
            {
                __instance.EndRound();
                isGameStarted = false;
                foreach (ulong clientId in GameManager.Instance.activePlayers.Keys)
                {
                    string playerName = GameManager.Instance.activePlayers[clientId].username;
                    ServerSend.SendChatMessage(1, playerName + " Wins");
                }
            }
        }


        public static List<ulong> GetAlivePlayers()
        {
            List<ulong> list = new();
            foreach (var player in GameManager.Instance.activePlayers)
            {
                if (player == null || player.Value.dead) continue;
                list.Add(player.Key);
            }
            return list;
        }
    }
}
