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

        for (int mn = 0; mn < maxNodes; mn++) {


            //get random position on playingField
            Vector2 newNodeposition = findNewNodePosition();


            Node newNode = Instantiate(node, newNodeposition, node.transform.rotation, this.transform);
            totalNodes.Add(newNode);

        }

    }

    void Start()
    {
        maxNodes = 500;
        turns = 250;
        instantiatePlayGround();


        // Collider2D[] hits = (Physics2D.OverlapCircleAll(newPosition, deadZone));
    }


    Vector2 findNewNodePosition() {
        Vector2 newPosition = Random.insideUnitCircle * 100;
        while (checkOverlap(newPosition))
        {
            newPosition = Random.insideUnitCircle * 100;
        }
        return newPosition;
    }


    public bool checkOverlap(Vector2 newPosition) {

        //check position for any other nodes
        Collider2D[] hits = (Physics2D.OverlapCircleAll(newPosition, deadZone));
        for (int i = 0; i < hits.Length; i++)
        {
            if (hits[i].name == "Sprite")
            {
                Debug.Log(hits[i].name);
                return true;
            }
        }
        return false;
    }

}
