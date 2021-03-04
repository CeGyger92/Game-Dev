using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public GameObject gameBoard;
    public GameObject selector;
    public GameObject compSelect;
    public GameObject[,] selectionArray = new GameObject[5, 5];
    int selectorPosX = 4;
    int selectorPosY = 4;
    int compSelectorPos = 0;
    public GameObject straight;
    public GameObject elbow;
    public GameObject goal;
    int[] sCount = new int[4];
    int[] cCount = new int[2];
    public GameObject elbowDirected;
    public GameObject elbowDirectedReverse;
    public GameObject valueNeg;
    GameObject [] compPosition = new GameObject[4];
    GameObject[] prefabs = new GameObject[4];
    public bool loadInit = false;
    public GameObject stdrdCounts;
    public GameObject stdrdInv;
    public GameObject compCounts;
    public GameObject compInv;
    public GameObject[] valueComps = new GameObject[2];
    public GameObject valuedOne;
    public GameObject valuedTwo;
    public int globalCharge = 0;
    public Text actualVal;
    public Text targetVal;
    public Text actual;
    public Text target;
    public GameObject panel;
    public Button okButton;
    public AudioSource rotate;
    public AudioSource higher;
    public AudioSource lower;
    public AudioSource success;
    public AudioSource background;
    public AudioSource place;


    // Start is called before the first frame update
    void Start()
    {
        AudioSource loop = background.GetComponent<AudioSource>();
        loop.loop = true;
        loop.Play();
        for (int i = 0; i < 5; i++)
        {
            for (int j = 0; j < 5; j++)
            {
                selectionArray[i, j] = gameBoard.transform.GetChild(i).GetChild(j).gameObject;
                //Debug.Log(selectionArray[i, j].name);
            }
        }
        compPosition[0] = elbowDirected;
        compPosition[1] = straight;
        compPosition[2] = elbowDirectedReverse;
        compPosition[3] = elbow;
        valueComps[0] = valuedTwo;
        valueComps[1] = valuedOne;
        sCount[0] = 2;
        sCount[1] = 2;
        sCount[2] = 1;
        sCount[3] = 4;
        cCount[0] = 1;
        cCount[1] = 4;
        GameObject parent = selectionArray[2, 3];
        GameObject child = Instantiate(valueNeg, parent.transform.position, Quaternion.identity);
        child.transform.parent = parent.transform;
        child.GetComponent<neighborScript>().xCoord = 3;
        child.GetComponent<neighborScript>().yCoord = 2;
        child.SetActive(true);
        child.GetComponent<neighborScript>().getNeighbors();
        selectionArray[2, 2].transform.GetChild(0).GetComponent<neighborScript>().neighborArray[1, 0] = 1;
        selectionArray[2, 2].transform.GetChild(0).GetComponent<neighborScript>().getNeighbors();
        loadInit = true;
    }

    public bool isConnected(GameObject end)
    {
        bool result = false;
        Queue<GameObject> neighbors = new Queue<GameObject>();
        neighbors.Enqueue(end);

        while(neighbors.Count > 0)
        {
            GameObject current = neighbors.Dequeue();
            current.GetComponent<neighborScript>().visited = true;
            for (int i = 0; i < 4; i++)
            {
                int x = current.GetComponent<neighborScript>().neighborArray[i, 1];
                int y = current.GetComponent<neighborScript>().neighborArray[i, 2];
                //Debug.Log("Make It" + y + x +" " + current.GetComponent<neighborScript>().neighborArray[i, 0] + " "+ selectionArray[y, x].transform.childCount);
                //is it a valid neighbor
                if (current.GetComponent<neighborScript>().neighborArray[i, 0] == 1 && x >= 0 &&
                    x <= 4 && y >= 0 &&
                    y <= 4 && selectionArray[y, x].transform.childCount > 0)
                {
                    //Debug.Log("Thru");
                    if (x == 2 && y == 4)
                    {
                        clearVisited();
                        return true;
                    }
                    if(!selectionArray[y, x].transform.GetChild(0).gameObject.GetComponent<neighborScript>().visited)
                    neighbors.Enqueue(selectionArray[y, x].transform.GetChild(0).gameObject);
                }
            }
        }
        clearVisited();
        return result;
    }

    public bool hasConnection(GameObject n, int x, int y)
    {
        for(int i = 0; i < 4; i++)
        {
            int nX = n.GetComponent<neighborScript>().neighborArray[i, 1];
            int nY = n.GetComponent<neighborScript>().neighborArray[i, 2];
            if (n.GetComponent<neighborScript>().neighborArray[i,0] == 1)
            {
                if(nX == x && nY == y)
                {
                    return true;
                }
            }
        }
        return false;
    }

    void clearVisited()
    {
        for (int i = 0; i < 5; i++)
        {
            for (int j = 0; j < 5; j++)
            {
                if (selectionArray[i, j].transform.childCount > 0 && selectionArray[i, j].transform.tag != ("OffLimits"))
                {
                    //Debug.Log("Error Location" + i + " " + j);
                    selectionArray[i, j].transform.GetChild(0).gameObject.GetComponent<neighborScript>().visited = false;
                }
            }
        }
    }

    public void updateConnected()
    {
        Color limeish = new Color(0.4791972f, 0.8679245f, 0.4053043f);
        int startingCharge = globalCharge;
        globalCharge = 0;
        for (int i = 0; i < 5; i++)
        {
            for(int j = 0; j < 5; j++)
            {
                //Debug.Log("Error Location" + i + " " + j);
                if (selectionArray[i, j].tag != "OffLimits")
                {
                    if (selectionArray[i, j].transform.childCount > 0 && isConnected(selectionArray[i,j].transform.GetChild(0).gameObject))
                    {
                        if(selectionArray[i, j].transform.GetChild(0).gameObject.tag == "Valued")
                        {
                            if (selectionArray[i, j].transform.GetChild(0).gameObject.name == ("valuedTwo(Clone)"))
                            {
                                globalCharge += 2;
                            }
                            else if (selectionArray[i, j].transform.GetChild(0).gameObject.name == ("valuedOne(Clone)"))
                            {
                                globalCharge += 1;
                            }else if (selectionArray[i, j].transform.GetChild(0).gameObject.name == ("valuedNeg(Clone)"))
                            {
                                Debug.Log("here");
                                globalCharge -= 1;
                            }
                        }
                        selectionArray[i, j].transform.GetChild(0).gameObject.GetComponent<Renderer>().material.SetColor("_Color", new Color(0.4791972f, 0.8679245f, 0.4053043f));
                    }else if (selectionArray[i, j].transform.childCount > 0 && (!isConnected(selectionArray[i, j].transform.GetChild(0).gameObject)))
                    {
                        selectionArray[i, j].transform.GetChild(0).gameObject.GetComponent<Renderer>().material.SetColor("_Color", Color.white);
                    }
                }
            }
        }
        actualVal.text = globalCharge.ToString();
        if(globalCharge > startingCharge)
        {
            higher.GetComponent<AudioSource>().Play();
        }
        else if(globalCharge < startingCharge)
        {
            lower.GetComponent<AudioSource>().Play();
        }
        if(globalCharge == 4)
        {
            actualVal.GetComponent<Text>().color = new Color(0.4791972f, 0.8679245f, 0.4053043f);
            targetVal.GetComponent<Text>().color = new Color(0.4791972f, 0.8679245f, 0.4053043f);
            actual.GetComponent<Text>().color = new Color(0.4791972f, 0.8679245f, 0.4053043f);
            target.GetComponent<Text>().color = new Color(0.4791972f, 0.8679245f, 0.4053043f);
        }
        else
        {
            actualVal.GetComponent<Text>().color = new Color(0.9433962f, 0.6917431f, 0.2358491f);
            targetVal.GetComponent<Text>().color = new Color(0.9433962f, 0.6917431f, 0.2358491f);
            actual.GetComponent<Text>().color = new Color(0.9433962f, 0.6917431f, 0.2358491f);
            target.GetComponent<Text>().color = new Color(0.9433962f, 0.6917431f, 0.2358491f);
        }
        if (isConnected(selectionArray[2, 2].transform.GetChild(0).gameObject) && int.Parse(actualVal.text) == 4)
        {
            StartCoroutine(GameOver());
        }
    }

    IEnumerator GameOver()
    {
        okButton.gameObject.SetActive(false);
        goal.GetComponent<Renderer>().material.SetColor("_Color", Color.red);
        selector.SetActive(false);
        success.GetComponent<AudioSource>().Play();
        yield return new WaitForSeconds(3.0f);
        Text[] panelText = panel.GetComponentsInChildren<Text>();
        panelText[1].text = "Success!\n\n\nYour company computers are safe now and Bezos won't know what hit him!";
        panel.SetActive(true);
        Time.timeScale = 0;
    }

    // Update is called once per frame
    void Update()
    {
        //
        if (Input.GetKeyDown("a"))
        {
            if (selectorPosX > 0 && selector.activeSelf)
            {
                selectorPosX -= 1;
                selector.transform.position = selectionArray[selectorPosY, selectorPosX].transform.position;
            } 
        }
        if (Input.GetKeyDown("d"))
        {
            if (selectorPosX < 4 && selector.activeSelf)
            {
                selectorPosX += 1;
                selector.transform.position = selectionArray[selectorPosY, selectorPosX].transform.position;
            }
        }
        if (Input.GetKeyDown("s"))
        {
            if (selectorPosY < 4 && selector.activeSelf)
            {
                selectorPosY += 1;
                selector.transform.position = selectionArray[selectorPosY, selectorPosX].transform.position;
            }
            else if (compSelectorPos < 3 && stdrdInv.activeSelf)
            {
                compSelectorPos += 1;
                compSelect.transform.position = compPosition[compSelectorPos].transform.position;
            }else if (compSelectorPos < 1 && compInv.activeSelf)
            {
                compSelectorPos += 1;
                compSelect.transform.position = valueComps[compSelectorPos].transform.position;
            }
        }
        if (Input.GetKeyDown("w"))
        {
            if (selectorPosY > 0 && selector.activeSelf)
            {
                selectorPosY -= 1;
                selector.transform.position = selectionArray[selectorPosY, selectorPosX].transform.position;
            }
            else if (compSelectorPos > 0)
            {
                compSelectorPos -= 1;
                compSelect.transform.position = compPosition[compSelectorPos].transform.position;
            }
        }


        if (Input.GetKeyDown("e"))
        {
            if (selector.activeSelf && selectionArray[selectorPosY, selectorPosX].tag == "Untagged")
            {
                selector.SetActive(false);
                compSelect.SetActive(true);
                stdrdCounts.SetActive(true);
                stdrdInv.SetActive(true);
                compCounts.SetActive(false);
                compInv.SetActive(false);
                compSelectorPos = 0;
                compSelect.transform.position = compPosition[compSelectorPos].transform.position;
            }
            else if (selector.activeSelf && selectionArray[selectorPosY, selectorPosX].tag == "Charge")
            {
                selector.SetActive(false);
                compSelect.SetActive(true);
                stdrdCounts.SetActive(false);
                stdrdInv.SetActive(false);
                compCounts.SetActive(true);
                compInv.SetActive(true);
                compSelectorPos = 0;
                compSelect.transform.position = valueComps[compSelectorPos].transform.position;
            }
            else if (compSelect.activeSelf && stdrdInv.activeSelf)
            {
                place.GetComponent<AudioSource>().Play();
                GameObject parent = selectionArray[selectorPosY, selectorPosX];
                if (parent.transform.childCount == 0 && sCount[compSelectorPos] > 0)
                {
                    GameObject child = Instantiate(compPosition[compSelectorPos], selectionArray[selectorPosY, selectorPosX].transform.position, Quaternion.identity);
                    child.transform.parent = parent.transform;
                    child.GetComponent<neighborScript>().xCoord = selectorPosX;
                    child.GetComponent<neighborScript>().yCoord = selectorPosY;
                    sCount[compSelectorPos]--;
                    Image[] skids = stdrdCounts.GetComponentsInChildren<Image>();
                    skids[compSelectorPos].transform.GetChild(0).GetComponent<Text>().text = sCount[compSelectorPos].ToString();
                }
                selector.SetActive(true);
                compSelect.SetActive(false);
            }else if (compSelect.activeSelf && compInv.activeSelf)
            {
                GameObject parent = selectionArray[selectorPosY, selectorPosX];
                if (parent.transform.childCount == 0 && cCount[compSelectorPos] > 0)
                {
                    GameObject child = Instantiate(valueComps[compSelectorPos], selectionArray[selectorPosY, selectorPosX].transform.position, Quaternion.identity);
                    child.transform.parent = parent.transform;
                    child.GetComponent<neighborScript>().xCoord = selectorPosX;
                    child.GetComponent<neighborScript>().yCoord = selectorPosY;
                    cCount[compSelectorPos]--;
                    Image[] ckids = compCounts.GetComponentsInChildren<Image>();
                    ckids[compSelectorPos].transform.GetChild(0).GetComponent<Text>().text = cCount[compSelectorPos].ToString();
                }
                selector.SetActive(true);
                compSelect.SetActive(false);
            }
        }
        if (Input.GetKeyDown("q"))
        {
            //Debug.Log(selectionArray[selectorPosY, selectorPosX].transform.childCount);
            if (selector.activeSelf && (selectionArray[selectorPosY, selectorPosX].transform.childCount == 1))
            {
                //Queue<GameObject> nOfDeleted = new Queue<GameObject>();
                //GameObject current = selectionArray[selectorPosY, selectorPosX].transform.GetChild(0).gameObject;
                //for (int i = 0; i < 4; i++)
                //{
                //    int x = current.GetComponent<neighborScript>().neighborArray[i, 1];
                //    int y = current.GetComponent<neighborScript>().neighborArray[i, 2];
                //    //Debug.Log("Make It" + y + x +" " + current.GetComponent<neighborScript>().neighborArray[i, 0] + " "+ selectionArray[y, x].transform.childCount);
                //    //is it a valid neighbor
                //    if (current.GetComponent<neighborScript>().neighborArray[i, 0] == 1 && x >= 0 &&
                //        x <= 4 && y >= 0 &&
                //        y <= 4 && selectionArray[y, x].transform.childCount > 0)
                //    {
                //            nOfDeleted.Enqueue(selectionArray[y, x].transform.GetChild(0).gameObject);
                //    }
                //}
                GameObject selected = selectionArray[selectorPosY, selectorPosX].transform.GetChild(0).gameObject;
                Image[] skids = stdrdCounts.GetComponentsInChildren<Image>();
                Image[] ckids = compCounts.GetComponentsInChildren<Image>();
                if (selected.tag == ("Elbow"))
                {
                    sCount[3]++;
                    skids[3].transform.GetChild(0).GetComponent<Text>().text = sCount[3].ToString();
                }
                else if(selected.tag == ("ElbowD"))
                {
                    sCount[0]++;
                    skids[0].transform.GetChild(0).GetComponent<Text>().text = sCount[0].ToString();
                }
                else if(selected.tag == ("ElbowDR"))
                {
                    sCount[2]++;
                    skids[2].transform.GetChild(0).GetComponent<Text>().text = sCount[2].ToString();
                }
                else if(selected.tag == ("Straight"))
                {
                    sCount[1]++;
                    skids[1].transform.GetChild(0).GetComponent<Text>().text = sCount[1].ToString();
                }
                else if(selected.tag == ("Valued"))
                {
                    if(selected.name == ("valuedTwo(Clone)"))
                    {
                        cCount[0]++;
                        ckids[0].transform.GetChild(0).GetComponent<Text>().text = cCount[0].ToString();
                    }
                    else if(selected.name == ("valuedOne(Clone)"))
                    {
                        cCount[1]++;
                        ckids[1].transform.GetChild(0).GetComponent<Text>().text = cCount[1].ToString();
                    }
                }
                Destroy(selectionArray[selectorPosY, selectorPosX].transform.GetChild(0).gameObject);

                updateConnected();
            }
        }
        if (Input.GetKeyDown("r"))
        {
            //Debug.Log(selectionArray[selectorPosY, selectorPosX].transform.childCount);
            if (selector.activeSelf && (selectionArray[selectorPosY, selectorPosX].transform.childCount == 1))
            {
                rotate.GetComponent<AudioSource>().Play();
                selectionArray[selectorPosY, selectorPosX].transform.GetChild(0).Rotate(0,0,-90);
                selectionArray[selectorPosY, selectorPosX].transform.GetChild(0).GetComponent<neighborScript>().rotate();
            }
        }
        
    }
}
