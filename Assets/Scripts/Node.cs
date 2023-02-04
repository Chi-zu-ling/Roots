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

	public typeEnum type;
	public modifierEnum modifier;

	public enum typeEnum
	{
		basic,
		water,
		nutrients
	}

	public enum modifierEnum
	{
		basic,
		turn,
		multi
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
		if (owner != "")
		{
			return;
		}

		var effectiveCost = cost * 2;
		foreach (var neighbour in connectedNodes)
		{
			if (neighbour.owner == "Player")
			{
				effectiveCost = cost;
				break;
			}
		}
		if (gamelogic.energy >= effectiveCost)
		{
			foreach (var connection in connections)
			{
				var neighbour = connection.GetOtherNode(this);
				if (neighbour.owner == "Player")
				{
					connection.owner = neighbour.owner;
					connection.GrowRoot(neighbour);
					owner = neighbour.owner;
					gamelogic.energy -= effectiveCost;
					Debug.Log(gamelogic.energy);
					break;
				}
			}
		}
	}

	private void OnMouseEnter()
	{
		Highlight(Color.green);
		var bridgableNodes = GetNodesInRadius(MaxConnectionRadius);
		var canConnect = false;
		
		// Highlight all nodes in bridging radius
		foreach(Node node in bridgableNodes)
		{
			node.Highlight(new(0.2f, 0.3f, 1.0f));
			if (node.owner == "Player")
			{
				canConnect = true;
			}
		}

		// Highlight the node green if can connect directly
		// Yellow if can connect only through bridging
		// Red if not at all
		if (canConnect)
		{
			var mustBridge = false;
			foreach (Node node in connectedNodes)
			{
				if (node.owner == "Player")
				{
					mustBridge = false;
				}
			}
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
		Popup.instance.transform.position = Camera.main.WorldToScreenPoint(transform.position);

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

	public void connectTo(Node node)
	{
		var connectionGo = Instantiate(connectionPrefab);
		var connection = connectionGo.GetComponent<Connection>();
		connection.node1 = this;
		connection.node2 = node;
		connections.Add(connection);
		node.connections.Add(connection);
		connectedNodes.Add(node);
		node.connectedNodes.Add(this);
		//is called when pressed on, deducts cost from gamelogic.turns
	}

	public void SetType(typeEnum t)
	{
		type = t;
		switch (type)
		{
			case typeEnum.water:
				sprite.sprite = waterSprite;
				break;
			case typeEnum.nutrients:
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
