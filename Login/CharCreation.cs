using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CharCreation : MonoBehaviour {

	public Text DescPanelText;
	
	public Button MaleButton;
	public Button FemaleButton;
	public Button EnchanterButton;
	public Button RecruitButton;
	
	public GameObject BackLightSelector1;
	public GameObject BackLightSelector2;
	
	public GameObject WizardMale;
	public GameObject WizardFemale;
	public GameObject RecruitMale;
	public GameObject RecruitFemale;
	
	public GameObject MaleSelectorLight;
	public GameObject FemaleSelectorLight;
	public GameObject EnchSelectorLight;
	public GameObject RecSelectorLight;
	
	public GameObject Flag1;
	public GameObject Flag2;
	
	private string Class = "Recruit";
	private string Gender = "Male";
	private string Side = "Empire";
	
	private GameObject GenderSelector;
	private GameObject ClassSelector;

	
	public void Side1_Btn_Click() {
		
		BackLightSelector1.SetActive(true);
		BackLightSelector2.SetActive(false);
		Flag1.SetActive(true);
		Flag2.SetActive(false);
		
		Side = "Alliance";
		
		DescPanelText.text = "\nThanes Alliance\n\nThis Alliance was formed in the response to " +
		"the gathering forces of the Southern Empire. Loyal to their land and ancestors, Thanes of Rhyn have joined their forces to defeat the invaders from the South." + 
		"\n\nThe Alliance values honor, loyalty, order, and a higher purpose of the land of Rhyn.";
	}
	
	public void Side2_Btn_Click() {
		
		BackLightSelector2.SetActive(true);
		BackLightSelector1.SetActive(false);
		
		Side = "Empire";
		
		Flag1.SetActive(false);
		Flag2.SetActive(true);
		
		DescPanelText.text = "\nSouthern Empire\n\n Long has the noble folk of the Southern seas been bullied by the commanding and fanatical inhabitants of the Old World. " + 
		"Scattered across many villages, forces of the South were collected under the new Emperor. He will bring the order of the new world to Rhyn.\n\n" + 
		"The Empire values valor, courage, strength, and ability.";
		
	}
	
	public void Male_Btn_Click() {
		
		Gender = "Male";
		
		if (Class == "Enchanter") {
			
			WizardMale.SetActive(true);
			WizardFemale.SetActive(false);
			RecruitFemale.SetActive(false);
			RecruitMale.SetActive(false);
			
		} else {
			
			WizardMale.SetActive(false);
			WizardFemale.SetActive(false);
			RecruitFemale.SetActive(false);
			RecruitMale.SetActive(true);
		}
		
		MaleSelectorLight.SetActive(true);
		FemaleSelectorLight.SetActive(false);
		
		DescPanelText.text = "\n\nMen have been traditionally trained from young age to be courageous fighters and defenders of their creed.";
		
	}
	
	public void Female_Btn_Click() {
		
		Gender = "Female";
		
		if (Class == "Enchanter") {
			
			WizardMale.SetActive(false);
			WizardFemale.SetActive(true);
			RecruitFemale.SetActive(false);
			RecruitMale.SetActive(false);
			
		} else {
			
			WizardMale.SetActive(false);
			WizardFemale.SetActive(false);
			RecruitFemale.SetActive(true);
			RecruitMale.SetActive(false);
		}
		
		MaleSelectorLight.SetActive(false);
		FemaleSelectorLight.SetActive(true);
		
		DescPanelText.text = "\n\nWomen of the land of Rhyn have shown true power to the previously all-paternal empire. They lack neither strength nor wit.";

	}
	public void Enchanter_Btn_Click() {
		
		Class = "Enchanter";
		
		if (Gender == "Male") {
			
			WizardMale.SetActive(true);
			WizardFemale.SetActive(false);
			RecruitFemale.SetActive(false);
			RecruitMale.SetActive(false);
			
		} else {
			
			WizardMale.SetActive(false);
			WizardFemale.SetActive(true);
			RecruitFemale.SetActive(false);
			RecruitMale.SetActive(false);
		}
		
		EnchSelectorLight.SetActive(true);
		RecSelectorLight.SetActive(false);
		
		DescPanelText.text = "\n'Enchanter' Class\n\nNot all kids are interested in fight and brute force. Some just prefer reading ancient scrolls and studying the mysteries of this land. " + 
		"Those who choose the path of Enchanters at the age of 10, join the Order of Magic, and can further elect to specialize in one of the three fundamental specialties:\n\nDruid, Battlemage, or Necromancer.";
		
	}
	
	public void Recruit_Btn_Click() {
		
		Class = "Recruit";
		
		if (Gender == "Male") {
			
			WizardMale.SetActive(false);
			WizardFemale.SetActive(false);
			RecruitFemale.SetActive(false);
			RecruitMale.SetActive(true);
			
		} else {
			
			WizardMale.SetActive(false);
			WizardFemale.SetActive(false);
			RecruitFemale.SetActive(true);
			RecruitMale.SetActive(false);
		}
		
		EnchSelectorLight.SetActive(false);
		RecSelectorLight.SetActive(true);
		
		DescPanelText.text = "\n'Recruit' Class\n\nRecruits have been training in the art of sword and shield all their childhood years. Becoming a fiest fierce of their legion is the goal of any young Recruit. " + 
		"When a child is old enough to confidently wield their weapon, they are sent to join the Order of Knights to then further become one of the three: \n\nKnight, Archer, or Berserk.";
		
	}
	
	public void ConfirmCharacter() {
		
		StartCoroutine(CreateCharacter());
		
	}
	
	IEnumerator CreateCharacter() {
		
		WWWForm CharForm = new WWWForm();
		
		Session.Class = Class;
		Session.Side = Side;
		Session.Gender = Gender;
		
		CharForm.AddField("id", Session.PlayerID);
		CharForm.AddField("class", Class);
		CharForm.AddField("gender", Gender);
		CharForm.AddField("side", Side);
		
		WWW sendRequest = new WWW(GlobalVars.SERVER_ADDRESS + "scripts/gameplay/create_character.php", CharForm);
		yield return sendRequest;
		
		string[] returnedData = sendRequest.text.Split('\t');
		
		if (returnedData[0] == "0") {
			
			/*	returnedData[] INDECES:
				
				0	:	Success Code
				1	: 	Last Location or CREATE_CHARACTER	(string)
				2	:	Player ID			(int)
				3	:	Side				(string)
				4	:	Class				(string)
				5	:	Gender				(string)
				6	:	Level 				(int)
				7	:	Health 				(float)
				8	:	Energy				(float)
				9	:	Experience 			(float)
				10	:	Strength 			(int)
				11	:	Intelligence 		(int)
				12	:	Initiative			(int)
			*/
				
			Session.Side = returnedData[3];
			Session.Class = returnedData[4];
			Session.Gender = returnedData[5];
			Session.Level = int.Parse(returnedData[6]);
			Session.Health = float.Parse(returnedData[7]);
			Session.Energy = float.Parse(returnedData[8]);
			Session.Exp	= float.Parse(returnedData[9]);
			Session.Strength = int.Parse(returnedData[10]);
			Session.Intelligence = int.Parse(returnedData[11]);
			Session.Initiative = int.Parse(returnedData[12]);
			
			Debug.Log("Character has been successfully created!");
			UnityEngine.SceneManagement.SceneManager.LoadScene(returnedData[1]);
			
		} else {
			
			Debug.Log("Error: Character could not be created! " + sendRequest.text);
			
		}
		
	}
	
}
