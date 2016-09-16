using UnityEngine;
using System.Collections;

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

    public static float HitChance(int attackPower, int defensePower)
    {
        // blabla logic
        return 1.0f;
    }
}
