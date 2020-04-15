using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    private Vector3 offset;
    public Transform PlayerHead;
    public Transform LookAtTransform;
    [SerializeField]private float CameraMoveRangeX;
    [SerializeField]private float CameraMoveRangeY;
    // Start is called before the first frame update
    void Start()
    {
        offset =  transform.position - PlayerHead.position;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        
        
        float CameraInputX = Input.GetAxis("CameraHorizontal");
        float CameraInputY = Input.GetAxis("CameraVertical");

        transform.LookAt(PlayerHead);
        // transform.position = PlayerHead.position + offset;

        //rotate camera based on controller input;
        //Vector3 LookRotation = new Vector3(CameraInputX*CameraMoveRangeX+transform.rotation.eulerAngles.x,CameraInputY*CameraMoveRangeY + transform.rotation.eulerAngles.y, 0 + transform.rotation.eulerAngles.z);
        //transform.rotation = Quaternion.Euler(LookRotation);
        //Vector3 LookRotation = new Vector3(CameraInputX * CameraMoveRangeX +PlayerHead.position.x, CameraInputY * CameraMoveRangeY+PlayerHead.position.y, PlayerHead.transform.position.z);


        //transform.RotateAround(PlayerHead,Vector3.up, );
        transform.position = offset+PlayerHead.position;
        
    }
}
