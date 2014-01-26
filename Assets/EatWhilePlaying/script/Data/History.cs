using UnityEngine;
using System.Collections;
using Parse;
namespace EatWhilePlaying.Data{
[System.Serializable]

public class History{
	public System.DateTime time;
	public Troop troop;
	public string text;
	public string type;
	public int from;
	public int to;
	public Card card;
	public bool isTurnEnd;
	public History(){
		this.time=System.DateTime.Now;
	}
}
}
