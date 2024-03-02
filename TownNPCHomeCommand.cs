using System.Linq;
using Terraria;
using Terraria.Localization;
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
            => Language.GetTextValue("Mods.TownNPCHome.CommandDesc");

        public override void Action(CommandCaller caller, string input, string[] args) {
            Core.CallTeleportAll = true;
            // foreach (var npc in from n in Main.npc where n is not null && n.active && n.townNPC && !n.homeless select n) {
            //     TownNPCHome.TownEntitiesTeleportToHome(npc, npc.homeTileX, npc.homeTileY);
            // }
        }
    }
}