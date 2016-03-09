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
		strana _str;

		public strana Strana {
			get {
				return _str;
			}
		}

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

		int _position;

		public int Position {
			get {
				return _position;
			}
			set {
				_position = value;
			}
		}
		public myTab(string pth,int locked, int position,strana str )
		{
			_pth = pth;
			if (locked==1) {
				_locked = true;
			} else {
				_locked = false;
			}
			
			_position = position;
			_str = str;
		}
	}
}
