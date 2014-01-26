using UnityEngine;
using System.Collections;
namespace EatWhilePlaying{
public class ChessTestAnimator:MonoBehaviour{
	public Animator animator;
	public bool attack;
	public bool hurt;
	public bool die;
	public bool skill;
	public bool ultimate;
	public bool born;
	void Awake(){
		if(!animator)animator=GetComponent<Animator>();
	}
	void Update(){
		if(attack)act("attack");
		if(hurt)act("hurt");
		if(die)act("die");
		if(skill)act("skill");
		if(ultimate)act("ultimate");
		if(born)act("born");
		attack=false;
		hurt=false;
		die=false;
		skill=false;
		ultimate=false;
		born=false;
	}
	void act(string option){
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
		if(animator)animator.SetFloat("random",Random.value);
	}
	IEnumerator parameterOnce(string str){
		if(animator){
			animator.SetBool(str,true);
			yield return new WaitForSeconds(0);
			animator.SetBool(str,false);
		}
	}
}
}