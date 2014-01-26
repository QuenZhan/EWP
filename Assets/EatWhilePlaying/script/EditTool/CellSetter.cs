using UnityEngine;
using System.Collections.Generic;
namespace EatWhilePlaying{
[ExecuteInEditMode]
public class CellSetter : MonoBehaviour {
	static internal float radius;
	public Cell prefab;
	Cell[] findCellNear(Cell cellThis,Cell[] cells){
		float radius=2;
		var list=new List<Cell>();
		foreach(Cell cell in cells){
			float dis=Vector2.Distance(new Vector2(cellThis.x,cellThis.y),new Vector2(cell.x,cell.y));
			if(dis<radius&&cellThis!=cell)list.Add(cell);
		}
		return list.ToArray();
	}
	[ContextMenu("clean")]
	void clean(){
		foreach(Transform e in transform){
			// DestroyImmediate(e.GetComponent<Cell>().nguiBc.gameObject);
			DestroyImmediate(e.gameObject);
		}
	}
	[ContextMenu("plant")]
	void plant(){
		float uh=Mathf.Sqrt(3f)*0.5f*2f;
		float uv=1.5f;
		int wmax=9;
		int ww=5;
		int counter=0;
		bool getBigger=true;
		var list=new List<Cell>();
		for(int h=0;h<wmax;h++){
			for(int w=0;w<ww;w++){
				Cell cell=Instantiate(prefab) as Cell;
				cell.index=counter;
				cell.x=(-ww*0.5f+w)*uh;
				cell.y=(-wmax*0.5f+h)*uv;
				cell.transform.parent=this.transform;
				cell.transform.localPosition=new Vector3(cell.x,0,cell.y);
				// cell.transform.localScale=Vector3.one;
				list.Add(cell);
				counter++;
			}
			if(h>wmax*0.5f-1)getBigger=false;
			ww+=getBigger?1:-1;
		}
		var cells=list.ToArray();
		foreach(Cell cell in cells){
			cell.neighbors=findCellNear(cell,cells);
		}
		radius=(cells[0].transform.position-cells[1].transform.position).magnitude;
	}
}
}