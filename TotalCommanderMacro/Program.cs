/*
 * Created by SharpDevelop.
 * User: Ondra
 * Date: 09/03/2016
 * Time: 15:47
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
using System;
using System.Linq;


namespace TotalCommanderMacro
{
	class Program
	{

		public static void Main(string[] args)
		{
			myINI oMyIni = myTotalCommander.CloseTCGetIni();
			oMyIni.DeleteUnlockedTabsL();
			oMyIni.DeleteUnlockedTabsR();
			oMyIni.AddTab(new myTab(@"c:\Users\Ondra\",0,0,myTab.strana.L));
			oMyIni.AddTab(new myTab(@"c:\Users\Ondra\",0,0,myTab.strana.R));
			oMyIni.Save();
			myTotalCommander.RunTotalCommander();
		}
	}
}