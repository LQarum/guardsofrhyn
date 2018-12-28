using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class BattleManager : MonoBehaviour {
	
	private GameObject Spot1;
	private GameObject Spot2;
	private GameObject Spot3;
	private GameObject Spot4;
	private GameObject Spot5;
	private GameObject Spot6;
	private GameObject Spot7;
	private GameObject Spot8;
	private GameObject Spot9;
	private GameObject Spot10;
	
	private Vector3 TargetPosition;
	
	private GameObject EnemyPrefab;
	private GameObject Player1;
	private GameObject Enemy1;
	
	private GameObject selfSkillPanel;
	private GameObject enemySkillPanel;
	public GameObject FireBall;
	private GameObject FireBallSpell;
	
	
	public Text CounterText;
	
	public GameObject SkillsPanelFriend;
	public GameObject SkillsPanelEnemy;
	
	public GameObject PlayerPrefab;
	public GameObject Troll1;
	public GameObject Spirit1;

    public GameObject SpellOrigin;
	
	private float timeLeft = 30.0f;
	private bool PlayerGoesFirst = false;
	private bool PlayerMoveChosen = false;
	
	private float MaxHealth = 45.0f;

	// Use this for initialization
	void Start () {
		
		Session.InBattleWith = "spirit1";
		
			
		//UnityEngine.SceneManagement.SceneManager.LoadScene("LoginScene");
			
		
		Spot1 = GameObject.Find("Spot1");
		Spot2 = GameObject.Find("Spot2");
		Spot3 = GameObject.Find("Spot3");
		Spot4 = GameObject.Find("Spot4");
		Spot5 = GameObject.Find("Spot5");
		Spot6 = GameObject.Find("Spot6");
		Spot7 = GameObject.Find("Spot7");
		Spot8 = GameObject.Find("Spot8");
		Spot9 = GameObject.Find("Spot9");
		Spot10 = GameObject.Find("Spot10");
		
		switch(Session.InBattleWith) {
			
			case "troll1":
				
				EnemyPrefab = Troll1;
				
			break;
			
			case "spirit1":
			
				EnemyPrefab = Spirit1;
			
			break;
			
			default:
				Debug.Log("Error: No Enemy to Load was specified!");
			break;
			
		}
		
		Player1 = Instantiate(PlayerPrefab, Spot1.transform);
		Enemy1 = Instantiate(EnemyPrefab, Spot6.transform);
		
		
		Player1.transform.Find("WizardBody").GetComponent<Button>().onClick.AddListener(onSelfClick);
		Enemy1.transform.Find("SpiritBody").GetComponent<Button>().onClick.AddListener(onEnemyClick);
		
		
		
		
		
		TargetPosition += Spot6.transform.position;
		TargetPosition += new Vector3(0f, 2f, 0f);
		
		 
	}
	
	// Update is called once per frame
	void Update()
     {
         timeLeft -= Time.deltaTime;
		 
		 CounterText.text = ((int)Math.Round(timeLeft)).ToString();
		 
         if(timeLeft <= 0)
         {
             //GameOver();
			 Debug.Log("Time has run out!");
			 timeLeft = 30.0f;
			 
         }
		 
		 if (PlayerGoesFirst && FireBallSpell != null) {
			 
			 float step = 20.0f * Time.deltaTime;
			
        // Move our position a step closer to the target.
			FireBallSpell.transform.position = Vector3.MoveTowards(FireBallSpell.transform.position, TargetPosition, step);
			 
			if (FireBallSpell.transform.position == TargetPosition) 
			{
				
				Destroy(FireBallSpell);
				PlayerGoesFirst = false;
			}
		 }
		 
     }
	 
	 public void onSelfClick() {
		 
		 Player1.transform.Find("Selector").gameObject.SetActive(true);
		 Enemy1.transform.Find("Selector").gameObject.SetActive(false);
		 
		 if (selfSkillPanel == null) {
			 
			selfSkillPanel = Instantiate(SkillsPanelFriend, Spot1.transform);
			
		 }
		 
		 if (enemySkillPanel != null) {
			 
			 Destroy(enemySkillPanel);
			 
		 }
		 
	 }
	 
	 
	 public void onEnemyClick() {
		 
		 Enemy1.transform.Find("Selector").gameObject.SetActive(true);
		 Player1.transform.Find("Selector").gameObject.SetActive(false);
		 
		 
		 if (enemySkillPanel == null) {
		 
			enemySkillPanel = Instantiate(SkillsPanelEnemy, Spot6.transform);
			enemySkillPanel.transform.Find("AttackButton1").transform.Find("Attack1").GetComponent<Button>().onClick.AddListener(FireBallSpellGo);;
		 
		 }
		 
		 if (selfSkillPanel != null) {
			 
			 Destroy(selfSkillPanel);
			 
		 }

	 }
	 
	 public void FireBallSpellGo() {
		 
		 PlayerGoesFirst = true;
		 FireBallSpell = Instantiate(FireBall, SpellOrigin.transform);
		 Destroy(enemySkillPanel);
		 
	 }
 
	 
}














