/*
 * Created by SharpDevelop.
 * User: Ondra
 * Date: 11/03/2016
 * Time: 17:21
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
using System;
using System.Drawing;
using System.Windows.Forms;
using System.Diagnostics;
using System.Runtime.InteropServices;

namespace TotalCommanderMacro
{
	/// <summary>
	/// Description of Form1.
	/// </summary>
	public partial class Form1 : Form
	{
		[DllImport("user32.dll")]
		private static extern bool SetForegroundWindow(IntPtr hWnd);
		
		Process[] _processNames;
		
		public Form1(Process[] processNames)
		{
			_processNames = processNames;
			
			InitializeComponent();
			
			foreach (Process prc in _processNames) {
				listBox1.Items.Add(prc.MainWindowTitle + " " + prc.StartTime);
			}
			
			
		}
		void ListBox1SelectedIndexChanged(object sender, EventArgs e)
		{
			SetForegroundWindow(_processNames[listBox1.SelectedIndex].MainWindowHandle);
		}

	}
	

}
