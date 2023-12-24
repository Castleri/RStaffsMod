using Terraria;
using Terraria.ModLoader;

namespace RStaffsMod.Buffs
{
    public class EmBuff : ModBuff
    {
        public override void SetStaticDefaults()
        {
            Main.debuff[Type] = false;
            Main.buffNoTimeDisplay[Type] = true;
            Main.buffNoSave[Type] = true;
            //DisplayName.SetDefault("Emerald Buff");
            //Description.SetDefault("Magic Damage and Regen +");
        }
        public override void Update(Player player, ref int buffIndex)
        {
            player.GetDamage(DamageClass.Magic) += 0.14f;
            player.manaRegenBonus += 25;
            if (player.ownedProjectileCounts[ModContent.ProjectileType<Staffs.EmeraldStaff.EmField>()] < 1)
            {
                player.ClearBuff(Type);
                buffIndex--;
            }
        }
    }
}
