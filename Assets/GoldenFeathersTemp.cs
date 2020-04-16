using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoldenFeathersTemp : MonoBehaviour
{

    public TextMesh tm;

    // Start is called before the first frame update
    void Start()
    {
        tm = GetComponent<TextMesh>();
    }

    // Update is called once per frame
    void Update()
    {
        tm.text = "" + ClaireController.me.goldenFeathers;
    }
}
