using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BGScroll : MonoBehaviour
{
    float _scrollSpeed = 0.5f;
    Material _mat;

    private void Start()
    {
        _mat = GetComponent<Renderer>().material;
    }

    private void Update()
    {
        _mat.mainTextureOffset += Vector2.down * _scrollSpeed * Time.deltaTime;
    }
}