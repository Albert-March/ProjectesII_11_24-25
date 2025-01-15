using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AmplyfyButtonOnHover : MonoBehaviour
{
    Vector2 realSize;
    Vector2 bigSize;
    // Start is called before the first frame update
    void Start()
    {
        realSize = transform.localScale;
        bigSize = realSize * 1.2f;
    }

    public void ChangeScale(bool big) 
    {
        if (big)
        {
            transform.localScale = bigSize;
        }
        else 
        {
            transform.localScale = realSize;
        }
    }

}
