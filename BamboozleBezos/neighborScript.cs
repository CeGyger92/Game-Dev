using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class neighborScript : MonoBehaviour
{
    public GameManager gm;
    public bool visited = false;
    public int[,] neighborArray = new int[4, 3];
    public int xCoord;
    public int yCoord;
    // Start is called before the first frame update
    void Start()
    {
        //Debug.Log("isCalled");
        if (this.gameObject.tag == ("Elbow"))
        {
            neighborArray[0, 0] = 1;
            neighborArray[3, 0] = 1;
        }else if (this.gameObject.tag == ("ElbowD"))
        {
            neighborArray[0, 0] = 1;
        }
        else if (this.gameObject.tag == ("ElbowDR"))
        {
            neighborArray[3, 0] = 1;
        }
        else if(this.gameObject.tag == ("Straight") || this.gameObject.tag ==("Valued"))
        {
            neighborArray[1, 0] = 1;
            neighborArray[3, 0] = 1;
        }
        if (gm.GetComponent<GameManager>().loadInit)
        {
            getNeighbors();
        }
    }

    public void getNeighbors()
    {
        //Debug.Log("Was called");
        if (neighborArray[0,0] == 1)
        {
            neighborArray[0, 1] = xCoord;
            neighborArray[0, 2] = yCoord - 1;
        }
        if (neighborArray[1, 0] == 1)
        {
            neighborArray[1, 1] = xCoord + 1;
            neighborArray[1, 2] = yCoord;
        }
        if (neighborArray[2, 0] == 1)
        {
            neighborArray[2, 1] = xCoord;
            neighborArray[2, 2] = yCoord + 1;
        }
        if (neighborArray[3, 0] == 1)
        {
            neighborArray[3, 1] = xCoord-1;
            neighborArray[3, 2] = yCoord;
        }

        //printNeighbors
        //for(int i = 0; i < 4; i++)
        //{
        //    if(neighborArray[i, 0] == 1)
        //    {
        //        Debug.Log(neighborArray[i, 1] + " , " + neighborArray[i, 2] + "\n");
        //    }
        //}
        gm.GetComponent<GameManager>().updateConnected();
    }

    public void rotate()
    {
        int temp = neighborArray [3,0];
        for(int i = 3; i > 0; i--)
        {
            neighborArray[i, 0] = neighborArray[i - 1, 0];
        }
        neighborArray[0, 0] = temp;

        getNeighbors();
        
    }


    // Update is called once per frame
    void Update()
    {
        
    }
}
