using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class ParrallaxBackground : MonoBehaviour
{

    [SerializeField] private new GameObject camera;
    [SerializeField] private float parallaxEffect;
    private float firstXPosition;
    private float xPosition;
    private float length;

    private void Start()
    {
        camera = GameObject.Find("Main Camera");
        length = GetComponent<SpriteRenderer>().bounds.size.x;
        xPosition = transform.position.x;
        firstXPosition = transform.position.x;
    }

    private void Update()
    {
        float distanceMoved = camera.transform.position.x * (1 - parallaxEffect);
        float distanceToMove = camera.transform.position.x * parallaxEffect;
        transform.position = new Vector3(xPosition + distanceToMove, transform.position.y );

        if (distanceMoved + firstXPosition > xPosition + length)
        {
            xPosition += length;
        }
        else if (distanceMoved + firstXPosition < xPosition - length) {
            xPosition -= length;
        }
    }
}
