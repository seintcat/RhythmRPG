using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseGrounds : MonoBehaviour
{
    [SerializeField]
    private SpriteRenderer spriteRenderer;

    public Color color
    {
        get
        {
            return spriteRenderer.color;
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
