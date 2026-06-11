using HarmonyLib;

namespace FreeForAllGameMode
{
    public sealed class CustomGameModeFFA : CustomGameModes.CustomGameMode
    {
        internal static CustomGameModeFFA Instance;
        internal Harmony patches;

        public CustomGameModeFFA() : base
        (
            name: "Free For All",
            description: "• Weapon Given\n\n• You must kill everyone\n\n• Pickup guns of those you killed to reload",
            gameModeType: GameModeType.SnowBrawl,
            vanillaGameModeType: GameModeType.SnowBrawl,
            waitForRoundOverToDeclareSoloWinner: false,

            shortModeTime: 60,
            mediumModeTime: 80,
            longModeTime: 100,

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

            GameServer.ForceGiveAllWeapon(0);
            GameServer.ForceGiveAllWeapon(1);

            return false;
        }
    }
}
