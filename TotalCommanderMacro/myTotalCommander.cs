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
using System.Linq;
using System.Windows.Forms;
using System.Text;

namespace TotalCommanderMacro
{

	public static class myTotalCommander
	{

		public enum Side {L,R}
		
		static string iniPath;
		

		public static myINI CloseTCGetIni()
		{
			
			bool cancel = false;
			
			Process[] processNames = Process.GetProcessesByName("TOTALCMD64");
			Process proc = null;
			if (processNames.Length>1) {
				Form1 f = new Form1(processNames);
				if (f.ShowDialog() == DialogResult.OK) {
					proc = processNames[f.listBox1.SelectedIndex];
				} else {
					cancel = true;
				}
			}
			if (processNames.Length==1) {
				proc = processNames[0];
			}
			
			if (cancel) {
				return null;
			}
			
			if (processNames.Length!=0) {
				proc.CloseMainWindow();
				System.Threading.Thread.Sleep(500);
			}
			
			
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
			
			string text = null;
			int count = 100;
			bool ok = false;
			while (!ok&&count>0) {
				try {
					using (var rd = new StreamReader(iniPath,Encoding.Default)) {
						text = rd.ReadToEnd();
					}
					ok = true;
				} catch  {
					count--;
					Console.WriteLine(iniPath+" is using by another process! "+count+" attempts.");
				}				
			}
			
			if (!ok) {
				Console.ReadKey(false);
				return;
			}
			

			ProcessStartInfo processStartInfo = new ProcessStartInfo();
    		processStartInfo.FileName = TCPath;
    		processStartInfo.Arguments = @"/I="+iniPath;
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
