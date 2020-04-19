using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoldenFeatherScript : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnTriggerEnter(Collider col)
    {
        if(col.gameObject.tag == "Claire")
        {
            Debug.Log("Added Golden Feather");
            ClaireController.me.goldenFeathersMax++;
            Destroy(this.gameObject);
        }
    }
}
