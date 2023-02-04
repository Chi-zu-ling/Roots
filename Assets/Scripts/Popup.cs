using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Popup : MonoBehaviour
{

	public static Popup instance;
	public int cost;
	public int energyGain;
	public int waterGain;

	private void Awake()
	{
		instance = this;
	}
}
