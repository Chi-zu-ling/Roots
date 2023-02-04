using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Node : MonoBehaviour
{
    
    [SerializeField] Gamelogic gamelogic;
    [SerializeField] GameObject connectionPrefab;
    public List<Node> connectedNodes = new();
    public List<Connection> connections = new();
    public bool connectedToRoot = false;
    Vector2 position;
    public int cost;
    float connectionRadius = 6;
    float MaxConnectionRadius = 10;

    public string owner = "";

    public typeEnum type;
    public modifierEnum modifier;

    public CircleCollider2D Connections;

    public enum typeEnum { 
        basic,
        water,
        nutrients
    }

    public enum modifierEnum { 
        basic,
        turn,
        multi
    }

    // Start is called before the first frame update
    void Start()
    {
        //MakeConnections();
    }

	private void OnMouseDown()
	{
        Debug.Log("HELP");
        if (owner != "")
		{
            return;
		}

        var effectiveCost = cost * 2;
        foreach(var neighbour in connectedNodes)
		{
            if(neighbour.owner == "Player")
			{
                effectiveCost = cost;
                break;
			}
		}
        if (gamelogic.energy >= effectiveCost)
		{
            foreach(var connection in connections)
			{
                var neighbour = connection.GetOtherNode(this);
                if (neighbour.owner == "Player")
				{
                    connection.owner = neighbour.owner;
                    owner = neighbour.owner;
                    gamelogic.energy -= effectiveCost;
                    Debug.Log(gamelogic.energy);
                    break;
				}
			}
		}
	}

	public void MakeConnections() {

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

    public void connectTo(Node node) {
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



}
