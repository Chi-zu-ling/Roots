using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Node : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    public List<Node> connectedNodes;
    Vector2 position;

    

    enum type { 
        basic,
        water,
        nutrients
    }

}
