using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//小球脚本，用于检测碰撞
public class Ball : MonoBehaviour
{

    public GameObject ground;
    public GameObject ball;
    // Start is called before the first frame update
    void Start()
    {
        EventManager.StartListening("BrushEnd", onBrushEnd);
    }

    private void OnDestroy()
    {
        EventManager.StopListening("BrushEnd", onBrushEnd);
    }

    private void onBrushEnd()
    {
        enableGravity();
    }

    private void enableGravity()
    {
        this.gameObject.GetComponent<Rigidbody2D>().gravityScale = 1;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        string tag = collision.gameObject.tag;
        Debug.Log("Collided with " + collision.gameObject.name+tag);

        switch (tag)
        {
            case "Ground":
                Physics2D.simulationMode = SimulationMode2D.Script;
                Debug.Log("Fail");
                EventManager.TriggerEvent("gamefail");
                break;
            case "Ball":
                Physics2D.simulationMode = SimulationMode2D.Script;
                Debug.Log("Success");
                EventManager.TriggerEvent("gamesuccess");
                break;
            default:
                break;
        }
    }
}
