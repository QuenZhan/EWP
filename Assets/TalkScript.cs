using UnityEngine;
using System.Collections;

public class NewBehaviourScript : MonoBehaviour {
	int index;
	public string[] dialouge;
	public UILabel label;
	void OnClick(){
		index=(index+1)%dialouge.Length;
		label.text=dialouge[index];
	}
}
