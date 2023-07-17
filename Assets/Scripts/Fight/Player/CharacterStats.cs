using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[Serializable]
public class CharacterStats
{
    public string characterName;
    public int hp;
    public int maxHp;
    public int mana;
    public int maxMana;
    public int speed;
    public int strenght;
    public int defense;
    public int intelligence;

    public int luck;

    public int xp;

    public CharacterStats Copy()
    {
        CharacterStats stats = new CharacterStats();
        stats.hp = this.hp;
        stats.maxHp = this.maxHp;
        stats.mana = this.mana;
        stats.maxMana = this.maxMana;
        stats.speed = this.speed;
        stats.strenght = this.strenght;
        stats.defense = this.defense;
        stats.intelligence = this.intelligence;
        stats.luck= this.luck;
        stats.xp = this.xp;
        return stats;
    }
}
