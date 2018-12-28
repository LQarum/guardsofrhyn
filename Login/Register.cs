using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Register: MonoBehaviour {

	public InputField nameField;
	public InputField passwordField;
	public InputField passwordConfirmField;
	public InputField emailField;
	public InputField emailConfirmField;

	public Button RegisterButton;
	
	public void CallRegisterPlayer() {
		
		StartCoroutine(RegisterPlayer());
		
	}
	
	IEnumerator RegisterPlayer() {
		
		WWWForm RegForm = new WWWForm();
		
		RegForm.AddField("playername", nameField.text);
		RegForm.AddField("password", passwordField.text);
		RegForm.AddField("email", emailField.text);
		
		WWW sendRequest = new WWW("http://localhost/gor/register.php", RegForm);
		yield return sendRequest;
		
		if (sendRequest.text == "0") {
			
			Debug.Log("Player has been successfully created!");
			UnityEngine.SceneManagement.SceneManager.LoadScene("LoginScene");
			
		} else {
			
			Debug.Log("Error: Player could not be created! " + sendRequest.text);
			
		}
		
	}
	
	public void VerifyInputs() {
		
		RegisterButton.interactable = (nameField.text.Length >= 4 && passwordField.text.Length >= 5 && passwordField.text == passwordConfirmField.text && emailField.text.Length >= 6 && emailField.text == emailConfirmField.text);
		
	}
	
	public void GoBackToScene() {
		
		UnityEngine.SceneManagement.SceneManager.LoadScene("LoginScene");
		
	}
}
