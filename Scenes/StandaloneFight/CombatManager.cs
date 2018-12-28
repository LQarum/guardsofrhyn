using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using System.Linq;

public class CombatManager
{

    private int EntityHasCurrentTurn;

    public int PlayerEntityIndex;
    private int[,] AttackQueue;

    public bool isTurnExecutionInProgress;
    public bool isTurnOwnerChanged;
    public bool isGlobalTimerReset;

    private bool[] HaveAllEntitiesSelectedTurnAction;
    private bool[] HaveAllEntitiesSelectedTarget;

    public float TimerTime;
    private float TIMER_MAX;

    private Entity[] Entities;
    private Entity[] EntitiesSortedInTurnOrder;

    private Text TurnText;

    private GameObject TurnExcl1;
    private GameObject TurnExcl2;

    // Sets up required initial conditions and parameters on battle start
    public void Initialize(Entity[] _Entities, float _TimerTime, float _TIMER_MAX) {

        /* ------------- START INITIALIZE LOCAL VARIABLES AND PARAMETERS -------------- */
        {
            // PASSED IN VARIABLES:                                                     //  OF TYPE:

            Entities = new Entity[_Entities.Length];                                //  Entity[]
            Entities = _Entities;                                                   //  Entity[]
            TimerTime = _TimerTime;                                                 //  float
            TIMER_MAX = _TIMER_MAX;                                                 //  float

            // LOCALLY OWNED VARIABLES:

            HaveAllEntitiesSelectedTurnAction = new bool[Entities.Length];                      //  bool[]
            HaveAllEntitiesSelectedTarget = new bool[Entities.Length];
            EntitiesSortedInTurnOrder = new Entity[Entities.Length];                            //  int[]
            EntitiesSortedInTurnOrder = TurnDecider(Entities);                                  //  int[]
            AttackQueue = new int[Entities.Length, 2];                                          //  int[]
            EntityHasCurrentTurn = Array.IndexOf(Entities, EntitiesSortedInTurnOrder[0]);       //  int  
            Entities[EntityHasCurrentTurn].hasTurn = true;                                      //  bool
            isTurnExecutionInProgress = false;                                                        //  bool
            isTurnOwnerChanged = false;
            isGlobalTimerReset = false;

        }
        /* ------------- END INITIALIZE LOCAL VARIABLES AND PARAMETERS -------------- */

        for (int i = 0; i < Entities.Length; i++)
        {
            // Could contain DmgText control too as Animation has access to it
            //  Entities[i].EntityButtonObject.AddComponent<LocalEntityControl>() as LocalEntityControl;
            HaveAllEntitiesSelectedTurnAction[i] = false;

            if (Entities[i].isPlayer) PlayerEntityIndex = i;

        }
    }


    // Called every frame to keep track of battle status and progress
    public void OnUpdate(Entity[] _Entities, float _TimerTime, Text _TurnText, GameObject _TurnExcl1, GameObject _TurnExcl2)
    {


        /* ------------- START UPDATE LOCAL VARIABLES AND PARAMETERS -------------- */
        {
            // PASSED IN VARIABLES:                                                 //  OF TYPE:
            Entities = _Entities;                                                   //  Entity[]
            TimerTime = _TimerTime;                                                 //  float
            TurnText = _TurnText;
            TurnExcl1 = _TurnExcl1;
            TurnExcl2 = _TurnExcl2;


            // LOCALLY OWNED:

        }
        /* ------------- END UPDATE LOCAL VARIABLES AND PARAMETERS -------------- */


        // Keep the list of entities updated in the order of highest Initiative
        // Needs to be updated because initiative can change by potions
        EntitiesSortedInTurnOrder = TurnDecider(Entities);
        EntitiesSortedInTurnOrder = TurnSwitcher(EntitiesSortedInTurnOrder, Entities, TimerTime);

        for (int i = 0; i < Entities.Length; i++) {

            //HaveAllEntitiesSelectedTurnAction[i] = Entities[i].isTurnActionSelected;
            Entities[i].CurrentPlayerHealth = Entities[PlayerEntityIndex].CurrentHealth;
            Entities[i].CurrentPlayerEnergy = Entities[PlayerEntityIndex].CurrentEnergy;
        }

        if (HaveAllEntitiesSelectedTurnAction.Any(x => x == false)) {

            for (int i = 0; i < Entities.Length; i++)
            {

                if (EntitiesSortedInTurnOrder[i].hasTurn) {

                    Debug.Log(":: Entity[" + i.ToString() + "] (" + Entities[i].Name + ") has turn!");
                    EntityHasCurrentTurn = i;

                    if (EntitiesSortedInTurnOrder[i].isPlayer)
                    {
                        TurnText.text = "Your Turn";
                        TurnExcl1.SetActive(true);
                        TurnExcl2.SetActive(false);

                        for (int j = 0; j < Entities.Length; j++) {
                            EntitiesSortedInTurnOrder[j].AllowControlsToPlayer(true);
                        }

                    }
                    else {

                        TurnText.text = "Enemy's Turn";
                        TurnExcl1.SetActive(false);
                        TurnExcl2.SetActive(true);

                        for (int j = 0; j < Entities.Length; j++)
                        {
                            EntitiesSortedInTurnOrder[j].AllowControlsToPlayer(false);
                        }
                    }

                }

                HaveAllEntitiesSelectedTurnAction[i] = EntitiesSortedInTurnOrder[i].isTurnActionSelected;
                HaveAllEntitiesSelectedTarget[i] = EntitiesSortedInTurnOrder[i].selectedForAttack;

                if (!EntitiesSortedInTurnOrder[i].isTurnActionSelected && EntitiesSortedInTurnOrder[i].hasTurn)
                    EntitiesSortedInTurnOrder[i].SelectTurnActions();

                /* ----------- NPC TARGET SELECTOR START ----------- */

                // Describes what target an NPC should attack or interact with, attack for now
                // TODO: Fix a procedural attacker AI

                if (HaveAllEntitiesSelectedTurnAction.All(x => x == true)) {

                    int StrongestOpponentEntityIndex;
                    int[] OpponentEntitiesIndeces = new int[Entities[i].OpponentToEntities.Length];
                    int[] ArrayOfThisEntitysOpponentsStrengths = new int[Entities[i].OpponentToEntities.Length];

                    // NPC Selects Strongest Opponent Enemy to attack!
                    if (!Entities[i].isPlayer) {

                        for (int j = 0; j < Entities[i].OpponentToEntities.Length; j++) {

                            ArrayOfThisEntitysOpponentsStrengths[j] = Entities[Entities[i].OpponentToEntities[j]].Strength;
                            OpponentEntitiesIndeces[j] = Array.IndexOf(Entities, Entities[Entities[i].OpponentToEntities[j]]);

                        }

                        StrongestOpponentEntityIndex = Array.IndexOf(ArrayOfThisEntitysOpponentsStrengths, Mathf.Max(ArrayOfThisEntitysOpponentsStrengths));

                        AttackQueue[StrongestOpponentEntityIndex, 0] = StrongestOpponentEntityIndex;
                        AttackQueue[StrongestOpponentEntityIndex, 1] = Entities[i].ID;

                        Entities[StrongestOpponentEntityIndex].selectedForAttack = true;

                        Debug.Log(":: NPC_TargetSelector :: Strongest Opponent to NPC Entity[" + Entities[i].ID.ToString() + "] (" + Entities[i].Name + ") is Entity[" + StrongestOpponentEntityIndex + "] (" + Entities[StrongestOpponentEntityIndex].Name + ")");

                        Debug.Log(":: NPC_TargetSelector :: Entity[" + StrongestOpponentEntityIndex + "] (" + Entities[StrongestOpponentEntityIndex].Name + ") has been selected for attack by Entity[" + Entities[i].ID.ToString() + "] (" + Entities[i].Name + ")");

                    }

                }

                /*------------ NPC TARGET SELECTOR END ------------- */


                // POPULATE ATTACK QUEUE
                // TODO: Create other Queues or a better state machine to account for other effects like NPC healing
                // Takes care of Player target selection but not NPC - FIX TODO	
                if (EntitiesSortedInTurnOrder[i].selectedForAttack)
                {

                    if (EntitiesSortedInTurnOrder[i].AttacksByPlayer.Any(x => x == true))
                    {

                        AttackQueue[EntitiesSortedInTurnOrder[i].ID, 0] = EntitiesSortedInTurnOrder[i].ID;
                        AttackQueue[EntitiesSortedInTurnOrder[i].ID, 1] = PlayerEntityIndex;

                    }

                    Debug.Log(":: Entity[" + EntitiesSortedInTurnOrder[i].ID.ToString() + "] (" + EntitiesSortedInTurnOrder[i].Name + ") has been selected for attack by Entity[" + AttackQueue[EntitiesSortedInTurnOrder[i].ID, 1].ToString() + "] (" + Entities[AttackQueue[EntitiesSortedInTurnOrder[i].ID, 1]].Name + ")");

                    EntitiesSortedInTurnOrder[AttackQueue[EntitiesSortedInTurnOrder[i].ID, 1]].isTurnActionSelected = true;
                }

            }
        }

        if (HaveAllEntitiesSelectedTurnAction.All(x => x == true) && HaveAllEntitiesSelectedTarget.All(x => x == true) && EntityHasCurrentTurn == -1) {

            Debug.Log(":: All entities selected their turn actions!");
            ExecuteTurnActions();

        }

    }

    private Entity[] TurnDecider(Entity[] entities)
    {

        Array.Sort<Entity>(entities, new Comparison<Entity>((i1, i2) => i2.Initiative.CompareTo(i1.Initiative)));

        return entities;
    }

    private Entity[] TurnSwitcher(Entity[] entitiesSortedInTurnOrder, Entity[] entities, float _TimerTime)
    {
        TimerTime = _TimerTime;

        for (int i = 0; i < entities.Length; i++)
        {
            if ((entitiesSortedInTurnOrder[i].isTurnActionSelected || TimerTime < 0.1f || isGlobalTimerReset) && !isTurnExecutionInProgress)
            {

                entitiesSortedInTurnOrder[i].hasTurn = false;

                if (isGlobalTimerReset || TimerTime < 0.1f) {

                    entitiesSortedInTurnOrder[i].isTurnActionSelected = true;

                }

                isGlobalTimerReset = false;

                // Give turn to the next Entity in queue, making sure index is not out of range
                if (i + 1 < entities.Length)
                {
                    entitiesSortedInTurnOrder[i + 1].hasTurn = true;
                    Debug.Log(":: TurnSwitcher :: Turn handed over to Entity[" + (i + 1).ToString() + "] (" + entitiesSortedInTurnOrder[i + 1].Name + ")!");
                }
                //else 
                //{

                //    entitiesSortedInTurnOrder[0].hasTurn = true;
                //    TimerTime = 0.0f;
                //    Debug.Log(":: TurnSwitcher :: Turn handed over to Entity[" + (0).ToString() + "] (" + entitiesSortedInTurnOrder[0].Name + ")!");
                //}

                isTurnOwnerChanged = true;
            }
        }

        return entitiesSortedInTurnOrder;
    }

    private int k;

    private bool AttackQueueItemComplete = false;
    private bool AttackQueueItemStarted = false;

    private bool isSpellObjectCreated = false;

    private void ExecuteTurnActions()//Entity[] entities)
    {

        isTurnExecutionInProgress = true;
        Debug.Log(":: Executor :: Turn Execution Started!");
        EntityHasCurrentTurn = -1;

        TurnText.text = "Wait...";
        TurnExcl1.SetActive(false);
        TurnExcl2.SetActive(true);

        if(!AttackQueueItemStarted) {

            AttackQueueItemStarted = true;

            k = 0;

            EntitiesSortedInTurnOrder[k].AllowControlsToPlayer(false);

            for (int j = 0; j < AttackQueue.GetLength(1); j++) {

                if (AttackQueue[j, 1] == EntitiesSortedInTurnOrder[k].ID) {

                    Debug.Log(":: Executor :: Entity[" + EntitiesSortedInTurnOrder[k].ID + "] (" + EntitiesSortedInTurnOrder[k].Name + ") is attacking Entity[" + Entities[j].ID + "] (" + Entities[j].Name + ")");

                    Entity[] newEntities = new Entity[2];

                    newEntities = AttackEntity(EntitiesSortedInTurnOrder[k], Entities[j]);

                    EntitiesSortedInTurnOrder[k] = newEntities[0];
                    Entities[EntitiesSortedInTurnOrder[k].ID] = EntitiesSortedInTurnOrder[k];
                    Entities[j] = newEntities[1];
                    //isTurnExecutionInProgress = false;
                }

            }

        }

        

       // return entities;
    }

    private Vector3 OldPosition;
    private bool spellSet = false;

    public Entity[] AttackEntity(Entity EntityStartedAttack, Entity EntityStartedDefense)
    {
        int spell = 999;

        if (EntityStartedAttack.isPlayer && !AttackQueueItemComplete)
        {

            if (!spellSet)
            {
                for (int j = 0; j < EntityStartedDefense.AttacksByPlayer.Length; j++)
                {

                    if (EntityStartedDefense.AttacksByPlayer[j])
                    {
                        spell = j;
                        spellSet = true;
                    }
                } 
            }

            if (spell > 5) {

                spell = 999;
                AttackQueueItemComplete = true;
                Debug.Log("Unexpected Error Occurred in the Attacker! No attack spell was selected by player! Falling through to next error.");

            }

            if (spell == 0)
            {

                if (!isSpellObjectCreated)
                {
                    EntityStartedAttack.AttackSkills[0] = MonoBehaviour.Instantiate(EntityStartedAttack.Spell_1_Prefab, GameObject.Find("FireBallOrigin").transform);
                    isSpellObjectCreated = true;
                    OldPosition = new Vector3(EntityStartedAttack.AttackSkills[0].transform.position.x, EntityStartedAttack.AttackSkills[0].transform.position.y);
                    Debug.Log(" :: Attacker :: Attack 1 Object is spawned by Entity[" + EntityStartedAttack.ID + "] (" + EntityStartedAttack.Name + ")");
                }
                else
                {

                    EntityStartedAttack.AttackSkills[0].transform.LookAt(EntityStartedDefense._GameObject.transform);
                    EntityStartedAttack.AttackSkills[0].transform.position = Vector3.MoveTowards(OldPosition, new Vector3(EntityStartedDefense._GameObject.transform.position.x, EntityStartedDefense._GameObject.transform.position.y), 3f);

                    if (EntityStartedAttack.AttackSkills[0].transform.position == EntityStartedDefense._GameObject.transform.position)
                    {
                        EntityStartedDefense.EntityButtonObject.GetComponent<Animator>().SetTrigger(EntityStartedDefense.MiscAnimations[0]);
                        AttackQueueItemComplete = true;
                        MonoBehaviour.Destroy(EntityStartedAttack.AttackSkills[0]);
                    }

                    Debug.Log("Moving the object!");

                }
            }
            else if (spell == 1)
            {
                Debug.Log("Spell = 1");
            }
            else if (spell == 2)
            {
                Debug.Log("Spell = 1");
            }
            else
            {
                Debug.Log(":: Attacker :: Incorrect player attack was selected or no attack was selected!");
            }
        }

        else {

            // NPC turn
            Debug.Log("No spell");

        }

        if (AttackQueueItemComplete)
        {
            if (k + 1 > EntitiesSortedInTurnOrder.Length) { k = 0; } else { k = k + 1; }
            AttackQueueItemStarted = false;
            AttackQueueItemComplete = false;
        }

        Entity[] overrideInvolvedEntities = new Entity[2];
        overrideInvolvedEntities[0] = EntityStartedAttack;
        overrideInvolvedEntities[1] = EntityStartedDefense;

        return overrideInvolvedEntities;

    }


}

/*
           if (EntityStartedAttack.isPlayer && !EntityStartedAttack.EntityButtonObject.GetComponent<LocalEntityControl>().isAttackFinished)
           {

               for (int i = 0; i < EntityStartedDefense.AttacksByPlayer.Length; i++)
               {
                   if (EntityStartedDefense.AttacksByPlayer[i])
                   {

                       if (i == 0)
                       {
                           EntityStartedAttack.EntityButtonObject.GetComponent<LocalEntityControl>().StartAttack("FireBallSpell", EntityStartedAttack.Spell_1_Prefab);
                           EntityStartedDefense.AttacksByPlayer[i] = false;
                       }

                       else
                       {

                           Debug.Log(" :: Entity [" + i + "] :: Animation for attack + " + i + " does not exist!");

                       }

                   }
               }
           }

           else if (!EntityStartedAttack.isPlayer && (EntityStartedAttack.EntityButtonObject.GetComponent<LocalEntityControl>().isAttackFinished))
           {

               EntityStartedDefense.PlayAnimation(EntityStartedDefense.AttackAnimations[0]);

           }
           */
