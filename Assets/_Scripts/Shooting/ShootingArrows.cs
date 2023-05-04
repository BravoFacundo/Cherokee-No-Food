using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShootingArrows : MonoBehaviour
{
    void Start()
    {
        
    }

    void Update()
    {
        
    }

    IEnumerator ChargeArrow()
    {
        bool charging = false;
        if (Input.GetMouseButtonDown(0))
        {            
            charging = true;
            //charge animation
        }
        if (Input.GetMouseButtonUp(0) && charging)
        {
            DontShootArrow();
            charging = false; StopCoroutine(nameof(ChargeArrow));
        }

        yield return new WaitForSeconds(.5f);
        if (Input.GetMouseButtonUp(0) && charging)
        {
            ShootWeakArrow();
            charging = false; StopCoroutine(nameof(ChargeArrow));
        }

        yield return new WaitForSeconds(.5f);
        if (Input.GetMouseButtonUp(0) && charging)
        {
            ShootArrow();
            charging = false; StopCoroutine(nameof(ChargeArrow));
        }
    }
    IEnumerator ChargeFastArrow()
    {
        bool charging = false;
        int fastArrows = 5;
        bool shooted = false;
        if (Input.GetButtonDown("e"))
        {
            charging = true;
            //charge animation
        }

        //Esto esta mal je
        for (int i = fastArrows; fastArrows <= 0; fastArrows--)
        {
            if (Input.GetMouseButtonDown(0) && charging && fastArrows != 0 && !shooted)
            {
                ShootFastArrow();
                shooted = true;
                yield return new WaitForSeconds(.2f);
            }
        }




    }

    private void ShootArrow()
    {

    }
    private void ShootWeakArrow()
    {

    }
    private void ShootFastArrow()
    {

    }
    private void DontShootArrow()
    {

    }
}
