/* 
Resource Manager Class
For the Airship Game
By Bongo Cat (aka Miles Soto Aguayo)
*/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResourceManager : MonoBehaviour
{
    /* 
    Resource Manager is a Singleton class that manages the resources of the Blimp
    For this purpose the internal instance is private and static
    Just making a static class could be an alternative.
    (But that's cumbersome and the Unity Editor doesn't like it)
    Even I like this way more.
    */
    private static ResourceManager instance;

    // Resources available are fuel, condition, and helium (air)
    [SerializeField] private float fuel = 100.0f;
    [SerializeField] private float condition = 100.0f;
    [SerializeField] private float air = 100.0f;

    [SerializeField] private float fuelConsumptionRate = 1f;
    [SerializeField] private float conditionDegradationRate = 0.5f;
    [SerializeField] private float airConsumptionRate = 1f;

    // The Instance can be accessed from anywhere
    public static ResourceManager Instance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<ResourceManager>();
                if (instance == null)
                {
                    instance = new GameObject("ResourceManager").AddComponent<ResourceManager>();
                }
            }

            return instance;
        }
    }

    /// Awake is called when the script instance is being loaded.
    void Awake()
    {
        // If there is an instance, and it's not me, delete myself.    
        if (instance != null && instance != this) { 
            Destroy(this); 
        } 
        else { 
            instance = this; 
        } 
    }

    void Update()
    {
        // Update resource values based on gameplay mechanics
        ConsumeFuel(fuelConsumptionRate / 100.0f);
        Damage(conditionDegradationRate / 100.0f);
        Deplete(airConsumptionRate / 100.0f);
    }

    /* The following methods are called by the ResourceButtons or Interactive Stations 
    ConsumeFuel, Refuel, Damage, Repair, Recharge, and Deplete are all self-explanatory
    They are floats because the Bars work with floats
    And also because they are percentages
    */
    public void ConsumeFuel(float amount)
    {
        if (fuel < 0.0f) {
            // Handle fuel depletion scenario
            Debug.Log("Blimp ran out of fuel! Bye bye!");
        } else {
            fuel -= amount;
        }
    }

    public void Refuel(float amount)
    {
        if (fuel > 100.0f)
        {
            // Handle reaching maximum fuel
            Debug.Log("Blimp has been refueled!");
        } else {
            fuel += amount;
        }
    }

    public void Damage(float damage)
    {
        if (condition < 0.0f)
        {
            // Handle blimp destruction scenario
            Debug.Log("Blimp has been destroyed! <(o_o)>)");
        } else {
            condition -= damage;
        }
    }

    public void Repair(float amount)
    {
        if (condition > 100.0f)
        {
            // Handle reaching maximum condition
            Debug.Log("Blimp has been fixed!");
        } else {
            condition += amount;
        }
    }

    public void Recharge(float amount) {
        if (air > 100.0f) {
            // Handle reaching maximum helium
            Debug.Log("Blimp has reached maximum helium!");
        } else {
            air += amount;
        }
    }

    public void Deplete(float amount) {
        if (air < 0.0f) {
            // Handle helium depletion scenario
            Debug.Log("Blimp has run out of helium! Bye bye!");
        } else {
            air -= amount;
        }
    }

    public float FetchValue(int i) {
        switch (i) {
            case 0:
                return fuel;
            case 1:
                return condition;
            case 2: 
                return air;
            default:
                return 0.0f;
        }
    }
}
