using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CursorManager : MonoBehaviour
{
    [SerializeField] private Texture2D cursorTexure;

    private Vector2 cursorHotspot;
    private object cursorTexture;

    void Start()
    {
        
        Cursor.SetCursor(cursorTexure, cursorHotspot, CursorMode.Auto);
    }

    // Update is called once per frame
     
}
