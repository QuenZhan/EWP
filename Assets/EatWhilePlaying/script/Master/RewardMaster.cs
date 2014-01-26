using UnityEngine;
using System.Collections;
using EatWhilePlaying.Data;
namespace EatWhilePlaying.Master{
public class RewardMaster : Master {
	static public Reward[] rewards;
	void start(){
		var reward=rewards[0];
		parseApi.modCard(reward.card,reward.amount);
		parseApi.save();
	}
}
}