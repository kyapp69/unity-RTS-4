﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class UnitButtonController : MonoBehaviour
{
    public GameObject player;
    ResourceManager RM;
    UIController UI;
    public BuildingController building;
    public bool isPlaceable;

    //Buttons
    public Button buttonOne, buttonTwo, buttonThree, buttonFour, buttonFive, buttonSix, buttonSeven, buttonEight, basicBuildings, advancedBuildings, basicBack, advancedBack;

    //Audio
    public AudioSource playerAudio;
    public AudioClip constructingBuilding;

    //Buildings
    public GameObject house;
    public GameObject townHall;
    public GameObject barracks;
    public GameObject fort;
    public GameObject farm;
    public GameObject blacksmith;
    public GameObject lumberMill;
    public GameObject stables;

    private Vector3 mousePosition;

    private GameObject currentPlaceableObject;
    public Material placing;
    public Renderer[] childColors;

    private MeshRenderer[] meshes;
    Color[] colors;
    Color color;
    private Vector3 currentLocation;

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        RM = player.GetComponent<ResourceManager>();
        UI = player.GetComponent<UIController>();

        //Calls the TaskOnClick/TaskWithParameters/ButtonClicked method when you click the Button

        basicBack.onClick.AddListener(UI.OpenVillagerPanels);
        advancedBack.onClick.AddListener(UI.OpenVillagerPanels);

        basicBuildings.onClick.AddListener(UI.OpenBasicBuildingsPanel);
        advancedBuildings.onClick.AddListener(UI.OpenAdvancedBuildingsPanel);

        buttonTwo.onClick.AddListener(BuildHouse);
        buttonThree.onClick.AddListener(BuildFarm);
        buttonFour.onClick.AddListener(BuildTownHall);

        buttonFive.onClick.AddListener(BuildBlacksmith);
        buttonSix.onClick.AddListener(BuildLumberMill);
        buttonSeven.onClick.AddListener(BuildStables);
        buttonEight.onClick.AddListener(BuildBarracks);
    }


    // Update is called once per frame
    void Update()
    {
        if(RM.townHallCount == 0) {
            buttonFive.interactable = false; 
            buttonSix.interactable = false;
            buttonSeven.interactable = false;
            buttonEight.interactable = false;
        } else if (RM.townHallCount > 0) {
            buttonFive.interactable = true; 
            buttonSix.interactable = true;
            buttonSeven.interactable = true;
            buttonEight.interactable = true;
        }
        
        if (currentPlaceableObject != null)
        {
            if (!EventSystem.current.IsPointerOverGameObject(-1))
            {
                building = currentPlaceableObject.GetComponent<BuildingController>();
                isPlaceable = building.placeable;
                MoveCurrentPlaceableObjectToMouse();
                if (isPlaceable == true)
                {
                    ReleaseIfClicked();
                }
            }

            if (isPlaceable == false)
            {
                foreach (MeshRenderer mesh in meshes)
                {
                    mesh.material.SetColor("_Color", Color.red);
                }
            }
            else
            {
                int colorIter = 0;
                foreach (MeshRenderer mesh in meshes)
                {
                    mesh.material.SetColor("_Color", colors[colorIter]);
                    colorIter += 1;
                }
                colorIter = 0;
            }

            if (Input.GetKeyDown(KeyCode.Escape))
            {
                Destroy(currentPlaceableObject);
            }
        }
    }

    void BuildHouse()
    {
        //Output this to console when Button1 or Button3 is clicked
        if (currentPlaceableObject == null && RM.gold >= 200 && RM.Wood >= 200)
        {
            currentPlaceableObject = Instantiate(house);
            meshes = currentPlaceableObject.GetComponentsInChildren<MeshRenderer>();

            int iter = 0;
            int colorNum = meshes.Length;
            colors = new Color[colorNum];

            foreach (MeshRenderer mesh in meshes)
            {
                color = mesh.material.GetColor("_Color");
                colors[iter] = color;
                iter += 1;
            }
            iter = 0;
        }
        else if (currentPlaceableObject == null && RM.gold < 200 || currentPlaceableObject == null && RM.Wood < 200)
        {
            UI.noResourcesText.SetActive(true);
            StartCoroutine(Wait());
        }
        else
        {
            Destroy(currentPlaceableObject);
        }
    }

    void BuildFarm()
    {
        //Output this to console when the Button2 is clicked
        if (currentPlaceableObject == null && RM.gold >= 400 && RM.Wood >= 400)
        {
            currentPlaceableObject = Instantiate(farm);
            meshes = currentPlaceableObject.GetComponentsInChildren<MeshRenderer>();

            BuildingController buildingScript = currentPlaceableObject.GetComponent<BuildingController>();
            BoxCollider boxCollider = buildingScript.GetComponent<BoxCollider>();
            boxCollider.isTrigger = true;

            int iter = 0;
            int colorNum = meshes.Length;
            colors = new Color[colorNum];

            foreach (MeshRenderer mesh in meshes)
            {
                color = mesh.material.GetColor("_Color");
                colors[iter] = color;
                iter += 1;
            }
            iter = 0;
        }
        else if (currentPlaceableObject == null && RM.gold < 400 || currentPlaceableObject == null && RM.Wood < 400)
        {
            UI.noResourcesText.SetActive(true);
            StartCoroutine(Wait());
        }
        else
        {
            Destroy(currentPlaceableObject);
        }
    }

    void BuildTownHall()
    {
        //Output this to console when the Button2 is clicked
        if (currentPlaceableObject == null && RM.gold >= 800 && RM.Wood >= 800)
        {
            currentPlaceableObject = Instantiate(townHall);
            meshes = currentPlaceableObject.GetComponentsInChildren<MeshRenderer>();

            BuildingController buildingScript = currentPlaceableObject.GetComponent<BuildingController>();
            BoxCollider boxCollider = buildingScript.GetComponent<BoxCollider>();
            boxCollider.isTrigger = true;

            int iter = 0;
            int colorNum = meshes.Length;
            colors = new Color[colorNum];

            foreach (MeshRenderer mesh in meshes)
            {
                color = mesh.material.GetColor("_Color");
                colors[iter] = color;
                iter += 1;
            }
            iter = 0;
        }
        else if (currentPlaceableObject == null && RM.gold < 800 || currentPlaceableObject == null && RM.Wood < 800)
        {
            UI.noResourcesText.SetActive(true);
            StartCoroutine(Wait());
        }
        else
        {
            Destroy(currentPlaceableObject);
        }
    }

    void BuildLumberMill()
    {
        //Output this to console when the Button2 is clicked
        if (currentPlaceableObject == null && RM.gold >= 500 && RM.Wood >= 500 && RM.townHallCount > 0)
        {
            currentPlaceableObject = Instantiate(lumberMill);
            meshes = currentPlaceableObject.GetComponentsInChildren<MeshRenderer>();

            int iter = 0;
            int colorNum = meshes.Length;
            colors = new Color[colorNum];

            foreach (MeshRenderer mesh in meshes)
            {
                color = mesh.material.GetColor("_Color");
                colors[iter] = color;
                iter += 1;
            }
            iter = 0;
        }
        else if (currentPlaceableObject == null && RM.gold < 500 || currentPlaceableObject == null && RM.Wood < 500 || RM.townHallCount < 1)
        {
            UI.noResourcesText.SetActive(true);
            StartCoroutine(Wait());
        }
        else
        {
            Destroy(currentPlaceableObject);
        }
    }

    void BuildStables()
    {
        //Output this to console when the Button2 is clicked
        if (currentPlaceableObject == null && RM.gold >= 500 && RM.Wood >= 700 & RM.townHallCount > 0)
        {
            currentPlaceableObject = Instantiate(stables);
            meshes = currentPlaceableObject.GetComponentsInChildren<MeshRenderer>();

            int iter = 0;
            int colorNum = meshes.Length;
            colors = new Color[colorNum];

            foreach (MeshRenderer mesh in meshes)
            {
                color = mesh.material.GetColor("_Color");
                colors[iter] = color;
                iter += 1;
            }
            iter = 0;
        }
        else if (currentPlaceableObject == null && RM.gold < 500 || currentPlaceableObject == null && RM.Wood < 700 || RM.townHallCount < 1)
        {
            UI.noResourcesText.SetActive(true);
            StartCoroutine(Wait());
        }
        else
        {
            Destroy(currentPlaceableObject);
        }
    }

    void BuildBarracks()
    {
        //Output this to console when the Button2 is clicked
        if (currentPlaceableObject == null && RM.gold >= 600 && RM.Wood >= 600 && RM.townHallCount > 0)
        {
            currentPlaceableObject = Instantiate(barracks);
            meshes = currentPlaceableObject.GetComponentsInChildren<MeshRenderer>();
            
            int iter = 0;
            int colorNum = meshes.Length;
            colors = new Color[colorNum];

            foreach (MeshRenderer mesh in meshes)
            {
                color = mesh.material.GetColor("_Color");
                colors[iter] = color;
                iter += 1;
            }
            iter = 0;
        }
        else if (currentPlaceableObject == null && RM.gold < 600 || currentPlaceableObject == null && RM.Wood < 600 || RM.townHallCount < 1)
        {
            UI.noResourcesText.SetActive(true);
            StartCoroutine(Wait());
        }
        else
        {
            Destroy(currentPlaceableObject);
        }
    }

    void BuildFort()
    {
        //Output this to console when the Button2 is clicked
        if (currentPlaceableObject == null && RM.gold >= 1400 && RM.Wood >= 1400 && RM.stone >= 1400)
        {
            currentPlaceableObject = Instantiate(fort);
            meshes = currentPlaceableObject.GetComponentsInChildren<MeshRenderer>();

            int iter = 0;
            int colorNum = meshes.Length;
            colors = new Color[colorNum];

            foreach (MeshRenderer mesh in meshes)
            {
                color = mesh.material.GetColor("_Color");
                colors[iter] = color;
                iter += 1;
            }
            iter = 0;
        }
        else if (currentPlaceableObject == null && RM.gold < 1400 || currentPlaceableObject == null && RM.Wood < 1400 || currentPlaceableObject == null && RM.stone < 1400)
        {
            UI.noResourcesText.SetActive(true);
            StartCoroutine(Wait());
        }
        else
        {
            Destroy(currentPlaceableObject);
        }
    }

    void BuildBlacksmith()
    {
        //Output this to console when the Button2 is clicked
        if (currentPlaceableObject == null && RM.gold >= 800 && RM.Wood >= 800 && RM.townHallCount > 0)
        {
            currentPlaceableObject = Instantiate(blacksmith);
            meshes = currentPlaceableObject.GetComponentsInChildren<MeshRenderer>();

            int iter = 0;
            int colorNum = meshes.Length;
            colors = new Color[colorNum];

            foreach (MeshRenderer mesh in meshes)
            {
                color = mesh.material.GetColor("_Color");
                colors[iter] = color;
                iter += 1;
            }
            iter = 0;
        }
        else if (currentPlaceableObject == null && RM.gold < 800 || currentPlaceableObject == null && RM.Wood < 800 || RM.townHallCount < 1)
        {
            UI.noResourcesText.SetActive(true);
            StartCoroutine(Wait());
        }
        else
        {
            Destroy(currentPlaceableObject);
        }
    }
    
    private void MoveCurrentPlaceableObjectToMouse()
    {
        building.isPlaced = false;


        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hitInfo;
        if (Physics.Raycast(ray, out hitInfo))
        {
            if (building.unitType == "House")
            {
                //HERE~!
                currentPlaceableObject.transform.position = new Vector3(hitInfo.point.x, hitInfo.point.y + 1.2f, hitInfo.point.z);
                currentLocation = new Vector3 (hitInfo.point.x, currentPlaceableObject.transform.position.y - 1.2f, hitInfo.point.z);
            }
            else if (building.unitType == "Farm")
            {
                currentPlaceableObject.transform.position = new Vector3(hitInfo.point.x + 0.4f, hitInfo.point.y, hitInfo.point.z + 0.4f);
                currentLocation = currentPlaceableObject.transform.position;
            }
            else if (building.unitType == "Town Hall")
            {
                currentPlaceableObject.transform.position = new Vector3(hitInfo.point.x, hitInfo.point.y, hitInfo.point.z);
                Vector3 newLocation = new Vector3(currentPlaceableObject.transform.position.x - 3.0f, currentPlaceableObject.transform.position.y, currentPlaceableObject.transform.position.z - 1.0f);
                currentLocation = newLocation;
            }
            else if (building.unitType == "Lumber Yard")
            {
                currentPlaceableObject.transform.position = new Vector3(hitInfo.point.x, hitInfo.point.y, hitInfo.point.z);
                currentLocation = currentPlaceableObject.transform.position;
            }
            else if (building.unitType == "Stables")
            {
                currentPlaceableObject.transform.position = new Vector3(hitInfo.point.x, hitInfo.point.y, hitInfo.point.z);
                Vector3 newLocation = new Vector3(currentPlaceableObject.transform.position.x, currentPlaceableObject.transform.position.y, currentPlaceableObject.transform.position.z +2.0f);
                currentLocation = newLocation;
            }
            else if (building.unitType == "Barracks")
            {
                currentPlaceableObject.transform.position = new Vector3(hitInfo.point.x, hitInfo.point.y, hitInfo.point.z);
                currentLocation = currentPlaceableObject.transform.position;
            }
            else if (building.unitType == "Fort")
            {
                currentPlaceableObject.transform.position = new Vector3(hitInfo.point.x, hitInfo.point.y, hitInfo.point.z);
                currentLocation = currentPlaceableObject.transform.position;
            }
            else if (building.unitType == "Blacksmith")
            {
                currentPlaceableObject.transform.position = new Vector3(hitInfo.point.x, hitInfo.point.y, hitInfo.point.z);
                Vector3 newLocation = new Vector3(currentPlaceableObject.transform.position.x + 1.2f, currentPlaceableObject.transform.position.y, currentPlaceableObject.transform.position.z - 2.0f);
                currentLocation = newLocation;
            }
            else if (building.unitType == "Resource")
            {
                currentPlaceableObject.transform.position = new Vector3(hitInfo.point.x, hitInfo.point.y, hitInfo.point.z);
                currentLocation = currentPlaceableObject.transform.position;
            }

            currentPlaceableObject.transform.rotation = Quaternion.FromToRotation(Vector3.up, hitInfo.normal);
            if(building.unitType == "Stables" || building.unitType == "Barracks")
            {
                currentPlaceableObject.transform.Rotate(0, 270, 0);
            }

            if (currentPlaceableObject.transform.rotation.x >= 0.2f || currentPlaceableObject.transform.rotation.x <= -0.2f || currentPlaceableObject.transform.rotation.z >= 0.2f || 
                currentPlaceableObject.transform.rotation.z <= -0.2f )
            {
                building.placeable = false;
            } else
            {
                if(building.inCollider == false)
                {
                    building.placeable = true;
                }
            }
        }
    }

    private void ReleaseIfClicked()
    {
        if (Input.GetMouseButtonDown(0) && building.unitType == "House")
        {
            RM.gold -= 200;
            RM.Wood -= 200;
            building.isPlaced = true;
            currentPlaceableObject.layer = 11;
            Destroy(currentPlaceableObject);
            currentPlaceableObject = Instantiate(building.foundation);
            currentPlaceableObject.transform.position = currentLocation;
            currentPlaceableObject = null;
            PlayBuildingSound();
        }
        else if (Input.GetMouseButtonDown(0) && building.unitType == "Farm")
        {
            RM.gold -= 400;
            RM.Wood -= 400;
            building.isPlaced = true;
            currentPlaceableObject.layer = 11;
            Destroy(currentPlaceableObject);
            currentPlaceableObject = Instantiate(building.foundation);
            currentPlaceableObject.transform.position = currentLocation;
            currentPlaceableObject = null;
            PlayBuildingSound();
        }
        else if (Input.GetMouseButtonDown(0) && building.unitType == "Town Hall")
        {
            RM.gold -= 800;
            RM.Wood -= 800;
            building.isPlaced = true;
            currentPlaceableObject.layer = 11;
            Destroy(currentPlaceableObject);
            currentPlaceableObject = Instantiate(building.foundation);
            currentPlaceableObject.transform.position = currentLocation;
            currentPlaceableObject = null;
            PlayBuildingSound();
        }
        else if (Input.GetMouseButtonDown(0) && building.unitType == "Lumber Yard")
        {
            RM.gold -= 500;
            RM.Wood -= 500;
            building.isPlaced = true;
            currentPlaceableObject.layer = 11;
            Destroy(currentPlaceableObject);
            currentPlaceableObject = Instantiate(building.foundation);
            currentPlaceableObject.transform.position = currentLocation;
            currentPlaceableObject = null;
            PlayBuildingSound();
        }
        else if (Input.GetMouseButtonDown(0) && building.unitType == "Stables")
        {
            RM.gold -= 500;
            RM.Wood -= 700;
            building.isPlaced = true;
            currentPlaceableObject.layer = 11;
            Destroy(currentPlaceableObject);
            currentPlaceableObject = Instantiate(building.foundation);
            currentPlaceableObject.transform.position = currentLocation;
            currentPlaceableObject = null;
            PlayBuildingSound();
        }
        else if (Input.GetMouseButtonDown(0) && building.unitType == "Barracks")
        {
            RM.gold -= 600;
            RM.Wood -= 600;
            building.isPlaced = true;
            currentPlaceableObject.layer = 11;
            Destroy(currentPlaceableObject);
            currentPlaceableObject = Instantiate(building.foundation);
            currentPlaceableObject.transform.position = currentLocation;
            currentPlaceableObject = null;
            PlayBuildingSound();
        }
        else if (Input.GetMouseButtonDown(0) && building.unitType == "Fort")
        {
            RM.gold -= 1400;
            RM.Wood -= 1400;
            RM.stone -= 1400;
            building.isPlaced = true;
            currentPlaceableObject.layer = 11;
            Destroy(currentPlaceableObject);
            currentPlaceableObject = Instantiate(building.foundation);
            currentPlaceableObject.transform.position = currentLocation;
            currentPlaceableObject = null;
            PlayBuildingSound();
        }
        else if (Input.GetMouseButtonDown(0) && building.unitType == "Resource")
        {
            RM.gold -= 200;
            RM.Wood -= 400;
            building.isPlaced = true;
            currentPlaceableObject.layer = 11;
            Destroy(currentPlaceableObject);
            currentPlaceableObject = Instantiate(building.foundation);
            currentPlaceableObject.transform.position = currentLocation;
            currentPlaceableObject = null;
            PlayBuildingSound();
        }
        else if (Input.GetMouseButtonDown(0) && building.unitType == "Blacksmith")
        {
            RM.gold -= 800;
            RM.Wood -= 800;
            building.isPlaced = true;
            currentPlaceableObject.layer = 11;
            Destroy(currentPlaceableObject);
            currentPlaceableObject = Instantiate(building.foundation);
            currentPlaceableObject.transform.position = currentLocation;
            currentPlaceableObject = null;
            PlayBuildingSound();
        }
        else if (Input.GetMouseButtonDown(0))
        {
            currentPlaceableObject = null;
        }
        
    }

    private void PlayBuildingSound()
    {
        playerAudio.clip = constructingBuilding;
        playerAudio.Play();
    }

    IEnumerator Wait()
    {
        yield return new WaitForSeconds(3);
        UI.noResourcesText.SetActive(false);
        //my code here after 3 seconds
    }

    IEnumerator PlaceBuilding()
    {
        yield return new WaitForSeconds(3);
        //my code here after 3 seconds
        PlayBuildingSound();
    }
}
