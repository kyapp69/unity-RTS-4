﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;

public class BuildingButtonController : MonoBehaviour
{
    public GameObject player;
    public GameObject team;
    ResourceManager RM;
    UIController UI;

    public Button buttonOne;
    public Button barracksButtonOne;
    public Button barracksButtonTwo;

    public Button barracksButtonFour;
    public Button barracksButtonNine;
    public Button barracksButtonTen;

    public AudioSource playerAudio;
    public AudioClip trainVillagerAudio;
    public AudioClip trainFootmanAudio;
    public AudioClip trainSwordsmanAudio;
    public AudioClip trainOutriderAudio;
    public AudioClip trainKnightAudio;
    public AudioClip trainArcherAudio;

    public GameObject villager;

    [SerializeField]
    private float nextSpawnTime;
    public bool isTraining;

    // UI Elements
    private CanvasGroup UnitPanel;
    private CanvasGroup BasicBuildingsPanel;
    private CanvasGroup AdvancedBuildingsPanel;
    private CanvasGroup PeasantPanel;
    private CanvasGroup BuildingProgressPanel;
    private CanvasGroup BuildingActionPanel;

    TownHallController townHallScript;
    BarracksController barracksScript;

    InputManager inputScript;
    BuildingController buildingScript;

    UnitController footmanUC;
    UnitController swordsmanUC;
    UnitController villagerUC;
    UnitController outriderUC;
    UnitController knightUC;
    UnitController archerUC;

    public GameObject selectedObj;
    private Vector3 spawnPosition;

    [SerializeField]
    private float spawnDelay;

    // Trained units
    public GameObject swordsmanPrefab;
    public GameObject footmanPrefab;
    public GameObject villagerPrefab;
    public GameObject outriderPrefab;
    public GameObject knightPrefab;
    public GameObject archerPrefab;

    // Button text
    Text [] blacksmithText;

    // Start is called before the first frame update
    void Start()
    {
        Scene currentScene = SceneManager.GetActiveScene();
        if(currentScene.name != "Main Menu") {
            isTraining = false;
            BuildingProgressPanel = GameObject.Find("BuildingProgressPanel").GetComponent<CanvasGroup>();
            BasicBuildingsPanel = GameObject.Find("BasicBuildingsPanel").GetComponent<CanvasGroup>();
            AdvancedBuildingsPanel = GameObject.Find("AdvancedBuildingsPanel").GetComponent<CanvasGroup>();
            PeasantPanel = GameObject.Find("VillagerPanel").GetComponent<CanvasGroup>();
            BuildingActionPanel = GameObject.Find("BuildingActions").GetComponent<CanvasGroup>();

            // Trained units
            swordsmanUC = swordsmanPrefab.GetComponent<UnitController>();
            footmanUC = footmanPrefab.GetComponent<UnitController>();
            villagerUC = villagerPrefab.GetComponent<UnitController>();
            archerUC = archerPrefab.GetComponent<UnitController>();
            outriderUC = outriderPrefab.GetComponent<UnitController>();
            knightUC = knightPrefab.GetComponent<UnitController>();

            player = GameObject.FindGameObjectWithTag("Player");
            team = GameObject.Find("Faction");
            inputScript = player.GetComponent<InputManager>();
            RM = team.GetComponent<ResourceManager>();
            UI = player.GetComponent<UIController>();

            buttonOne.onClick.AddListener(HireVillager);
            barracksButtonOne.onClick.AddListener(HireSwordsman);
            barracksButtonTwo.onClick.AddListener(HireFootman);
            barracksButtonFour.onClick.AddListener(HireArcher);
            barracksButtonNine.onClick.AddListener(HireOutrider);
            barracksButtonTen.onClick.AddListener(HireKnight);
        }
    }

    // Update is called once per frame
    void Update()
    {
            if(RM.lumberYardCount > 0) {
                barracksButtonFour.interactable = true;
            } else {
                barracksButtonFour.interactable = false;
            }
            if(RM.blacksmithCount > 0) {
                barracksButtonTwo.interactable = true;
            } else {
                barracksButtonTwo.interactable = false;
            }
            if(RM.stablesCount > 0) {
                barracksButtonNine.interactable = true;
            } else {
                barracksButtonNine.interactable = false;
            }
            if(RM.stablesCount > 0 && RM.blacksmithCount > 0) {
                barracksButtonTen.interactable = true;
            } else {
                barracksButtonTen.interactable = false;
            }


    }

    void HireVillager()
    {
        if (RM.gold >= villagerUC.gold && RM.wood >= villagerUC.wood && RM.food >= villagerUC.food && RM.iron >= villagerUC.iron && RM.steel >= villagerUC.steel && RM.skymetal >= villagerUC.skymetal && RM.stone >= villagerUC.stone && RM.housing < RM.maxHousing)
        {
            UI.TownHallTraining();
            RM.gold -= villagerUC.gold;
            RM.wood -= villagerUC.wood;
            RM.food -= villagerUC.food;
            RM.iron -= villagerUC.iron;
            RM.steel -= villagerUC.steel;
            RM.skymetal -= villagerUC.skymetal;
            RM.stone -= villagerUC.stone;
            RM.housing += 1;
            selectedObj = inputScript.selectedObj;
            townHallScript = selectedObj.GetComponent<TownHallController>();
            buildingScript = selectedObj.GetComponent<BuildingController>();

           
          //  spawnPosition = new Vector3(buildingScript.location.x, buildingScript.location.y, buildingScript.location.z +50f);
            playerAudio.clip = trainVillagerAudio;
            playerAudio.Play();
            townHallScript.HireVillager();
        }
        else 
        {
            UI.noResourcesText.SetActive(true);
            StartCoroutine(Wait());
        }
    }

    void HireSwordsman()
    {
         if (RM.gold >= swordsmanUC.gold && RM.wood >= swordsmanUC.wood && RM.food >= swordsmanUC.food && RM.iron >= swordsmanUC.iron && RM.steel >= swordsmanUC.steel && RM.skymetal >= swordsmanUC.skymetal && RM.stone >= swordsmanUC.stone && RM.housing < RM.maxHousing)
        {
            UI.BarracksTraining();
            RM.gold -= swordsmanUC.gold;
            RM.wood -= swordsmanUC.wood;
            RM.food -= swordsmanUC.food;
            RM.iron -= swordsmanUC.iron;
            RM.steel -= swordsmanUC.steel;
            RM.skymetal -= swordsmanUC.skymetal;
            RM.stone -= swordsmanUC.stone;
            RM.housing += 1;
            selectedObj = inputScript.selectedObj;
            barracksScript = selectedObj.GetComponent<BarracksController>();
            buildingScript = selectedObj.GetComponent<BuildingController>();
            playerAudio.clip = trainSwordsmanAudio;
            playerAudio.Play();
            barracksScript.HireSwordsman();
        } 
        else   
        {
            UI.noResourcesText.SetActive(true);
            StartCoroutine(Wait());
        }
    }

    void HireFootman()
    {
         if (RM.gold >= footmanUC.gold && RM.wood >= footmanUC.wood && RM.food >= footmanUC.food && RM.iron >= footmanUC.iron && RM.steel >= footmanUC.steel && RM.skymetal >= footmanUC.skymetal && RM.stone >= footmanUC.stone && RM.housing < RM.maxHousing)
        {
            UI.BarracksTraining();
            RM.gold -= footmanUC.gold;
            RM.wood -= footmanUC.wood;
            RM.food -= footmanUC.food;
            RM.iron -= footmanUC.iron;
            RM.steel -= footmanUC.steel;
            RM.skymetal -= footmanUC.skymetal;
            RM.stone -= footmanUC.stone;
            RM.housing += 1;
            selectedObj = inputScript.selectedObj;
            barracksScript = selectedObj.GetComponent<BarracksController>();
            buildingScript = selectedObj.GetComponent<BuildingController>();
            playerAudio.clip = trainFootmanAudio;
            playerAudio.Play();
            barracksScript.HireFootman();
        } 
        else   
        {
            UI.noResourcesText.SetActive(true);
            StartCoroutine(Wait());
        }
    }

    void HireArcher()
    {
         if (RM.gold >= archerUC.gold && RM.wood >= archerUC.wood && RM.food >= archerUC.food && RM.iron >= archerUC.iron && RM.steel >= archerUC.steel && RM.skymetal >= archerUC.skymetal && RM.stone >= archerUC.stone && RM.housing < RM.maxHousing)
        {
            UI.BarracksTraining();
            RM.gold -= archerUC.gold;
            RM.wood -= archerUC.wood;
            RM.food -= archerUC.food;
            RM.iron -= archerUC.iron;
            RM.steel -= archerUC.steel;
            RM.skymetal -= archerUC.skymetal;
            RM.stone -= archerUC.stone;
            RM.housing += 1;
            selectedObj = inputScript.selectedObj;
            barracksScript = selectedObj.GetComponent<BarracksController>();
            buildingScript = selectedObj.GetComponent<BuildingController>();
            playerAudio.clip = trainArcherAudio;
            playerAudio.Play();
            barracksScript.HireArcher();
        } 
        else   
        {
            UI.noResourcesText.SetActive(true);
            StartCoroutine(Wait());
        }
    }

    void HireOutrider()
    {
         if (RM.gold >= outriderUC.gold && RM.wood >= outriderUC.wood && RM.food >= outriderUC.food && RM.iron >= outriderUC.iron && RM.steel >= outriderUC.steel && RM.skymetal >= outriderUC.skymetal && RM.stone >= outriderUC.stone && RM.housing < RM.maxHousing)
        {
            UI.BarracksTraining();
            RM.gold -= outriderUC.gold;
            RM.wood -= outriderUC.wood;
            RM.food -= outriderUC.food;
            RM.iron -= outriderUC.iron;
            RM.steel -= outriderUC.steel;
            RM.skymetal -= outriderUC.skymetal;
            RM.stone -= outriderUC.stone;
            RM.housing += 1;
            selectedObj = inputScript.selectedObj;
            barracksScript = selectedObj.GetComponent<BarracksController>();
            buildingScript = selectedObj.GetComponent<BuildingController>();
            playerAudio.clip = trainOutriderAudio;
            playerAudio.Play();
            barracksScript.HireOutrider();
        } 
        else   
        {
            UI.noResourcesText.SetActive(true);
            StartCoroutine(Wait());
        }
    }

    void HireKnight()
    {
         if (RM.gold >= knightUC.gold && RM.wood >= knightUC.wood && RM.food >= knightUC.food && RM.iron >= knightUC.iron && RM.steel >= knightUC.steel && RM.skymetal >= knightUC.skymetal && RM.stone >= knightUC.stone && RM.housing < RM.maxHousing)
        {
            UI.BarracksTraining();
            RM.gold -= knightUC.gold;
            RM.wood -= knightUC.wood;
            RM.food -= knightUC.food;
            RM.iron -= knightUC.iron;
            RM.steel -= knightUC.steel;
            RM.skymetal -= knightUC.skymetal;
            RM.stone -= knightUC.stone;
            RM.housing += 1;
            selectedObj = inputScript.selectedObj;
            barracksScript = selectedObj.GetComponent<BarracksController>();
            buildingScript = selectedObj.GetComponent<BuildingController>();
            playerAudio.clip = trainKnightAudio;
            playerAudio.Play();
            barracksScript.HireKnight();
        } 
        else   
        {
            UI.noResourcesText.SetActive(true);
            StartCoroutine(Wait());
        }
    }
    IEnumerator Wait()
    {
        yield return new WaitForSeconds(3);
        //my code here after 3 seconds
        UI.noResourcesText.SetActive(false);
    }

}
