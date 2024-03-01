using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class CharacterFightController : MonoBehaviour
{
    #region Field Declarations

    public GameObject dollPrefab;
    public CharacterDoll doll;


    private int criticalMultiplier;


    private List<Buff> temporaryBuffs = new List<Buff>();

    private FightAnimation fightAnimation;

    private GameObject model;

    private UnityAction<CharacterDecisionResult> getResultBackMethod;

    private GameObject dollInstance;

    #endregion

    // Start is called before the first frame update
    void Start()
    {
        dollInstance = Instantiate(dollPrefab);
        doll = dollInstance.GetComponent<CharacterDoll>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    //Updates player condition or buff stats if necessary (Burnt/ Shield)
    public void TurnUpdate()
    {
        List<Buff> buffsToRemove = new List<Buff>();
        foreach(Buff buff in temporaryBuffs)
        {
            buff.turns -= 1;
            Debug.Log(buff.turns);
            if(buff.turns <= 0)
            {
                buffsToRemove.Add(buff);
            }
        }
        //cannot modify list during iteration so i use 2 loops
        foreach(Buff buff in buffsToRemove)
        {
            removeBoost(buff);
        }
    }

    public void Attack(CharacterDecision characterDecision, UnityAction<CharacterDecisionResult> _getResultBackMethod)
    {
        if(doesItMiss(doll.weapon.missingChance))
        {
            _getResultBackMethod.Invoke(new CharacterDecisionResult(characterDecision, false, 0));
            return;
        }
        int attackDamage = doll.stats.strenght + doll.weapon.damage;
        bool critical = isItCritical(doll.weapon.missingChance);
        if(critical)
        {
            attackDamage *= (int) (attackDamage * FightConstants.CriticalMultiplier);
        }
        int points = characterDecision.target.GetHit(attackDamage);
        CharacterDecisionResult result = new CharacterDecisionResult(characterDecision, true, points);
        result.critical = critical;
        //Letting the animation play before we return result;
        getResultBackMethod = _getResultBackMethod;
        attackAnimation(result);
    }

    public void Attack(CharacterDecision characterDecision, AttackItem item, UnityAction<CharacterDecisionResult> _getResultBackMethod)
    {
        characterDecision.from.doll.inventory.removeItem(item);
        CharacterDecisionResult characterDecisionResult =  new CharacterDecisionResult(characterDecision, true, characterDecision.target.GetHit(item.damage));
        getResultBackMethod = _getResultBackMethod;
        getResultBackMethod(characterDecisionResult);
    }

    public void Attack(CharacterDecision characterDecision, DamageSpell spell, UnityAction<CharacterDecisionResult> _getResultBackMethod)
    {
        this.doll.stats.mana -= spell.mana;
        CharacterDecisionResult characterDecisionResult = new CharacterDecisionResult(characterDecision, true, characterDecision.target.GetHit(spell.damage));
        getResultBackMethod = _getResultBackMethod;
        getResultBackMethod(characterDecisionResult);
        
    }

    public void Heal(CharacterDecision characterDecision, HealSpell spell, UnityAction<CharacterDecisionResult> _getResultBackMethod)
    {
        this.doll.stats.mana -= spell.mana;
        CharacterDecisionResult characterDecisionResult = new CharacterDecisionResult(characterDecision, true, characterDecision.target.doll.Heal(spell.heal));
        getResultBackMethod = _getResultBackMethod;
        getResultBackMethod(characterDecisionResult);
        
    }

    public void applyBoost(Buff buff)
    {
        switch(buff.type){
            case Buff.Type.Defense:
                doll.stats.defense += buff.boost;
            break;
        }
        temporaryBuffs.Add(buff);
    }

    private void removeBoost(Buff buff)
    {
        switch(buff.type){
            case Buff.Type.Defense:
                doll.stats.defense -= buff.boost;
            break;
        }
        temporaryBuffs.Remove(buff);
    }

    public int GetHit(int damage)
    {
        doll.stats.hp -= damage;
        if(fightAnimation != null)
            fightAnimation.GetHit();
        if(doll.stats.hp <= 0){
            doll.stats.hp = 0;
            die();
        }
        return damage;
        //Debug.Log("HIT");
    }

    public void DisplayModel()
    {
        model = Instantiate(doll.modelPrefab, this.transform);
        fightAnimation = model.GetComponent<FightAnimation>();
    }

    public void RemoveModel()
    {
        if(model != null)
        {
            Destroy(model);
        }
    }

    private void die()
    {
        doll.status = Status.Dead;
        if(fightAnimation != null)
        {
            fightAnimation.Death();
        }
        else
        {
        this.gameObject.SetActive(false);
        }
    }

    private bool isItCritical(int weaponCriticalChance)
    {
        return getChance(weaponCriticalChance);
    }

    private bool doesItMiss(int weaponMissChance)
    {
        return getChance(weaponMissChance);
    }

    private bool getChance(int chance)
    {
        int number = Random.Range(1,101);
        if(number <= chance)
        {
            return true;
        }
        else
        {
            return false;
        }
    }


    private void attackAnimation(CharacterDecisionResult result)
    {
        if(fightAnimation == null)
        {
            getResultBackMethod.Invoke(result);
            return;
        }
        fightAnimation.Attack();
        StartCoroutine(waitForAnimationToEnd(result));
    }

    IEnumerator waitForAnimationToEnd(CharacterDecisionResult result)
    {
        yield return new WaitUntil(() => !fightAnimation.isInAction());
        getResultBackMethod.Invoke(result);
    }
    
}
