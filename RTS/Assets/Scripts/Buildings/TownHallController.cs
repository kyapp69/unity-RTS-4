﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;
using UnityEngine.UI;

public class TownHallController : MonoBehaviour
{
    // Villager names
    UIController UI;
    string[] villagers = new string[14]{ "Farah", "Dan", "Dave", "Steve", "Katie", "Sam", "Ryan", "Sid", "Bill", "Will", "Sarah", "Arj", "Izzy", "Aron"};

    string[] firstNames = new string[14]{ "Victor", "Dominik", "Kevan", "Criston", "Alistar", "Grant", "Abram", "Guy", "Braum", "Will", "David", "Kevan", "Ryan", "Daniel"};
    string[] lastNameFirst = new string[14]{ "Strong", "Fast", "Frost", "Hard", "Quick", "Brick", "Stone", "Dawn", "Sky", "Storm", "Thunder", "Rock", "Fair", "Swift"};
    string[] lastNameSecond = new string[14]{ "beam", "arm", "bull", "hammer", "saw", "pike", "healer", "cook", "smith", "hunter", "mason", "builder", "fletcher", "swimmer"};


    private float nextSpawnTime;
    public int i = 0;

    public GameObject villagerPrefab;
    private AudioSource villagerAudio;
    public AudioClip villagerReporting;

    [SerializeField]
    public float spawnDelay;
    public bool selected = false;

    GameObject player;
    InputManager inputScript;
    UnitController workerUC;
    Selection workerSelection;
    BuildingController buildingScript;

    public GameObject selectedObj;
    private Vector3 spawnPosition;
    public bool isTraining;

    //Progress bar
    private GameObject BuildingProgressBar;
    private Slider BuildingProgressSlider;
    public Image progressIcon;

    //UI Elements
    private CanvasGroup BuildingProgressPanel;
    private CanvasGroup BuildingActionPanel;



    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        UI = player.GetComponent<UIController>();

        inputScript = player.GetComponent<InputManager>();
        BuildingProgressPanel = GameObject.Find("BuildingProgressPanel").GetComponent<CanvasGroup>();
        BuildingActionPanel = GameObject.Find("BuildingActions").GetComponent<CanvasGroup>();

        // Progress bar
        BuildingProgressBar = GameObject.Find("BuildingProgressBar");
        BuildingProgressSlider = BuildingProgressBar.GetComponent<Slider>();
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void HireVillager()
    {
        selectedObj = inputScript.selectedObj;
        buildingScript = selectedObj.GetComponent<BuildingController>();

        StartCoroutine(VillagerSpawn());
    }
    
    IEnumerator VillagerSpawn()
    {
        isTraining = true;
        selectedObj = inputScript.selectedObj;
        buildingScript = selectedObj.GetComponent<BuildingController>();
        spawnPosition = new Vector3(buildingScript.location.x, buildingScript.location.y, buildingScript.location.z - 5f);
        nextSpawnTime = Time.time + spawnDelay;

        for (i = 1; i < 11; i++)
        {
            yield return new WaitForSeconds(1);
        }
        isTraining = false;

        var random1 = Random.Range(0, firstNames.Length);
        var random2 = Random.Range(0, lastNameFirst.Length);
        var random3 = Random.Range(0, lastNameSecond.Length);
        progressIcon = GameObject.Find("ProgressIcon").GetComponent<Image>();
        workerUC = villagerPrefab.GetComponent<UnitController>();
        workerSelection = villagerPrefab.GetComponent<Selection>();
        progressIcon.sprite = workerUC.unitIcon;
        workerUC.unitName = firstNames[random1] + " " + lastNameFirst[random2] + lastNameSecond[random3];
        workerSelection.owner = player;

        Instantiate(villagerPrefab, spawnPosition, Quaternion.identity);
        villagerAudio = selectedObj.GetComponent<AudioSource>();
        villagerAudio.clip = villagerReporting;
        villagerAudio.Play();
        UI.TownHallSelect();
    }
}
