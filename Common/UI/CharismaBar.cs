using System.Collections.Generic;
using AthleteClass.Common.Players;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.GameContent;
using Terraria.GameContent.UI.Elements;
using Terraria.ModLoader;
using Terraria.UI;

namespace AthleteClass.Common.UI
{

    internal class CharismaBar : UIState
    {
        private UIImage frame;
        private Color gradientA;
        private Color gradientB;

        public override void OnInitialize()
        {

            // frame
            frame = new UIImage(ModContent.Request<Texture2D>("AthleteClass/Common/UI/CharismaBarFrame"));
            frame.Left.Set( -85, 0.5f);
            frame.Top.Set(-48, 0.5f);
            frame.Height.Set(100, 0f);
            frame.Width.Set(28, 0f);

            // Gradient colours
            gradientA = new(0, 0, 0);
            gradientB = new Color(255, 255, 255);

            Append(frame);
        }

        /*public override void Draw(SpriteBatch spriteBatch)
        {
            base.Draw(spriteBatch);
        }*/

        protected override void DrawSelf(SpriteBatch spriteBatch)
        {
            // set direction
            frame.Left.Set(Main.LocalPlayer.direction * -55 -14, 0.5f);
            base.DrawSelf(spriteBatch);
            
            // get fill percentage
            float quotient = Main.LocalPlayer.GetModPlayer<CharismaPlayer>().GetQuotient();
            
            //set fill area
            Rectangle fill = frame.GetInnerDimensions().ToRectangle();
            
            fill.X += 8;
            fill.Width = 9;
            fill.Y += 9;
            fill.Height -= 20;
            
            // draw fill
            int bottom = fill.Bottom;
            int top = fill.Top;
            int steps = (int) ((bottom - top) * quotient);
            
            //spriteBatch.Draw(TextureAssets.MagicPixel.Value, fill, Color.Red);

            for (int i = 1; i <= steps; i++)
            {
                spriteBatch.Draw(TextureAssets.MagicPixel.Value,
                    new Rectangle(fill.X, fill.Bottom - i, 9, 1),
                    Color.Lerp(gradientA, gradientB,
                    (float) i / (bottom - top)));

            }
        }
    }
    
    [Autoload(Side = ModSide.Client)]
    internal class CharismaBarSystem : ModSystem
    {
        private UserInterface CharismaUI;

        internal CharismaBar BarInstance;

        public override void Load() {
            BarInstance = new();
            CharismaUI = new();
            
            //BarInstance.Activate();
            CharismaUI.SetState(BarInstance);
        }

        public override void UpdateUI(GameTime gameTime) {
            CharismaUI?.Update(gameTime);
        }

        public override void ModifyInterfaceLayers(List<GameInterfaceLayer> layers) {
            int barIndex = layers.FindIndex(layer => layer.Name.Equals("Vanilla: Resource Bars"));
            if (barIndex != -1) {
                layers.Insert(barIndex, new LegacyGameInterfaceLayer(
                    "AthleteClass: Charisma Bar",
                    delegate {
                        CharismaUI.Draw(Main.spriteBatch, new GameTime());
                        return true;
                    }, InterfaceScaleType.UI)
                );
            }
        }
    }
}