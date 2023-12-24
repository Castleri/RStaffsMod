using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.GameContent;
using Terraria.Graphics.Shaders;
using Terraria.ID;
using Terraria.ModLoader;
using ReLogic.Content;
using Terraria.GameContent.Creative;

namespace RStaffsMod
{
	public class RStaffsMod : Mod
	{
        private Asset<Texture2D> ogAmStaff,ogTopStaff,ogSapStaff,ogEmStaff,ogRuStaff,ogDiaStaff;
        public override void Load()
        {
            if (!Main.dedServ)
            {

                GameShaders.Misc["EmShader"] = new MiscShaderData(new Ref<Effect>(Assets.Request<Effect>("Assets/Effects/EmShader",
                    AssetRequestMode.ImmediateLoad).Value), "TGambleShaders");
                ogAmStaff = TextureAssets.Item[ItemID.AmethystStaff];
                ogTopStaff = TextureAssets.Item[ItemID.TopazStaff];
                ogSapStaff = TextureAssets.Item[ItemID.SapphireStaff];
                ogEmStaff = TextureAssets.Item[ItemID.EmeraldStaff];
                ogRuStaff = TextureAssets.Item[ItemID.RubyStaff];
                ogDiaStaff = TextureAssets.Item[ItemID.DiamondStaff];

                TextureAssets.Item[ItemID.AmethystStaff] = Assets.Request<Texture2D>("Resprites/AmStaff");
                TextureAssets.Item[ItemID.TopazStaff] = Assets.Request<Texture2D>("Resprites/TopStaff");
                TextureAssets.Item[ItemID.SapphireStaff] = Assets.Request<Texture2D>("Resprites/SapStaff");
                TextureAssets.Item[ItemID.EmeraldStaff] = Assets.Request<Texture2D>("Resprites/EmStaff");
                TextureAssets.Item[ItemID.DiamondStaff] = Assets.Request<Texture2D>("Resprites/DiaStaff");
                TextureAssets.Item[ItemID.RubyStaff] = Assets.Request<Texture2D>("Resprites/RuStaff");
            }
        }
        public override void Unload()
        {
            if (!Main.dedServ)
            {
                TextureAssets.Item[ItemID.AmethystStaff] = ogAmStaff;
                TextureAssets.Item[ItemID.TopazStaff] = ogTopStaff;
                TextureAssets.Item[ItemID.SapphireStaff] = ogSapStaff;
                TextureAssets.Item[ItemID.EmeraldStaff] = ogEmStaff;
                TextureAssets.Item[ItemID.RubyStaff] = ogRuStaff;
                TextureAssets.Item[ItemID.DiamondStaff] = ogDiaStaff;
            }
        }
    }
}