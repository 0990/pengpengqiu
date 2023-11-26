using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Logic : MonoBehaviour
{

    private EdgeCollider2D edgeCollider;
    private List<Vector2> points;

    void Awake()
    {
        edgeCollider = GetComponent<EdgeCollider2D>();
        points = new List<Vector2>();
    }

    void Update()
    {
        if (Input.GetMouseButton(0))
        {
            Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            if (!points.Contains(mousePosition))
            {
                points.Add(mousePosition);
                edgeCollider.points = points.ToArray();
            }
        }

        if (Input.GetMouseButtonUp(0))
        {
            GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Dynamic;
        }
    }
}
