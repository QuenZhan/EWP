using UnityEngine;
using System.Collections;
using EatWhilePlaying.Data;
namespace EatWhilePlaying.Master{
public class Tutorial:Worldmap{
	public string troopName="紅鴉騎士團";
	public string nameOfHero="馬丹那";
	public void setup(){
		base.setup();
		Troop troop=new Data.Troop();
		troop.hero=Data.Card.findByName(nameOfHero);
		troop.name=troopName;
		troop.ghost=Ghost.player;
		BattleMaster.host=troop;
	}
}
}