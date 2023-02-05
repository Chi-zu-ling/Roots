using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Gamelogic : MonoBehaviour
{
    // Start is called before the first frame update

    [SerializeField] public int energy;
    [SerializeField] public float water;
    [SerializeField] public int maxWater;

    [SerializeField] public int score = 0;

    [SerializeField] int maxNodes;
    [SerializeField] public int maxConnections;

    [SerializeField] float playFieldSize;
    [SerializeField] float deadZone;
    [SerializeField] float twoArea;
    [SerializeField] float oneArea;

    [SerializeField] int waterNodes;

    [SerializeField] int nutriNodes;

    List<Node> totalNodes = new List<Node>();

    public List<Node> threesList = new();
    public List<Node> twosList = new();
    public List<Node> onesList = new();

    public Node node;

    [SerializeField] public TMP_Text energyText;
    [SerializeField] public TMP_Text scoreText;
    [SerializeField] public Image waterLevelUI;
    [SerializeField] public TMP_Text waterLabel;

    [SerializeField] CameraController camCon;

    [SerializeField] public GameObject GameOverPanel;
    [SerializeField] public TMP_Text GOscoreText;
    [SerializeField] public TMP_Text GODescriptionText;

    public void instantiatePlayGround() {

        //change some of the instantiated nodes to "water" type
        //change some "basic" type nodes 


        Node startNode = Instantiate(node, new Vector2(0, 0), node.transform.rotation, this.transform);
        totalNodes.Add(startNode);

        for (int mn = 0; mn < maxNodes - 1; mn++)
        {

            //get random position on playingField
            Vector2 newNodeposition = findNewNodePosition();
            Node newNode = Instantiate(node, newNodeposition, node.transform.rotation, this.transform);
            totalNodes.Add(newNode);
        }

        foreach (Node node in totalNodes)
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
        makeNodeModifiers();

        var startNode = totalNodes[0];
        startNode.connectedToRoot = true;
        startNode.owner = "Player";
        var connectedNodes = new List<Node>() { startNode};
        for(int i = 0; i < connectedNodes.Count && connectedNodes.Count < totalNodes.Count; i++)
		{
            var node = connectedNodes[i];
            node.MakeConnections();
            foreach (var cNode in node.connectedNodes)
            {
                if (!cNode.connectedToRoot)
                {
                    cNode.connectedToRoot = true;
                    connectedNodes.Add(cNode);
				}
			}
		}

        UpdateUI();

        foreach(var connection in startNode.connections)
		{
            var node = connection.GetOtherNode(startNode);
            connection.owner = "Player";
            node.owner = "Player";
            connection.GrowRoot(startNode);
		}
    }



    Vector2 findNewNodePosition()
    {
        Vector2 newPosition = Random.insideUnitCircle * playFieldSize;

        checkOverlap(newPosition);

        while (checkOverlap(newPosition))
        {
            newPosition = Random.insideUnitCircle * playFieldSize;
        }

        return newPosition;
    }


    public bool checkOverlap(Vector2 newPosition)
    {

        //check position for any other nodes
        Collider2D[] hits = (Physics2D.OverlapCircleAll(newPosition, deadZone));
        for (int i = 0; i < hits.Length; i++)
        {
            if (hits[i].GetComponent<Node>() != null)
            {
                return true;
            }
        }
        return false;

    }


    public void createCostAreas()
    {
        // 3 cost area
        foreach (Node node in totalNodes)
        {
            node.cost = 3;
        }

        // 2 cost area
        Collider2D[] twos = (Physics2D.OverlapCircleAll(new Vector2(0, 0), (playFieldSize * 0.6f)));
        foreach (var hit in twos)
        {
            Node node = hit.GetComponent<Node>();
            node.cost = 2;
        }

        //one cost area
        Collider2D[] ones = (Physics2D.OverlapCircleAll(new Vector2(0, 0), (playFieldSize * 0.3f)));
        foreach (var hit in ones)
        {
            Node node = hit.GetComponent<Node>();
            node.cost = 1;
        }

        foreach (Node node in totalNodes)
        {

            if (node.cost == 3)
            {
                threesList.Add(node);
            }

            else if (node.cost == 2)
            {
                twosList.Add(node);
            }

            else onesList.Add(node);

        }
    }


    public void makeNodeModifiers()
    {

        //might not ed up with exatly as many a specified, as they can be overwritten

        for (int w = 0; w < waterNodes; w++)
        {
            int r = Random.Range(0, totalNodes.Count);
            totalNodes[r].modifier = Node.modifierEnum.water;
            totalNodes[r].SetType(Node.modifierEnum.water);
        }


        for (int m = 0; m < nutriNodes; m++)
        {
            int r = Random.Range(0, 100);

            if (r > 50){
                Node target = getRandomFromList(threesList);
                target.modifier = Node.modifierEnum.nutri;
                totalNodes[r].SetType(Node.modifierEnum.nutri);
            }

            else if (r < 15){
                Node target = getRandomFromList(onesList);
                target.modifier = Node.modifierEnum.nutri;
                totalNodes[r].SetType(Node.modifierEnum.nutri);
            }

            else{
                Node target = getRandomFromList(twosList);
                target.modifier = Node.modifierEnum.nutri;
                totalNodes[r].SetType(Node.modifierEnum.nutri);
            }
        }

    }


    public Node getRandomFromList(List<Node> nodeList)
    {
        int r = Random.Range(0, nodeList.Count);
        Node returnNode = nodeList[r];
        return returnNode;
    }

    public void UpdateUI()
    {

        energyText.text = energy.ToString();
        scoreText.text = $"Score: {score}";
        waterLevelUI.fillAmount = water/10;
        waterLabel.text = water.ToString();
    }


    public void doNodeModifiers(Node node)
    {

        Node.modifierEnum modifier = node.modifier;

        switch (modifier)
        {

            case (Node.modifierEnum.basic):
                break;


            case (Node.modifierEnum.nutri):
                energy += 5;
                Debug.Log("+ 5");
                UpdateUI();
                break;


            case (Node.modifierEnum.water):
                adjustWater(3);
                break;
        }
    }

    public void adjustWater(int amount) {

        water += amount;

        if (water > maxWater){
            water = maxWater;}

        else if (water <= 0){
            water = 0;
            GODescriptionText.text = "You wilted from lack of Water";
            GameoVer();
        }

        //Debug.Log("W: " + water);

        UpdateUI();

    }

    public void GameoVer() {

        camCon.OnRecenterCamera();
        GOscoreText.text = $"Score: {score}";
        UpdateUI();
        GameOverPanel.SetActive(true);

    }
}
