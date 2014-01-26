using UnityEngine;
using System.Collections.Generic;
namespace EatWhilePlaying.Master{
public class Worldmap : Master{
	static public Data.Troop troop=null;
	public Quest quest;
	Data.Troop makeTroop(){
		// if(!quest)return null;
		var troop=new Data.Troop();
		troop.hero=Data.Card.findByName(quest.nameOfEnemyHero);
		troop.name=quest.nameOfEnemyTroop;
		var list=new List<Data.Card>();
		foreach(var e in quest.cards){
			list.Add(Data.Card.find(e));
		}
		troop.cards=list.ToArray();
		return troop;
	}
	[ContextMenu("setup")]
	public virtual void setup(){
		BattleMaster.challenger=makeTroop();
		BattleMaster.isHost=true;
		var list=new List<Data.Reward>();
		foreach(var e in quest.rewards){
			var reward=new Data.Reward();
			reward.card=Data.Card.findByName(e);
			list.Add(reward);
		}
		RewardMaster.rewards=list.ToArray();
		BattleMaster.host=troop;
	}
}
}