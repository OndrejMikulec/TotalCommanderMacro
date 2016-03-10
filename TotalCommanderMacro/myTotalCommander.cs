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
using System.Text;

namespace TotalCommanderMacro
{

	public static class myTotalCommander
	{

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
		

		public static myINI CloseTCGetIni()
		{
			
			Process[] processNames = Process.GetProcessesByName("TOTALCMD64");
			if (processNames.Length>1) {
				MessageBox.Show("Macro is writed for only one TC running!");
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
			
			string iniPath = null;
			bool definedINI = false;
			readDefinedLocations( getDefinedINIPath(),out iniPath, out definedINI);
			
			if (!definedINI) {
				iniPath = autoDetectionINI(out definedINI);
			}
			
			if (!definedINI) {
				MessageBox.Show("Ini not found!");
				return null;
			}

			myINI oMyIni = new myINI(iniPath);
			
			if (winRect.HasValue) {
      			System.Drawing.Rectangle rec = System.Windows.Forms.Screen.PrimaryScreen.Bounds;
      			myAtribute scr = oMyIni.AtributesList.Find(item => item.Name.Contains(rec.Width+"x"+rec.Height));
      			if (scr!=null) {
      				myValue x = scr.ValuesList.Find(item => item.ValName == "x");
      				x.Value = winRect.Left.ToString();
      				myValue y = scr.ValuesList.Find(item => item.ValName == "y");
      				y.Value = winRect.Top.ToString();
      				myValue dx = scr.ValuesList.Find(item => item.ValName == "dx");
      				dx.Value = winRect.Width.ToString();
      				myValue dy = scr.ValuesList.Find(item => item.ValName == "dy");
      				dy.Value = winRect.Height.ToString();
      			} 
  			}
			
			return oMyIni;
			
		}
		

		
		public static void RunTotalCommander()
		{
			string TCPath = null;
			bool definedTC = false;
			readDefinedLocations( getDefinedTCPath(),out TCPath, out definedTC);
			
			if (!definedTC) {
				TCPath = autoDetectionTC(out definedTC);
			}
			
			if (!definedTC) {
				MessageBox.Show("TC not found!");
				return;
			}
			

			ProcessStartInfo processStartInfo = new ProcessStartInfo();
    		processStartInfo.FileName = TCPath;
   			 Process.Start(processStartInfo);
   			 return;

			


		}
		
		static void readDefinedLocations(string path ,out string output, out bool ok)
		{
			if (!File.Exists(path)) {
				using (File.Create(path)) {	}
			}
			
			try {
				using (StreamReader rdr = new StreamReader(path,Encoding.Default)) {
					output = rdr.ReadLine().Trim();
					ok = File.Exists(output);
				}
			} catch {
				output = null;
				ok = false;
			}
		}
		
		static string getEXEDirectory()
		{
			string exePath = System.Reflection.Assembly.GetExecutingAssembly().Location;
        	return Path.GetDirectoryName(exePath);
		}
		
		static string getDefinedTCPath()
		{
        	return getEXEDirectory() + @"\TCDefinedPath.ini";
		}
				
		static string getDefinedINIPath()
		{
        	return getEXEDirectory() + @"\INIDefinedPath.ini";
		}
		
		static readonly string[] INILocations = {
			Path.GetDirectoryName( Environment.GetFolderPath(Environment.SpecialFolder.Personal)) + @"\wincmd.ini",
			Path.GetDirectoryName( Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData)) + @"\GHISLER\wincmd.ini",
			Path.GetDirectoryName( Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData)) + @"\Roaming\GHISLER\wincmd.ini"
		};
		static string autoDetectionINI(out bool ok)
		{
			var iniList = new List<FileInfo>();
			foreach (var fil in INILocations) {
				if (File.Exists(fil)) {
					iniList.Add(new FileInfo(fil));
				}
			}
			
			if (iniList.Count < 1) {
				ok = false;
				return null;
			}
			
			FileInfo ini = iniList.Find( item => item.LastWriteTime==iniList.Max(item2 => item2.LastWriteTime));
			ok = true;
			return ini.FullName;
		}
		
		static readonly string[] TCLocations = {
			@"c:\Program Files (x86)\totalcmd\TOTALCMD64.EXE",
			@"c:\Program Files\totalcmd\TOTALCMD64.EXE",
			@"c:\totalcmd\\TOTALCMD64.EXE"
		};
		
		public static string autoDetectionTC(out bool ok)
		{
			foreach (var loc in TCLocations) {
				if (File.Exists(loc)) {
					ok = true;
					return loc;
				}
			}
			ok = false;
			return null;
		}

	}
}
