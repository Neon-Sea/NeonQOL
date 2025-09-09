using Terraria.ModLoader;

namespace NeonQOL
{
	internal class NeonQOL : Mod
	{
        public override object Call(params object[] args)
        {
            return AlchemySystem.instance.Call(args);
        }
    }
}