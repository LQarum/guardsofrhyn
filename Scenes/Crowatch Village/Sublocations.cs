using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Sublocations : MonoBehaviour {
	
	public GameObject Center;
	public GameObject Left;
	public GameObject Chapel;
	public GameObject Right;
	public GameObject Down;
	
	void Start() {
		
		Left.SetActive(false);
		Right.SetActive(false);
		Down.SetActive(false);
		Chapel.SetActive(false);
		Center.SetActive(true);
		
	}
	

	public void GoLeft() {
		
		Left.SetActive(true);
		Right.SetActive(false);
		Down.SetActive(false);
		Chapel.SetActive(false);
		Center.SetActive(false);
	}
	
	public void GoRight() {
		
		Left.SetActive(false);
		Right.SetActive(true);
		Down.SetActive(false);
		Chapel.SetActive(false);
		Center.SetActive(false);
	}
	
	public void GoChapel() {
		
		Left.SetActive(false);
		Right.SetActive(false);
		Down.SetActive(false);
		Chapel.SetActive(true);
		Center.SetActive(false);
	}
	
	public void GoDown() {
		
		Left.SetActive(false);
		Right.SetActive(false);
		Down.SetActive(true);
		Chapel.SetActive(false);
		Center.SetActive(false);
	}
	
	public void GoBackLeft() {
		
		Left.SetActive(true);
		Right.SetActive(false);
		Down.SetActive(false);
		Chapel.SetActive(false);
		Center.SetActive(false);
		
	}
	
	public void GoCenter() {
		
		Left.SetActive(false);
		Right.SetActive(false);
		Down.SetActive(false);
		Chapel.SetActive(false);
		Center.SetActive(true);
		
	}
	
	
	
	
}
