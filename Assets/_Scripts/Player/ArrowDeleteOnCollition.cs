using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArrowDeleteOnCollition : MonoBehaviour {

    [SerializeField] private Texture2D exclusionTexture;

    private void OnTriggerEnter(Collider col)
    {
        if (col.gameObject.CompareTag("Arrow"))
        {
            var collisionPoint = col.ClosestPoint(transform.position);
            var collisionPointScreenCoor = Camera.main.WorldToScreenPoint(collisionPoint);

            if (CheckAlpha())
            {
                var particlesObject = col.transform.GetChild(1);
                particlesObject.parent = null;
                Destroy(col.gameObject);
                Destroy(particlesObject.gameObject, 2);
            }
            else
            {
                //Stop all forces
                col.attachedRigidbody.isKinematic = true;

                //Delete all mesh children besides fork
                Destroy(col.transform.GetChild(0).GetChild(0).gameObject);
                Destroy(col.transform.GetChild(0).GetChild(1).gameObject);

                //Delete colisions
                Destroy(col.GetComponent<Collider>());
                Destroy(col.GetComponent<Rigidbody>());

                //Stop particle emitions
                col.transform.GetChild(1).GetChild(0).GetComponent<ParticleSystem>().Stop();

                //Set scale for better visibility
                col.transform.localScale = Vector3.one * 1.5f;
            }
        }
    }

    private bool CheckAlpha()
    {
        //Convert hit coordinates
        Vector2 pixelUV = new Vector2(Input.mousePosition.x, Input.mousePosition.y);
        int uvX = (int)pixelUV.x;
        int uvY = (int)pixelUV.y;

        //Alpha Check                    
        Color hitColor = exclusionTexture.GetPixel(uvX, uvY);
        if (hitColor.a > 0.05)
        {
            return false;
            //Debug.Log("La coordenada " + uvX + "/" + uvY + " en " + "el fondo" + " NO ES ALPHA");
        }
        else
        {
            return true;
            //Debug.Log("La coordenada " + uvX + "/" + uvY + " en " + "el fondo" + " ES ALPHA");
        }
    }

}
