using System;
using TShockAPI;
using Terraria;
using Hooks;
using System.ComponentModel;
using System.Text.RegularExpressions;
using System.Collections.Generic;

namespace PlainNames
{
    [APIVersion(1, 12)]
    public class PlainNames : TerrariaPlugin
    {
        public override Version Version
        {
            get { return new Version("1.0.0.4"); }
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

        Dictionary<string, string> words = new Dictionary<string, string>();

        public override void Initialize()
        {
            ServerHooks.Join += OJ;
            ServerHooks.Chat += OC;
            words.Add("а", "a");
            words.Add("б", "b");
            words.Add("в", "v");
            words.Add("г", "g");
            words.Add("д", "d");
            words.Add("е", "e");
            words.Add("ё", "yo");
            words.Add("ж", "zh");
            words.Add("з", "z");
            words.Add("и", "i");
            words.Add("й", "j");
            words.Add("к", "k");
            words.Add("л", "l");
            words.Add("м", "m");
            words.Add("н", "n");
            words.Add("о", "o");
            words.Add("п", "p");
            words.Add("р", "r");
            words.Add("с", "s");
            words.Add("т", "t");
            words.Add("у", "u");
            words.Add("ф", "f");
            words.Add("х", "h");
            words.Add("ц", "c");
            words.Add("ч", "ch");
            words.Add("ш", "sh");
            words.Add("щ", "sch");
            words.Add("ъ", "j");
            words.Add("ы", "i");
            words.Add("ь", "j");
            words.Add("э", "e");
            words.Add("ю", "yu");
            words.Add("я", "ya");
            words.Add("А", "A");
            words.Add("Б", "B");
            words.Add("В", "V");
            words.Add("Г", "G");
            words.Add("Д", "D");
            words.Add("Е", "E");
            words.Add("Ё", "Yo");
            words.Add("Ж", "Zh");
            words.Add("З", "Z");
            words.Add("И", "I");
            words.Add("Й", "J");
            words.Add("К", "K");
            words.Add("Л", "L");
            words.Add("М", "M");
            words.Add("Н", "N");
            words.Add("О", "O");
            words.Add("П", "P");
            words.Add("Р", "R");
            words.Add("С", "S");
            words.Add("Т", "T");
            words.Add("У", "U");
            words.Add("Ф", "F");
            words.Add("Х", "H");
            words.Add("Ц", "C");
            words.Add("Ч", "Ch");
            words.Add("Ш", "Sh");
            words.Add("Щ", "Sch");
            words.Add("Ъ", "J");
            words.Add("Ы", "I");
            words.Add("Ь", "J");
            words.Add("Э", "E");
            words.Add("Ю", "Yu");
            words.Add("Я", "Ya");
        }
		
		protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                ServerHooks.Join -= OJ;
                ServerHooks.Chat -= OC;
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
            if (Regex.IsMatch(player.Name, "[а-яА-ЯёЁ()]+"))
            {
                TShock.Utils.ForceKick(player, "Sorry, but your name has illegal characters", true, false);
				handler.Handled = true;
				return;
            }
        }
        private void OC(messageBuffer msg, int ply, string text, HandledEventArgs e)
        {
            if (e.Handled)
                return;

            var tsplr = TShock.Players[msg.whoAmI];
            if (tsplr == null)
            {
                e.Handled = true;
                return;
            }
            if (text.StartsWith("/"))
            {
                if (Regex.IsMatch(text, "[а-яА-ЯёЁ]+"))
                {
                    foreach (KeyValuePair<string, string> pair in words)
                    {
                        text = text.Replace(pair.Key, pair.Value);
                    }
                    try
                    {
                        e.Handled = Commands.HandleCommand(tsplr, text);
                    }
                    catch (Exception ex)
                    {
                        Log.ConsoleError("Command exception");
                        Log.Error(ex.ToString());
                    }
                }
                return;
            }
            else if (!tsplr.mute && Regex.IsMatch(text, "[а-яА-ЯёЁ]+"))
            {
                foreach (KeyValuePair<string, string> pair in words)
                {
                    text = text.Replace(pair.Key, pair.Value);
                }
                if (TShock.Config.EnableChatAboveHeads)
                {
                    TShock.Utils.Broadcast(ply, String.Format(TShock.Config.ChatAboveHeadsFormat, tsplr.Group.Name, tsplr.Group.Prefix, tsplr.Name, tsplr.Group.Suffix, text), tsplr.Group.R, tsplr.Group.G, tsplr.Group.B);
                }
                else
                {
                    TShock.Utils.Broadcast(String.Format(TShock.Config.ChatFormat, tsplr.Group.Name, tsplr.Group.Prefix, tsplr.Name, tsplr.Group.Suffix, text), tsplr.Group.R, tsplr.Group.G, tsplr.Group.B);
                }
                e.Handled = true;
            }
        }
    }
}
