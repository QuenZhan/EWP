using UnityEngine;
using System.Collections;
using System.Collections.Generic;
namespace EatWhilePlaying.Master{
public class DeckEdit:Master{
	internal Data.Troop troop{get{return troops[0];}}
	public int indexTroop=0;
	public Deck from;
	public Deck to;
	public Card card;
	public Deck dLibrary;
	public Deck dTroop;
	public Card ucDetail;
	public Card ucHero;
	[ContextMenu("drop")]
	public void drop(){
		if(from&&to&&card);
		else return;
		if(from==to)return;
		if(to==dTroop){
			switch(card.card.type){
			case"hero":
				troop.setHero(card.card.name);
				break;
			default:
				troop.add(card.card.name);
				break;
			}
		}
		if(to==dLibrary){
			troop.remove(card.card.name);
		}
		from=null;
		to=null;
		card=null;
		dTroop.refresh(troop.cards);
	}
	[ContextMenu("refresh")]
	public void refresh(){
		if(troops.Length==0)troops=parseApi.troops;
		dLibrary.refresh(Data.Card.cards);
		dTroop.refresh(troop.cards);
		// ucDetail.card=troop.hero;
		ucHero.card=troop.hero;
	}
	Data.Troop[] troops={};
	void Awake(){
	}
	void OnEnable(){
		// troops=parseApi.troops;
		// if(dTroop=null)Debug.Log("dTroop 為必要項目");
		// if(dLibrary=null)Debug.Log("dLibrary 為必要項目");
		// if(ucDetail=null)Debug.Log("ucDetail 為必要項目");
	}
	void Update(){
		switch(sur){
		case"drop":drop();break;
		case"refresh":refresh();break;
		case"clear":
			troop.cards=new Data.Card[]{};
			refresh();
			;break;
		}
		sur="";
	}
	void OnDisable(){
		parseApi.updateTroops(troops);
		Worldmap.troop=troop;
	}
}
}
