using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public GameObject ParkPrefab;
    public GameObject HousePrefab;
    public GameObject CityPrefab;
    public GameObject PersonPrefab;
    public GameObject[] people;
    public GameObject[] virusPeople;
    public TextMeshProUGUI virusCounter;

    public GameObject endGameUI;
    public GameObject pauseUI;


    public int virusChance = 2000;
    int virusChanceDelta = 1;

    bool isPaused = false;

    void Awake()
    {
        Random.InitState((int) System.DateTime.Now.Ticks);
        print(System.DateTime.Now.Ticks);
        people = new GameObject[11];
        virusPeople = new GameObject[11];
        //Spawn Building Relative to the center of the island
        GameObject[] POI = { ParkPrefab, ParkPrefab, ParkPrefab, HousePrefab, HousePrefab, HousePrefab, CityPrefab, CityPrefab, CityPrefab };
        foreach (GameObject place in POI)
        {
            Debug.Log(place.name);
            GameObject childPlace = Instantiate(place, new Vector3(Random.Range(-186f, 209f), Random.Range(155f, -189f), 0f), new Quaternion(0f, 0f, 0f, 0f));
            childPlace.transform.SetParent(this.gameObject.transform, false);
        }
        for (int i = 0; i <= people.GetUpperBound(0); i++)
        {
            GameObject person = Instantiate(PersonPrefab, new Vector3(Random.Range(-186f, 311f), Random.Range(155f, 189f), 0f), new Quaternion(0f, 0f, 0f, 0f));
            person.GetComponent<PeopleAI>().island = this.gameObject;
            people.SetValue(person, i);
            person.transform.SetParent(this.gameObject.transform.parent, false);
        }
        people[0].GetComponent<PeopleAI>().hasVirus = true;

    }

    // Update is called once per frame
    void Update()
    {
        if (virusChance <= 0) 
        {
            virusChance = 1000;
        }
        if (Random.Range(0, 200) == 199)
        {
            virusChance -= virusChanceDelta;
            virusChanceDelta++;
        }
        virusPeople = new GameObject[11];

        int z = 0;
        foreach (GameObject person in people)
        {
            PeopleAI peopleAI = person.GetComponent<PeopleAI>();
            bool hasVirus = peopleAI.hasVirus;
            if (hasVirus == true)
            {
                virusPeople[z] = person.gameObject;
                z++;
            }

        }
        z = 0;
        foreach (GameObject person in virusPeople)
        {
            if (person != null)
            {
                PeopleAI peopleAI = person.GetComponent<PeopleAI>();
                bool hasVirus = peopleAI.hasVirus;
                if (hasVirus)
                {
                    z++;
                }
            }
        }
        int virusPercent = z * 100;
        virusPercent = virusPercent / people.GetUpperBound(0);


        virusCounter.text = virusPercent.ToString() + "%";

        if (virusPercent >= 100)
        {
            Debug.Log("You lost");
            Time.timeScale = 0f;
            endGameUI.transform.GetChild(1).gameObject.SetActive(true);

            foreach (GameObject person in people)
            {
                person.GetComponent<PeopleAI>().gameIsGoing = false;
            }

        } else if (virusPercent <= 0)
        {
            Debug.Log("You Won!");
            Time.timeScale = 0f;
            endGameUI.transform.GetChild(0).gameObject.SetActive(true);

            foreach (GameObject person in people)
            {
                person.GetComponent<PeopleAI>().gameIsGoing = false;
            }
        }

        if (isPaused) 
        {
            Time.timeScale = 0f;
        }
        else
        {
            Time.timeScale = 1f;
        }


    }

    public void GiveMask() {
        virusChance += 150;
        virusChanceDelta--;
        virusChanceDelta--;
    }
    public void GiveSanitizer() {
        virusChance += 75;
        virusChanceDelta--;

        foreach (GameObject person in people)
        {
            PeopleAI peopleAI = person.GetComponent<PeopleAI>();
            peopleAI.virusPotential -= 5;
        }
    }
    public void GiveLockdown() {
        virusChance += 200;
        virusChanceDelta--;
        foreach (GameObject person in people)
        {
            PeopleAI peopleAI = person.GetComponent<PeopleAI>();
            if (peopleAI.lockdown)
            {
                peopleAI.lockdown = false;
            }
            else {
                peopleAI.lockdown = true;
            }
            GameObject[] houses = new GameObject[3];
            houses = GameObject.FindGameObjectsWithTag("House");
            //Debug.Log(houses);
            peopleAI.POI = houses[Random.Range(0, houses.GetUpperBound(0))];
        }
    }

    public void Restart(){
        SceneManager.LoadScene(1);
    }

    public void Quit() 
    {
        Application.Quit();
    }

    public void Pause() 
    {
        isPaused = true;
        pauseUI.SetActive(true);
        foreach (GameObject person in people)
        {
            person.GetComponent<PeopleAI>().gameIsGoing = false;
        }
    }

    public void Resume() 
    {
        isPaused = false;
        pauseUI.SetActive(false);
        foreach (GameObject person in people)
        {
            person.GetComponent<PeopleAI>().gameIsGoing = true;
        }
    }
}
