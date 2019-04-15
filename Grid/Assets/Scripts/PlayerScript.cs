using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerScript : MonoBehaviour
{
    public float speed;

    Transform player;
    float playerX;
    float playerY;
    //Vector2 lastPosition;
    public Vector2 PlayerPos;

    //numbers
    
    //public Text movesText;
    public float moves;
    
    

    // Start is called before the first frame update
    void Start()
    {
        player = GetComponent<Transform>();
        PlayerPos = new Vector2(GridMaker.Width / 2, GridMaker.Height / 2);
        moves = 6;
    }

    // Update is called once per frame
    void Update()
    {
        //moving with keys
        if (Input.GetKeyDown("d") || (Input.GetKeyDown(KeyCode.RightArrow)))
        {
            if (PlayerPos.x - 1 >= 0)
            {
                movePlayer(-1, 0);
                

            }
        }

        if (Input.GetKeyDown("a") || (Input.GetKeyDown(KeyCode.LeftArrow)))
        {

            if (PlayerPos.x + 1 <= GridMaker.Width - 1)
            {
                movePlayer(1, 0);
                
            }
        }
       
        if (Input.GetKeyDown("w") || (Input.GetKeyDown(KeyCode.UpArrow)))
        {
    
            if (PlayerPos.y - 1 >= 0)
            {
                movePlayer(0, -1);
                Debug.Log("movedup");
                
            }
        }

        if (Input.GetKeyDown("s") || (Input.GetKeyDown(KeyCode.DownArrow)))
        {
            if (PlayerPos.y + 1 <= GridMaker.Height - 1)
            {
                movePlayer(0, 1);
                
                
            }
            
        }
       
    }
    void movePlayer(int x, int y)
    {
        Vector2 oldLoc = new Vector2(PlayerPos.x, PlayerPos.y);
        Vector2 newLoc = new Vector2(PlayerPos.x + x, PlayerPos.y + y);

        GameObject swapTile = GridMaker.tiles[(int)newLoc.x, (int)newLoc.y];

        Vector3 swapPos = swapTile.transform.localPosition;

        swapTile.transform.localPosition = transform.localPosition;

        transform.localPosition = swapPos;

        GridMaker.tiles[(int)oldLoc.x, (int)oldLoc.y] = swapTile;

        GridMaker.tiles[(int)newLoc.x, (int)newLoc.y] = gameObject;

        PlayerPos = newLoc;

        moves -= 1;

        Debug.Log("moved");

        

    }

    



}
