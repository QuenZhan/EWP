using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using EatWhilePlaying.Master;
using TRNTH;
namespace EatWhilePlaying.Data{
public class Ghost{
	class Move{
		internal Chess chess;
		internal string command="";
		public Move(Chess chess,string command){
			this.chess=chess;
			this.command=command;
		}
	}
	static public Ghost player{
		get{
			Ghost ghost=new Ghost();
			ghost.type="player";
			return ghost;
		}
	}
	public bool isFigureOut{get{
		switch(command){
		case "summon":
		case "magic":
			return from!=null
				&&to!=null
				&&card!=null;
		case "move":
		case "attack":
			return from!=null
				&&to!=null
				&&card==null;
		case "endTurn":
			return true;
		default:
			return false;
		}
	}}
	public string msg="";
	public string type="ai";
	public string command="";
	public Cell from;
	public Cell to;
	public Data.Card card;
	public void forget(){
		command="";
		from=null;
		to=null;
		card=null;
	}
	public void think(BattleMaster bm){
		forget();
		this.bm=bm;
		cells=bm.getCells;
		// filterCells("replace","*","");
		// filterCells("markChess","*","chess");
		// return;
		switch(type){
		case"ai":
			from=null;
			to=null;
			card=null;
			cells=bm.getCells;
			command="";
			//統計我方可行動人員資料
			var arr=chessFilter(bm.getChesses,"myParty");
			var list=new List<Move>();
			foreach(var e in arr){
				// Debug.Log("daab");
				for(var i=0;i<e.numMove;i++)list.Add(new Move(e,"move"));
				for(var i=0;i<e.numAttack;i++)list.Add(new Move(e,"attack"));
				for(var i=0;i<e.numSummon;i++)list.Add(new Move(e,"summon"));
				for(var i=0;i<e.numSkill;i++)list.Add(new Move(e,"skill"));
				for(var i=0;i<e.numUltimate;i++)list.Add(new Move(e,"ultimate"));
			}
			//若無可行動人員則結束回合
			if(list.Count<1||tryErrror>100){
				command="endTurn";
				// Debug.Log("endTurn");
				return;
			}
			//隨機選擇一個行動
			Move move=U.choose(list.ToArray()) as Move;
			var chessSel=move.chess;
			if(chessSel==null)return;
			//選擇打誰
			var chessTarget=chooseChess("nearestEnemy",chessSel.cell);
			if(chessTarget==null)return;
			//清空Cells tag
			filterCells("replace","*","");
			//根據不同指令作分歧
			msg=move.command;
			switch(move.command){
			case"attack":
				//判斷可否攻擊此名最近敵人
				if(Cell.Distance(chessSel.cell,chessTarget.cell)<2){
					// sDebug.Log(Cell.Distance(chessSel.cell,chessTarget.cell));
					from=chessSel.cell;
					to=chessTarget.cell;
					command="attack";
					tryErrror=0;
				}else tryErrror++;
				// chessSel.cell
				break;
			case"summon":
				// Debug.Log("ddd");
				card=new Card();
				from=chessSel.cell;
				to=find("nearest",cellFilterMove(chessSel.cell,1),chessTarget.cell);
				command="magic";
				tryErrror=0;
				break;
			case"move":
				//尋找離目標最近的cell
				from=chessSel.cell;
				to=find("nearest",cellFilterMove(chessSel.cell,1),chessTarget.cell);
				command="move";
				tryErrror=0;
				// Debug.Log(cellFilterMove(chessSel.cell,1).Length);
				break;
			}
			break;
		case"player":
			to=bm.to;
			from=bm.from;
			card=bm.card;
			command=bm.command;
			break;
		}
	}
	BattleMaster bm;
	Troop troop;
	internal int tryErrror=0;
	internal string party="";
	Cell[] cells;
	Cell[] cellFilterMove(Cell start,int step){
		filterCells("replace","*","");
		filterCells("markChess","*","chess");
		// Debug.Log(cells[0].tag);
		start.tag="move";
		for(int i=0;i<step;i++)filterCells("expand","","move");
		return filterCells("replace","move","move");
	}
	Chess[] chessFilter(Chess[] chesses,string option){
		var list=new List<Chess>();
		foreach(var e in chesses){
			switch(option){
			case"canMove":
				if(e.numMove>0)list.Add(e);
				break;
			case"notMyParty":
				if(e.party!=party)list.Add(e);
				break;
			case"myParty":
				if(e.party==party)list.Add(e);
				break;
			}
		}
		// Debug.Log(chesses.Length+"");
		return list.ToArray();
	}
	Chess chooseChess(string option){
		return chooseChess(option,null);
	}
	Chess chooseChess(string option,Cell cell){
		Chess[] arr;
		switch(option){
		case"whoToMove":
			arr=chessFilter(bm.getChesses,"myParty");
			arr=chessFilter(arr,"canMove");
			return arr[0];
			break;
		case"nearestEnemy":
			arr=chessFilter(bm.getChesses,"notMyParty");
			var vec=new Vector2(cell.x,cell.y);
			var nearest=arr[0];
			foreach(var e in arr){
				if(Vector2.Distance(vec,new Vector2(e.cell.x,e.cell.y))
					<Vector2.Distance(vec,new Vector2(nearest.cell.x,nearest.cell.y)))
					nearest=e;
			}
			return nearest;
			break;
		}
		return null;
	}
	Cell find(string option,Cell[] cells,Cell cell){
		var ccc=cells[0];
		// Debug.Log(cells.Length);
		foreach(var e in cells){
			if((e.pos-cell.pos).magnitude<(ccc.pos-cell.pos).magnitude)ccc=e;
		}
		return ccc;
	}
	Chess find(Chess[] chesses,Cell cell){
		foreach(var e in chesses){
			if(e.cell==cell)return e;
		}
		return null;
	}
	Cell[] filterCells(string option,string from,string to){
		var list=new List<Cell>();
		for(var i=0;i<cells.Length;i++){
			var cell=cells[i];
			if(from!="*"&&from!=cell.tag)continue;
			switch(option){
			case"replace":break;
			case"expand":
				var cellNear=cell.find(to);
				if(cellNear==null)continue;
				// Debug.Log(cellNear.tag);
				cell.score=cellNear.score+1;
				break;
			case"markChess":
				var chess=find(bm.getChesses,cell);
				if(chess==null)continue;
				break;
			}
			cell.tag=to;
			list.Add(cell);
		}
		return list.ToArray();
	}
}
}