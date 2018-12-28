using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class EnemyAnimControl : MonoBehaviour {

    public GameObject Damage;
    private GameObject DamageCont;

    //public void SetAnimationFinished() {

    //    SceneControl.enemyAttackAnimationPlayed = true;

    //}

    //public void SetHitMade()
    //{

    //    Debug.Log("Entered enemy attack");

    //    SceneControl.Enemy.Hit = SceneControl.Enemy.GetAttackResults();

    //    DamageCont = Instantiate(Damage, GameObject.Find("DmgTarget").transform);

    //    if (UnityEngine.Random.Range(0.0f, 100.0f) < 15.0f)
    //    {
    //        DamageCont.transform.Find("DamageText").transform.GetComponent<Text>().text = "Dodge";
    //    }
    //    else
    //    {
    //        SceneControl.PlayerCurrentHealth -= SceneControl.Enemy.Hit;
    //        DamageCont.transform.Find("DamageText").transform.GetComponent<Text>().text = "-" + ((int)Math.Round(SceneControl.Enemy.Hit)).ToString();
    //    }

    //    DamageCont.transform.Find("DamageText").transform.GetComponent<Text>().GetComponent<Animator>().SetTrigger("DamageReceived");
    //    SceneControl.enemyAttackHitMade = true;

    //}

}
