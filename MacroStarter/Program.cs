/*
 * Created by SharpDevelop.
 * User: Ondra
 * Date: 10/03/2016
 * Time: 08:36
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
using System;
using TotalCommanderMacro;
using System.Collections.Generic;
using System.Linq;
using System.IO;

namespace MacroStarter
{
	class Program
	{
		const string help = @"Valid arguments example: 
""-Clear"" ""-LU c:\Users\Ondra\Documents\SharpDevelop Projects"" ""-RU c:\Users\Ondra"" ""-RL c:\Users\Ondra\Documents\SharpDevelop Projects""

Must have """"
Target folder without \ at the end
-LU left table unlocked
-RU right table unlocked
-LL left table lock tab
-Rl right table lock tab
-Clear clear all unlocked tabs

For keep TC window size and position, you must first once save position in TC configuration menu.

Macro is looking for standart TC and ini file locations. You can difine your locations in files INIDefinedPath.ini and TCDefinedPath.ini";
		
		public static void Main(string[] args)
		{
			
			if (args.Length<1||args[0]=="help") {
				Console.Write(help);
				Console.ReadKey(false);
				return;
			}
			
			List<string> argsL = args.ToList();
			bool clear = argsL.RemoveAll(item => item == "-Clear") > 0;
			
			List<myTab> tabs = new List<myTab>();
			foreach (string arg in argsL) {
				myTab.strana table = myTab.strana.L;
				if (arg.Substring(1, 1) == "R")
					table = myTab.strana.R;
				
				bool locked = arg.Substring(2,1)=="L";
				
				tabs.Add(new myTab(arg.Substring(4),locked,0,table));

			}
			
			if (!TabCheck(tabs)) {
				Console.Write(@"Some of input paths do not exists or the arguments are fault!"); 
				Console.ReadKey(false);
				return;
			}
			
			int positionL = 0;
			int positionR = 0;
			foreach (myTab tb in tabs) {
				if (tb.Strana == myTab.strana.L) {
					tb.Position = positionL;
					positionL++;
				}
				if (tb.Strana == myTab.strana.R) {
					tb.Position = positionR;
					positionR++;
				}
			}
			
			myINI oMyIni = myTotalCommander.CloseTCGetIni();
			
			if (clear) {
				oMyIni.DeleteUnlockedTabsL();
				oMyIni.DeleteUnlockedTabsR();
			}
			
			foreach (myTab tb in tabs)
				oMyIni.AddTab(tb);
			
			oMyIni.Save();
			myTotalCommander.RunTotalCommander();
		}
		
		static bool TabCheck(List<myTab> tabs)
		{
			foreach (myTab tb in tabs) {
				if (!Directory.Exists(tb.Pth))
					return false;
			}
			
			return true;
		}
	}
}