/*
 * Created by SharpDevelop.
 * User: Ondra
 * Date: 09/03/2016
 * Time: 15:48
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
using System;
using System.Collections.Generic;

namespace TotalCommanderMacro
{
	/// <summary>
	/// Description of myAtribute.
	/// </summary>
	public class myAtribute
	{
		List<myValue> _valuesList = new List<myValue>();

		public List<myValue> ValuesList {
			get {
				return _valuesList;
			}
			set {
				_valuesList = value;
			}
		}
		string _name;

		public string Name {
			get {
				return _name;
			}
		}
		
		public myAtribute(string name)
		{
			_name = name;
		}

		public void AddValue (myValue val) {
			_valuesList.Add(val);
		}

	}
}
