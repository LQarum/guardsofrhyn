using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LocalEntityControl : MonoBehaviour
{

    public bool HitPartOfAttackReached = false;
    public bool isAttackFinished = false;

    //public GameObject AttackSpell;

    public void SetHitPartOfAttackReached() {

        HitPartOfAttackReached = true;

    }

    public void SetAttackFinished() {

        isAttackFinished = true;

    }

    public void DestroySelf() {

        Destroy(gameObject);
        isAttackFinished = true;
    }

    //public void StartAttack(string AttackName, GameObject SpellPrefab) {

    //    switch (AttackName) {

    //        case "FireBallSpell":

    //            break;

    //        case "SpiritAttack1":

    //            gameObject.GetComponent<Animator>().SetTrigger(AttackName);

    //            break;

    //        default:
    //        break;


    //    }


    //}

}
