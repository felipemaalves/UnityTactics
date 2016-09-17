using UnityEngine;
using System.Collections;

public class SkillTemplate : Skill {

	// Use this for initialization
	void Start () {
        this.element = Element.FIRE;
        this.dmgType = DamageType.MAGIC;
        this.combatType = CombatType.SPELL;
        this.splashType = SplashType.POINT;

        this.baseDamage = 10;
        this.range = 5;
        this.splash = 0;
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public void Animation()
    {

    }
}
