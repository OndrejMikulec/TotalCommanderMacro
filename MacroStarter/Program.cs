﻿/*
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
		const string sign = @"Author: Ondrej Mikulec
Vsetin, Czech Republic
o.mikulec@seznam.cz
Mikulec.Ondrej@gmail.com

";
		
		const string help = @"Valid arguments example: 
""-Clear"" ""-LUP c:\Users\Ondra\Documents\SharpDevelop Projects"" ""-RUA c:\Users\Ondra"" ""-RLP c:\Users\Ondra\Documents\SharpDevelop Projects""

Must have """"
Target folder without \ at the end
-LUP left table unlocked push
-RUA right table unlocked add
-LLP left table lock tab push
-RlA right table lock tab add
-Clear clear all unlocked tabs

Macro is looking for standart TC and ini file locations. You can difine your locations in files INIDefinedPath.ini and TCDefinedPath.ini
";
		
		public static void Main(string[] args)
		{
			Console.Write(sign);
			
			if (args.Length<1||args[0].ToLower()=="help") {
				Console.Write(help);
				Console.ReadKey(false);
				return;
			}
			
			List<string> argsL = args.ToList();
			bool clear = argsL.RemoveAll(item => item.ToLower() == "-clear") > 0;
			
			List<myTab> tabsPush = new List<myTab>();
			List<myTab> tabsAdd = new List<myTab>();
			foreach (string arg in argsL) {
				myTab.strana table = myTab.strana.L;
				if (arg.Substring(1, 1) == "R")
					table = myTab.strana.R;
				
				bool locked = arg.Substring(2,1).ToLower()=="l";
				
				if (arg.Substring(3, 1).ToLower() == "p")
					tabsPush.Add(new myTab(arg.Substring(5), locked, 0, table));
				if (arg.Substring(3, 1).ToLower() == "a")
					tabsAdd.Add(new myTab(arg.Substring(5), locked, 0, table));

			}
			
			if (!TabCheck(tabsPush)) {
				Console.Write(@"Some of input paths do not exists or the arguments are fault!"); 
				Console.ReadKey(false);
				return;
			}
			
			
			myINI oMyIni = myTotalCommander.CloseTCGetIni();
			
			if (oMyIni == null)
				return;
			
			if (clear) {
				oMyIni.DeleteUnlockedTabsL();
				oMyIni.DeleteUnlockedTabsR();
			}
			
			foreach (myTab tb in tabsPush)
				oMyIni.PushTabToFirst(tb);
			
			foreach (myTab tb in tabsAdd)
				oMyIni.AddTabToLast(tb);
			
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