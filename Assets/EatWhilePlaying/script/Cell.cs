using UnityEngine;
using TRNTH;
namespace EatWhilePlaying{
[RequireComponent(typeof(NguiAnchor))]
public class Cell : Battle{
	static public int Distance(Cell a,Cell b){
		// return Mathf.FloorToInt((a.pos-b.pos).magnitude/CellSetter.radius+0.1f);
		return Mathf.FloorToInt((a.pos-b.pos).magnitude);
	}
	public int index;
	public float x;
	public float y;
	[HideInInspector]public Cell[] neighbors;
	internal string tag="";
	internal float score=0f;
	internal bool isNear(string tag){
		foreach(var e in neighbors){
			if(e.tag==tag)return true;
		}
		return false;
	}
	internal Cell find(string tag){
		foreach(var e in neighbors){
			// Debug.Log(tag+"_"+e.tag);
			if(e.tag==tag)return e;
		}
		return null;
	}
	void Awake(){
		base.Awake();
		nguiBc=Instantiate(nguiBcPrefab) as NguiButton;
		nguiBc.name=nguiBcPrefab.name;
		nguiBc.target=bm.gameObject;
		nguiBc.cell=this;
		var na=GetComponent<NguiAnchor>();
		na.ngui=nguiBc.transform;
	}
}
}