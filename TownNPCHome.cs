using System.IO;
using System.Linq;
using System.Reflection;
using Terraria;
using Terraria.ModLoader;

namespace TownNPCHome
{
    public class TownNPCHome : Mod
    {
        public override void Load() {
            On_WorldGen.moveRoom += WorldGen_moveRoom;
        }

        private void WorldGen_moveRoom(On_WorldGen.orig_moveRoom orig, int x, int y, int n) {
            orig.Invoke(x, y, n);
            if (Main.npc.IndexInRange(n) && Main.npc[n] is not null)
                TownEntitiesTeleportToHome(Main.npc[n], Main.npc[n].homeTileX, Main.npc[n].homeTileY);
        }
        
        internal static void TownEntitiesTeleportToHome(NPC npc, int homeFloorX, int homeFloorY) {
            npc?.GetType().GetMethod("AI_007_TownEntities_TeleportToHome",
                BindingFlags.Instance | BindingFlags.NonPublic,
                new[] { typeof(int), typeof(int) })?
                .Invoke(npc, new object[] {homeFloorX, homeFloorY});
        }

        public override void HandlePacket(BinaryReader reader, int whoAmI) {
            foreach (var npc in from n in Main.npc where n is not null && n.active && n.townNPC && !n.homeless select n) {
                TownEntitiesTeleportToHome(npc, npc.homeTileX, npc.homeTileY);
            }
        }
    }
}