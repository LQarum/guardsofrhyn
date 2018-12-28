using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireBallLogic : MonoBehaviour {

    public void FireballEndOfAnimation()
    {

        Destroy(gameObject);
        //SceneControl.Enemy.Prefab.transform.Find("SpiritBody").GetComponent<Animator>().SetTrigger("SpiritTakeDamage");
    }

}
