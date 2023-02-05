using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Popup : MonoBehaviour
{

	public static Popup instance;

	[SerializeField] TMPro.TMP_Text label;

	private void Awake()
	{
		instance = this;
	}

	public void DisplayNodeInfo(Node node)
	{
		label.text = $"-{node.cost} Energy";
	}

	public void Hide()
	{
		label.text = "";
	}
}
