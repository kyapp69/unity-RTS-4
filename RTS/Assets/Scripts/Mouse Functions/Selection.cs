﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.EventSystems;

public class Selection : MonoBehaviour
{
     // Set Resource
    public NodeManager.ResourceTypes heldResourceType;
        
    public Tasklist task;
    private ResourceManager RM;
    private AudioSource peasantAudio;
    public AudioClip peasantMoveClip;

    // Player 
    public GameObject player;

    // Grab nodemanager script and harvest speed on gameobject
    FoundationController buildScript;
    float buildSpeed;
    NodeManager harvestScript;
    float harvestSpeed;

    // villager target node
    public GameObject targetNode;

    // selection variables
    public bool selected = false;

    // shows if villager is gathering
    public bool isGathering = false;
    public bool isBuilding = false;

    private NavMeshAgent agent;

    // Number of held resources
    public int heldResource;
    public int maxHeldResource;

    public GameObject[] drops;

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        RM = player.GetComponent<ResourceManager>();
        StartCoroutine(GatherTick());
        agent = GetComponent<NavMeshAgent>();
    }

    // Update is called once per frame
    void Update()
    {
        // if target node is destroyed
        if (targetNode == null)
        {
            isGathering = false;
            if (heldResource != 0)
            {
                //stop gathering immediately
                //Drop off point here for resource yards
                drops = GameObject.FindGameObjectsWithTag("Yard");
                if(drops.Length > 0)
                {
                    agent.destination = GetClosestDropOff(drops).transform.position;
                    drops = null;
                    task = Tasklist.Delivering;
                } else
                {
                    task = Tasklist.Idle;
                }
            } else
            {
                //task = Tasklist.Idle;
            }
        } else
        {
            if (heldResource >= maxHeldResource && targetNode.tag == "Resource")
            {
                //stop gathering immediately
                isGathering = false;
                //Drop off point here for resource yards
                drops = GameObject.FindGameObjectsWithTag("Yard");
                if (drops.Length > 0)
                {
                    agent.destination = GetClosestDropOff(drops).transform.position;
                    drops = null;
                    task = Tasklist.Delivering;
                }
                else
                {
                    task = Tasklist.Idle;
                }
            }
        }

        
        if (Input.GetMouseButtonDown(1) && selected == true)
        {
            if (!EventSystem.current.IsPointerOverGameObject(-1))
            {
                RightClick();
            }
        }
    }

    // Find the closest dropoff after gathering and go there
    GameObject GetClosestDropOff(GameObject[] dropOffs)
    {
        GameObject closestDrop = null;
        float closestDistance = Mathf.Infinity;
        Vector3 position = transform.position;

        foreach(GameObject targetDrop in dropOffs)
        {
            Vector3 direction = targetDrop.transform.position - position;
            float distance = direction.sqrMagnitude;
            if(distance < closestDistance)
            {
                closestDistance = distance;
                closestDrop = targetDrop;
            }
        }
        return closestDrop;
    }

    // Right click function
    public void RightClick()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, 350))
        {
            targetNode = hit.collider.gameObject;
            if (hit.collider.tag != "Yard")
            {
                if (hit.collider.tag == "Ground")
                {
                    task = Tasklist.Moving;
                    agent.destination = hit.point;

                    Debug.Log("Moving, Sir!");
                }
                else if (hit.collider.tag == "Resource")
                {
                    task = Tasklist.Gathering;
                    agent.destination = hit.collider.gameObject.transform.position;
                    Debug.Log("Harvesting, Sir!");
                    targetNode = hit.collider.gameObject;
                }
                else if (hit.collider.tag == "Foundation")
                {
                    task = Tasklist.Building;
                    agent.destination = hit.collider.gameObject.transform.position;
                    Debug.Log("Building, Sir!");
                    targetNode = hit.collider.gameObject;
                }
                else if (hit.collider.tag == "Doorway")
                {
                    Debug.Log("Smashing down that door, Sir!");
                }
            }
            else if (hit.collider.tag == "Yard")
            {
                agent.destination = hit.collider.gameObject.transform.position;
                targetNode = hit.collider.gameObject;
                task = Tasklist.Delivering;
                Debug.Log("Dropping off resources, Sir!");
            }
            peasantAudio = agent.GetComponent<AudioSource>();
            peasantAudio.clip = peasantMoveClip;
            peasantAudio.Play();
        }
    }

    public void OnTriggerStay(Collider other)
    {
        GameObject hitObject = other.gameObject;

        if (hitObject.tag == "Yard" && task == Tasklist.Delivering && heldResource != 0) 
        {
            //if (heldResourceType == Skymetal)
            //{
            //    DropSkyMetal();
            //}

            //if (heldResourceType == NodeManager.ResourceTypes.Wood)
            //{
            //    DropWood();
            //}
        }
    }

    // Allows collider of resources 
    public void OnTriggerEnter(Collider other)
    {
        GameObject hitObject = other.gameObject;
        // add  (&& task == Tasklist.Gathering) once u know whats up
        if (hitObject.tag == "Resource" && hitObject.gameObject == targetNode)
        {
            isGathering = true;
           // hitObject.GetComponent<NodeManager>().gatherers++;
            harvestScript = targetNode.GetComponent<NodeManager>();
            harvestSpeed = harvestScript.harvestTime;

            heldResourceType = hitObject.GetComponent<NodeManager>().resourceType;
        } else if (hitObject.tag == "Foundation" && hitObject.gameObject == targetNode)
        {
            isBuilding = true;
            hitObject.GetComponent<FoundationController>().builders++;
            buildScript = targetNode.GetComponent<FoundationController>();
            buildSpeed = buildScript.buildTime;
        }
        else if(hitObject.tag == "Yard" && task == Tasklist.Delivering)
        {
            if (heldResourceType == NodeManager.ResourceTypes.Skymetal)
            {
                DropSkyMetal();
            }

            if (heldResourceType == NodeManager.ResourceTypes.Wood)
            {
                DropWood();
            }

            if (heldResourceType == NodeManager.ResourceTypes.Iron)
            {
                DropIron();
            }

            if (heldResourceType == NodeManager.ResourceTypes.Stone)
            {
                DropStone();
            }

            if (heldResourceType == NodeManager.ResourceTypes.Gold)
            {
                DropGold();
            }

            if (heldResourceType == NodeManager.ResourceTypes.Food)
            {
                DropFood();
            }
        }
    }

    public void DropSkyMetal()
    {
        Debug.Log("Skymetal");
        //Handle drop off!
        if (RM.skymetal >= RM.maxSkymetal)
        {
            task = Tasklist.Idle;
        }
        else
        {
            // if target node is destroyed
            if (targetNode == null)
            {
                isGathering = false;
                if (heldResource != 0)
                {
                    //stop gathering immediately
                    //Drop off point here for resource yards
                   
                    drops = GameObject.FindGameObjectsWithTag("Yard");
                    agent.destination = GetClosestDropOff(drops).transform.position;

                    RM.skymetal += heldResource;
                    task = Tasklist.Delivering;
                    heldResource = 0;
                    drops = null;
                }
                else
                {
                    task = Tasklist.Idle;
                }
            }
            else
            {
                RM.skymetal += heldResource;
                heldResource = 0;
                task = Tasklist.Gathering;
                agent.destination = targetNode.transform.position;
            }
        }
    }

    public void DropWood()
    {
        //Handle drop off!
        if (RM.Wood >= RM.maxWood)
        {
            task = Tasklist.Idle;
        }
        else
        {
            // if target node is destroyed
            if (targetNode == null)
            {
                isGathering = false;
                if (heldResource != 0)
                {
                    //stop gathering immediately
                    //Drop off point here for resource yards
                    drops = GameObject.FindGameObjectsWithTag("Yard");
                    agent.destination = GetClosestDropOff(drops).transform.position;

                    RM.Wood += heldResource;
                    heldResource = 0;
                    task = Tasklist.Delivering;
                    drops = null;
                }
                else
                {
                    task = Tasklist.Idle;
                }
            }
            else
            {
                RM.Wood += heldResource;
                heldResource = 0;
                task = Tasklist.Gathering;
                agent.destination = targetNode.transform.position;
            }
        }
    }

    public void DropIron()
    {
        Debug.Log("Iron");
        //Handle drop off!
        if (RM.iron >= RM.maxIron)
        {
            task = Tasklist.Idle;
        }
        else
        {
            // if target node is destroyed
            if (targetNode == null)
            {
                isGathering = false;
                if (heldResource != 0)
                {
                    //stop gathering immediately
                    //Drop off point here for resource yards
                    drops = GameObject.FindGameObjectsWithTag("Yard");
                    agent.destination = GetClosestDropOff(drops).transform.position;

                    RM.iron += heldResource;
                    task = Tasklist.Delivering;
                    heldResource = 0;
                    drops = null;
                }
                else
                {
                    task = Tasklist.Idle;
                }
            }
            else
            {
                RM.iron += heldResource;
                heldResource = 0;
                task = Tasklist.Gathering;
                agent.destination = targetNode.transform.position;
            }
        }
    }

    public void DropStone()
    {
        Debug.Log("Stone");
        //Handle drop off!
        if (RM.stone >= RM.maxStone)
        {
            task = Tasklist.Idle;
        }
        else
        {
            // if target node is destroyed
            if (targetNode == null)
            {
                isGathering = false;
                if (heldResource != 0)
                {
                    //stop gathering immediately
                    //Drop off point here for resource yards
                    drops = GameObject.FindGameObjectsWithTag("Yard");
                    agent.destination = GetClosestDropOff(drops).transform.position;

                    RM.stone += heldResource;
                    task = Tasklist.Delivering;
                    heldResource = 0;
                    drops = null;
                }
                else
                {
                    task = Tasklist.Idle;
                }
            }
            else
            {
                RM.stone += heldResource;
                heldResource = 0;
                task = Tasklist.Gathering;
                agent.destination = targetNode.transform.position;
            }
        }
    }

    public void DropGold()
    {
        Debug.Log("Gold");
        //Handle drop off!
        if (RM.gold >= RM.maxGold)
        {
            task = Tasklist.Idle;
        }
        else
        {
            // if target node is destroyed
            if (targetNode == null)
            {
                isGathering = false;
                if (heldResource != 0)
                {
                    //stop gathering immediately
                    //Drop off point here for resource yards
                    drops = GameObject.FindGameObjectsWithTag("Yard");
                    agent.destination = GetClosestDropOff(drops).transform.position;

                    RM.gold += heldResource;
                    task = Tasklist.Delivering;
                    heldResource = 0;
                    drops = null;
                }
                else
                {
                    task = Tasklist.Idle;
                }
            }
            else
            {
                RM.gold += heldResource;
                heldResource = 0;
                task = Tasklist.Gathering;
                agent.destination = targetNode.transform.position;
            }
        }
    }

    public void DropFood()
    {
        Debug.Log("Food");
        //Handle drop off!
        if (RM.food >= RM.maxFood)
        {
            task = Tasklist.Idle;
        }
        else
        {
            // if target node is destroyed
            if (targetNode == null)
            {
                isGathering = false;
                if (heldResource != 0)
                {
                    //stop gathering immediately
                    //Drop off point here for resource yards
                    drops = GameObject.FindGameObjectsWithTag("Yard");
                    agent.destination = GetClosestDropOff(drops).transform.position;

                    RM.food += heldResource;
                    task = Tasklist.Delivering;
                    heldResource = 0;
                    drops = null;
                }
                else
                {
                    task = Tasklist.Idle;
                }
            }
            else
            {
                RM.food += heldResource;
                heldResource = 0;
                task = Tasklist.Gathering;
                agent.destination = targetNode.transform.position;
            }
        }
    }

    public void OnTriggerExit(Collider other)
    {
        GameObject hitObject = other.gameObject;

        if (hitObject.tag == "Resource" && hitObject.gameObject == targetNode)
        {
            isGathering = false;
        } 
    }

    // Ticks down while villager is gathering - Adjust with heldResource in GatherTick in Selection Script
    IEnumerator GatherTick()
    {
        while(true)
        {

            yield return new WaitForSeconds(harvestSpeed);
            if(isGathering && heldResourceType == NodeManager.ResourceTypes.Skymetal)
            {
                heldResource += 5;
            } else if (isGathering && heldResourceType == NodeManager.ResourceTypes.Wood)
            {
                heldResource += 5;
            }
            else if (isGathering && heldResourceType == NodeManager.ResourceTypes.Iron)
            {
                heldResource += 5;
            }
            else if (isGathering && heldResourceType == NodeManager.ResourceTypes.Stone)
            {
                heldResource += 5;
            }
            else if (isGathering && heldResourceType == NodeManager.ResourceTypes.Gold)
            {
                heldResource += 5;
            }
            else if (isGathering && heldResourceType == NodeManager.ResourceTypes.Food)
            {
                heldResource += 5;
            }
            else if (isGathering)
            {
                heldResource += 5;
            }
        }
    }

}
