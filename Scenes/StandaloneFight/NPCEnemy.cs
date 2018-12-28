using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class NPCEnemy {

    public string Name;
    public float Strength;
    public float Health;
    public float MaxHealth;
    public float Energy;
    public float MaxEnergy;
    public float Hit;

    public GameObject Prefab;

    public NPCEnemy() {


    }

    public float GetAttackResults() {

        return Strength * UnityEngine.Random.Range(0.80f, 1.2f);

    }

    public void UpdateGUIStats(Image EnemyHealthBar, Text EnemyHealthText, Image EnemyEnergyBar, Text EnemyEnergyText) {

        EnemyHealthBar.fillAmount = Health / MaxHealth;
        EnemyEnergyBar.fillAmount = Energy / MaxEnergy;
        EnemyHealthText.text = ((int)Math.Round(Health)).ToString() + "/" + ((int)Math.Round(MaxHealth)).ToString();
        EnemyEnergyText.text = ((int)Math.Round(Energy)).ToString() + "/" + ((int)Math.Round(MaxEnergy)).ToString();

    }
	
}
