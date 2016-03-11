/*
 * Created by SharpDevelop.
 * User: Ondra
 * Date: 09/03/2016
 * Time: 15:49
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
using System;

namespace TotalCommanderMacro
{
	/// <summary>
	/// Description of myTab.
	/// </summary>
	public class myTab
	{
		public enum strana {L,R}
		public strana Strana { get; set;}

		string _pth;
		public string Pth {
			get {
				return _pth;
			}
		}

		bool _locked;
		public bool Locked {
			get {
				return _locked;
			}
		}

		public int InitialPosition { get; set;	}
		
		public myTab(string pth,int locked, int position,strana str )
		{
			_pth = pth;
			_locked = locked == 1;
			
			InitialPosition = position;
			Strana = str;
		}
		
		public myTab(string pth,bool locked, int position,strana str )
		{
			_pth = pth;
			_locked = locked;

			InitialPosition = position;
			Strana = str;
		}
	}
}
