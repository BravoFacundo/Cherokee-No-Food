using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PrintScreenPixel : MonoBehaviour
{
    
    void Start()
    {
        
    }

    void Update()
    {
        if (Input.GetKeyDown("e"))
        {
            CheckAlpha();
        }
    }

    private void CheckAlpha()
    {
        print(Input.mousePosition.x + " | " + Input.mousePosition.y);

        var characterTexture = (Texture2D)transform.GetChild(0).GetComponent<SpriteRenderer>().sprite.texture;
        if (characterTexture != null)
        {
            //Convert hit coordinates
            Vector2 pixelUV = new Vector2(Input.mousePosition.x, Input.mousePosition.y);            
            //int uvX = Mathf.FloorToInt(pixelUV.x * characterTexture.width);
            //int uvY = Mathf.FloorToInt(pixelUV.y * characterTexture.height);
            int uvX = (int)pixelUV.x;
            int uvY = (int)pixelUV.y;
            //Alpha Check                    
            Color hitColor = characterTexture.GetPixel(uvX, uvY);
            if (hitColor.a > 0.05)
            {
                Debug.Log("La coordenada " + uvX + "/" + uvY + " en " + "Enemigo" + " NO ES ALPHA");
            }
            else
            {
                Debug.Log("La coordenada " + uvX + "/" + uvY + " en " + "Enemigo" + " ES ALPHA");
            }
        }
    }
}
