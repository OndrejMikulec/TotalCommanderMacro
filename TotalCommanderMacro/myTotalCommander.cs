/*
 * Created by SharpDevelop.
 * User: Ondra
 * Date: 09/03/2016
 * Time: 16:31
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
using System;
using System.IO;
using System.Diagnostics;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Linq;
using System.Windows.Forms;

namespace TotalCommanderMacro
{

	public static class myTotalCommander
	{
		static string[] TCLocations = {
			@"c:\Program Files (x86)\totalcmd\TOTALCMD64.EXE",
			@"c:\Program Files\totalcmd\TOTALCMD64.EXE",
			@"c:\totalcmd\\TOTALCMD64.EXE"
		};
		
		public static string getTCLocation()
		{
			foreach (var loc in TCLocations) {
				if (File.Exists(loc)) {
					return loc;
				}
			}
			return null;
		}
		
		static string[] INILocations = {
			Path.GetDirectoryName( Environment.GetFolderPath(Environment.SpecialFolder.Personal)) + @"\wincmd.ini",
			Path.GetDirectoryName( Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData)) + @"\GHISLER\wincmd.ini",
			Path.GetDirectoryName( Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData)) + @"\Roaming\GHISLER\wincmd.ini"
		};
		
		public enum Side {L,R}
		
		[DllImport("user32.dll")]
		public static extern bool GetWindowRect(IntPtr hwnd, ref Rect rectangle);
		
		public struct Rect {
   		public int Left { get; set; }
	   	public int Top { get; set; }
	   	public int Right { get; set; }
	   	public int Bottom { get; set; }
   		public int Width { get { return Right-Left;}}
	   	public int Height { get { return Bottom-Top;}}
	   	public bool HasValue { get; set;}
		}
		
		[STAThreadAttribute]
		public static myINI CloseTCGetIni()
		{
			
			Process[] processNames = Process.GetProcessesByName("TOTALCMD64");
			if (processNames.Length>1) {
				MessageBox.Show("Aplikace pracuje pouze s jedním otevřeným Total Commanderem!");
				return null;
			}
			
			var winRect = new Rect();
			if (processNames.Length==1) {
				IntPtr ptr = processNames[0].MainWindowHandle;
				
				GetWindowRect(ptr, ref winRect);
				winRect.HasValue = true;
				processNames[0].CloseMainWindow();
			} else {
				winRect.HasValue = false;
			}
			
			var iniList = new List<FileInfo>();
			foreach (var fil in INILocations) {
				if (File.Exists(fil)) {
					iniList.Add(new FileInfo(fil));
				}
			}
			
			if (iniList.Count==0) {
				MessageBox.Show("Ini soubor nenalezen!");
				return null;
			}
			
			FileInfo ini = iniList.Find( item => item.LastWriteTime==iniList.Max(item2 => item2.LastWriteTime));
			
			return new myINI(ini.FullName);
			
		}
		
		public static void RunTotalCommander()
		{
			foreach (string loc in TCLocations) {
				if (File.Exists(loc)) {
					ProcessStartInfo processStartInfo = new ProcessStartInfo();
		    		processStartInfo.FileName = loc;
		   			 Process.Start(processStartInfo);
		   			 return;
				}
			}
			
			MessageBox.Show("Total Commander nebyl nalezen!");
			
			
		}

	}
}
