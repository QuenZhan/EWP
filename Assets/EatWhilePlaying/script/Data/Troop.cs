using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using TRNTH;
namespace EatWhilePlaying.Data{
[System.Serializable]
public class Troop{
	static public Troop player{get{
		var troop=new Troop();
		troop.ghost=Ghost.player;
		troop.party="host";
		troop.ghost.party=troop.party;
		return troop;
	}}
	public Troop(){
		this.ghost.party=this.party;
		this.hpTroop.length=5;
	}
	public string name="預設";
	public string party="challenger";
	public Card[] cards={new Card(),new Card(),new Card()};
	public Card hero=new Card();
	public RadioBase hpTroop=new RadioBase();
	public RadioBase hpHero=new RadioBase();
	public List<CardHand> hand=new List<CardHand>();
	public Ghost ghost=new Ghost();
	public bool contains(Card card){
		var list=new List<Card>(cards);
		list.Add(hero);
		foreach(var e in list.ToArray()){
			if(e.name==card.name)return true;
		}
		return false;
	}
	public void modHand(int amount){}
	public void setHero(string name){
		hero=Card.findByName(name);
	}
	public int add(string name){
		return setCards(name,true);
	}
	public int remove(string name){
		return setCards(name,false);
	}
	int setCards(string name,bool add){
		var list=new List<string>();
		foreach(var e in cards)list.Add(e.name);
		if(add){
			if(!list.Contains(name))list.Add(name);
		}else{
			list.Remove(name);
		}
		cards=new Card[list.Count];
		for(int i=0;i<list.Count;i++)cards[i]=Card.findByName(list[i]);
		return cards.Length;
	}
	internal Vector3 coorCh(){
		var paddingH=30;
		var paddingV=70;
		var dd=1;
		Vector3 vec;
		switch(party){
		case"host":vec=new Vector3(paddingH,Screen.height-paddingV,0);break;
		default:
			vec=new Vector3(Screen.width-paddingH,Screen.height-paddingV,0);
			dd=-1;
			break;
		}
		for(int i=0;i<hand.Count;i++){
			hand[i].coorTarget=vec+Vector3.right*i*15*dd;
		}
		return vec;
	}
	internal void coorChess(Chess chess){
		float radius=100;
		float degreeStart=90;
		float rrr=0;
		float ur=360.0f/hand.Count;
		for(int i=0;i<hand.Count;i++){
			rrr=(degreeStart+i*ur)*Mathf.Deg2Rad;
			hand[i].coorTarget=chess.coor+new Vector3(Mathf.Cos(rrr)*radius,Mathf.Sin(rrr)*radius,0);
		}
	}
}
}