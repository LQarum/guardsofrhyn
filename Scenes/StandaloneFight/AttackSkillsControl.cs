using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackSkillsControl : MonoBehaviour {

    public GameObject FireballPrefab;
    private GameObject FireBall;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void AttackSkill1()
    {

        //Destroy(SceneControl.EnemySkillPanel);
        //SceneControl.EnemySkillPanelCreated = false;
        //SceneControl.Enemy.Prefab.transform.Find("Selector").gameObject.SetActive(false);

        //FireBall = Instantiate(FireballPrefab, GameObject.Find("Canvas").transform);
        //FireBall.transform.GetComponent<Animator>().SetTrigger("FireBallSpell");

    }
}
