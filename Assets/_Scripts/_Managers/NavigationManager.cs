using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NavigationManager : MonoBehaviour
{    
    public void ButtonPressed_Back()
    {
        //Vuelve a la pantalla anterior
        //Si la pantalla anterior es el menu, entonces reproduce el video desde el frame donde la madera ya cayo, y las luces se prenden.
        print("Back");
    }
    public void ButtonPressed_Exit()
    {
        Application.Quit();
        print("Exit");
    }
}
