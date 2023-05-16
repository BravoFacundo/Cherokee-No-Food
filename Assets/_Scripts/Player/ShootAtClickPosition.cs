using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShootAtClickPosition : MonoBehaviour
{   

    [Header("Configuration")]
    public float shootForce;
    private float shootForceSave;

    private ForceMode forceMode;

    [SerializeField] private float shootChargeSpeed;
    [SerializeField] private float shootMaxCharge;
    [SerializeField] private float shootReloadTime;

    public enum ShootMode { ShootFromBowPosition, ShootFromInputPosition, ShootToHUDPosition }
    public ShootMode shootMode;

    [Header("Debug")]
    public bool isReloading;
    private bool isCharging;
    [SerializeField] private float chargeTime;

    [Header("Bow Rotation")]
    [SerializeField] private float maxBowYRotation;
    private float mouseXpos;
    private float bowYRotation;

    [Header("References")]
    [SerializeField] private GameObject bowObject;
    [SerializeField] private GameManager gameManager;
    [SerializeField] private ParticleController particleController;
    private Animator bowAnimator;
    private Camera cam;
    private GameObject enemyAttack;

    [Header("Prefabs")]
    [SerializeField] private Rigidbody arrowPrefab;
    [SerializeField] private Rigidbody arrowForkPrefab;

    void Start()
    {
        cam = Camera.main;
        enemyAttack = transform.parent.GetChild(2).gameObject;

        bowAnimator = bowObject.GetComponent<Animator>();
        bowAnimator.SetFloat("Arrow_ChargeSpeed", 1 / shootMaxCharge);
        bowAnimator.SetFloat("Arrow_ReloadSpeed", 1 / shootReloadTime);

        shootForceSave = shootForce;
        isReloading = false;
    }


    void Update()
    {
        if (Input.GetMouseButtonDown(2)) bowAnimator.SetTrigger("Arrow_Reload");
        if (Input.GetMouseButtonDown(1)) 
        {
            var f = bowAnimator.GetFloat("Arrow_ChargeSpeed");
            bowAnimator.SetFloat("Arrow_ChargeSpeed", f * -1);
        }

        if (Input.GetMouseButtonDown(0))
        {            
            //print("Start Charging Shoot");
            chargeTime = 0; 
            bowAnimator.SetBool("Arrow_Charge", true);

        }
        if (Input.GetMouseButton(0))
        {            
            isCharging = true;
            //bowAnimator.SetTrigger("Arrow_Charge");
            bowAnimator.SetBool("Arrow_Charge", true);
            if (isCharging && !isReloading && chargeTime < shootMaxCharge)
            {
                chargeTime += Time.deltaTime * shootChargeSpeed;
                //bowAnimator.SetBool("Arrow_Charge", true);
            }
        }

        if (Input.GetMouseButtonUp(0))
        {            
            if (chargeTime <= shootMaxCharge * 0.5)
            {
                //print("No Shoot");
                chargeTime = 0;
                bowAnimator.SetBool("Arrow_Charge", false);
                bowAnimator.SetTrigger("Arrow_CancelCharge");
            }
            else
            if (chargeTime >= shootMaxCharge * 0.5 && chargeTime <= shootMaxCharge * 0.75)
            {
                //print("Weak Shoot");
                StartCoroutine(nameof(ShootArrow), 1);
            }
            else
            if (chargeTime >= shootMaxCharge * 0.75 && chargeTime <= shootMaxCharge + 1)
            {
                //print("Hard Shoot");
                StartCoroutine(nameof(ShootArrow), 2);
            }

            bowAnimator.SetBool("Arrow_Charge", false);
        }

        RotateBowByMouseXPosition();
    }

    IEnumerator ShootArrow(int shootDamage)
    {
        isCharging = false;
        chargeTime = 0;
        isReloading = true;

        //Bow Animation
        bowAnimator.SetBool("Arrow_Charge", false);
        bowAnimator.SetTrigger("Arrow_Shoot");

        //Shoot Direction
        Ray ray = cam.ScreenPointToRay(Input.mousePosition);
        Quaternion rotation = Quaternion.LookRotation(ray.direction);

        //Shoot Stats
        switch (shootDamage)
        {
            case 1:

                break;
            case 2:
                shootForce = shootForce * 1.25f;
                break;
        }

        //Check Shoot Mode
        if (enemyAttack.GetComponent<SpriteRenderer>().sprite != null)
        {
            if (CheckAlpha()) shootMode = ShootMode.ShootFromInputPosition;
            else shootMode = ShootMode.ShootToHUDPosition;
        }
        else shootMode = ShootMode.ShootFromBowPosition;

        //Shoot Mode
        Rigidbody arrow = arrowPrefab;
        Vector3 spawnPosition = Vector3.zero;

        if (shootMode != ShootMode.ShootToHUDPosition)
        {
            if (shootMode == ShootMode.ShootFromBowPosition)
            {
                Vector2 pivotPosition = new Vector2(
                 bowObject.transform.position.x - (bowObject.GetComponent<RectTransform>().sizeDelta.x * 0.5f),
                 bowObject.transform.position.y + (bowObject.GetComponent<RectTransform>().sizeDelta.y * 0.5f));
                spawnPosition = cam.ScreenToWorldPoint(new Vector3(pivotPosition.x, pivotPosition.y, cam.nearClipPlane + .6f));
            }
            else if (shootMode == ShootMode.ShootFromInputPosition)
            {
                spawnPosition = cam.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, cam.nearClipPlane + .6f));
            }

            //Shoot Instance        
            Rigidbody newArrow = Instantiate(arrow, spawnPosition, rotation) as Rigidbody;
            newArrow.AddForce(ray.direction * shootForce, ForceMode.VelocityChange);
            newArrow.transform.Rotate(0, 0, Random.Range(0, 180), Space.Self);
        }
        else
        {
            arrow = arrowForkPrefab;
            spawnPosition = cam.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, cam.nearClipPlane + .9f));

            Rigidbody newArrow = Instantiate(arrow, spawnPosition, rotation) as Rigidbody;
            newArrow.transform.Rotate(0, 0, Random.Range(0, 180), Space.Self);
            newArrow.isKinematic = true;
            Destroy(newArrow.gameObject, .5f);

            particleController.ImpactExplosion(spawnPosition + new Vector3(0,0,-.1f) , Quaternion.identity, false);
            StartCoroutine(gameManager.StopReceivingDamage());
        }        

        yield return new WaitForSeconds(shootReloadTime);
        bowAnimator.SetTrigger("Arrow_Reload");
        shootForce = shootForceSave;
        isReloading = false;
    }

    private void RotateBowByMouseXPosition()
    {
        mouseXpos = Mathf.InverseLerp(0, Screen.width, Input.mousePosition.x);
        bowYRotation = Mathf.Lerp(0, maxBowYRotation, mouseXpos);
        bowObject.transform.rotation = Quaternion.Euler(new Vector3(0, bowYRotation, 0));
    }

    private bool CheckAlpha()
    {
        Texture2D attackTexture = (Texture2D)enemyAttack.GetComponent<SpriteRenderer>().sprite.texture;
        
        //Convert hit coordinates
        Vector2 pixelUV = new Vector2(Input.mousePosition.x, Input.mousePosition.y);
        int uvX = (int)pixelUV.x;
        int uvY = (int)pixelUV.y;

        //Alpha Check                    
        Color hitColor = attackTexture.GetPixel(uvX, uvY);
        if (hitColor.a > 0.05)
        {
            return false;
            //Debug.Log("La coordenada " + uvX + "/" + uvY + " en " + "Enemigo" + " NO ES ALPHA");
        }
        else
        {
            return true;
            //Debug.Log("La coordenada " + uvX + "/" + uvY + " en " + "Enemigo" + " ES ALPHA");
        }
    }


}



    
