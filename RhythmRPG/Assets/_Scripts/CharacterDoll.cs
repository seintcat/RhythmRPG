using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterDoll : MonoBehaviour
{
    [SerializeField]
    private SpriteRenderer spriteRenderer;
    [SerializeField]
    private Animator animator;
    [SerializeField]
    private bool startsLeft;

    public Color color { set{ spriteRenderer.color = value; } }
    public bool lookLeft { set { spriteRenderer.flipX = !startsLeft ? value : !value; } }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
