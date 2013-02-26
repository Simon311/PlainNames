using System;
using TShockAPI;
using Terraria;
using Hooks;
using System.ComponentModel;
using System.Text.RegularExpressions;

namespace PlainNames
{
    [APIVersion(1, 12)]
    public class PlainNames : TerrariaPlugin
    {
        public override Version Version
        {
            get { return new Version("1.0.0.0"); }
        }

        public override string Name
        {
            get { return "PlainNames"; }
        }

        public override string Author
        {
            get { return "Simon311"; }
        }

        public override string Description
        {
            get { return "Filters bad names"; }
        }
		
        public PlainNames(Main game)
            : base(game)
        {
            Order = -1;
        }
		
        public override void Initialize()
        {
            ServerHooks.Join += OJ;
        }
		
		protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                ServerHooks.Join -= OJ;
            }
            base.Dispose(disposing);
        }
		
        private void OJ(int ply, HandledEventArgs handler)
        {
			var player = TShock.Players[ply];
			if (player == null)
			{
				handler.Handled = true;
				return;
			}
			if(Regex.IsMatch(player.Name,"[^a-z0-9,. !]+", RegexOptions.IgnoreCase))
            {
                TShock.Utils.ForceKick(player, "Unacceptable name", true, false);
				handler.Handled = true;
				return;
            }
        }
    }
}
