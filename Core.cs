using System;
using System.Linq;
using System.Reflection;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace TownNPCHome;

public class Core : ModSystem
{
    public static bool CallTeleportAll;
    private static int _indexReadyToTeleport = -1;

    public override void Load() {
        On_WorldGen.moveRoom += WorldGen_moveRoom;
    }

    public override void PreUpdateEntities() {
        if (CallTeleportAll) {
            // for mp we send a packet to server and let the server process the code
            if (Main.netMode is NetmodeID.MultiplayerClient) {
                Mod.GetPacket().Send();
            }
            else
                foreach (var npc in from n in Main.npc
                         where n is not null && n.active && n.townNPC && !n.homeless
                         select n) {
                    TownEntitiesTeleportToHome(npc, npc.homeTileX, npc.homeTileY);
                }

            CallTeleportAll = false;
        }

        ref int victim = ref _indexReadyToTeleport;
        if (Main.npc.IndexInRange(victim) && Main.npc[victim] is not null) {
            TownEntitiesTeleportToHome(Main.npc[victim], Main.npc[victim].homeTileX, Main.npc[victim].homeTileY);
            victim = -1;
        }
    }

    private void WorldGen_moveRoom(On_WorldGen.orig_moveRoom orig, int x, int y, int n) {
        orig.Invoke(x, y, n);
        if (Main.npc.IndexInRange(n) && Main.npc[n] is not null)
            _indexReadyToTeleport = n;
        // TownEntitiesTeleportToHome(Main.npc[n], Main.npc[n].homeTileX, Main.npc[n].homeTileY);
    }

    internal static void TownEntitiesTeleportToHome(NPC npc, int homeFloorX, int homeFloorY) {
        npc?.GetType().GetMethod("AI_007_TownEntities_TeleportToHome",
                BindingFlags.Instance | BindingFlags.NonPublic,
                new[] {typeof(int), typeof(int)})?
            .Invoke(npc, new object[] {homeFloorX, homeFloorY});
    }
}