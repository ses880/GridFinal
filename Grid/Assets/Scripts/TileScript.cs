using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileScript : MonoBehaviour
{
    public int type;
    //public Sprite[] tileSprites;
    public Color[] Colors;

    private bool inSlide = false;

    public Vector3 startPos;
    public Vector3 destPos;

    
    void Awake()
    {
        // Colors = new Color[] { Color.red, new Color(255f,128f,0f), Color.yellow, Color.green, Color.blue, new Color(128f,0,128f) };
        
    }

    public void SetColor(int rand)
    {
        type = rand;
        //Debug.Log(type);
        //Debug.Log(Colors.Length);
        GetComponent<SpriteRenderer>().color = Colors[type];
    }

    /*public void SetType(int i)
    {
        type = i;
        GetComponent<SpriteRenderer>().color = Colors[type];
    }
    */

    void Update()
    {
        if(inSlide)
        {
            if(GridMaker.slideLerp < 0)
            {
                transform.localPosition = destPos;
                inSlide = false;
                
            }
            else
            {
                transform.localPosition = Vector3.Lerp(startPos, destPos, GridMaker.slideLerp);
            }
        }
        
    }

    public bool isMatching(GameObject gameObject1, GameObject gameObject2)
    {
        TileScript ts1 = gameObject1.GetComponent<TileScript>();
        TileScript ts2 = gameObject2.GetComponent<TileScript>();
        return ts1 != null && ts2 != null && type == ts1.type && type == ts2.type;
    }

    public void SetupSlide(Vector2 newDestPos)
    {
        inSlide = true;
        startPos = transform.localPosition;
        destPos = newDestPos;
        
    }
    
    



}

