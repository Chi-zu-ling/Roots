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
		if (node.owner == "Player")
		{
			Hide();
			return;
		}
		var cost = node.cost;
		var waterCost = 1;
		if (node.connectionStatus == Node.ConnectionStatus.Bridge)
		{
			cost *= 2;
		}
		if (node.modifier == Node.modifierEnum.nutri)
		{
			cost -= 5;
		}
		if (node.modifier == Node.modifierEnum.water)
		{
			waterCost -= 3;
		}
		label.text = $"{-cost} Energy\n{-waterCost} Water";
	}

	public void Hide()
	{
		label.text = "";
	}
}
