using UnityEngine;
using System.Collections;
namespace EatWhilePlaying{
[ExecuteInEditMode]
public class Card : UIDragDropItem{
	public Data.Card data{
		get{return _card;}
	}
	public Data.Card card{
		get{
			return _card;
		}
		set{
			var card=value;
			if(card==null)return;
			this._card=card;
			lName.text=card.name;
		}
	}
	public UILabel lName;
	internal Master.DeckEdit deckedit;
	internal Deck deck;
	Data.Card _card;
	void select(){
		if(!deckedit)return;
		deckedit.card=this;
		deckedit.ucDetail.card=_card;
		deckedit.from=deck;
	}
	void OnPress(){
		select();
	}
	protected override void OnDragDropStart(){
		select();
	}
	protected override void OnDragDropRelease (GameObject surface){
		Deck container = surface ? NGUITools.FindInParents<Deck>(surface) : null;
		if (container&&deckedit){
			deckedit.to=container;
			deckedit.drop();
			deck.refreshNow=true;
			deckedit.sur="refresh";
		}
	}
}
}