using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitButton : MonoBehaviour
{
    [SerializeField] GameObject player1;
    [SerializeField] GameObject player2;
    public string dir;

    private Vector3 position;

    // Start is called before the first frame update
    void Start()
    {
        position = this.transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        if (collidePlayer()) {
            this.GetComponent<SpriteRenderer>().color = Color.red;
            switch(dir) {
                case "up": 
                    up();
                    break;
                case "down":
                    down();
                    break;
                case "left":
                    left();
                    break;
                case "right":
                    right();
                    break;
            }
        } else {
            this.GetComponent<SpriteRenderer>().color = Color.white;
        }
    }

    bool collidePlayer() {
        float dist1 = Vector3.Distance(position, player1.transform.position);
        float dist2 = Vector3.Distance(position, player2.transform.position);

        if (dist1 < 1 || dist2 < 1) {
            return true;
        }
        return false;
    }

    void up() {

    }

    void down() {

    }

    void left() {

    }

    void right() {

    }
}
