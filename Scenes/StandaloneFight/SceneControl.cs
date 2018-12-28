using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System;
using System.Linq;

public class SceneControl : MonoBehaviour {

    public Image HealthBar;
    public Image GenEnergyBar;
    public Text HealthText;
    public Text GenEnergyText;

    public Image EnemyHealthBar;
    public Image EnemyEnergyBar;
    public Text EnemyHealthText;
    public Text EnemyEnergyText;

    public GameObject PlayerPrefab;
    public GameObject EnemyPrefab;

    public GameObject EnemySkillPanelPrefab;
    public GameObject FriendSkillPanelPrefab;

    private bool enemyAttackAnimationStarted;
    public static bool enemyAttackAnimationPlayed;
    public static bool enemyAttackHitMade;

    public static GameObject Damage;
    public GameObject DamagePlus;
    public static GameObject DamageCont;

    public GameObject FireballSpellPrefab1;

    private Text TimerText;
    private Text TimerTextBehind;
    private Text TurnText;
    private Text TurnTextShadow;

    private int OPPONENTS_COUNT;
    private int FRIENDS_COUNT;

    private GameObject TurnExcl1;
    private GameObject TurnExcl2;

    private GameObject Spot1;
    private GameObject Spot6;

    private float TimerTime;
    private float TIMER_MAX;

    public static Entity[] Entities = new Entity[2];

    private CombatManager _CombatManager;

    // Use this for initialization
    void Start()
    {

        Time.timeScale = 1.0f;
        TIMER_MAX = 30.0f;
        TimerTime = TIMER_MAX;

        TimerText = GameObject.Find("CounterText").GetComponent<Text>();
        TimerTextBehind = GameObject.Find("CounterTextBehind").GetComponent<Text>();
        TurnText = GameObject.Find("TurnText").GetComponent<Text>();
        TurnTextShadow = GameObject.Find("TurnTextShadow").GetComponent<Text>();

        Spot1 = GameObject.Find("Spot1").gameObject;
        Spot6 = GameObject.Find("Spot6").gameObject;

        TurnText.text = "Your Turn!";

        TurnExcl1 = GameObject.Find("Exclamation_Red").gameObject;
        TurnExcl1.SetActive(true);
        TurnExcl2 = GameObject.Find("Exclamation_Gray").gameObject;
        TurnExcl2.SetActive(false);

        OPPONENTS_COUNT = 1;
        FRIENDS_COUNT = 1;


        Entities[0] = new Entity
        {
            Name = "Mighty Hero",
            ID = 0,
            MaxHealth = 200,
            CurrentHealth = 200,
            MaxEnergy = 240,
            CurrentEnergy = 240,
            Initiative = 14,
            Strength = 16,
            isPlayer = true,
            _GameObject = Instantiate(PlayerPrefab, Spot1.transform)
        };

        
        Entities[0].EntityButtonObject = Entities[0]._GameObject.transform.Find("WizardBody").gameObject;
        Entities[0]._ObjectSelector = Entities[0]._GameObject.transform.Find("Selector").gameObject;
        Entities[0].SkillPanel = Instantiate(FriendSkillPanelPrefab, Entities[0]._GameObject.transform);
        Entities[0].Av_AttackSkills = new int[6, 2] { { 1, 10 }, { 2, 15 }, { 3, 30 }, { 0, 0 }, { 0, 0 }, { 0, 0 } };
        Entities[0].Av_FriendlySkills = new int[6, 2] { { 1, 15 }, { 2, 25 }, { 3, 35 }, { 0, 0 }, { 0, 0 }, { 0, 0 } };
        Entities[0].AttackAnimations = new string[] {"FireBallSpell"}; // attack movement
        Entities[0].FriendlyAnimations = new string[] { "" }; // heal, summon
        Entities[0].MiscAnimations = new string[] { "PlayerHurt1" }; // dodge, hurt
        Entities[0].Spell_1_Prefab = FireballSpellPrefab1;
        Entities[0].Initialize();


        Entities[1] = new Entity
        {
            Name = "Restless Spirit",
            ID = 1,
            MaxHealth = 300,
            CurrentHealth = 300,
            MaxEnergy = 80,
            CurrentEnergy = 80,
            Initiative = 11,
            Strength = 12,
            isPlayer = false,
            _GameObject = Instantiate(EnemyPrefab, Spot6.transform)
        };

        Entities[1].EntityButtonObject = Entities[1]._GameObject.transform.Find("SpiritBody").gameObject;
        Entities[1]._ObjectSelector = Entities[1]._GameObject.transform.Find("Selector").gameObject;
        Entities[1].SkillPanel = Instantiate(EnemySkillPanelPrefab, Entities[1]._GameObject.transform);
        Entities[1].Av_AttackSkills = new int[6, 2] { {1, 10}, {2, 15}, {3, 30}, {0, 0}, {0, 0}, {0, 0} };
        Entities[1].Av_FriendlySkills = new int[6, 2] { { 0, 0 }, { 0, 0 }, { 0, 0 }, { 0, 0 }, { 0, 0 }, { 0, 0 } };
        Entities[1].AttackAnimations = new string[] { "SpiritAttack1" }; // attack movement
        Entities[1].FriendlyAnimations = new string[] { "" }; // heal, summon
        Entities[1].MiscAnimations = new string[] { "SpiritTakeDamage" }; // dodge, hurt
        Entities[1].Initialize();


        // SET ENTITY RELATIONS
        Entities[0].OpponentToEntities = new int[] { 1 };
        Entities[1].OpponentToEntities = new int[] { 0 };

        _CombatManager = new CombatManager();
        _CombatManager.Initialize(Entities, TimerTime, TIMER_MAX);

    }


    private void Update()
    {
        updateTimer();

        //TODO: make the character stats GUI be spawnable and put TieUpdateGui calls with Intantiate into the for loop
        Entities[0].TieUpdateGUI(HealthBar, HealthText, GenEnergyBar, GenEnergyText);
        Entities[1].TieUpdateGUI(EnemyHealthBar, EnemyHealthText, EnemyEnergyBar, EnemyEnergyText);

        _CombatManager.OnUpdate(Entities, TimerTime, TurnText, TurnExcl1, TurnExcl2);

    }


    private void updateTimer() {

        // TODO: IF ALL TURN ACTIONS EXECUTION MODE - PAUSE TIMER
        if (_CombatManager.isTurnExecutionInProgress)
        {
            TimerTime = 0.0f;
        }
        else
        {
            if (_CombatManager.isTurnOwnerChanged)
            {
                TimerTime = TIMER_MAX;
                _CombatManager.isGlobalTimerReset = true;
                _CombatManager.isTurnOwnerChanged = false;
            }

            TimerTime -= Time.deltaTime * Time.timeScale;
        }

        TimerText.text = ((int)Math.Round(TimerTime)).ToString();
        TimerTextBehind.text = TimerText.text;

        TurnTextShadow.text = TurnText.text;

    }

    public void onPotionClick(GameObject Potion) {

        string text = "";

        if (Entities[_CombatManager.PlayerEntityIndex].isPlayer && Entities[_CombatManager.PlayerEntityIndex].hasTurn)
        {
            switch (Potion.name)
            {
                case "hp":

                    Entities[_CombatManager.PlayerEntityIndex].ApplyEffect("hp", 20.0f);

                    //if (PlayerCurrentHealth + 20f > PlayerHealthMax)
                    //{
                    //    text = "+" + ((int)(Math.Round(PlayerHealthMax - PlayerCurrentHealth))).ToString();
                    //    PlayerCurrentHealth += PlayerHealthMax - PlayerCurrentHealth;
                    //}
                    //else
                    //{
                    //    PlayerCurrentHealth += 20f;
                    //    text = "+20";
                    //}

                    //DamageCont = Instantiate(DamagePlus, GameObject.Find("DmgTarget").transform);
                    //DamageCont.transform.Find("HealingText").transform.GetComponent<Text>().text = text;
                    //DamageCont.transform.Find("HealingText").transform.GetComponent<Text>().GetComponent<Animator>().SetTrigger("HealingReceived");

                    break;

                case "mp":

                    Entities[_CombatManager.PlayerEntityIndex].ApplyEffect("mp", 15.0f);

                    //if (PlayerCurrentEnergy + 15f > PlayerGenEnergyMax)
                    //{
                    //    text = "+" + ((int)(Math.Round(PlayerGenEnergyMax - PlayerCurrentEnergy))).ToString();
                    //    PlayerCurrentEnergy += PlayerGenEnergyMax - PlayerCurrentEnergy;
                    //}
                    //else
                    //{
                    //    PlayerCurrentEnergy += 15f;
                    //    text = "+15";
                    //}
                    //DamageCont = Instantiate(DamagePlus, GameObject.Find("DmgTarget").transform);
                    //DamageCont.transform.Find("HealingText").transform.GetComponent<Text>().text = text;
                    //DamageCont.transform.Find("HealingText").transform.GetComponent<Text>().GetComponent<Animator>().SetTrigger("ManaReceived");

                    break;

                default:
                    break;

            }

            
            Destroy(Potion);

        }
    }

    
}
