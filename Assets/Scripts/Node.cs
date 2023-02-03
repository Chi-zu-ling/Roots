using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Node : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    [SerializeField] Gamelogic gamelogic;
    public List<Node> connectedNodes;
    Vector2 position;
    int cost;
    

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


    public void checkForConnections() { 
        
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
