using UnityEngine;
using System.Collections;
namespace EatWhilePlaying{
public class CardHand : TRNTH.MonoBehaviour {
	public TRNTH.FxShake fxshake;
	public Vector3 coorTarget;
	internal Data.Troop troop;
	internal Master.BattleMaster bm;
	Data.Card card;
	Vector3 velocity=Vector3.zero;
	TRNTH.Alarm a=new TRNTH.Alarm();
	void Start(){
		card=Data.Card.find(name);
	}
	void Update(){
		// tra.LookAt(Camera.main.transform);
		tra.localPosition=Vector3.SmoothDamp(tra.localPosition,coorTarget,ref velocity,0.1f);
		// if(a.a){
			// a.s=10;
		// }
	}
	IEnumerator shake(){
		yield return new WaitForSeconds(0.5f);
		if(fxshake)fxshake.play();
	}
	internal void comsume(){
		troop.hand.Remove(this);
		Destroy(gameObject,2);
		StartCoroutine(shake());
		a.s=0.5f;
		coorTarget=new Vector3(Screen.width*0.5f,Screen.height*0.5f,0);
	}
}
}