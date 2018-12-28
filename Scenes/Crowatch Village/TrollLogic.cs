using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TrollLogic : MonoBehaviour {
	
	//public GameObject Troll;
	
	void Start() {
		
		Animator anim = GetComponent<Animator>();
		AnimatorStateInfo state = anim.GetCurrentAnimatorStateInfo (1);
		anim.Play (state.fullPathHash, -1, Random.Range(0f,1f));
		
	}
	// Update is called once per frame
	void Update () {
		
		
	}
	
	
}