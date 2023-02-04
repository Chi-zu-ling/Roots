using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Node : MonoBehaviour
{

    [SerializeField] Gamelogic gamelogic;
    public List<Node> connectedNodes;
    Vector2 position;
    int cost;

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

    public void checkForConnections() {

        Collider2D[] hits = (Physics2D.OverlapCircleAll(this.transform.position, 6)); //6 being connectionsRadius
        //check for a radius around node
        //get all nodes available ("possibleConnections")
        //get random value between 1 and "maxConnections"
        //get random nodes from "possibleConnections"
        //make connection indicators?

    }

    public void connectTo() {
        //is called when pressed on, deducts cost from gamelogic.turns
    }



}
