/*
 * Created by SharpDevelop.
 * User: Ondra
 * Date: 09/03/2016
 * Time: 15:48
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
using System;

namespace TotalCommanderMacro
{
	/// <summary>
	/// Description of myValue.
	/// </summary>
	public class myValue
	{
		string _valName;

		public string ValName {
			get {
				return _valName;
			}
			set {
				_valName = value;
			}
		}

		string _value;

		public string Value {
			get {
				return _value;
			}
			set {
				_value = value;
			}
		}
		
		public myValue(string lineText)
		{
			string[] sp =lineText.Split(new string[] {"="},StringSplitOptions.RemoveEmptyEntries);
			_valName = sp[0];
			for (int i = 1; i <= sp.Length-1; i++) {
				_value += sp[i];
			}
		}
		
	}
}
