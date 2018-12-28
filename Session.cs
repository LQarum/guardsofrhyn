using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Session : MonoBehaviour {

	public static string PlayerName;
	public static string Location;
	public static string Class;
	public static string Gender;
	public static string Side;
	public static int PlayerID;
	
	// ---------- STATS ----------
	public static int Level;
	public static float Health;
	public static float Energy;
	public static float Exp;
	public static int Strength;
	public static int Intelligence;
	public static int Initiative;
	
	public static bool InBattle;
	public static string InBattleWith;
	
	// ---------- STATS -----------
	
	
	// ---------- METHODS ---------
	
	public static bool LoggedIn { get {return PlayerName != null;} }
	
}
