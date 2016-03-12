/*
 * Created by SharpDevelop.
 * User: val01039
 * Date: 8.3.2016
 * Time: 9:09
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
using System;
using System.IO;
using System.Collections.Generic;
using System.Text;
using System.Linq;

namespace TotalCommanderMacro
{
	/// <summary>
	/// Description of myINI.
	/// </summary>
	public class myINI
	{
		string _iniPath;
		public string IniPath {
			get {
				return _iniPath;
			}
		}

		List<myAtribute> _atributesList = new List<myAtribute>();
		public List<myAtribute> AtributesList {
			get {
				return _atributesList;
			}
		}
		
		List<myTab> _leftTabs;
		public List<myTab> LeftTabs {
			get {
				return _leftTabs;
			}
		}
		
		List<myTab> _rightTabs;
		public List<myTab> RightTabs {
			get {
				return _rightTabs;
			}
		}
		
		
		public myINI(string iniPath)
		{
			_iniPath = iniPath;

			string text = null;
			int count = 10;
			bool ok = false;
			while (!ok&&count>0) {
				try {
					using (var rd = new StreamReader(_iniPath,Encoding.Default)) {
						text = rd.ReadToEnd();
					}
					ok = true;
				} catch  {
					count--;
					Console.WriteLine(_iniPath+" is using by another process! "+count+" attempts.");
				}				
			}
			
			if (!ok) {
				Console.ReadKey(false);
				return;
			}

			
			
			string[] lines = text.Split(new string[] {Environment.NewLine},StringSplitOptions.RemoveEmptyEntries);
			foreach (var str in lines) {
				string strTrim = str.Trim();
				if (strTrim.IndexOf('[')==0&&strTrim.IndexOf(']')==strTrim.Length-1) {
					_atributesList.Add(new myAtribute(strTrim));
					continue;
				}
				if (_atributesList.Count>0) {
					_atributesList[_atributesList.Count-1].AddValue(new myValue(strTrim));
				}
			}
			
			object[] tabs = GetTabs();
			_leftTabs = (List<myTab>)tabs[0];
			_rightTabs = (List<myTab>)tabs[1];
			
		}
		
		public void Save()
		{
			setTabsToIni();
			
			int count = 100;
			bool ok = false;
			while (!ok&&count>0) {
				try {
					using (var wr = new StreamWriter(_iniPath,false,Encoding.Default)) {
						foreach (myAtribute at in _atributesList) {
							if (!string.IsNullOrEmpty(at.Name)) {
								wr.WriteLine(at.Name);
								foreach (myValue vl in at.ValuesList) {
									if (!string.IsNullOrEmpty(vl.ValName)&&!string.IsNullOrEmpty(vl.Value)) {
										wr.WriteLine(vl.ValName+"="+vl.Value);
									}
								}
							}
						}						
					}
					ok = true;
				} catch  {
					count--;
					Console.WriteLine(_iniPath+" is using by another process! "+count+" attempts.");
				}				
			}
			
			if (!ok) {
				Console.ReadKey(false);
				return;
			}
			
		}
		
		object[] GetTabs()
		{
		
  			myAtribute right = AtributesList.Find(item => item.Name == "[right]");
  			myAtribute left = AtributesList.Find(item => item.Name == "[left]");
  			myAtribute rightTabs = AtributesList.Find(item => item.Name == "[righttabs]");
  			myAtribute leftTabs = AtributesList.Find(item => item.Name == "[lefttabs]");
  			
  			List<myTab> tabsListRight = new List<myTab>();
  			List<myTab> tabsListLeft = new List<myTab>();
  			
  			if (rightTabs!=null) {
      			for (int i = 0; i <= rightTabs.ValuesList.Count-1; i++) {
  					if (rightTabs.ValuesList[i].ValName.Contains("_path")) {
  						tabsListRight.Add(new myTab(rightTabs.ValuesList[i].Value,
  						                        int.Parse(getTabValue( rightTabs.ValuesList[i+1].Value,5)),
  						                       int.Parse(rightTabs.ValuesList[i+1].ValName.Substring(0,1)),myTab.strana.R));
      				}
      			}
  			}
  			
  			if (leftTabs!=null) {
      			for (int i = 0; i <= leftTabs.ValuesList.Count-1; i++) {
  					if (leftTabs.ValuesList[i].ValName.Contains("_path")) {
  						tabsListLeft.Add(new myTab(leftTabs.ValuesList[i].Value,
  						                        int.Parse(getTabValue( leftTabs.ValuesList[i+1].Value,5)),
  						                       int.Parse(leftTabs.ValuesList[i+1].ValName.Substring(0,1)),myTab.strana.L));
      				}
      			}
  			}
  			
  			if (right!=null) {
  				string pth = right.ValuesList.Find(item => item.ValName == "path").Value;
  				
  				int locked = 0;
  				int position = 0;
  				if (rightTabs!=null) {
  					if (rightTabs.ValuesList.Find(item => item.ValName == "activelocked")!=null) {
  						locked = int.Parse(rightTabs.ValuesList.Find(item => item.ValName == "activelocked").Value);
  					}
  					if (rightTabs.ValuesList.Find(item => item.ValName == "activetab")!=null) {
  						position =  int.Parse(rightTabs.ValuesList.Find(item => item.ValName == "activetab").Value);
  					}
  				}

				tabsListRight.Add(new myTab(pth,
				                       locked,
				                       position,
									myTab.strana.R));

  			}
  			
  			
  			if (left!=null) {
  				string pth = left.ValuesList.Find(item => item.ValName == "path").Value;
  				
  				int locked = 0;
  				int position = 0;
  				if (leftTabs!=null) {
  					if (leftTabs.ValuesList.Find(item => item.ValName == "activelocked")!=null) {
  						locked = int.Parse(leftTabs.ValuesList.Find(item => item.ValName == "activelocked").Value);
  					}
  					if (leftTabs.ValuesList.Find(item => item.ValName == "activetab")!=null) {
  						position =  int.Parse(leftTabs.ValuesList.Find(item => item.ValName == "activetab").Value);
  					}
  				}

				tabsListLeft.Add(new myTab(pth,
				                       locked,
				                       position,
									myTab.strana.L));

  			}
  			
  			return new object[] {tabsListLeft,tabsListRight};
		}
		
		public void DeleteUnlockedTabsL()
		{
			_leftTabs.RemoveAll(item => !item.Locked);
		}
		
		public void DeleteUnlockedTabsR()
		{
			_rightTabs.RemoveAll(item => !item.Locked);
		}
		
		
		public void PushTabToFirst(myTab oMyTab)
		{
			
			if (oMyTab.Strana == myTab.strana.L) {
				List<myTab> tempList = new List<myTab>();
				tempList.Add(oMyTab);
				foreach (myTab tb in _leftTabs) {
					tempList.Add(tb);
				}
				_leftTabs = tempList;
			}
			
			if (oMyTab.Strana == myTab.strana.R) {
				List<myTab> tempList = new List<myTab>();
				tempList.Add(oMyTab);
				foreach (myTab tb in _rightTabs) {
					tempList.Add(tb);
				}
				_rightTabs = tempList;
			}			
		}
		
		public void AddTabToLast(myTab oMyTab)
		{
			
			if (oMyTab.Strana == myTab.strana.L)
				_leftTabs.Add(oMyTab);
			
			if (oMyTab.Strana == myTab.strana.R)
				_rightTabs.Add(oMyTab);				
		}
		
		
		void setTabsToIni()
		{
			AtributesList.RemoveAll(item => item.Name == "[right]");
  			AtributesList.RemoveAll(item => item.Name == "[left]");
  			AtributesList.RemoveAll(item => item.Name == "[righttabs]");
  			AtributesList.RemoveAll(item => item.Name == "[lefttabs]");

  			
  			myAtribute right = new myAtribute("[right]");
  			if (_rightTabs.Count>0) {
  				right.ValuesList.Add(new myValue(@"path="+_rightTabs[0].Pth));
  				right.ValuesList.Add(new myValue(@"show=1"));
  			}
  			myAtribute rightTabs = new myAtribute("[righttabs]");
  			if (_rightTabs.Count>1) {
				for (int i = 1; i <= _rightTabs.Count-1; i++) {
					rightTabs.ValuesList.Add(new myValue(i-1+"_path="+_rightTabs[i].Pth));
					if (_rightTabs[i].Locked) {
						rightTabs.ValuesList.Add(new myValue(i-1+"_options=1|0|0|0|0|1|0"));
					} else {
						rightTabs.ValuesList.Add(new myValue(i-1+"_options=1|0|0|0|0|0|0"));
					}
				}  				
  			}
  			if (_rightTabs.Count>0) {
				rightTabs.ValuesList.Add(new myValue(@"activetab=0"));
				if (_rightTabs[0].Locked) {
					rightTabs.ValuesList.Add(new myValue(@"activelocked=1"));
				} else{
					rightTabs.ValuesList.Add(new myValue(@"activelocked=0"));
				}
  			}

  			
			if (right != null)
				_atributesList.Add(right);
			
			if (rightTabs != null)
				_atributesList.Add(rightTabs);
			
			
  			
  			myAtribute left = new myAtribute("[left]");
  			if (_leftTabs.Count>0) {
  				left.ValuesList.Add(new myValue(@"path="+_leftTabs[0].Pth));
  				left.ValuesList.Add(new myValue(@"show=1"));
  			}
  			myAtribute leftTabs = new myAtribute("[leftTabs]");
  			if (_leftTabs.Count>1) {
  				for (int i = 1; i <= _leftTabs.Count-1; i++) {
  					leftTabs.ValuesList.Add(new myValue(i-1+"_path="+_leftTabs[i].Pth));
  					if (_leftTabs[i].Locked) {
  						leftTabs.ValuesList.Add(new myValue(i-1+"_options=1|0|0|0|0|1|0"));
  					} else {
  						leftTabs.ValuesList.Add(new myValue(i-1+"_options=1|0|0|0|0|0|0"));
  					}
  				}
			}
  			if (_leftTabs.Count>0) {
				leftTabs.ValuesList.Add(new myValue(@"activetab=0"));
				if (_leftTabs[0].Locked) {
					leftTabs.ValuesList.Add(new myValue(@"activelocked=1"));
				} else{
					leftTabs.ValuesList.Add(new myValue(@"activelocked=0"));
				}
  			}
  			
  			
			if (left != null)
				_atributesList.Add(left);
			
			if (leftTabs != null)
				_atributesList.Add(leftTabs);
  			

		}
		
		string getTabValue(string value, int position)
		{
			string rv = value.Split(new char[]{'|'},StringSplitOptions.RemoveEmptyEntries)[position];
			return rv;
			
		}

	}

}
