using System;
using System.Reflection;
using Terraria;
using Terraria.ModLoader;
using Terraria.ID;

namespace TownNPCHome
{
    public class TownNPCHome : Mod
    {
        public override void Load() {
            On.Terraria.WorldGen.moveRoom += WorldGen_moveRoom;
        }

        private void WorldGen_moveRoom(On.Terraria.WorldGen.orig_moveRoom orig, int x, int y, int n) {
            orig.Invoke(x, y, n);
            TownEntitiesTeleportToHome(Main.npc[n], Main.npc[n].homeTileX, Main.npc[n].homeTileY);
        }

        internal static void TownEntitiesTeleportToHome(NPC npc, int homeFloorX, int homeFloorY) {
            for (int i = 0; i < 3; i++) {
                int num;
                switch (i) {
                    default:
                        num = 1;
                        break;
                    case 1:
                        num = -1;
                        break;
                    case 0:
                        num = 0;
                        break;
                }

                int num2 = homeFloorX + num;
                if (npc.type == NPCID.OldMan || !Collision.SolidTiles(num2 - 1, num2 + 1, homeFloorY - 3, homeFloorY - 1)) {
                    npc.velocity.X = 0f;
                    npc.velocity.Y = 0f;
                    npc.position.X = num2 * 16 + 8 - npc.width / 2;
                    npc.position.Y = (float)(homeFloorY * 16 - npc.height) - 0.1f;
                    npc.netUpdate = true;
                    return;
                }
            }

            npc.homeless = true;
            WorldGen.QuickFindHome(npc.whoAmI);
        }
    }
}