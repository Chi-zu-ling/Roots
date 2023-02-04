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
    Vector2 position;
    public int cost;
    float connectionRadius = 6;
    float maxConnections = 10;

    public CircleCollider2D Connections;

    enum type { 
        basic,
        water,
        nutrients
    }

    enum modifier { 
        basic,
        turn,
        multi
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

	private void OnMouseDown()
	{
        
	}

	public void checkForConnections() {

        Collider2D[] hits = (Physics2D.OverlapCircleAll(this.transform.position, connectionRadius));
        foreach(var hit in hits)
		{
            Node node = hit.GetComponent<Node>();
            if (node != null || connectedNodes.Contains(node))
			{
                connectTo(node);
			}
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
        //is called when pressed on, deducts cost from gamelogic.turns
    }



}
