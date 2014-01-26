using Parse;
namespace EatWhilePlaying.Data{
[System.Serializable]
public class Card{
	static public Card find(string str){
		foreach(var e in cards){
			if(e.id==str)return e;
			if(e.name==str)return e;
		}
		return new Card();
	}
	static public Card findById(string id){return find(id);}
	static public Card findByName(string name){return find(name);}
	static public Card[] cards={new Card(),new Card()};
	public ParseObject pobj;
	public string id="";
	public string name="name";
	public string title="title";
	public string description="description";
	public string armor="輕裝";
	public string weapon="劍士";
	public string type="chess";
	public string tagetMask="enemy";
	public string caster="any";
	public string time="any";
	public int life=1;
	public int attackDamage=1;
	public int magicPower=0;
	public int rarity=0;
	public Card(){}
	public Card(ParseObject po){
		this.pobj=po;
		var card=this;
		card.id=po.ObjectId;
		po.TryGetValue<string>("caster",out caster);
		po.TryGetValue<string>("name",out name);
		po.TryGetValue<string>("title",out title);
		po.TryGetValue<string>("description",out description);
		po.TryGetValue<string>("armor",out armor);
		po.TryGetValue<string>("weapon",out weapon);
		po.TryGetValue<string>("type",out type);
		po.TryGetValue<string>("tagetMask",out tagetMask);
		po.TryGetValue<int>("life",out life);
		po.TryGetValue<int>("attackDamge",out attackDamage);
		po.TryGetValue<int>("magicPower",out magicPower);
		po.TryGetValue<int>("rarity",out rarity);
	}
	public int amount; //擁有數，大於零則顯示擁有
}
}
