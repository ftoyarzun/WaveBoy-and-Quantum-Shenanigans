using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleVisuals : MonoBehaviour
{
    private SpriteRenderer spriteRenderer;

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        spriteRenderer.material = new Material(spriteRenderer.material);
    }
}
