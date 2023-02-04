using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gamelogic : MonoBehaviour
{
    // Start is called before the first frame update

    [SerializeField] public int energy;

    [SerializeField] int maxNodes;
    [SerializeField] public int maxConnections;

    [SerializeField] float playFieldSize;
    [SerializeField] float deadZone;
    [SerializeField] float twoArea;
    [SerializeField] float oneArea;

    [SerializeField] int waterNodes;

    [SerializeField] int turnNodes;
    [SerializeField] int multNodes;

    List<Node> totalNodes = new List<Node>();

    public List<Node> threesList = new();
    public List<Node> twosList = new();
    public List<Node> onesList = new();

    public Node node;

    public void instantiatePlayGround() {

        //change some of the instantiated nodes to "water" type
        //change some "basic" type nodes 


        Node startNode = Instantiate(node, new Vector2(0, 0), node.transform.rotation, this.transform);
        totalNodes.Add(startNode);

        for (int mn = 0; mn < maxNodes-1; mn++) {

            //get random position on playingField
            Vector2 newNodeposition = findNewNodePosition();
            Node newNode = Instantiate(node, newNodeposition, node.transform.rotation, this.transform);
            totalNodes.Add(newNode);
        }

        foreach(Node node in totalNodes)
		{
            node.gamelogic = this;
		}

    }

	private void Awake()
	{
        Debug.Log(energy);
	}


	void Start()
    {
        instantiatePlayGround();
        createCostAreas();
        makeNodeTypes();
        makeNodeModifiers();

        var startNode = totalNodes[0];
        startNode.connectedToRoot = true;
        startNode.owner = "Player";
        var connectedNodes = new List<Node>() { totalNodes[0] };
        for(int i = 0; i < connectedNodes.Count && connectedNodes.Count < totalNodes.Count; i++)
		{
            var node = connectedNodes[i];
            node.MakeConnections();
            foreach(var cNode in node.connectedNodes)
			{
                if (!cNode.connectedToRoot)
				{
                    cNode.connectedToRoot = true;
                    connectedNodes.Add(cNode);
				}
			}
		}
    }


    Vector2 findNewNodePosition() {
        Vector2 newPosition = Random.insideUnitCircle * playFieldSize;

        while (checkOverlap(newPosition))
        {
            newPosition = Random.insideUnitCircle * playFieldSize;
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
                return true;
            }
        }
        return false;
    }

    public void createCostAreas() {
        // 3 cost area
        foreach (Node node in totalNodes)
        {
            node.cost = 3;
        }

        // 2 cost area
        Collider2D[] twos = (Physics2D.OverlapCircleAll(new Vector2(0, 0), (playFieldSize * 0.9f)));
        foreach (var hit in twos)
        {
            Node node = hit.GetComponent<Node>();
            node.cost = 2;
        }

        //one cost area
        Collider2D[] ones = (Physics2D.OverlapCircleAll(new Vector2(0, 0), (playFieldSize * 0.7f)));
        foreach (var hit in ones)
        {
            Node node = hit.GetComponent<Node>();
            node.cost = 1;
        }

        foreach (Node node in totalNodes) {

            if (node.cost == 3) {
                threesList.Add(node);
            }

            else if (node.cost == 2) {
                twosList.Add(node);
            }

            else onesList.Add(node);

        }
    }

    public void makeNodeTypes() {

        for (int w = 0; w < waterNodes; w++)
        {
            int r = Random.Range(0, totalNodes.Count);
            totalNodes[r].type = Node.typeEnum.water;
            totalNodes[r].gameObject.transform.Find("Sprite").GetComponent<SpriteRenderer>().color = new Color32(100,120, 255, 255);
        }

    }

    public void makeNodeModifiers(){
        
        //might not ed up with exatly as many a specified, as they can be overwritten

        for (int t = 0; t < turnNodes; t++) {
            int r = Random.Range(0, 100);

            if (r > 50) {
                Node target = getRandomFromList(threesList);
                target.modifier = Node.modifierEnum.turn;
            }

            else if (r < 15) {
                Node target = getRandomFromList(onesList);
                target.modifier = Node.modifierEnum.turn;
            }

            else {
                Node target = getRandomFromList(twosList);
                target.modifier = Node.modifierEnum.turn;
            }
        }


        for (int m = 0; m < multNodes; m++)
        {
            int r = Random.Range(0, 100);

            if (r > 50)
            {
                Node target = getRandomFromList(threesList);
                target.modifier = Node.modifierEnum.multi;
            }

            else if (r < 15)
            {
                Node target = getRandomFromList(onesList);
                target.modifier = Node.modifierEnum.multi;
            }

            else
            {
                Node target = getRandomFromList(twosList);
                target.modifier = Node.modifierEnum.multi;
            }
        }

    }

    public Node getRandomFromList(List<Node> nodeList) {
        int r = Random.Range(0, nodeList.Count);
        Node returnNode = nodeList[r];
        return returnNode;
    }

}
