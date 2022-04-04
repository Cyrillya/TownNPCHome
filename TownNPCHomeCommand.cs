using System.Linq;
using Terraria;
using Terraria.ModLoader;

namespace TownNPCHome
{
    internal class TownNPCHomeCommand : ModCommand
    {
        public override CommandType Type
               => CommandType.World;

        public override string Command
            => "npchome";

        public override string Usage
            => "/npchome";

        public override string Description
            => "Teleport all town NPC to home immediately.";

        public override void Action(CommandCaller caller, string input, string[] args) {
            foreach (var npc in from n in Main.npc where n != null && n.active && n.townNPC && !n.homeless select n) {
                TownNPCHome.TownEntitiesTeleportToHome(npc, npc.homeTileX, npc.homeTileY);
            }
        }
    }
}