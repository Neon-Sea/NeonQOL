using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using System.ComponentModel;
using Terraria;
using Terraria.GameContent;
using Terraria.Localization;
using Terraria.ModLoader.Config;
using Terraria.ModLoader.Config.UI;
using Terraria.ModLoader.UI;
using Terraria.UI;

namespace NeonQOL
{
    internal class AlchemyConfig : ModConfig
    {
        public override ConfigScope Mode => ConfigScope.ServerSide;

        [DefaultValue(true)]
        [CustomModConfigItem(typeof(LockableBool))]
        [TooltipKey("$")]
        public bool Replant;

        [DefaultValue(true)]
        [CustomModConfigItem(typeof(LockableBool))]
        [TooltipKey("$")]
        public bool AutoSelect;

        [DefaultValue(true)]
        [CustomModConfigItem(typeof(LockableBool))]
        [TooltipKey("$")]
        public bool SmartCursor;
    }

    internal class LockableBool : ConfigElement<bool>
    {
        //modified copy of BooleanElement from tModLoader

        private Asset<Texture2D> _toggleTexture;
        private static readonly LocalizedText AutoSelectToolTip = Language.GetText("Mods.NeonQOL.Configs.AlchemyConfig.AutoSelect.DefaultToolTip");
        private static readonly LocalizedText SmartCursorToolTip = Language.GetText("Mods.NeonQOL.Configs.AlchemyConfig.SmartCursor.DefaultToolTip");
        private static readonly LocalizedText ReplantToolTip = Language.GetText("Mods.NeonQOL.Configs.AlchemyConfig.Replant.DefaultToolTip");
        private static readonly LocalizedText DisabledSingle = Language.GetText("Mods.NeonQOL.Configs.AlchemyConfig.Disabled.SingleMod");
        private static readonly LocalizedText DisabledMulti = Language.GetText("Mods.NeonQOL.Configs.AlchemyConfig.Disabled.MultiMod");

        public override void OnBind()
        {
            base.OnBind();
            _toggleTexture = Main.Assets.Request<Texture2D>("Images/UI/Settings_Toggle");
            OnLeftClick += (ev, v) =>
            {
                if ((MemberInfo.Name == "AutoSelect" && AlchemySystem.ModsDisablingAutoSelect.Count == 0) || (MemberInfo.Name == "SmartCursor" && AlchemySystem.ModsDisablingSmartCursor.Count == 0) || (MemberInfo.Name == "Replant" && AlchemySystem.ModsDisablingReplant.Count == 0))
                {
                    Value = !Value;
                }
            };
            
        }

        protected override void DrawSelf(SpriteBatch spriteBatch)
        {
            base.DrawSelf(spriteBatch);
            CalculatedStyle dimensions = base.GetDimensions();
            // "Yes" and "No" since no "True" and "False" translation available
            Terraria.UI.Chat.ChatManager.DrawColorCodedStringWithShadow(spriteBatch, FontAssets.ItemStack.Value, Value ? Lang.menu[126].Value : Lang.menu[124].Value, new Vector2(dimensions.X + dimensions.Width - 60, dimensions.Y + 8f), Color.White, 0f, Vector2.Zero, new Vector2(0.8f));
            Rectangle sourceRectangle = new (Value ? ((_toggleTexture.Width() - 2) / 2 + 2) : 0, 0, (_toggleTexture.Width() - 2) / 2, _toggleTexture.Height());
            Vector2 drawPosition = new (dimensions.X + dimensions.Width - sourceRectangle.Width - 10f, dimensions.Y + 8f);
            spriteBatch.Draw(_toggleTexture.Value, drawPosition, sourceRectangle, Color.White, 0f, Vector2.Zero, Vector2.One, SpriteEffects.None, 0f);

            if (IsMouseHovering)
            {
                if (MemberInfo.Name == "AutoSelect")
                {
                    if (AlchemySystem.ModsDisablingAutoSelect.Count == 0)
                    {
                        UICommon.TooltipMouseText(AutoSelectToolTip.Value);
                    }
                    else if (AlchemySystem.ModsDisablingAutoSelect.Count == 1)
                    {
                        UICommon.TooltipMouseText(DisabledSingle.Format(AlchemySystem.ModsDisablingAutoSelect[0].DisplayName));
                    }
                    else
                    {
                        UICommon.TooltipMouseText(DisabledMulti.Format(AlchemySystem.ModsDisablingAutoSelect[0].DisplayName, AlchemySystem.ModsDisablingAutoSelect.Count - 1));
                    }
                }
                if (MemberInfo.Name == "SmartCursor")
                {
                    if (AlchemySystem.ModsDisablingSmartCursor.Count == 0)
                    {
                        UICommon.TooltipMouseText(SmartCursorToolTip.Value);
                    }
                    else if (AlchemySystem.ModsDisablingSmartCursor.Count == 1)
                    {
                        UICommon.TooltipMouseText(DisabledSingle.Format(AlchemySystem.ModsDisablingSmartCursor[0].DisplayName));
                    }
                    else
                    {
                        UICommon.TooltipMouseText(DisabledMulti.Format(AlchemySystem.ModsDisablingSmartCursor[0].DisplayName, AlchemySystem.ModsDisablingSmartCursor.Count - 1));
                    }
                }
                if (MemberInfo.Name == "Replant")
                {
                    if (AlchemySystem.ModsDisablingReplant.Count == 0)
                    {
                        UICommon.TooltipMouseText(ReplantToolTip.Value);
                    }
                    else if (AlchemySystem.ModsDisablingReplant.Count == 1)
                    {
                        UICommon.TooltipMouseText(DisabledSingle.Format(AlchemySystem.ModsDisablingReplant[0].DisplayName));
                    }
                    else
                    {
                        UICommon.TooltipMouseText(DisabledMulti.Format(AlchemySystem.ModsDisablingReplant[0].DisplayName, AlchemySystem.ModsDisablingReplant.Count - 1));
                    }
                }
            }
        }
    }
}
