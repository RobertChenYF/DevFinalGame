using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoldenFeathersUI : MonoBehaviour
{

    public GoldenFeather[] goldenFeathers;


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        for (int i = 0; i < goldenFeathers.Length; i++)
        {
            if(ClaireController.me.goldenFeathersMax > i)
            {
                goldenFeathers[i].dead.color = Color.white;
            }
            else
            {
                goldenFeathers[i].dead.color = Color.clear;
            }

            if (ClaireController.me.goldenFeathers < i)
            {
                goldenFeathers[i].golden.color = Color.clear;
            }
            else if(ClaireController.me.goldenFeathers > i + 1)
            {
                goldenFeathers[i].golden.color = Color.white;
            }
            else
            {
                goldenFeathers[i].golden.color = new Color(1, 1, 1, ClaireController.me.goldenFeathers - i);
            }


        }
    }
}

[System.Serializable]
public struct GoldenFeather
{
    public SpriteRenderer golden;
    public SpriteRenderer dead;
}