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
		var cost = node.cost;
		if (node.connectionStatus == Node.ConnectionStatus.Bridge)
		{
			cost *= 2;
		}
		if (node.modifier == Node.modifierEnum.nutri)
		{
			cost -= 3;
		}
		label.text = $"{-cost} Energy";
	}

	public void Hide()
	{
		label.text = "";
	}
}
