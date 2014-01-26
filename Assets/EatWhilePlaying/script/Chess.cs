using UnityEngine;
using System.Collections;
using TRNTH;
namespace EatWhilePlaying{
[RequireComponent(typeof(Creature))]
[RequireComponent(typeof(NguiAnchor))]
public class Chess:Battle{
	internal RadioBase hp=new RadioBase();
	internal Cell cell;
	internal string party="host";
	internal int numMove=1;
	internal int numAttack=1;
	internal int numSummon=0;
	internal int numSkill=0;
	internal int numUltimate=0;
	internal int addBuff(string str){return 0;}
	internal int moveSpeed{get{return 1;}}
	internal int attackDamage{get{return card.attackDamage;}}
	internal int magicPower{get{return card.magicPower;}}
	internal Vector3 posTarget;
	internal Vector3 posLook;
	internal void act(string option){
		switch(option){
		case"attack":
		case"hurt":
		case"die":
		case"skill":
		case"ultimate":
		case"born":
			StartCoroutine(parameterOnce(option));break;
			break;
		}
		if(c.animator)c.animator.SetFloat("random",Random.value);
	}
	internal void summon(Cell cell){
		posTarget=cell.transform.position;
		this.cell=cell;
		transform.position=posTarget+Vector3.up*2;
		act("born");
	}
	internal void die(){
		a[0].s=1;
		a[4].s=10;
		Destroy(nguiBc.gameObject);
		Destroy(gameObject,5);
	}
	IEnumerator parameterOnce(string str){
		if(c.animator){
			c.animator.SetBool(str,true);
			yield return new WaitForSeconds(0);
			c.animator.SetBool(str,false);
		}
	}
	Data.Card card;
	Creature c;
	Alarm[] a=Alarm.alarms;
	internal void recover(){
		numMove=1;
		numAttack=1;
		numSummon=0;
		numSkill=0;
		numUltimate=0;
	}
	void Awake(){
		base.Awake();
		nguiBc=Instantiate(nguiBcPrefab) as NguiButton;
		nguiBc.name=nguiBcPrefab.name;
		nguiBc.chess=this;
		var na=GetComponent<NguiAnchor>();
		na.ngui=nguiBc.transform;
		c=GetComponent<Creature>();
		card=Data.Card.find(name.Replace("(Clone)",""));
		hp.length=card.life;
	}
	void Start(){
		nguiBc.target=bm.gameObject;
	}
	void Update(){
		if(!a[4].a){
			// if(a[0].a)tra.Rotate(Vector3.up*60f);
			return;
		}
		c.animator.SetFloat("speed",Vector3.Scale(c.vecForce,new Vector3(1,0,1)).magnitude/c.speedMoveMax);
		c.walk(posTarget);
		c.lookAt(posLook);
	}
	void OnClick(){
		Debug.Log("bah");
		bm.from=this.cell;
	}
}
}