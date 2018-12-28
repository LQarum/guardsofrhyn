using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SceneInit : MonoBehaviour {
	
	public Text PlayerName;
	public Text PlayerLevel;
	public Text HealthBar;
	public Text EnergyBar;
	
	public GameObject MaleIcon;
	public GameObject FemaleIcon;
	
	public GameObject SmallPopup;

	// Use this for initialization
	void Start () {
		
		SmallPopup.SetActive(false);
		
		if (Session.LoggedIn) {
			
			Session.Location = "InitTestArea";
			
			PlayerName.text = Session.PlayerName;
			PlayerLevel.text = Session.Level.ToString();
			EnergyBar.text = Session.Energy.ToString() + "/" + Session.Energy.ToString();
			HealthBar.text = Session.Health.ToString() + "/" + Session.Health.ToString();
			
			if (Session.Gender == "Male") {
				
				MaleIcon.SetActive(true);
				FemaleIcon.SetActive(false);
				
			} else if (Session.Gender == "Female") {
				
				MaleIcon.SetActive(false);
				FemaleIcon.SetActive(true);
				
			}
		}
	}
	
	// Update is called once per frame
	void Update () {
		
	}
	
	public void Logout() { StartCoroutine(Logout_Initiate()); }
	
	private IEnumerator Logout_Initiate() {
		
		WWWForm LogoutForm = new WWWForm();
		
		LogoutForm.AddField("id", Session.PlayerID);
		LogoutForm.AddField("location", Session.Location);
		
		WWW sendRequest = new WWW(GlobalVars.SERVER_ADDRESS + "logout.php", LogoutForm);
		yield return sendRequest;
		
		if (sendRequest.text == "0") {
			
			UnityEngine.SceneManagement.SceneManager.LoadScene("LoginScene");
		
		} else {
			
			Debug.Log("Unable to logout the player!" + sendRequest.text);
			
		}
		
		// TODO: create logout.php and finish logout script + destroy PHP session + reload page or send to INDEX
		
	}
	
}
