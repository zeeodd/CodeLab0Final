using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SpriteSwapper : MonoBehaviour
{
    public Sprite state1;
    public Sprite state2;
    public KeyCode key;

    private Image spriteRenderer;

    void Start()
    {
        spriteRenderer = GetComponent<Image>();
        if (spriteRenderer.sprite == null)
        {
            spriteRenderer.sprite = state1;
        }
    }

    void Update()
    {
        if (Input.GetKeyDown(key))
        {
            SpriteSwap();
        }
    }

    void SpriteSwap()
    {
        if (spriteRenderer.sprite = state1)
        {
            spriteRenderer.sprite = state2;
        }
        Invoke("RestoreSprite", 0.1f);
    }

    void RestoreSprite()
    {
        if (spriteRenderer.sprite = state2)
        {
            spriteRenderer.sprite = state1;
        }
    }
}
