using UnityEngine;
using System.Collections;
namespace EatWhilePlaying.Master{
public class MainTtitle:Master{
	public string username;
	public string password;
	public string status;
	[ContextMenu("login")]
	public void login(){
		parseApi.load(username,password);
	}
	[ContextMenu("update")]
	public void update(){
		status=parseApi.defect;
		parseApi.update();
	}
	public void hisLogin(){
		parseApi.hisLogin();
		Worldmap.troop=parseApi.troops[0];
	}
	TRNTH.Alarm aUpdate=new TRNTH.Alarm();
	void Start(){
		parseApi.setup();
		username=parseApi.username;
	}
	void Update(){
		aUpdate.routine(1,update); 
		switch(sur){
		case"update":update();break;
		case"login":login();break;
		case"hisLogin":parseApi.hisLogin();break;
		}
	}
}
}