using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CharacterHealth : MonoBehaviour {
    
    public float CurrentHealth;// { get; set; }
    public float MaxHealth; //{ get; set; }

    public Slider healthbar;
    public Animator LifeNahiely;
    public GameObject LifeNahielyObject;

    private ShootAtClickPosition Wait;
    private GameObject WaitObject;

    private Animator Bow;
    private GameObject BowObject;

    private Animator Explotando;
    private GameObject ExplotandoObject;




    public void Start () {        

        MaxHealth = 3f;
        CurrentHealth = MaxHealth;
        healthbar.value = CalculateHealth();

        LifeNahielyObject = GameObject.FindWithTag("LifeNahiely");
        WaitObject = GameObject.FindWithTag("MainCamera");
        BowObject = GameObject.FindWithTag("Bow");
        ExplotandoObject = GameObject.FindWithTag("EnemigosExplotando");

    }
	

	 public void Update () {   
        //DealDamage(1);
         
    }

    public void DealDamage(float damageValue)
    {
        //print ("damageValue");
        CurrentHealth -= damageValue;
        healthbar.value = CalculateHealth();
                
        if (CurrentHealth == 2) StartCoroutine(PrimerGolpe());

        if (CurrentHealth == 1) StartCoroutine(SegundoGolpe());
        
        if (CurrentHealth == 0) StartCoroutine(TercerGolpe());            
        
    } 

    public float CalculateHealth()
    {
        return CurrentHealth / MaxHealth;
    }

    IEnumerator PrimerGolpe()
    {
        Bow = BowObject.GetComponent<Animator>();
        Bow.SetTrigger("DispararPriori");
        Wait = WaitObject.GetComponent<ShootAtClickPosition>();
        Wait.isReloading = true;
        yield return new WaitForSeconds(0.15f);
        LifeNahiely = LifeNahielyObject.GetComponent<Animator>();
        LifeNahiely.SetTrigger("PrimerGolpe");
        yield return new WaitForSeconds(0.15f);
        Explotando = ExplotandoObject.GetComponent<Animator>();
        Explotando.SetTrigger("Explosion");
        yield return new WaitForSeconds(1.35f);
        Wait.isReloading = false;
    }
    IEnumerator SegundoGolpe()
    {
        Bow = BowObject.GetComponent<Animator>();
        Bow.SetTrigger("DispararPriori");
        Wait = WaitObject.GetComponent<ShootAtClickPosition>();
        Wait.isReloading = true;
        yield return new WaitForSeconds(0.15f);
        LifeNahiely = LifeNahielyObject.GetComponent<Animator>();
        LifeNahiely.SetTrigger("SegundoGolpe");
        yield return new WaitForSeconds(0.15f);
        Explotando = ExplotandoObject.GetComponent<Animator>();
        Explotando.SetTrigger("Explosion");
        yield return new WaitForSeconds(1.35f);
        Wait.isReloading = false;
    }
    IEnumerator TercerGolpe()
    {
        
        Wait = WaitObject.GetComponent<ShootAtClickPosition>();
        Wait.isReloading = true;
        yield return new WaitForSeconds(0.15f);
        LifeNahiely = LifeNahielyObject.GetComponent<Animator>();
        LifeNahiely.SetTrigger("TercerGolpe");
        yield return new WaitForSeconds(0.15f);        
        yield return new WaitForSeconds(1.35f);
        Wait.isReloading = false;
    }
}
