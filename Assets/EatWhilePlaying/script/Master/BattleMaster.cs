using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using EatWhilePlaying.Data;
namespace EatWhilePlaying.Master{
[RequireComponent (typeof (Performance))]
public class BattleMaster:Master{
	public string thought;
	public Transform parentCommandMenu;
	public UISlider nguiTimeThink;
	public UISlider[] nguiHpHost;
	public UISlider[] nguiHpChallenger;
	public UILabel nguiNameHost;
	public UILabel nguiNameChallenger;
	public Transform parentCells;
	public Transform nguiFullCancel;
	public GameObject cMove;
	public GameObject cAttack;
	public GameObject cMagic;
	public GameObject cSkill;
	public GameObject cUltimate;
	public Cell cellPrefab;
	[ContextMenu("asign")]
	static internal bool isHost=true;
	static internal string analytics{get{return "戰鬥結果";}}
	static internal Troop host=Troop.player;
	static internal Troop challenger=new Troop();
	internal string uiSur="";
	internal int numTurn=0;
	// internal CardHand ch;
	internal Data.Card card;
	internal Cell from;
	internal Cell to;
	internal string command;
	internal Cell[] getCells{get{return cells;}}
	internal Chess[] getChesses{get{return lChess.ToArray();}}
	internal Chess findChess(Cell cell){
		foreach(var e in getChesses){
			// Debug.Log(e.cell.index+" "+cell.index);
			if(e.cell==cell)return e;
		}
		return null;
	}
	internal Troop troop{get{
		switch(numTurn%2){
		case 0:
			if(isHost)return host;
			else return challenger;
		default:
			if(isHost)return challenger;
			else return host;
		}
	}}
	internal Troop troopOpp{get{
		switch(numTurn%2){
		case 0:
			if(isHost)return challenger;
			else return host;
		default:
			if(isHost)return host;
			else return challenger;
		}
	}}
	int _numTurn=0;
	Chess[] heros=new Chess[2];
	Performance performance=new Performance();
	internal TRNTH.Alarm aThink=new TRNTH.Alarm();
	TRNTH.Control control;
	Cell[] cells=new Cell[0];
	internal List<Chess> lChess=new List<Chess>();
	void handComsume(string cardName){
		if(troop.hand.Count>0)troop.hand[0].comsume();
		return ;
		foreach(var e in troop.hand.ToArray()){
			if(e.name!=cardName)continue;
			e.comsume();
			break;
		}
	}
	void refreshCommandMenu(){
		var chess=findChess(from);
		cMove.SetActive(false);
		cAttack.SetActive(false);
		cMagic.SetActive(false);
		cSkill.SetActive(false);
		cUltimate.SetActive(false);
		if(chess){
			if(chess.numMove>0)cMove.SetActive(true);
			if(chess.numAttack>0)cAttack.SetActive(true);
			// if(chess.numSummon>0)cMagic.SetActive(true);
			if(chess.numSkill>0)cSkill.SetActive(true);
			if(chess.numUltimate>0)cUltimate.SetActive(true);
		}
		// tableCommands.repositionNow=true;
	}
	bool nguiCommand{
		set{
			if(isHost&&troop.party=="host");
			else return;
			parentCommandMenu.gameObject.SetActive(value);
			var chess=findChess(from);
			if(chess&&chess.numSummon>0){
				switch(chess.party){
				case"host":host.coorChess(chess);break;
				default:challenger.coorChess(chess);break;
				}
			}
			if(!value){
				host.coorCh();
				challenger.coorCh();
			}
		}
	}
	void toggleCellAll(){
		foreach(var e in getCells)e.ngui=true;
		foreach(var e in getChesses)e.ngui=true;
	}
	internal void toggleChessSelf(){
		toggleCell();
		foreach(var e in getChesses){
			if(e.party!=troop.party)continue;
			if(e.numMove>0
				||e.numAttack>0
				||e.numSummon>0)e.ngui=true;
		}
	}
	void toggleCell(){
		foreach(var e in getCells){
			e.ngui=(false);
			e.collider.enabled=true;
		}
		foreach(var e in getChesses){
			e.ngui=(false);
			e.collider.enabled=true;
		}
	}
	void toggleCellMove(Cell from,int range){
		toggleCell();
		foreach(var e in from.neighbors){
			if(!findChess(e))e.ngui=true;
		}
		from.ngui=true;
	}
	void toggleCellAttack(Cell from,int range){
		toggleCell();
		foreach(var e in from.neighbors){
			var chess=findChess(e);
			if(!chess||chess.party==troop.party)continue;
			e.ngui=true;
			chess.ngui=true;
		}
	}
	void excuteTalk(string text){}
	void excuteDisconnect(){}
	void excute(Ghost ghost){
		var cf=findChess(ghost.from);
		var ct=findChess(ghost.to);
		int num=0;
		Performance.Period p;
		switch(ghost.command){
		case "endTurn":
			foreach(var e in getChesses){
				e.recover();
			}
			host.ghost.tryErrror=0;
			challenger.ghost.tryErrror=0;
			heros[0].numSummon=1;
			heros[1].numSummon=1;
			numTurn++;
			performance.turnToggle(numTurn);
			performance.draw(new Data.Card[]{new Data.Card()},troop);
			break;
		case "move":
			cf.cell=ghost.to;
			p=performance.push();
			p.chesses=new Chess[]{cf};
			p.to=ghost.to;
			p.type="move";
			p.time=0.5f;
			cf.numMove-=1;
			break;
		case "attack":
			num=findChess(ghost.from).attackDamage;
			ct.hp-=num;
			troopOpp.hpTroop-=num;
			p=performance.push();
			p.chesses=new Chess[]{cf,ct};
			p.type="attack";
			p.time=0.5f;
			p=performance.push();
			p.chesses=new Chess[]{ct};
			p.type="damage";
			p.value=num;
			p.time=0.1f;
			cf.numAttack-=1;
			break;
		case "summon":
		case "magic":
			handComsume(ghost.card.name);
			p=performance.push();
			p.chesses=new Chess[]{cf};
			p.to=ghost.to;
			p.type="cast";
			p.time=0.5f;
			p.cardName=ghost.card.name;
			num=0;
			switch(ghost.card.name){
			case"火球術":
				num=findChess(ghost.from).magicPower*2;
				findChess(ghost.to).hp-=num;
				performance.push("damage",ghost,num);
				break;
			case"政治行動":
				troopOpp.hpTroop-=cf.magicPower*1;
				break;
			case"追擊":
				if(ct.numAttack==0)ct.numAttack+=1;
				break;
			case"順風":
				ct.addBuff("順風");
				break;
			case"恢復術":
				ct.hp+=cf.magicPower*5;
				break;
			case"財富":
				troop.modHand(2);
				break;
			case"惡意":
				troop.modHand(-1);
				troopOpp.modHand(-1-cf.magicPower);
				break;
			case"颶風":
				cf.cell=ghost.to;
				break;
			case"增援":
				ct.numSummon+=1*ct.magicPower;
				break;
			case"黑洞":
				cf.cell=ghost.to;
				break;
			case"旅者":
				if(ct.numMove==0)ct.numMove+=1;
				break;
			case"協力":
				ct.addBuff("協力");
				break;
			case"炸藥":
				ct.hp-=1;
				break;
			case"捕獸夾":
			case"牆壁":
				performance.summon(ghost.card,ghost.to,troop.party);
				break;
			case"骷髏弓箭手":
			case"紅鴉法師":
			case"紅鴉護衛":
			case"紅鴉弓箭手":
			case"紅鴉劍士":
			case"帝國重槍兵":
			case"骷髏戰士":
			case"帝國劍兵":
			default:
				cf.numSummon-=1;
				performance.summon(ghost.card,ghost.to,troop.party);
				break;
			}
			break;
		}
		//judgement
		var score=new int[]{0,0};
		foreach(var e in getChesses){
			if(e.party=="host")score[0]+=e.hp.value;
			else score[1]+=e.hp.value;
			if(e.hp.rate>0)continue;
			lChess.Remove(e);
			performance.die(e);
		}
		var winner="";
		if(numTurn>=18){
			winner=(host.hpTroop.value+host.hpHero.value>=challenger.hpTroop.value+challenger.hpHero.value)?"host":"challenger";
		}
		if(host.hpTroop.rate<=0||host.hpHero.rate<=0)winner="challenger";
		if(challenger.hpTroop.rate<=0||challenger.hpHero.rate<=0)winner="host";
		if(winner=="host"){
			if(isHost)performance.win();
			else performance.lose();
		}if(winner=="challenger"){
			if(isHost)performance.lose();
			else performance.win();
		}
		if(winner!=""){
			sur="set";
			enabled=false;
		}
	}
	void Awake(){
		var list=new List<Cell>();
		// foreach(Transform e in tableCells.parentCells){
		foreach(Transform e in parentCells){
			list.Add(e.GetComponent<Cell>());
		}
		cells=list.ToArray();
		control=GetComponent<TRNTH.Control>();
		performance=GetComponent<Performance>();
	}
	void Start(){
		if(host==null||challenger==null){
			enabled=false;
			sur="no host or challenger";
			return;
		}
		nguiNameHost.text=host.name;
		nguiNameChallenger.text=challenger.name;
		Chess chess;
		performance.begin(host,challenger);
		chess=performance.summon(host.hero,cells[6],"host");
		chess.recover();
		chess.numSummon=1;
		heros[0]=chess;
		host.hpHero=chess.hp;
		chess=performance.summon(challenger.hero,cells[cells.Length-7],"challenger");
		chess.recover();
		chess.numSummon=1;
		challenger.hpHero=chess.hp;
		heros[1]=chess;
		performance.draw(new Data.Card[]{new Data.Card(),new Data.Card(),new Data.Card()},host);
		performance.draw(new Data.Card[]{new Data.Card(),new Data.Card(),new Data.Card()},challenger);
		performance.turnToggle(numTurn);
	}
	void OnDestroy(){
		sur="";
		host=null;
		challenger=null;
		excuteDisconnect();
	}
	void Update(){
		nguiHpHost[0].value=host.hpHero.rate;
		nguiHpHost[1].value=host.hpTroop.rate;
		nguiHpChallenger[0].value=challenger.hpHero.rate;
		nguiHpChallenger[1].value=challenger.hpTroop.rate;
		nguiTimeThink.value=aThink.rate;
		if(!performance.isFinished||sur=="set")return;
		//surface 切換
		switch(sur){
		case"pickTo":
			if(command=="cancel"){
				sur="pickCommand";
				command="";
				nguiCommand=(true);
				card=null;
				to=null;
				toggleCell();
			}
			break;
		case"pickFromCard":
			break;
		case"pickCommand":
			switch(command){
			case"summon":
			case"magic":
				sur="pickTo";
				toggleCellMove(from,1);
				break;
			case"attack":
				toggleCellAttack(from,1);
				sur="pickTo";
				break;
			case"move":
				toggleCellMove(from,1);
				sur="pickTo";
				break;
			case"cancel":
				sur="pickFrom";
				from=null;
				to=null;
				card=null;
				toggleCellAll();
				toggleChessSelf();
				break;
			}
			if(command!=""){
				nguiCommand=false;
				// command="";
			}
			break;
		default:
			if(!from)break;
			var chess=findChess(from);
			if(!chess)break;
			command="";
			to=null;
			card=null;
			sur="pickCommand";
			refreshCommandMenu();
			nguiCommand=true;
			parentCommandMenu.localPosition=chess.coor;
			toggleCell();
			break;
		}
		//ghost 執行
		var ghost=troop.ghost;
		ghost.think(this);
		thought=ghost.msg;
		if(aThink.a)ghost.command="endTurn";
		if(ghost.isFigureOut){
			excute(ghost);
			performance.play();
			toggleCell();
			from=null;
			to=null;
			card=null;
			command="";
			nguiCommand=false;
			sur="performing";
		}
	}
	void button(GameObject gobj){
		Cell cell;
		var card=Data.Card.find(gobj.name);
		switch(gobj.name){
		case"commandEndTurn":
			command="endTurn";
			break;
		case"nguiButtonCell":
			cell=gobj.GetComponent<NguiButton>().cell;
			switch(sur){
			case"pickTo":
				to=cell;
				break;
			default:
				from=cell;
				break;
			}
			break;
		case"nguiButtonChess":
			cell=gobj.GetComponent<NguiButton>().chess.cell;
			switch(sur){
			case"pickTo":
				to=cell;
				break;
			default:
				from=cell;
				break;
			}
			break;
		case"commandAttack":
			command="attack";
			break;
		case"commandMove":
			command="move";
			break;
		case"commandOff":
		case"buttonFullCancel":
			command="cancel";
			break;
		case"commandSummon":
		default:
			if(card==null)break;
			// ch=gobj.GetComponent<CardHand>();
			command="magic";
			this.card=card;
			break;
		}
	}
}
}