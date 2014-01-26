using UnityEngine;
using System.Collections;
namespace EatWhilePlaying.Master{
public class Scretary:Master{
	public string username;
	public string password;
	[ContextMenu("signUp")]
	public void signUp(){
		parseApi.signUp(username,password);
	}
}
}