using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    private Vector3 offset;
    public Transform Player;
    // Start is called before the first frame update
    void Start()
    {
        offset =  transform.position - Player.position;
    }

    // Update is called once per frame
    void LateUpdate()
    {
        transform.position = Player.position + offset;
    }
}
