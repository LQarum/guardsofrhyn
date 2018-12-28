using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GhostLogic : MonoBehaviour {

	public GameObject Popup;
	public Text PopupTitleText;
	public Text PopupDescText;
	public Text ButtonYesText;
	public Text ButtonNoText;
	public Image PopupImage;
	public Sprite GhostPortrait;
	public Button PopupButtonYes;
	
	
	public void BringUpPopup() {
	
	
		PopupTitleText.text = "Restless Spirit";
		PopupDescText.text = "Weak Restless Spirit (Lvl 1) near Abandoned Mansion";
		ButtonYesText.text = "Attack";
		ButtonNoText.text = "Cancel";
		PopupImage.sprite = GhostPortrait;
		PopupImage.preserveAspect = false;
		
		
		PopupButtonYes.onClick.AddListener(AttackThis);
		
		Popup.SetActive(true);
		
	}
	
	public void AttackThis() {
		
		PopupButtonYes.onClick.RemoveAllListeners();
		StartCoroutine(StartBattleWithSpirit());
		
	}
	
	IEnumerator StartBattleWithSpirit() {
		
		WWWForm AttackForm = new WWWForm();
		
		
		
		AttackForm.AddField("id", Session.PlayerID);
		AttackForm.AddField("enemytype", "spirit1");
		
		WWW sendRequest = new WWW(GlobalVars.SERVER_ADDRESS + "scripts/gameplay/begin_battle.php", AttackForm);
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
			
			*/
			
			Debug.Log(returnedData[1]);
			Session.InBattle = true;
			//UnityEngine.SceneManagement.SceneManager.LoadScene("BattleScene1");
			
		} else {
			
			Debug.Log("Error: Cannot attack the spirit! " + sendRequest.text);
			
		}
			
		
	}
	
}
