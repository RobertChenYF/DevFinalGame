using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoldenFeathersUI : MonoBehaviour
{

    public SpriteRenderer[] goldenFeathers;

    public Color col_gold;
    public Color col_dead;


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        for (int i = 0; i < goldenFeathers.Length; i++)
        {
            if(ClaireController.me.goldenFeathersMax > i)
            {
                goldenFeathers[i].color = col_dead;

                if (ClaireController.me.goldenFeathers > i + 1)
                {
                    goldenFeathers[i].color = col_gold;
                }
                else
                {
                    goldenFeathers[i].color = Color.Lerp(col_dead, col_gold, ClaireController.me.goldenFeathers - i);
                }
            }
            else
            {
                goldenFeathers[i].color = Color.clear;    
            }
        }
    }
}
