using UnityEngine;
using System.Collections;
namespace EatWhilePlaying{
public class Deck:MonoBehaviour{
	public bool refreshNow=false;
	public Master.DeckEdit deckedit;
	public Card ucPrefab;
	public Filter filter;
	public UITable table;
	public int refresh(){
		return refresh(_cards);
	}
	public int refresh(Data.Card[] cards){
		_cards=cards;
		var tra=table.transform;
		foreach(Transform e in tra)Destroy(e.gameObject);
		var ucSeed=ucPrefab;
		foreach(var e in cards){
			// Debug.Log(e);
			bool isHidden=false;
			if(filter){
				
			}
			if(isHidden)continue;
			var uCard=Instantiate(ucSeed) as Card;
			uCard.card=e;
			uCard.deck=this;
			uCard.transform.parent=tra;
			uCard.transform.localScale=ucSeed.transform.localScale;
			uCard.deckedit=deckedit;
			if(filter){
				// switch(filter.sorting){
				// case Filter.Sorting.name:	uCard.name=e.name;	break;
				// case Filter.Sorting.id:		uCard.name=e.id;	break;
				// case Filter.Sorting.life:	uCard.name=e.life+"";	break;
				// case Filter.Sorting.ad:		uCard.name=e.attackDamage+"";	break;
				// case Filter.Sorting.mp:		uCard.name=e.magicPower+"";	break;
				// case Filter.Sorting.rarity:	uCard.name=e.rarity+"";	break;
				// case Filter.Sorting.time:	uCard.name=e.time;	break;
				switch(filter.sorting.value){
				case "name":	uCard.name=e.name;	break;
				case "id":		uCard.name=e.id;	break;
				case "life":	uCard.name=e.life+"";	break;
				case "attack":		uCard.name=e.attackDamage+"";	break;
				case "magic":		uCard.name=e.magicPower+"";	break;
				case "rarity":	uCard.name=e.rarity+"";	break;
				case "time":	uCard.name=e.time;	break;
				}
				if(deckedit.troop.contains(e))uCard.transform.localScale=Vector3.one*0.5f;
				// switch(filter.troopFilter.value){
				// case"HideInTroop":
					// break;
				// }
			}
		}
		table.repositionNow=true;
		return tra.childCount;
	}
	Data.Card[] _cards;
	void Update(){
		if(refreshNow)refresh();
		refreshNow=false;
	}
}
}