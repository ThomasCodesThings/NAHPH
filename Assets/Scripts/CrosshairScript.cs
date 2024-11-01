using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrosshairScript : MonoBehaviour
{

    [SerializeField] private Texture2D crosshairTexture;
    private Vector2 cursorHotspot;
    // Start is called before the first frame update
    void Start()
    {
        cursorHotspot = new Vector2(crosshairTexture.width / 2, crosshairTexture.height / 2);
        Cursor.SetCursor(crosshairTexture, cursorHotspot, CursorMode.Auto);
    }

    // Update is called once per frame
    void Update()
    {
         

    }
}
