using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//https://forum.unity.com/threads/raycast-alpha-information.24159/

public class Test : MonoBehaviour
{
    public Texture2D characterTexture;
    public Texture2D headshotTexture;
    public Color hitColor;
    
    void Start()
    {
        
    }
        
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit, 100))
            {
                //Debug.Log(hit.transform.name); //Debug.Log(hit.point)

                //Get Texture in Renderer 
                if (hit.transform.parent != null && hit.transform.parent.gameObject.GetComponentInChildren<SpriteRenderer>() != null)
                {
                    characterTexture = (Texture2D)hit.transform.parent.gameObject.GetComponentInChildren<SpriteRenderer>().sprite.texture;
                    //Obtener tambien la headshot texture (En este caso esta en la mesh renderer de quad que toma la hit.coord)
                }
                else
                if (hit.collider.GetComponent<MeshRenderer>() != null)
                {
                    characterTexture = (Texture2D)hit.collider.GetComponent<MeshRenderer>().material.mainTexture;
                    //Obtener tambien la headshot texture (En este caso se debe tener de algun otro metodo)

                }

                //Only if the Texture is obtain is posible to convert the coordinates
                if (characterTexture != null)
                {
                    //Convert hit coordinates
                    Vector2 pixelUV = hit.textureCoord;
                    int uvX = Mathf.FloorToInt(pixelUV.x * characterTexture.width);
                    int uvY = Mathf.FloorToInt(pixelUV.y * characterTexture.height);

                    //Alpha Check                    
                    hitColor = characterTexture.GetPixel(uvX, uvY);
                    if (hitColor.a > 0.05)
                    {
                        Debug.Log("La coordenada " + uvX + "/" + uvY + " en " + hit.transform.name + " NO ES ALPHA");
                    }
                    else
                    {
                        Debug.Log("La coordenada " + uvX + "/" + uvY + " en " + hit.transform.name + " ES ALPHA");
                    }
                }
                
            }
        }
    }
}
