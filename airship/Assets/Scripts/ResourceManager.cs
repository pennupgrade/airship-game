using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// public class ResourceManager : MonoBehaviour
// {
//     // Start is called before the first frame update
//     void Start()
//     {
        
//     }

//     // Update is called once per frame
//     void Update()
//     {
        
//     }
// }

public class ResourceManager : MonoBehaviour
{
    private static ResourceManager instance;

    [SerializeField] private float fuel = 100.0f;
    [SerializeField] private float condition = 100.0f;
    [SerializeField] private float helium = 100.0f;

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
        ConsumeFuel(0.1f);
        Damage(0.05f);
        AdjustHelium(-0.1f);
    }

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

    public void AdjustHelium(float amount) {
        if (helium > 100.0f) {
            // Handle reaching maximum helium
            Debug.Log("Blimp has reached maximum helium!");
        } else if (helium < 0.0f) {
            // Handle helium depletion scenario
            Debug.Log("Blimp has run out of helium! Bye bye!");
        } else {
            helium += amount;
        }
    }
}
