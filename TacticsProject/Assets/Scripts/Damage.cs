using UnityEngine;
using System.Collections;

public enum Element
{
	NEUTRAL,
	FIRE,
	WATER,
	LIGHTNING,
	EARTH,
	AIR,
    HOLY,
    DARK,
    POISON
}

public enum DamageType 
{
	PIERCE,
	CONTUSION,
	SLASH,
	MAGIC,
    MIND,
    SOUND,
    TRUE
};

public class Damage {

	public DamageType damageType;
    public Element damageElement;
	public int damage;

	public Damage(){
		
	}

	public Damage(int dmg, DamageType dmgType){
		damage = dmg;
		damageType = dmgType;
	}

	public static void doDamageTo(Player target, Damage dmg){
		doDamageTo (target, dmg.damage, dmg.damageType);
	}

	public static void doDamageTo(Player target, int dmg, DamageType dmgType ){
		dmg = Mathf.FloorToInt (dmg * (1 + target.damageReduction));
		target.HP -= dmg;

		displayDamage (target, dmg);
	}

	static void displayDamage(Player target, int dmg){
	}
}
