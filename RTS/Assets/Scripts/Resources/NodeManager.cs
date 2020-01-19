﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NodeManager : MonoBehaviour
{
    public enum ResourceTypes { Skymetal, Iron, Steel, Stone, Wood, Food, Gold, Housing }
    public ResourceTypes resourceType;
    InputManager IM;
    ResearchController RC;

    private Selection selectscript;
    public List<Collider> collidedObjects = new List<Collider>();

    // Time villager is at node
    public float harvestTime;
    public float availableResource;

    // indicates when resource is being gathered
    public int gatherers;


    // Start is called before the first frame update
    void Start()
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        RC = player.GetComponent<ResearchController>();
        // Starts the resource tick (means its true)
        StartCoroutine(ResourceTick());   
    }

    // Update is called once per frame
    void Update()
    {
        var numberOfColliders = collidedObjects.Count; // this should give you the number you need
        if(numberOfColliders > 0)
        {

        }
        if (availableResource <= 0)
        {
            // Need to add isGathering = false
            foreach(Collider collidedObject in collidedObjects)
            {
                UnitController unit = collidedObject.gameObject.GetComponent<UnitController>();
                Selection unitSelection = collidedObject.gameObject.GetComponent<Selection>();
                if(unit) {
                    if(unit.unitType == "Worker") {
                        unitSelection.isGathering = false;
                    }
                }
            }
            Destroy(gameObject);
        }
    }

    // Ticks down while villager is gathering resource - Adjust with heldResource in GatherTick in Selection Script
    public void ResourceGather()
    {
        int toolModifier;
        if(RC.artisanToolSmithing) {
            toolModifier = 3;
        } else if (RC.basicToolSmithing) {
            toolModifier = 2;
        } else {
            toolModifier = 1;
        }
        if(collidedObjects.Count != 0)
        {
            availableResource -= collidedObjects.Count * 5 * toolModifier;
        }
    }

    void OnCollisionEnter(Collision col)
    {
        selectscript = col.gameObject.GetComponent<Selection>();
        if (!collidedObjects.Contains(col.collider) && col.collider.tag == "Selectable" && selectscript.isGathering == true)
        {
            collidedObjects.Add(col.collider);
        }
    }

    void OnCollisionStay(Collision col)
    {
        OnCollisionEnter(col); //same as enter
    }

    void OnCollisionExit(Collision col)
    {
        if (collidedObjects.Contains(col.collider) && col.collider.tag == "Selectable")
        {
            collidedObjects.Remove(col.collider);
        }
    }

    IEnumerator ResourceTick()
    {
        while (true)
        {
            yield return new WaitForSeconds(harvestTime);
            ResourceGather();
        }
    }
}
