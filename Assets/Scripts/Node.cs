using System.Collections.Generic;
using UnityEngine;

public class Node : MonoBehaviour
{

	public Gamelogic gamelogic;
	[SerializeField] GameObject connectionPrefab;
	[SerializeField] SpriteRenderer sprite;
	[SerializeField] SpriteRenderer highlight;
	[SerializeField] Sprite waterSprite;
	[SerializeField] Sprite NutrientSprite;
	public List<Node> connectedNodes = new();
	public List<Connection> connections = new();
	public bool connectedToRoot = false;
	public int cost;
	float connectionRadius = 6;
	float MaxConnectionRadius = 10;

	public string owner = "";


	public modifierEnum modifier;

	public enum modifierEnum
	{
		basic,
		nutri,
		water
	}

	enum ConnectionStatus
	{
		Direct,
		Bridge,
		None,
	}

	ConnectionStatus connectionStatus = ConnectionStatus.None;
	Node nearestConnectableNode;

	// Start is called before the first frame update
	void Start()
	{
		//MakeConnections();
	}

	private void OnMouseDown()
	{

		//Double check we don't own the node
		if (owner == "Player")
		{
			return;
		}

		// double price if bridging
		var effectiveCost = cost;
		if (connectionStatus == ConnectionStatus.None)
		{
			return;
		} else if (connectionStatus == ConnectionStatus.Bridge)
		{
			effectiveCost *= 2;
		}

		// check for sufficient funds
		if (gamelogic.energy < effectiveCost)
		{
			gamelogic.energy = 0;
			gamelogic.GODescriptionUI.text = "You have no more energy to grow your Roots";
			gamelogic.UpdateUI();
			gamelogic.GameOver();
			//game over
			return;
		}

		gamelogic.adjustWater(-1);

		gamelogic.doNodeModifiers(this);
		gamelogic.energy -= effectiveCost;
		gamelogic.UpdateUI();

		// Connected to node
		owner = "Player";

		Connection connection = null;

		if (connectedNodes.Contains(nearestConnectableNode))
		{
			foreach(var c in connections)
			{
				if (c.GetOtherNode(this) == nearestConnectableNode)
				{
					connection = c;
					break;
				}
			}
		} else
		{
			connection = connectTo(nearestConnectableNode);
			connection.gameObject.name = "Fresh Connection";
			connection.Start();
		}
		connectionStatus = ConnectionStatus.None;
		Highlight(Color.blue);
		nearestConnectableNode.DisableHighlight();
		connection.GrowRoot(nearestConnectableNode);
	}

	private void OnMouseEnter()
	{
		var bridgableNodes = GetNodesInRadius(MaxConnectionRadius);
		var canConnect = false;

		var minDist = float.MaxValue;

		// Highlight all nodes in bridging radius
		foreach(Node node in bridgableNodes)
		{
			if (node.owner == "Player")
			{
				canConnect = true;
				var distToNode = Vector3.Distance(node.transform.position, transform.position);
				if (distToNode < minDist)
				{
					minDist = distToNode;
					nearestConnectableNode = node;
				}
			}
		}
		

		// Highlight the node 
		// Blue if already owned
		// Green if can connect directly
		// Yellow if can connect only through bridging
		// Red if not at all
		if (owner == "Player")
		{
			connectionStatus = ConnectionStatus.None;
			Highlight(Color.blue);
		}
		else if (canConnect)
		{
			var mustBridge = true;
			foreach (Node node in connectedNodes)
			{
				if (node.owner == "Player")
				{
					nearestConnectableNode = node;
					mustBridge = false;
					break;
				}
			}
			nearestConnectableNode.Highlight(new(0.2f, 0.3f, 1.0f));
			if (mustBridge)
			{
				connectionStatus = ConnectionStatus.Bridge;
				Highlight(Color.yellow);
			} else
			{
				connectionStatus = ConnectionStatus.Direct;
				Highlight(Color.green);
			}
		} else
		{
			connectionStatus = ConnectionStatus.None;
			Highlight(Color.red);
		}
		Popup.instance.gameObject.SetActive(true);
		Popup.instance.DisplayNodeInfo(this);
		//Popup.instance.transform.position = Camera.main.WorldToScreenPoint(transform.position);

	}

	private void OnMouseExit()
	{
		var nearbyNodes = GetNodesInRadius(MaxConnectionRadius);
		foreach(Node node in nearbyNodes)
		{
			node.DisableHighlight();
		}
		Popup.instance.gameObject.SetActive(false);
	}

	public void MakeConnections()
	{

		int newConnections = 0;
		while (newConnections == 0 && connectionRadius < MaxConnectionRadius)
		{

			Collider2D[] hits = (Physics2D.OverlapCircleAll(this.transform.position, connectionRadius));
			foreach (var hit in hits)
			{
				Node node = hit.GetComponent<Node>();
				if (node != null && node != this && !connectedNodes.Contains(node) && !node.connectedToRoot)
				{
					connectTo(node);
					newConnections += 1;
				}
			}
			connectionRadius += 1;
		}
		//check for a radius around node
		//get all nodes available ("possibleConnections")
		//get random value between 1 and "maxConnections"
		//get random nodes from "possibleConnections"
		//make connection indicators?
	}

	public Connection connectTo(Node node)
	{
		var connectionGo = Instantiate(connectionPrefab);
		var connection = connectionGo.GetComponent<Connection>();
		connection.node1 = this;
		connection.node2 = node;
		connections.Add(connection);
		node.connections.Add(connection);
		connectedNodes.Add(node);
		node.connectedNodes.Add(this);
		return connection;
	}

	public void SetType(modifierEnum m)
	{
		modifier = m;
		switch (modifier)
		{
			case modifierEnum.water:
				sprite.sprite = waterSprite;
				break;

			case modifierEnum.nutri:
				Debug.Log("setting Nutri sprite");
				sprite.sprite = NutrientSprite;
				break;

		}
	}

	public void DisableHighlight()
	{
		highlight.enabled = false;
	}

	public void Highlight(Color color)
	{
		highlight.enabled = true;
		highlight.color = color;
	}

	public List<Node> GetNodesInRadius(float radius)
	{
		Collider2D[] hits = (Physics2D.OverlapCircleAll(this.transform.position, radius));
		List<Node> nodes = new();
		foreach(var hit in hits)
		{
			var node = hit.GetComponent<Node>();
			if (node != null)
			{
				nodes.Add(node);
			}
		}
		return nodes;
	}
}
