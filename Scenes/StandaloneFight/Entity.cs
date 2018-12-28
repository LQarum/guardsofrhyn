using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System;

public class Entity 
{

    public string Name;

    public bool isTurnActionSelected;
    public bool TurnCompleted;
    public bool isPlayer; 
    public bool isSkillPanelOpen;
    public bool selectedForAttack;
    public bool hasTurn;
    public bool isAttackStarted;
    public bool isAttackFinished;

    public int Initiative;
    public int Strength;
    public int ID;

    public float MaxHealth;
    public float CurrentHealth;
    public float MaxEnergy;
    public float CurrentEnergy;

    public GameObject _GameObject;
    public GameObject _ObjectSelector;
    public GameObject SkillPanel;
    public GameObject[] AttackSkills;
    public GameObject[] FriendlySkills;
    public GameObject EntityButtonObject;
    public GameObject EffectTextBox;

    public GameObject Spell_1_Prefab;

    public int[,] Av_FriendlySkills;
    public int[,] Av_AttackSkills;
    public int[] OpponentToEntities; // list indeces of all entities that are opponets to this one
    public int[] FriendToEntities; // list indeces of all entities that are friends to this one

    public float HitValue;

    public bool HitDodge;
    public bool[] AttacksByPlayer;


    public string[] AttackAnimations;
    public string[] FriendlyAnimations;
    public string[] MiscAnimations;


    // NOT BELONGING TO THIS INSTANCE OF THE ENTITY
    public float CurrentPlayerEnergy;
    public float CurrentPlayerHealth;

    public Entity() { }

    public void Initialize() {

        Entity thisCopy = this;

        
        isTurnActionSelected = false;
        isSkillPanelOpen = false;
        SkillPanel.SetActive(false);
        HitDodge = false;
        selectedForAttack = false;
        hasTurn = false;
        isAttackStarted = false;
        isAttackFinished = false;

        AttackSkills = new GameObject[6];

        EntityButtonObject.transform.GetComponent<Button>().onClick.AddListener(OnEntitySelected);

        AllowControlsToPlayer(true);

        Debug.Log("Objeact associated with this Entity is: " + EntityButtonObject.name);

        AttacksByPlayer = new bool[6] { false, false, false, false, false, false };

        HitValue = 0.0f;

        if (!isPlayer)
        {
            AttackSkills = new GameObject[Av_AttackSkills.GetLength(0)];

            for (int i = 0; i < Av_AttackSkills.GetLength(0); i++)
            {
                // Assign event listener only if the available skills exists (not zero)
                if (Av_AttackSkills[i, 0] != 0)
                {
                    // Must delcare new instance every loop so that event listeners are new every time and not the last one of the loop
                    int b = i;

                    AttackSkills[i] = SkillPanel.transform.Find("AttackButton" + (i + 1).ToString()).transform.Find("Attack" + (i + 1).ToString()).gameObject;
                    AttackSkills[i].transform.GetComponent<Button>().onClick.AddListener(() => { UseSkill(AttackSkills[b]); });
                }
            }
        }
        else
        {

            FriendlySkills = new GameObject[Av_FriendlySkills.GetLength(0)];

            for (int i = 0; i < Av_FriendlySkills.GetLength(0); i++)
            {
                // Assign event listener only if the available skills exists (not zero)
                if (Av_FriendlySkills[i, 0] != 0)
                {
                    // Must delcare new instance every loop so that event listeners are new every time and not the last one of the loop
                    int b = i;

                    FriendlySkills[i] = SkillPanel.transform.Find("SkillButton" + (i + 1).ToString()).transform.Find("Skill" + (i + 1).ToString()).gameObject;
                    FriendlySkills[i].transform.GetComponent<Button>().onClick.AddListener(() => { UseSkill(FriendlySkills[b]); });
                }
            }

        }


    }

    public void ShowSkillPanel()
    {
        if (!isSkillPanelOpen)
        {

            SkillPanel.SetActive(true);
            isSkillPanelOpen = true;

            if (AttackSkills != null && AttackSkills.Length != 0)
            {
                for (int i = 0; i < AttackSkills.Length; i++)
                {

                    if (Av_AttackSkills[i, 0] != 0 && Av_AttackSkills[i, 1] <= CurrentPlayerEnergy)
                    {
                        AttackSkills[i].transform.GetComponent<Button>().interactable = true;
                    }
                    else if (Av_AttackSkills[i, 0] != 0 && Av_AttackSkills[i, 1] > CurrentPlayerEnergy) {
                        AttackSkills[i].transform.GetComponent<Button>().interactable = false;
                    }
                }
            }

            if (FriendlySkills != null && FriendlySkills.Length != 0)
            {
                for (int i = 0; i < FriendlySkills.Length; i++)
                {
                    if (Av_FriendlySkills[i, 0] != 0 && Av_FriendlySkills[i, 1] <= CurrentPlayerEnergy)
                    {
                        FriendlySkills[i].transform.GetComponent<Button>().interactable = true;

                    }
                    else if (Av_FriendlySkills[i, 0] != 0 && Av_FriendlySkills[i, 1] > CurrentPlayerEnergy) {
                        FriendlySkills[i].transform.GetComponent<Button>().interactable = false;
                    }
                        

                }
            }


        }
        else {

            SkillPanel.SetActive(false);
            isSkillPanelOpen = false;

        }

    }

    public void OnEntitySelected()
    {

        Debug.Log(":: Entity[" + ID + "] (" + Name + ") is selected by Player (static information)!");
        ShowSkillPanel();

    }

    public void TieUpdateGUI(Image Healthbar, Text Healthbar_Text, Image Energybar, Text Energybar_Text) {

        Healthbar.fillAmount = CurrentHealth / MaxHealth;
        Energybar.fillAmount = CurrentEnergy / MaxEnergy;

        Healthbar_Text.text = ((int)Math.Round(CurrentHealth)).ToString() + "/" + ((int)Math.Round(MaxHealth)).ToString();
        Energybar_Text.text = ((int)Math.Round(CurrentEnergy)).ToString() + "/" + ((int)Math.Round(MaxEnergy)).ToString();
    }

   

    public void SelectTurnActions() {


        if (!isTurnActionSelected)
        {
            if (isPlayer)
            {

                AllowControlsToPlayer(true);


            }
            else
            {

                AllowControlsToPlayer(false);

                HitValue = Strength * UnityEngine.Random.Range(0.80f, 1.2f);

                if (UnityEngine.Random.Range(0.0f, 100.0f) < 15.0f) HitDodge = true;

                isTurnActionSelected = true;

            }

        }

        // Select HIT AND VALUE
        // if player, enable controls


    }

    public void AllowControlsToPlayer(bool ControlTrigger) {

        EntityButtonObject.transform.GetComponent<Button>().interactable = ControlTrigger;

    }

    public void PlayAnimation(string AnimationTrigger) {

        // EMPTY SELECTED ACTIONS
        // PROJECT DAMAGE
        // RUN ANIMATIONS

        EntityButtonObject.GetComponent<Animator>().SetTrigger(AnimationTrigger);

    }

    public void UseSkill(GameObject Skill)
    {

        switch (Skill.name)
        {
            // 

            case "Attack1":
                    
                    Debug.Log("AttackButton1 Invoked");
                    AttacksByPlayer[0] = true;
                    AllowControlsToPlayer(false);
                    selectedForAttack = true;

                break;

            case "Attack2":
                    
                    Debug.Log("AttackButton2 Invoked");
                    AttacksByPlayer[1] = true;
                    AllowControlsToPlayer(false);
                    selectedForAttack = true;

                break;

            case "Attack3":
                    Debug.Log("AttackButton3 Invoked");
                    AttacksByPlayer[2] = true;
                    AllowControlsToPlayer(false);
                    selectedForAttack = true;

                break;

            case "Skill1":
                Debug.Log("SkillButton1 Invoked");
                break;

            case "Skill2":
                Debug.Log("SkillButton2 Invoked");
                break;

            case "Skill3":
                Debug.Log("SkillButton3 Invoked");
                break;

            default:
                break;

        }

        ShowSkillPanel();


    }

    public void ApplyEffect(string EffectName, float EffectMagnitude) {

        // Round to INT
        int RoundedMagnitude = 0;

        switch (EffectName) {


            case "hp":

                if (EffectMagnitude + CurrentHealth > MaxHealth) {

                    EffectMagnitude = MaxHealth - CurrentHealth;
                    CurrentHealth += EffectMagnitude;
                }

                
                RoundedMagnitude = (int)Math.Round(EffectMagnitude);

                //SPAWN TEXTBOX



                break;

            case "mp":

                if (EffectMagnitude + CurrentEnergy > MaxEnergy)
                {

                    EffectMagnitude = MaxEnergy - CurrentEnergy;
                    CurrentEnergy += EffectMagnitude;
                }

                RoundedMagnitude = (int)Math.Round(EffectMagnitude);

                // SPAWN TEXTBOX


                break;

            default:
                break;


        }


    }

    

}