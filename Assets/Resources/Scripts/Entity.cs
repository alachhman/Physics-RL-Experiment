using UnityEditor;

public class Entity {
	public string name;
	public int HP;
	public int currHP;
	public int ATK;
	public int currATK;
	public int DEF;
	public int currDEF;
	public Entity(string name, int HP, int ATK, int DEF) {
		this.name = name;
		this.HP = HP;
		this.currHP = HP;
		this.ATK = ATK;
		this.currATK = ATK;
		this.DEF = DEF;
		this.currDEF = DEF;
	}

	// class Player : Entity {
	// 	public Player(string name, int HP, int ATK, int DEF) : base(name, HP, ATK, DEF) {
	// 		
	// 	}
	// }
	
	public string statsToString() {
		return "ATK: " + this.ATK + " \nDEF: " + this.DEF;
	}
}