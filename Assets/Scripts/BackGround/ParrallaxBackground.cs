using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParrallaxBackground : MonoBehaviour
{

    [SerializeField] private GameObject camera;
    [SerializeField] private float parallaxEffect;
    private float xPosition;

    private void Start()
    {
        camera = GameObject.Find("Main Camera");
        xPosition = transform.position.x;
    }

    private void Update()
    {
        float distanceToMove = camera.transform.position.x * parallaxEffect;
        transform.position = new Vector3(xPosition + distanceToMove, transform.position.y );
    }
}
