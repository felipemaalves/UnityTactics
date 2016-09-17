using UnityEngine;
using System.Collections;

public enum SplashType
{
    POINT,
    CIRCLE,
    LINE,
    WALL,
    CONE
}

public class Skill : MonoBehaviour {

    protected Element element;
    protected DamageType dmgType;
    protected CombatType combatType;
    protected SplashType splashType;

    protected Damage damage;
    protected Player user;

    protected int baseDamage = 5;
    protected int range = 1;
    protected int splash = 0;

    protected int actionPointsCost;
    protected int movePointsCost;

    protected string tooltipText = "Default text";

    public virtual void setPlayer(Player user)
    {
        this.user = user;
        this.damage = new Damage(this.baseDamage + user.getInt(), this.dmgType);
    }

    public virtual void doDamageTo(Player target)
    {
        bool hit;
        switch (combatType)
        {
            case CombatType.IMPACT:
                hit = CombatSystem.IsImpactHit(this.user, target);
                break;
            case CombatType.SLICE:
                hit = CombatSystem.IsSliceHit(this.user, target);
                break;
            case CombatType.SPELL:
                hit = CombatSystem.IsSpellHit(this.user, target);
                break;
            case CombatType.MIND:
                hit = CombatSystem.IsMindHit(this.user, target);
                break;
            default:
                hit = false;
                break;
        }

        if (hit)
            Damage.doDamageTo(target, this.damage, user.rollDice());
    }

    public void tooltip()
    {
        // TODO
    }

    public Element getElement()
    {
        return element;
    }

    public DamageType getDamageType()
    {
        return dmgType;
    }

    public CombatType getCombatType()
    {
        return combatType;
    }

    public SplashType getSplashType()
    {
        return splashType;
    }

    public Damage getDamage()
    {
        return damage;
    }

    public int getBaseDamage()
    {
        return baseDamage;
    }

    public int getRange()
    {
        return range;
    }

    public int getSplash()
    {
        return splash;
    }

    public int getActionPointsCost()
    {
        return actionPointsCost;
    }

    public int getMovePointsCost()
    {
        return movePointsCost;
    }
}
