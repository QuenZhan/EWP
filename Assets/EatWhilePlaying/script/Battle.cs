using UnityEngine;
using System.Collections;
namespace EatWhilePlaying{
public class Battle : TRNTH.MonoBehaviour {
	public NguiButton nguiBcPrefab;
	public Master.BattleMaster bm;
	internal NguiButton nguiBc;
	internal bool ngui{
		set{
			nguiBc.collider.enabled=(true);
			nguiBc.gameObject.SetActive(value);
		}
	}
}
}