using UnityEngine;
using System.Net;
using System.IO;
using System.Net.Sockets;
using System.Collections;
using System.Collections.Generic;
using Parse;
using EatWhilePlaying.Data;
namespace EatWhilePlaying{
public class ParseApi{
	string status="";
	public string username{get{return ParseUser.CurrentUser.Get<string>("username");}}
	public string defect{get{
		var user=ParseUser.CurrentUser;
		if(task!=null&&!task.IsCompleted)return "task!=null&&!task.IsCompleted";
		if(user==null)return "user==null";
		if(!user.IsDataAvailable)return "!user.IsDataAvailable";
		if(Data.Card.cards==null)return "Card.cards==null";
		if(!isAsset)return "!isAsset";
		return "okay";
	}}
	public bool isOkay{get{
		return defect=="okay";
	}}
	public void load(string username,string password){
		status="logging in";
		task=ParseUser.LogInAsync(username,password);
	}
	public void quit(){
		var poHis=new ParseObject("History");
		var user=ParseUser.CurrentUser;
		poHis["user"]=user;
		poHis["dscription"]="quit game";
		poHis.SaveAsync();
	}
	public void save(){
		var user=ParseUser.CurrentUser;
		task=user.SaveAsync();
	}
	public void signUp(string username,string password){
		var user=new ParseUser(){
			Username=username
			,Password=password
		};
		if(ParseUser.CurrentUser!=null){
			user["asset"]=ParseUser.CurrentUser.Get<IDictionary<string,object>>("asset");
			user["troop"]=ParseUser.CurrentUser.Get<IList<object>>("troop");
		}else{
			user["asset"]=new Dictionary<string,int>();
			user["troop"]=new string[0];
		}
		task=user.SignUpAsync();
		task.ContinueWith(t =>{
			if (t.IsFaulted || t.IsCanceled){
				task=ParseUser.LogInAsync(username,password);
			}
		});
	}
	public Troop[] troops{get{
		var user=ParseUser.CurrentUser;
		var list=new List<string>();
		// if(!user.IsDataAvailable)return new Troop[]{new Troop()};
		// var lll=user.Get<IList<object>>("troop");
		// if(lll)
		foreach(var e in user.Get<IList<object>>("troop"))list.Add(System.Convert.ToString(e));
		var troop=new Troop();
		if(list.Count>0)troop.hero=Data.Card.findById(list[0]);
		var lCard=new List<Data.Card>();
		for(int i=1;i<list.Count;i++){
			lCard.Add(Data.Card.findById(list[i]));
		}
		troop.cards=lCard.ToArray();
		return new Troop[]{troop};
	}}
	public void updateTroops(Troop[] troops){
		var user=ParseUser.CurrentUser;
		var troop=troops[0];
		var list=new List<string>();
		list.Add(troop.hero!=null?troop.hero.name:"");
		foreach(var e in troop.cards){
			list.Add(e.name);
		}
		user["troop"]=list.ToArray();
		save();
	}
	public void modCard(Data.Card card,int amount){
		var user=ParseUser.CurrentUser;
		card=Data.Card.findByName(card.name);
		card.amount+=amount;
		var dAsset=new Dictionary<string,int>();
		foreach(var e in Data.Card.cards){
			dAsset.Add(e.id,e.amount);
		}
		var poHis=new ParseObject("History");
		poHis["user"]=user;
		poHis["amount"]=amount;
		poHis["dscription"]="mod card";
		poHis["cardName"]=card.name;
		poHis.SaveAsync();
		user["asset"]=dAsset;
		save();
	}
	public void setup(){
		// signUp(Network.player.guid,"0000");
		var user=ParseUser.CurrentUser;
		if(user==null)signUp(Network.player.guid,"0000");
	}
	public void update(){
		var user=ParseUser.CurrentUser;
		switch(defect){
		case"user==null":
			setup();
			break;
		case"!user.IsDataAvailable":
			task=user.FetchAsync();
			break;
		case"Card.cards==null":
			fetchCards();
			break;
		case"!isAsset":
			var dAsset=new Dictionary<string,int>();
			foreach(var e in user.Get<IDictionary<string,object>>("asset"))dAsset.Add(e.Key,System.Convert.ToInt32(e.Value));
			for(int i=0;i<Data.Card.cards.Length;i++){
				var card=Data.Card.cards[i];
				if(dAsset.ContainsKey(card.id))card.amount=dAsset[card.id];
				else card.amount=0;
			}
			isAsset=true;
			break;
		}
	}
	string getIp(){
		return "bah";
	}
	public void hisLogin(){
		var poHis=new ParseObject("History");
		var user=ParseUser.CurrentUser;
		poHis["user"]=user;
		poHis["dscription"]="continue game";
		poHis["machineName"]=System.Environment.MachineName;
		poHis["ip"]=getIp();
		poHis.SaveAsync();
	}
	System.Threading.Tasks.Task task=null;
	bool isAsset=false;
	void fetchCards(){
		if(Data.Card.cards!=null)return;
		var query = ParseObject.GetQuery("Card");
		status="cards all data is fetching";
		task=query.FindAsync().ContinueWith(t =>{
			List<Data.Card> lCards=new List<Data.Card>();
			foreach(var po in t.Result){
				Data.Card card=new Data.Card(po);
				lCards.Add(card);
			}
			status="cards all data fetching succeeded";
			Data.Card.cards=lCards.ToArray();
		});
	}
}
}