/* 
Resource Button Class
For the Airship Game
By Bongo Cat (aka Miles Soto Aguayo)
*/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// This class is attached to the buttons that refuel, repair, and recharge the blimp
public class ResourceButton : MonoBehaviour {
    // Calls the Fuel method in the ResourceManager
    public void Refuel() {
        ResourceManager.Instance.Refuel(10.0f);
    }

    // Calls the Condition method in the ResourceManager
    public void Repair() {
        ResourceManager.Instance.Repair(5.0f);
    }    

    // Calls the Air method in the ResourceManager
    public void Recharge() {
        ResourceManager.Instance.Recharge(8.0f);
    }
}
