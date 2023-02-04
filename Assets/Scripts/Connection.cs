using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Connection : MonoBehaviour
{
	public Node node1;
	public Node node2;
	public string owner = "";
	LineRenderer line;

	public Node GetOtherNode(Node node)
	{
		if (node == node1)
		{
			return node2;
		} else
		{
			return node1;
		}
	}

	private void Start()
	{
		line = GetComponent<LineRenderer>();
		transform.position = 0.5f * (node1.transform.position + node2.transform.position);
		line.SetPosition(0, node1.transform.position);
		line.SetPosition(1, node2.transform.position);
	}
}
