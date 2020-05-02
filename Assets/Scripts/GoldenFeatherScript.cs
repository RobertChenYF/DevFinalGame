using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoldenFeatherScript : MonoBehaviour
{
    [SerializeField]private float rotationSpeed;
    // Start is called before the first frame update

    public AudioSource aud_goldenFeather;

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        transform.Rotate(0, rotationSpeed*Time.deltaTime, 0, Space.Self);
    }

    void OnTriggerEnter(Collider col)
    {
        if(col.gameObject.tag == "Claire")
        {
            Debug.Log("Added Golden Feather");
            ClaireController.me.goldenFeathersMax++;
            aud_goldenFeather.pitch = Random.Range(0.8f, 1.2f);
            aud_goldenFeather.Play();
            Destroy(this.gameObject);
        }
    }
}
