using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParallaxBackground : MonoBehaviour
{
    private GameObject cam;
    private float xPosition;
    private float length;

    [SerializeField] private float parallaxEffect;


    void Start()
    {
        cam = GameObject.Find("Main Camera");

        xPosition = transform.position.x;

        length = GetComponent<SpriteRenderer>().bounds.size.x;
    }

    void Update()
    {
        float distanceToMove = cam.transform.position.x * parallaxEffect;
        float distanceMoved = cam.transform.position.x * (1 - parallaxEffect);

        transform.position = new Vector3(xPosition + distanceToMove, transform.position.y);

        if (distanceMoved > xPosition + length)
            xPosition += length;
        else if (distanceMoved < xPosition - length)
            xPosition -= length;
    }
}
