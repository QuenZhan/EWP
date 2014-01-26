using UnityEngine;
using System.Collections.Generic;
namespace EatWhilePlaying{
[System.Serializable]
public class Log{
	static List<string> lMsg=new List<string>();
	public string msgLast="";
	public void push(string str){
		lMsg.Add(str);
		msgLast=str;
		Debug.Log(str);
	}
}
}