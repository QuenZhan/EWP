using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using EatWhilePlaying.Data;
using EatWhilePlaying.Master;
namespace EatWhilePlaying{
[RequireComponent (typeof (TRNTH.Control))]
public class Performance:MonoBehaviour{
	internal class Period{
		public string cardName="";
		public string type="none";
		public string party;
		public Chess[] chesses;
		public Cell from;
		public Cell to;
		public int value=0;
		public float time=1f;
	}
	internal Chess summon(Data.Card card,Cell cell,string party){
		var p=new Period();
		p.type="summon";
		var chess=setChess(Data.Card.find(card.name),cell);
		chess.party=party;
		chess.gameObject.SetActive(false);
		p.chesses=new Chess[]{chess};
		queuePeriod.Add(p);
		return chess;
	}
	public Chess[] chessesPrefab;
	public bool isFinished;
	public UILabel label;
	public UIScrollBar nguiMsgSliderBar;
	public Transform parentChess;
	public GameObject nguiVictory;
	public GameObject nguiDefeated;
	public UITweener tweenPhase;
	public CardHand chPrefab;
	public void draw(Data.Card[] cards,Troop troop){
		foreach(var e in cards){
			var p=new Period();
			p.type="draw";
			p.cardName=e.name;
			p.party=troop.party;
			p.time=0.2f;
			queuePeriod.Add(p);
		}
		play();
	}
	internal Period push(){
		var p=new Period();
		queuePeriod.Add(p);
		return p;
	}
	internal Period push(string type,Data.Ghost ghost){
		return push(type,ghost,0);
	}
	internal Period push(string type,Data.Ghost ghost,int value){
		var p=new Period();
		p.type=type;
		if(ghost.card!=null)p.cardName=ghost.card.name;
		p.from=ghost.from;
		p.to=ghost.to;
		p.value=value;
		p.time=2f;
		queuePeriod.Add(p);
		return p;
	}
	public void play(){
		enabled=true;
		isFinished=false;
	}
	public void begin(Troop host,Troop challenger){
		var p=new Period();
		p.type="begin";
		queuePeriod.Add(p);
		play();
	}
	public void turnToggle(int turn){
		var p=new Period();
		p.type="turnToggle";
		p.value=turn;
		queuePeriod.Add(p);
		play();
	}
	public void win(){
		var p=push();
		p.type="win";
	}
	public void lose(){
		var p=push();
		p.type="lose";
	}
	public void die(Chess chess){
		var p=new Period();
		p.type="die";
		p.chesses=new Chess[]{chess};
		p.time=0f;
		queuePeriod.Add(p);
	}
	public void showCell(string option){}
	public void showCell(Ghost ghost){}
	TRNTH.Control control;
	Master.BattleMaster bm;
	internal List<Period> queuePeriod=new List<Period>();
	TRNTH.Alarm aQueue=new TRNTH.Alarm();
	Ghost ghost;
	Chess findChess(Cell cell){return null;}
	Vector3 findPos(Cell cell){return Vector3.zero;}
	int qIndex=0;
	private Chess setChess(Data.Card card,Cell cell){
		Chess chess=chessesPrefab[0];
		foreach(var e in chessesPrefab){
			if(e.name==card.name)chess=e;
		}
		chess=Instantiate(chess) as Chess;
		chess.summon(cell);
		chess.transform.parent=parentChess;
		chess.bm=bm;
		bm.lChess.Add(chess);
		return chess;
		// lChess.Add(chess);
	}
	IEnumerator phaseSwitch(){
		tweenPhase.Play(true);
		yield return new WaitForSeconds(0.5f);
		tweenPhase.Play(false);
	}
	void Awake(){
		control=GetComponent<TRNTH.Control>();
		bm=GetComponent<Master.BattleMaster>();
	}
	void OnEnable(){
	}
	void Update(){
		if(!aQueue.a&&!control.isSkip)return;
		isFinished=false;
		if(qIndex<queuePeriod.Count){
			var text="";
			var p=queuePeriod[qIndex];
			var chess=findChess(p.from);
			var pos=Vector3.zero;
			aQueue.s=p.time;
			// var chess=(p.chess);
			switch(p.type){
			case"lose":
				nguiDefeated.SetActive(true);
				text="Defeated";
				break;
			case"win":
				nguiVictory.SetActive(true);
				text="Victory!";
				break;
			case"turnToggle":
				text="phase changed";
				bm.aThink.s=30;
				tweenPhase.Play(true);
				StartCoroutine(phaseSwitch());
				break;
			case"draw":
				text=p.party+" draw "+ p.cardName;
				var ch=Instantiate(chPrefab) as CardHand;
				ch.name=p.cardName;
				ch.transform.parent=chPrefab.transform.parent;
				ch.transform.localScale=Vector3.one;
				ch.transform.localPosition=new Vector3(380,640,0);
				switch(p.party){
				case"host":
					BattleMaster.host.hand.Add(ch);
					BattleMaster.host.coorCh();
					ch.troop=BattleMaster.host;
					break;
				default:
					BattleMaster.challenger.hand.Add(ch);
					BattleMaster.challenger.coorCh();
					ch.troop=BattleMaster.challenger;
					break;
				}
				break;
			case"begin":
				text="begin";
				break;
			case"die":
				chess=p.chesses[0];
				text=chess.name+" died";
				bm.lChess.Remove(chess);
				chess.act("die");
				chess.die();
				break;
			case"move":
				chess=p.chesses[0];
				pos=p.to.pos;
				text=chess.name+" move to "+p.to.index;
				chess.posLook=pos;
				chess.posTarget=pos;
				// chess.cell=p.to;
				break;
			case"attack":
				chess=p.chesses[0];
				text=chess.name+" attack at "+p.chesses[1].name;
				chess.posLook=p.chesses[1].transform.position;
				chess.act("attack");
				break;
			case"magic":break;
			case"cast":
				chess=p.chesses[0];
				text=chess.name+" cast a magic : "+p.cardName;
				chess.posLook=findPos(p.to);
				chess.act("skill");
				break;
			case"damage":
				chess=(p.chesses[0]);
				text=chess.name + " got "+p.value+ " point damage";
				chess.act("hurt");
				break;
			case"summon":
				// chess=setChess(Data.Card.find(p.cardName),p.to);
				// chess.party=p.party;
				// chess=findChess(p.to);
				chess=p.chesses[0];
				chess.gameObject.SetActive(true);
				chess.act("born");
				text=chess.name + " in field";
				break;
			}
			text=qIndex+". "+p.type+" : "+text;
			label.text=label.text+"\n"+text;
			nguiMsgSliderBar.value=1;
			// queuePeriod.Remove(p);
			qIndex++;
		}else{
			bm.toggleChessSelf();
			isFinished=true;
			enabled=false;
		}
	}
}
}