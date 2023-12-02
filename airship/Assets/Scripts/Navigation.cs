using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Navigation : MonoBehaviour
{
    //ranges from -100 to 100
    public int xPos;
    //ranges from -100 to 100
    public int zPos;

    //ranges from 0 to 50
    public int yPos;

    public int theta;

    [SerializeField] GameObject rotation;

    // Start is called before the first frame update
    void Start()
    {
        xPos = 0;
        zPos = 0;
        yPos = 25;
        theta = 0;
    }

    // Update is called once per frame
    void Update()
    {
        if (Time.frameCount % 1800 == 0) yPos--;
    }

    public void increaseY() {
        if (yPos < 50) yPos++;
    }

    public void decreaseY() {
        if (yPos > 0) yPos--;
    }

    public void turnLeft() {
        theta--;
        rotation.transform.Rotate(0.0f, 0.0f, -1.0f, Space.Self);
    }

    public void turnRight() {
        theta++;
        rotation.transform.Rotate(0.0f, 0.0f, 1.0f, Space.Self);
    }
}
