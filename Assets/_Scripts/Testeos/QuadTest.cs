using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuadTest : MonoBehaviour
{
    private SpriteRenderer spriteRenderer;
    public Vector3 spriteSizeScale;

    public bool rendererMode = false;
    public bool spriteMode = false;    

    void Start()
    {
        spriteRenderer = gameObject.transform.parent.gameObject.GetComponentInChildren<SpriteRenderer>();
    }

    //Este codigo solo funciona con Billboard, si el personaje rota entonces deja de funcionar, se necesitan hacer modificaciones en base a eso
    //Un objetivo a lograr es que si se rota el sprite respecto de la camara el quad siga manteniendo la forma y escala que deberia mantener
    //Actualmente se deforma en todas las formas de rotacion)

    //Agregar que el objeto siga el centro del objeto del sprite (En el caso de que tenga la misma gerarquia y no sea su hijo)

    //Este codigo se debe ajustar automaticamente a la escala porque la escala que uso no siempre es 1


    void Update() //Se puede hacer de dos formas, usando el bound size del sprite o del spriterenderer. En ambos casos se deben fixear las carencias
    {
        if (spriteMode)   //Este modo para calcular el tamaño del sprite utiliza "Sprite.Bounds.Size"
        {
            //Defecto: Este modo no se rompe con la rotacion del sprite, pero si con la escala.
            //Solucion: Multiplicar la escala de los Bounds por la escala del transform

            Vector3 spriteSize = spriteRenderer.sprite.bounds.size;

            spriteSizeScale = gameObject.transform.parent.Find("Sprite").transform.localScale;

            transform.localScale = new Vector3(spriteSize.x * spriteSizeScale.x, spriteSize.y * spriteSizeScale.y, spriteSize.z); //spriteSize * spriteSizeScale.x; //3.33

        }
        else
        if (rendererMode) //Este modo para calcular el tamaño del sprite utiliza "Renderer.Bounds.Size"
        {
            //Defecto: Este modo funciona con la escala, pero se rompe con la rotacion.
            //Solucion: ???

            Vector3 spriteBoundSize = spriteRenderer.bounds.size; //Vector3 spriteSize = spriteRenderer.sprite.bounds.size;

            //Solucion 1: Hipotenusas

            ///float spriteX = Mathf.Sqrt(spriteBoundSize.x * spriteBoundSize.x + spriteBoundSize.z * spriteBoundSize.z); //El tamaño en X se corresponde con la hipotenusa entre (X y Z)
            ///float spriteY = Mathf.Sqrt(spriteBoundSize.y * spriteBoundSize.y + spriteBoundSize.z * spriteBoundSize.z); //El tamaño en Y se corresponde con la hipotenusa entre (Y y Z)

            ///transform.localScale = new Vector3(spriteX, spriteY, spriteBoundSize.z); //Si se utilizan en simultaneo el eje opuesto se expande mas de lo debido


            //Solucion 2: CREO que con los datos de las dos hipotenusas se podria hacer algun calculo para obtener el valor deseado de Z

            float hipotenusaDeXyZ = Mathf.Sqrt(spriteBoundSize.x * spriteBoundSize.x + spriteBoundSize.z * spriteBoundSize.z); //spriteBoundSize.z esta mal, necesito ZdeX y ZdeY por separado
            float hipotenusaDeYyZ = Mathf.Sqrt(spriteBoundSize.y * spriteBoundSize.y + spriteBoundSize.z * spriteBoundSize.z);

            float ZdeX = Mathf.Sqrt(hipotenusaDeXyZ * hipotenusaDeXyZ - spriteBoundSize.x * spriteBoundSize.x);
            float ZdeY = Mathf.Sqrt(hipotenusaDeYyZ * hipotenusaDeYyZ - spriteBoundSize.y * spriteBoundSize.y);

            float hipotenusaRealDeXyZ = Mathf.Sqrt(spriteBoundSize.x * spriteBoundSize.x + ZdeX * ZdeX);
            float hipotenusaRealDeYyZ = Mathf.Sqrt(spriteBoundSize.y * spriteBoundSize.y + ZdeY * ZdeY);

            transform.localScale = new Vector3(hipotenusaDeXyZ, hipotenusaDeYyZ, spriteBoundSize.z);
        }
    }
}
