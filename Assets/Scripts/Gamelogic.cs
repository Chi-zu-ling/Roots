using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gamelogic : MonoBehaviour
{
    // Start is called before the first frame update

    [SerializeField] int turns;

    [SerializeField] int maxNodes;
    [SerializeField] int maxConnections;
    [SerializeField] float deadZone;
    
    [SerializeField] int waterNodes;

    [SerializeField] int turnNodes;
    [SerializeField] int multNodes;

    List<Node> totalNodes;

    public Node node;

    public void instantiatePlayGround() {

        //get maxNodes
        //get a radius and get 500 random positions within the circle
        //instantiate 500 nodes
        //get the areas and check if any nodes overlap
        //adjust Cost depending on area
        //change some of the instantiated nodes to "water" type
        //change some "basic" type nodes 

    }

    void Start()
    {
        turns = 250;
        
    }

}
