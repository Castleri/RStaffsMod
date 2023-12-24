using Terraria;
using Terraria.ModLoader;
using Terraria.ID;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria.GameContent;
using Terraria.DataStructures;

namespace RStaffsMod.Staffs.AmberStaff
{
    public class Charger:ModProjectile
    {
        private float maxSpeed;
        private int state;      //0 = Walking/Attack  1 = Idle
        private bool targetFound;
        public override void SetStaticDefaults()
        {
            ProjectileID.Sets.MinionTargettingFeature[Projectile.type] = true;
            ProjectileID.Sets.MinionSacrificable[Projectile.type] = true;
            ProjectileID.Sets.TrailingMode[Projectile.type] = 0;
            ProjectileID.Sets.TrailCacheLength[Projectile.type] = 6;
        }
        public override void SetDefaults()
        {
            Projectile.DamageType = DamageClass.Summon;
            Projectile.minion = true;
            Projectile.width = 20;
            Projectile.height = 20;
            Projectile.alpha = 12;
            DrawOffsetX = -2;
            DrawOriginOffsetY = -4;
            Projectile.friendly = true;
            Projectile.penetrate = -1;
            maxSpeed = 7f;
        }
        public override void AI()
        {
            Player player = Main.player[Projectile.owner];
            Projectile.spriteDirection = Projectile.direction;
            if (player.active) Projectile.timeLeft = 3;
            if (targetFound)
            {
                state = 0;
                Attack();
            }
            else
            {
                Idle(player);
            }
            Projectile.velocity.Y += 0.25f;
            base.AI();
        }
        private void Idle(Player player)
        {
            Rectangle recF = new((int)player.position.X + (12 * Projectile.minionPos * player.direction), (int)Projectile.position.Y, 24, 8);
            Vector2 posF = new(recF.X, recF.Y);
            if (!Projectile.Hitbox.Intersects(recF))
            {
                int direction;
                if (recF.X < Projectile.position.X)
                {
                    direction = -1;
                }
                else direction = 1;
                Main.NewText(direction);
                Projectile.velocity.X += 0.2f * direction;
                if (Projectile.velocity.X > maxSpeed - 2f) Projectile.velocity.X = maxSpeed - 2f;
                if(Projectile.velocity.X < -maxSpeed + 2f) Projectile.velocity.X = -maxSpeed + 2f;

                Tile tilefront = Main.tile[(int)(Projectile.position.X / 16) - direction, (int)Projectile.position.Y / 16];
                Main.NewText(tilefront.HasTile);
                Tile tilebelow = Main.tile[(int)Projectile.position.X / 16, (int)(Projectile.position.Y / 16) + 1];
                Main.NewText(tilefront);
                Main.NewText(tilebelow);
                Main.NewText(Main.tileSolid[tilebelow.TileType]);
                Main.NewText(Main.tileSolid[tilebelow.TileType]);
                if (tilefront.BlockType == BlockType.Solid && Main.tileSolid[tilefront.TileType] && tilebelow.BlockType == BlockType.Solid && tilebelow.TileType == 0 && Main.tileSolid[tilebelow.TileType] && Projectile.velocity.Y == 0)
                {
                    Projectile.velocity.Y -= 6f;
                }
            }
            else
            {
                Projectile.velocity.X *= 0.8f;
                if (Projectile.velocity.X < 0.1f) state = 1;
            }
        }
        private void Attack()
        {

        }
        public override bool OnTileCollide(Vector2 oldVelocity)
        {
            return false;
        }
        public override bool PreDraw(ref Color lightColor)
        {
            Texture2D texture = TextureAssets.Projectile[Projectile.type].Value;
            Rectangle source = new(52 * state, Projectile.frame * 26 + Projectile.frame * 2, 50, 26);
            Vector2 origin = source.Size() / 2f;
            Vector2 value = new Vector2(Projectile.width, Projectile.height) / 2;
            DrawData drawData = new(texture, Projectile.position, source, lightColor, Projectile.rotation, origin, Projectile.scale,
                Projectile.spriteDirection == 1 ? SpriteEffects.None : SpriteEffects.FlipHorizontally, 1);
            Main.EntitySpriteDraw(drawData);
            for (int i = 0; i < Projectile.oldPos.Length; i++)
            {
                Vector2 pos = Projectile.oldPos[i] + value - Main.screenPosition;
                DrawData drawDataShadows = new(texture, pos, source, lightColor * (0.875f - 0.125f * i) , Projectile.rotation, origin, Projectile.scale, 
                    Projectile.spriteDirection == -1? SpriteEffects.None : SpriteEffects.FlipHorizontally, 1);
                Main.EntitySpriteDraw(drawDataShadows);
            }
            return false;
        }
    }
}
