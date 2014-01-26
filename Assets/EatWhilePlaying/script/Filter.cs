using UnityEngine;
using System.Collections;
namespace EatWhilePlaying{
public class Filter:MonoBehaviour{
	public enum Amount{all,greaterThanZero,zero}
	public enum Type{all,hero,chess,magic,item}
	public enum Sorting{time,name,id,life,ad,mp,rarity}
	public enum Troop{all,notInTroop}
	public bool sortingReverse=false;
	public bool showHero=true;
	public bool showChess=true;
	public bool showMagic=true;
	public bool showItem=true;
	public UIPopupList sorting;
}
}