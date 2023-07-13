using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterDoll : MonoBehaviour
{
    public string name;

    [SerializeField] public CharacterStats stats;

    public Status status;

    public Inventory inventory;

    public MagicSkills magicSkills;

    public Weapon weapon;

    public GameObject modelPrefab;

    public int Heal(int heal)
    {
        int beforeHeal = stats.hp;
        stats.hp += heal;
        if(stats.hp > stats.maxHp)
        {
            stats.hp = stats.maxHp;
        }
        return stats.hp - beforeHeal;
    }

    public CharacterDoll Copy()
    {
        CharacterDoll characterDoll = this;
        characterDoll.name = (string)this.name.Clone();
        characterDoll.stats = stats.Copy();
        characterDoll.inventory = this.inventory;
        characterDoll.magicSkills = this.magicSkills;
        characterDoll.weapon = this.weapon;
        return characterDoll;
    }
    //this will reset the hp, mana back to their max and the status back to normal
    public void resetToNormal()
    {
        stats.hp = stats.maxHp;
        stats.mana = stats.maxMana;
        status = Status.Normal;
    }
}
