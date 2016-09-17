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
        damageElement = Element.NEUTRAL;
	}

    public Damage(int dmg, DamageType dmgType, Element dmgEle)
    {
        damage = dmg;
        damageType = dmgType;
        damageElement = dmgEle;
    }

	public static void doDamageTo(Player target, Damage dmg){
		doDamageTo (target, dmg.damage, dmg.damageType, dmg.damageElement);
	}

    public static void doDamageTo(Player target, Damage dmg, int bonus)
    {
        doDamageTo(target, dmg.damage + bonus, dmg.damageType, dmg.damageElement);
    }

    public static void doDamageTo(Player target, int dmg, DamageType dmgType, Element dmgEle ){
		dmg = Mathf.FloorToInt (dmg * (1 + target.damageReduction));
		target.HP -= dmg;

		displayDamage (target, dmg);
	}

	public static void displayDamage(Player target, int dmg){
	}
}