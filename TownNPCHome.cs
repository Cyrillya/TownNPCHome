using System.IO;
using System.Linq;
using Terraria;
using Terraria.ModLoader;

namespace TownNPCHome
{
    public class TownNPCHome : Mod
    {
        public override void HandlePacket(BinaryReader reader, int whoAmI) {
            foreach (var npc in from n in Main.npc
                     where n is not null && n.active && n.townNPC && !n.homeless
                     select n) {
                Core.TownEntitiesTeleportToHome(npc, npc.homeTileX, npc.homeTileY);
            }
        }
    }
}