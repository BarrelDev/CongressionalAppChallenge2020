using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PeopleAI : MonoBehaviour
{
    public GameObject island;
    public float speed = 10f;
    public bool hasVirus;
    public int virusPotential;
    float step = 0;
    bool reachedPOI = false;
    public GameObject POI;
    GameObject[] places;
    GameManager manager;
    public bool gameIsGoing = true;
    public bool lockdown = false;


    // Start is called before the first frame update
    void Start()
    {
        gameIsGoing = true;
        manager = island.GetComponent<GameManager>();
        virusPotential = UnityEngine.Random.Range(0, 100);
        GetBuildings();
        POI = SetPOI();

        if (UnityEngine.Random.Range(0, 150) == UnityEngine.Random.Range(0, 151))
        {
            hasVirus = true;
        }
    }

    // Update is called once per frame
    void Update()
    {
        manager = island.GetComponent<GameManager>();
        if (gameIsGoing) {
            GoToPOI(POI);
            if (reachedPOI)
            {
                if (UnityEngine.Random.Range(0, 100) == virusPotential)
                {
                    step = 0;
                    if (!lockdown) { POI = SetPOI(); }
                    reachedPOI = false;
                }

            }
            if (virusPotential > 75)
            {
                if (!lockdown) {
                    if (UnityEngine.Random.Range(0, manager.virusChance / 20) == 10)
                    {
                        hasVirus = true;
                    }
                }
                
            }
            else
            {
                if (!lockdown) 
                {
                    if (UnityEngine.Random.Range(0, manager.virusChance) == 15)
                    {
                        hasVirus = true;
                    }
                }
            }

            if (hasVirus)
            {
                var image = this.gameObject.GetComponent<UnityEngine.UI.Image>();
                //Color red = new Color(255f, 31f, 0f);
                image.color = Color.red;

                if (UnityEngine.Random.Range(0, virusPotential * 250) == virusPotential / 2)
                {
                    image.color = Color.yellow;
                    hasVirus = false;
                    Debug.Log("Healed");
                }
            }

            if (virusPotential <= 0)
            {
                virusPotential = UnityEngine.Random.Range(0, 100);
            }
        }
        
    }

    GameObject SetPOI()
    {
        //Select a random place

        
        GameObject SelectedPOI = places[UnityEngine.Random.Range(0, places.GetUpperBound(0))];
        //Debug.Log(SelectedPOI.name);
        return SelectedPOI;
    }

    public void GoToPOI(GameObject POI)
    {
        //Move towards POI
        step += speed * Time.fixedDeltaTime;
        this.gameObject.transform.position = Vector2.MoveTowards(transform.position, POI.transform.position, step);
        if (this.gameObject.transform.position == POI.transform.position)
        {
            reachedPOI = true;
        }
    }

    void GetBuildings()
    {
        int islandChildCount = island.transform.childCount;
        //Debug.Log(islandChildCount);
        places = new GameObject[islandChildCount];
        for (int i = 0; i < islandChildCount; i++)
        {
            places[i] = island.transform.GetChild(i).gameObject;
        }
        /*foreach (GameObject place in places)
        {
            Debug.Log(place.name);
        }*/
    }

    

}
