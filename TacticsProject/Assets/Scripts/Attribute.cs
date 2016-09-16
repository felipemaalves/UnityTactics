using UnityEngine;
using System.Collections;

public class Attribute {

    private int strength;
    private int agility;
    private int dexterity;
    private int inteligence;
    private int wisdom;
    private int vitality;
    private int perception;

    public Attribute()
    {
        this.strength = 10;
        this.agility = 10;
        this.dexterity = 10;
        this.inteligence = 10;
        this.wisdom = 10;
        this.vitality = 10;
        this.perception = 10;
    }

    public Attribute(int s, int a, int d, int i, int w, int v, int p)
    {
        this.strength = s;
        this.agility = a;
        this.dexterity = d;
        this.inteligence = i;
        this.wisdom = w;
        this.vitality = v;
        this.perception = p;
    }

    /*
     * SET/GET 
     */

    public int getStrength()
    {
        return this.strength;
    }

    public int getAgility()
    {
        return this.agility;
    }

    public int getDexterity()
    {
        return this.dexterity;
    }

    public int getInteligence()
    {
        return this.inteligence;
    }

    public int getWisdom()
    {
        return this.wisdom;
    }

    public int getVitality()
    {
        return this.vitality;
    }

    public int getPerception()
    {
        return this.perception;
    }

    public void setStrength(int s)
    {
        this.strength = s;
    }

    public void setAgility(int a)
    {
        this.agility = a;
    }

    public void setDexterity(int d)
    {
        this.dexterity = d;
    }

    public void setInteligence(int i)
    {
        this.inteligence = i;
    }

    public void setWisdom(int w)
    {
        this.wisdom = w;
    }

    public void setVitality(int v)
    {
        this.vitality = v;
    }

    public void setPerception(int p)
    {
        this.perception = p;
    }

    public void setAll(int s, int a, int d, int i, int w, int v, int p)
    {
        this.strength = s;
        this.agility = a;
        this.dexterity = d;
        this.inteligence = i;
        this.wisdom = w;
        this.vitality = v;
        this.perception = p;
    }
}
