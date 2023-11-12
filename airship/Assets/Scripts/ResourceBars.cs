/* 
Resource Bars Class
For the Airship Game
By Bongo Cat (aka Miles Soto Aguayo)
*/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// This class is attached to the non-interactive bars that display resource values
public class ResourceBars : MonoBehaviour
{
    // public GameObject bar;
    private Scrollbar bar;
    public int mode;

    // Get the Scrollbar component from the parent object
    private void Awake() {
        bar = GetComponentInParent<Scrollbar>();
    }

    // Update is called once per frame
    void Update() {
        // Update the Scrollbar value based on the resource value
        bar.value = ResourceManager.Instance.FetchValue(mode) / 100.0f;
    }
}
