﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;
using UnityEngine.UI;

public class BlacksmithController : MonoBehaviour
{
    UIController UI;
    // footmen names
    string[] footmen = new string[14]{ "Bron", "Darek", "Krom", "Turin", "Zerk", "Rua", "Vos", "Barros", "Braxis", "Kraye", "Sloa", "Kolin", "Kaleb", "Arvan"};
    string[] firstNames = new string[14]{ "Aubrey", "Braum", "Braxis", "Davin", "Garen", "Oren", "Gavin", "Derek", "Kevan", "Stephen", "David", "Ruan", "Edward", "Marcus"};

    string[] lastNameFirst = new string[14]{ "Foe", "Strong", "Ox", "Deer", "Swift", "Bright", "Light", "Dark", "Fire", "Shade", "Stout", "Quick", "Moose", "Dread"};
    string[] lastNameSecond = new string[14]{ "hammer", "fist", "bridge", "wind", "blade", "spear", "shield", "bane", "sheen", "whip", "strike", "stone", "wind", "arm"};

    string[] SMFirstNames = new string[14]{ "Aubrey", "Braum", "Braxis", "Davin", "Garen", "Oren", "Gavin", "Derek", "Kevan", "Stephen", "David", "Ruan", "Edward", "Marcus"};

    string[] SMLastNameFirst = new string[14]{ "Foe", "Fear", "Doom", "Gloom", "Dusk", "Dawn", "Light", "Dark", "Hope", "Swift", "Summer", "Winter", "Fall", "Tall"};
    string[] SMLastNameSecond = new string[14]{ "arm", "fist", "biter", "slayer", "hammer", "fighter", "shield", "crusher", "shredder", "rune", "strike", "stone", "wind", "arm"};


    private float nextSpawnTime;
    public int i = 0;

    private AudioSource swordsmanAudio;
    private AudioSource footmanAudio;
    public AudioClip swordsmanReporting;
    public AudioClip footmanReporting;

    [SerializeField]
    public float spawnDelay;
    public bool selected = false;

    GameObject player;
    InputManager inputScript;
    Selection swordsmanSelection;
    Selection footmanSelection;
    BuildingController buildingScript;
    ResourceManager RM;
    ResearchController RC;

    public GameObject selectedObj;
    private Vector3 spawnPosition;
    public bool isTraining;
    public string research;

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
        RM = player.GetComponent<ResourceManager>();
        RC = player.GetComponent<ResearchController>();
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
        //if (ShouldSpawn())
        //{
        //    Spawn();
        //}
    }

    public void ResearchBlacksmithing () 
    { 
        selectedObj = inputScript.selectedObj;
        buildingScript = selectedObj.GetComponent<BuildingController>();

        if(RM.gold < RC.basicBlacksmithingGold || RM.iron < RC.basicBlacksmithingIron) {
            UI.noResourcesText.SetActive(true);
            StartCoroutine(CloseResourcesText());
        } else {
            RM.gold -= RC.basicBlacksmithingGold;
            RM.iron -= RC.basicBlacksmithingIron;
            StartCoroutine(Research());
            RC.basicBlacksmithingButton.interactable = false;
            RC.basicBlacksmithing = true;
            UI.BlacksmithTraining();
        }
    }
    public void ResearchBasicToolSmithing () 
    { 
        if(RM.gold < RC.basicToolSmithingGold || RM.iron < RC.basicToolSmithingIron) {
            UI.noResourcesText.SetActive(true);
            StartCoroutine(CloseResourcesText());
        } else {
            RM.gold -= RC.basicToolSmithingGold;
            RM.iron -= RC.basicToolSmithingIron;
            StartCoroutine(Research());
            RC.basicToolSmithingButton.interactable = false;
            RC.basicToolSmithing = true;
            UI.BlacksmithTraining();
        }
    }

    public void ResearchBasicArmourSmithing () 
    { 
        if(RM.gold < RC.basicArmourSmithingGold || RM.iron < RC.basicArmourSmithingIron) {
            UI.noResourcesText.SetActive(true);
            StartCoroutine(CloseResourcesText());
        } else {
            RM.gold -= RC.basicArmourSmithingGold;
            RM.iron -= RC.basicArmourSmithingIron;
            StartCoroutine(Research());
            RC.basicArmourSmithingButton.interactable = false;
            RC.basicArmourSmithing = true;
            UI.BlacksmithTraining();
        }
    }

    public void ResearchBasicWeaponSmithing () 
    { 
        if(RM.gold < RC.basicWeaponSmithingGold || RM.iron < RC.basicWeaponSmithingIron) {
            UI.noResourcesText.SetActive(true);
            StartCoroutine(CloseResourcesText());
        } else {
            RM.gold -= RC.basicWeaponSmithingGold;
            RM.iron -= RC.basicWeaponSmithingIron;
            StartCoroutine(Research());
            RC.basicWeaponSmithingButton.interactable = false;
            RC.basicWeaponSmithing = true;
        }
    }

    public void ResearchArtisanBlacksmithing () 
    { 
        if(RM.gold < RC.artisanBlacksmithingGold || RM.iron < RC.artisanBlacksmithingIron) {
            UI.noResourcesText.SetActive(true);
            StartCoroutine(CloseResourcesText());
        } else {
            RM.gold -= RC.artisanBlacksmithingGold;
            RM.iron -= RC.artisanBlacksmithingIron;
            StartCoroutine(Research());
            RC.artisanBlacksmithingButton.interactable = false;
            RC.artisanBlacksmithing = true;
        }
    }

    public void ResearchArtisanToolSmithing () 
    { 
        if(RM.gold < RC.artisanToolSmithingGold || RM.iron < RC.artisanToolSmithingIron) {
            UI.noResourcesText.SetActive(true);
            StartCoroutine(CloseResourcesText());
        } else {
            RM.gold -= RC.artisanToolSmithingGold;
            RM.iron -= RC.artisanToolSmithingIron;
            StartCoroutine(Research());
            RC.artisanToolSmithingButton.interactable = false;
            RC.artisanWeaponSmithing = true;
        }
    }

    public void ResearchArtisanArmourSmithing () 
    { 
        if(RM.gold < RC.artisanArmourSmithingGold || RM.iron < RC.artisanArmourSmithingIron) {
            UI.noResourcesText.SetActive(true);
            StartCoroutine(CloseResourcesText());
        } else {
            RM.gold -= RC.artisanArmourSmithingGold;
            RM.iron -= RC.artisanArmourSmithingIron;
            StartCoroutine(Research());
            RC.artisanArmourSmithingButton.interactable = false;
            RC.artisanArmourSmithing = true;
        }
    }

    public void ResearchArtisanWeaponSmithing () 
    { 
        if(RM.gold < RC.artisanWeaponSmithingGold || RM.iron < RC.artisanWeaponSmithingIron) {
            UI.noResourcesText.SetActive(true);
            StartCoroutine(CloseResourcesText());
        } else {
            RM.gold -= RC.artisanWeaponSmithingGold;
            RM.iron -= RC.artisanWeaponSmithingIron;
            RC.artisanWeaponSmithingButton.interactable = false;
            RC.artisanWeaponSmithing = true;
        }
    }

    public void ResearchHorseshoes () 
    { 
        if(RM.gold < RC.horshoesGold || RM.iron < RC.horshoesIron) {
            UI.noResourcesText.SetActive(true);
            StartCoroutine(CloseResourcesText());
        } else {
            RM.gold -= RC.horshoesGold;
            RM.iron -= RC.horshoesIron;
            StartCoroutine(Research());
            RC.horshoesButton.interactable = false;
            RC.horshoes = true;
        }
    }

    public void ResearchMinecarts () 
    { 
        if(RM.gold < RC.minecartsGold || RM.iron < RC.minecartsGold) {
            UI.noResourcesText.SetActive(true);
            StartCoroutine(CloseResourcesText());
        } else {
            RM.gold -= RC.minecartsGold;
            RM.iron -= RC.minecartsIron;
            StartCoroutine(Research());
            RC.minecartsButton.interactable = false;
            RC.minecarts = true;
        }
    }
    
    public void ResearchCaltrops () 
    { 
        if(RM.gold < RC.caltropsGold || RM.iron < RC.caltropsIron) {
            UI.noResourcesText.SetActive(true);
            StartCoroutine(CloseResourcesText());
        } else {
            RM.gold -= RC.caltropsGold;
            RM.iron -= RC.caltropsIron;
            StartCoroutine(Research());
            RC.caltropsButton.interactable = false;
            RC.caltrops = true;
        }
    }
    
    public void ResearchReinforcedBuildings () 
    { 
        if(RM.gold < RC.reinforcedBuildingsGold || RM.iron < RC.reinforcedBuildingsIron) {
            UI.noResourcesText.SetActive(true);
            StartCoroutine(CloseResourcesText());
        } else {
            RM.gold -= RC.reinforcedBuildingsGold;
            RM.iron -= RC.reinforcedBuildingsIron;
            StartCoroutine(Research());
            RC.reinforcedBuildingsButton.interactable = false;
            RC.reinforcedBuildings = true;
        }
    }

    IEnumerator CloseResourcesText()
    {
        yield return new WaitForSeconds(3);
        //my code here after 3 seconds
        UI.noResourcesText.SetActive(false);
    }

    IEnumerator Research() 
    {
        UI.BlacksmithTraining();
        isTraining = true;
        selectedObj = inputScript.selectedObj;
        buildingScript = selectedObj.GetComponent<BuildingController>();

        // if(unit == "Footman") {
        //     // var iteration1 = Random.Range(0, firstNames.Length);
        //     // var iteration2 = Random.Range(0, lastNameFirst.Length);
        //     // var iteration3 = Random.Range(0, lastNameSecond.Length);
        //     // progressIcon = GameObject.Find("ProgressIcon").GetComponent<Image>();
        //     // progressIcon.sprite = footmanPrefab.GetComponent<UnitController>().unitIcon;
        //     // footmanPrefab.GetComponent<UnitController>().unitName = firstNames[iteration1] + " " + lastNameFirst[iteration2] + lastNameSecond[iteration3];
        //     // footmanSelection = footmanPrefab.GetComponent<Selection>();
        //     // footmanSelection.owner = player;

        //     // footmanAudio = selectedObj.GetComponent<AudioSource>();
        //     // footmanAudio.clip = footmanReporting;
        //     // prefab = footmanPrefab;
        // } else if (unit == "Swordsman") {
        //     // var iteration1 = Random.Range(0, SMFirstNames.Length);
        //     // var iteration2 = Random.Range(0, SMLastNameFirst.Length);
        //     // var iteration3 = Random.Range(0, SMLastNameSecond.Length);
        //     // progressIcon = GameObject.Find("ProgressIcon").GetComponent<Image>();
        //     // progressIcon.sprite = swordsmanPrefab.GetComponent<UnitController>().unitIcon;
        //     // swordsmanPrefab.GetComponent<UnitController>().unitName = SMFirstNames[iteration1] + " " + SMLastNameFirst[iteration2] + SMLastNameSecond[iteration3];
        //     // swordsmanSelection = swordsmanPrefab.GetComponent<Selection>();
        //     // swordsmanSelection.owner = player;

        //     // swordsmanAudio = selectedObj.GetComponent<AudioSource>();
        //     // swordsmanAudio.clip = swordsmanReporting;
        //     // prefab = swordsmanPrefab;
        // }

        for (i = 1; i < 11; i++)
        {
            yield return new WaitForSeconds(1);
        }
    // if(unit == "Swordsman") {
    //     // Instantiate(prefab, spawnPosition, Quaternion.identity);

    //     // if(unit == "Footman") {
    //     //     footmanAudio.Play();
    //     // } else if (unit == "Swordsman") {
    //     //     swordsmanAudio.Play();
    //     // }
    // } else {

    // }
        Debug.Log("Done research!");
        isTraining = false;
        // UI.BarracksSelect();
    }
}