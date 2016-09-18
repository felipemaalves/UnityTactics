using UnityEngine;
using System.Collections;

public class SkillTemplate : Skill {

    public SkillTemplate()
    {
        this.skillName = "Magma Ball";

        this.element = Element.FIRE;
        this.dmgType = DamageType.MAGIC;
        this.combatType = CombatType.SPELL;
        this.splashType = SplashType.POINT;

        this.baseDamage = 10;
        this.range = 5;
        this.splash = 0;
    }

	// Use this for initialization
	void Start () {
        
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public void Animation()
    {

    }
}
