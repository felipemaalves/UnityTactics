using UnityEngine;
using System.Collections;

public enum CombatType
{
    IMPACT,
    SLICE,
    SPELL,
    MIND
}

public class CombatSystem {

	public static int ImpactAttackPower(Player player)
    {
        Attribute attributes = player.getAttributes();
        return attributes.getStrength() + attributes.getDexterity();
    }

    public static int ImpactDefensePower(Player player)
    {
        Attribute attributes = player.getAttributes();
        return attributes.getAgility() + attributes.getVitality();
    }

    public static int SliceAttackPower(Player player)
    {
        Attribute attributes = player.getAttributes();
        return attributes.getPerception() + attributes.getDexterity();
    }

    public static int SliceDefensePower(Player player)
    {
        Attribute attributes = player.getAttributes();
        return attributes.getPerception() + attributes.getAgility();
    }

    public static int SpellAttackPower(Player player)
    {
        Attribute attributes = player.getAttributes();
        return attributes.getWisdom() + attributes.getDexterity();
    }

    public static int SpellDefensePower(Player player)
    {
        Attribute attributes = player.getAttributes();
        return attributes.getAgility() + attributes.getPerception();
    }

    public static int MindAttackPower(Player player)
    {
        Attribute attributes = player.getAttributes();
        return attributes.getInteligence() + attributes.getDexterity();
    }

    public static int MindDefensePower(Player player)
    {
        Attribute attributes = player.getAttributes();
        return attributes.getWisdom() + attributes.getPerception();
    }

    public static bool IsImpactHit(Player attacker, Player defender)
    {
        if (ImpactAttackPower(attacker) + attacker.rollDice() 
            > ImpactDefensePower(defender) + defender.rollDice())
            return true;
        else
            return false;
    }

    public static bool IsSliceHit(Player attacker, Player defender)
    {
        if (SliceAttackPower(attacker) + attacker.rollDice()
            > SliceDefensePower(defender) + defender.rollDice())
            return true;
        else
            return false;
    }

    public static bool IsSpellHit(Player attacker, Player defender)
    {
        if (SpellAttackPower(attacker) + attacker.rollDice()
            > SpellDefensePower(defender) + defender.rollDice())
            return true;
        else
            return false;
    }

    public static bool IsMindHit(Player attacker, Player defender)
    {
        if (MindAttackPower(attacker) + attacker.rollDice()
            > MindDefensePower(defender) + defender.rollDice())
            return true;
        else
            return false;
    }

    public static float HitChance(Player attacker, Player defender, CombatType type)
    {
        float att;
        float def;

        float percent = 0.0f;

        switch (type)
        {
            case CombatType.IMPACT:
                att = (float)ImpactAttackPower(attacker);
                def = (float)ImpactDefensePower(defender);
                break;
            case CombatType.SLICE:
                att = (float)SliceAttackPower(attacker);
                def = (float)SliceDefensePower(defender);
                break;
            case CombatType.SPELL:
                att = (float)SpellAttackPower(attacker);
                def = (float)SpellDefensePower(defender);
                break;
            case CombatType.MIND:
                att = (float)MindAttackPower(attacker);
                def = (float)MindDefensePower(defender);
                break;
            default:
                att = 0.0f;
                def = 0.0f;
                break;
        }

        if (att + 1 > def + defender.rollSides)
            percent = 1.0f;
        else if (att + attacker.rollSides < def + 1)
            percent = 0.0f;
        else
        {
            int hits = 0;
            int total = attacker.rollSides * defender.rollSides;
            for (int i = 1; i <= attacker.rollSides; i++)
                for(int j = 1; j <= defender.rollSides; j++)
                    if (att + i > def + j)
                        hits++;
            percent = (float)hits / total;
        }

        return percent;
    }
}
