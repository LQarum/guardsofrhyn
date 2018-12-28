using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;

public class Login : MonoBehaviour {

	public InputField nameField;
	public InputField passwordField;
	public Button LoginButton;

    public int _PORT = 8888;
    public string _ADDRESS = "127.0.0.1";

    private static bool isUserLoggedIn = false;

    private NetworkClient Client;

    private void Start() {

        Client = new NetworkClient();
        Client.RegisterHandler(MsgType.Connect, OnClientConnectedToServer);
        Client.Connect(_ADDRESS, _PORT);
    }

    private void OnClientConnectedToServer(NetworkMessage netMsg)
    {

        Debug.Log(":: Client Connected to Server!");
        isUserLoggedIn = true;

      //  NetworkMessage msg;
      //  msg.name = "";

    }


    public void CallLoginPlayer() {

        //StartCoroutine(LoginPlayer());

        if(isUserLoggedIn)
            UnityEngine.SceneManagement.SceneManager.LoadScene("InitTestArea");

	}
	
   
    public void RegisterButtonClick() {

            UnityEngine.SceneManagement.SceneManager.LoadScene("RegisterScreen");

        }

    /*

        IEnumerator LoginPlayer() {

            WWWForm LoginForm = new WWWForm();

            LoginForm.AddField("playername", nameField.text);
            LoginForm.AddField("password", passwordField.text);

            WWW sendRequest = new WWW("http://localhost/gor/login.php", LoginForm);
            yield return sendRequest;

            string[] returnedData = sendRequest.text.Split('\t');

            if (returnedData[0] == "0") {

                Debug.Log("Player has been successfully logged in! " + returnedData[1] + "\t" + int.Parse(returnedData[2]) + "\t");


                Session.PlayerName = nameField.text;
                Session.PlayerID = int.Parse(returnedData[2]);

                if (returnedData[1] == "CREATE_CHARACTER") {

                    Debug.Log("Character not created!");
                    UnityEngine.SceneManagement.SceneManager.LoadScene("CharCreateScene");

                } else {

                        returnedData[] INDECES:

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



                    Session.Location = returnedData[1];
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

                    Session.InBattle = false;


                    Debug.Log("Character is already created! Loading Location: " + returnedData[1]);
                    UnityEngine.SceneManagement.SceneManager.LoadScene(returnedData[1]);

                }

            } else {

                Debug.Log("Error: Unable to login! " + sendRequest.text);

            }

        }

    */

    public void VerifyInputs() {
		
		LoginButton.interactable = (nameField.text.Length >= 4 && passwordField.text.Length >= 5);
		
	}
}
