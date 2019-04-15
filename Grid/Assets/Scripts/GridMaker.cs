using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class GridMaker : MonoBehaviour
{
    public const int Width = 5;
    public const int Height = 7;

    //grid offset
    float xOffset = Width / 2f - 0.5f;
    float yOffset = Height / 2f - 0.5f;

    //2d arrary

    public static GameObject[,] tiles;
    public static GridMaker Instance;
    

    //lerping shit
    public static float slideLerp = -1;
    public float lerpSpeed = 0.25f;

    //text
    public Text scoreText;
    public int score;

    //object that holds the tile sprite

    public GameObject tilePrefab;
    public GameObject playerPrefab;

    //empty object to hold grid
    GameObject gridHolder;

    public Vector2Int newPlayerPos;
    public Vector2Int currentPlayerPos;

    //particles
    public ParticleSystem particles;

    // Start is called before the first frame update
    void Start()
    {
        tiles = new GameObject[Width, Height];
        gridHolder = new GameObject();
        gridHolder.transform.position = new Vector3(0, 0, 0);

        Instance = this;


        for (int x = 0; x < Width; x++)
        {
            for (int y = 0; y < Height; y++)
            {
                GameObject newTile = Instantiate(tilePrefab);
                newTile.transform.parent = gridHolder.transform;
                newTile.transform.localPosition = new Vector2(Width - x - xOffset, Height - y - yOffset);
                tiles[x, y] = newTile;
                TileScript tileScript = newTile.GetComponent<TileScript>();
                tileScript.SetColor(Random.Range(0, tileScript.Colors.Length));

                
            }
        }

        //particles

        /*ParticleSystem particleSys = tilePrefab.GetComponent<ParticleSystem>();
        var em = particleSys.emission;
        em.enabled = false;
        */


        GameObject centerTile = tiles[Width / 2, Height / 2];
        GameObject newPlayer = Instantiate(playerPrefab);
        newPlayer.transform.parent = gridHolder.transform;
        newPlayer.transform.localPosition = centerTile.transform.localPosition;
        Destroy(centerTile);
        tiles[Width / 2, Height / 2] = newPlayer;
        //Destroy(tiles[2,3]);
    }

    // Update is called once per frame
    void Update()
    {
        GameObject centerTile = tiles[Width / 2, Height / 2];
        scoreText.text = "SCORE:" + score;

        if (slideLerp < 0 && !Repopulate() && HasMatch())
        {
            Debug.Log("match");
            RemoveMatch();
            score += 3; 
            
        }

        else if (slideLerp >= 0)
        {
            slideLerp += Time.deltaTime / lerpSpeed;
            if (slideLerp >= 1)
            {
                slideLerp = -1;
            }
        }

       
        if (GameObject.Find("Player(Clone)") == null)
        {
            
            Destroy(centerTile);
            GameObject newPlayer = Instantiate(playerPrefab);
            newPlayer.transform.parent = gridHolder.transform;
            newPlayer.transform.localPosition = centerTile.transform.localPosition;
            
            tiles[Width / 2, Height / 2] = newPlayer;
        }

    }

    public bool Repopulate()
    {
        bool repop = false;

        for(int x = 0; x < Width;  x++)
        {
            for(int y = 0; y < Height; y++)
            {
                if(tiles[x,y] == null)
                {
                    repop = true;

                    if (y == 0)
                    {
                        tiles[x, y] = Instantiate(tilePrefab);
                        TileScript tileScript = tiles[x, y].GetComponent<TileScript>();
                        tileScript.SetColor(Random.Range(0, tileScript.Colors.Length));
                        tiles[x, y].transform.parent = gridHolder.transform;
                        tiles[x, y].transform.localPosition = new Vector2(Width - x - xOffset, Height - y - yOffset);
                    }
                    else
                    {
                        slideLerp = 0;
                        tiles[x, y] = tiles[x, y - 1];
                        TileScript tileScript = tiles[x, y].GetComponent<TileScript>();
                        if (tileScript != null)
                        {
                            tileScript.SetupSlide(new Vector2(Width - x - xOffset, Height - y - yOffset));
                        }
                        PlayerScript movePlayerScript = tiles[x, y].GetComponent<PlayerScript>();
                        if (movePlayerScript != null)
                        {
                            movePlayerScript.PlayerPos.Set(x, y);
                        }
                        

                    } tiles[x, y - 1] = null;
                }
            }
        }
     return repop;
    }
    
    public TileScript HasMatch()
    {
        for (int x = 0; x < Width; x++)
        {
            for (int y = 0; y < Height; y++)
            {
                TileScript tileScript = tiles[x, y].GetComponent<TileScript>();
                if (tileScript != null)
                {
                    if (x < Width - 2 && tileScript.isMatching(tiles[x + 1, y], tiles[x + 2, y]))
                    {
                        return tileScript;
                    }
                    if (y < Height - 2 && tileScript.isMatching(tiles[x, y + 1], tiles[x, y + 2]))
                    {
                        return tileScript;
                    }
                }

            }
        }
        return null;

    }

    public void RemoveMatch()
    {
        for (int x = 0; x < Width; x++)
        {
            for (int y = 0; y < Height; y++)
            {
                
                TileScript tileScript = tiles[x, y].GetComponent<TileScript>();
                if (tileScript != null)
                {
                    if (x < Width - 2 && tileScript.isMatching(tiles[x + 1, y], tiles[x + 2, y]))
                    {

                        //tiles[x,y].SendMessage("Particles");
                        particles.transform.position = tiles[x, y].transform.position;
                        particles.Emit(50);

                        Destroy(tiles[x,y]);
                        Destroy(tiles[x + 1, y]);
                        Destroy(tiles[x + 2, y]);
                        
                    }
                    if (y < Height - 2 && tileScript.isMatching(tiles[x, y + 1], tiles[x, y + 2]))
                    {
                        //tiles[x,y].SendMessage("Particles");
                        particles.transform.position = tiles[x, y].transform.position;
                        particles.Emit(50);

                        Destroy(tiles[x, y]);
                        Destroy(tiles[x, y + 1]);
                        Destroy(tiles[x, y + 2]);
                        

                    }
                }
            }
        }
    }



    /*void movePlayer()
    {
        GameObject swapTile = tiles[newPlayerPos.x, newPlayerPos.y];

        Vector2 OldPlayerPos = new Vector2(playerPrefab.transform.localPosition.x, playerPrefab.transform.localPosition.y);

        playerPrefab.transform.localPosition = swapTile.transform.localPosition;
        swapTile.transform.localPosition = OldPlayerPos;
        tiles[currentPlayerPos.x, currentPlayerPos.y] = swapTile;
        tiles[newPlayerPos.x, newPlayerPos.y] = playerPrefab;
        currentPlayerPos = newPlayerPos;

        //CheckMatches();
    }
    */
    







}
