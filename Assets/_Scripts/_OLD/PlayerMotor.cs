using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMotor : MonoBehaviour
{
    private CharacterController controller;
    private Vector3 moveVector;
    public float speed = 5.0f;
    private float gravity = 12.0f;

    private CharacterHealth CharacterHealth;
    private GameObject CharacterHealthObject;

    public Animator anim;
    public Animator anim2;
    public Animator anim3;
    public Animator anim4;
    public Animator anim5;
    private GameObject animObject;
    private GameObject animObject2;
    private GameObject animObject3;
    private GameObject animObject4;
    private GameObject animObject5;

    public GameObject Maton;
    public GameObject Zumo;
    public GameObject Ninja;

    private bool mueveDerecha;
    private bool mueveIzquierda;
    private bool stop;
    private bool back;
    private bool run;
    private bool up;
    private bool down;

    private int ran=1;
    private int ram = 1;
    private int RyuDamageCounter=3;
    private int ZumoDamageCounter = 2;
    private int NinjaDamageCounter = 3;

    void Start()
    {
        controller = GetComponent<CharacterController>();         
        CharacterHealthObject = GameObject.FindWithTag("MainCamera");   //characterHealthObject es igual a la camara

        animObject = GameObject.FindWithTag("RyuAnimation");            //animObject es igual al plano delante de la hitbox de ryu (el que tiene las animaciones de ryu: correr,saltar,atacar,etc)
        animObject2 = GameObject.FindWithTag("RyuExplosion");           //animObject2 es igual al plano delante de las animaciones de ryu (el que tiene la explosion de cuando resive daño)
        animObject3 = GameObject.FindWithTag("ZumoAnimation");          //animObject3 es igual al plano delante de la hitbox del sumo
        animObject5 = GameObject.FindWithTag("NinjaAnimation");

        mueveDerecha = false;       //mueveDerecha es falso
        mueveIzquierda = false;     //mueveIzquierda es falso
        stop = false;               //stop es falso
        back = false;               //back es falso
        down = false;               //down es falso
        up = false;                 //up es falso
        run = true;                 //run es verdadero  

        
    }



    void Update() /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    {
        Vector3 spawnPosition = new Vector3(5, -0.50f, 48);
        //movimientos
        moveVector = Vector3.zero;        

        if (run) moveVector.z = -speed;         //si run es verdadero los enemigos corren hacia el carro
        
        if (stop)                               //si stop es verdadero
        {
            run=false;                          //run deja de ser verdadero
            mueveDerecha = false;               //mueveDerecha deja de ser verdadero
            mueveIzquierda = false;             //mueveIzquierda deja de ser verdadero
        } else                                  //caso contrario (si stop es falso)
        {
            run = true;                         //run es verdadero
        }

        if (mueveDerecha) moveVector.x = speed;    //si mueveDerecha es verdadero el enemigo se mueve hacia la derecha
        
        if (mueveIzquierda) moveVector.x = -speed;  //si mueveIzquierda es verdadero el enemigo se mueve hacia la izquierda
        
        if (up) moveVector.y = speed * 5;           //si up es verdadero el enemigo se mueve hacia arriba
        
        if (down) moveVector.y = -speed * 5;        //si down es verdadero el enemigo se mueve hacia arriba

        if(back)
        {
            run = false;
            moveVector.z = speed;
        } else
        {
            run = true;
        }
        
        controller.Move(moveVector * Time.deltaTime);  //el movimiento es el mismo, da igual la cantidad de fps
    }


    public void OnTriggerEnter(Collider o)                //si el gameobject choca con un trigger

    {       
        if (o.gameObject.tag == "Bullet")                 //si el trigger es la flecha 
        {
            Destroy(GameObject.FindWithTag("Bullet"));    //se elimina la flecha
            
            if (GameObject.FindWithTag("RyuHitBox"))      //si a quien le pega es ryu
            {
                RyuDamageCounter= RyuDamageCounter - 1;   //se le resta un punto de vida
                if (RyuDamageCounter == 2)                //si la vida restante de ryu es 2
                {
                    StartCoroutine(damageRyuFloor());     //comienza la corutina de ryu siendo dañado             
                }
                if (RyuDamageCounter == 1)                //si la vida restante de ryu es 1
                {
                    StartCoroutine(damageRyuFloor());     //comienza la corutina de ryu siendo dañado
                }
                if (RyuDamageCounter == 0)                //si la vida restante de ryu es 1
                {
                    StartCoroutine(muerteRyu());          //comienza la corutina de ryu muriendo
                }
            }
            
            if (GameObject.FindWithTag("ZumoHitBox"))     //si a quien le pega es el sumo
            {
                ZumoDamageCounter = ZumoDamageCounter - 1;                              //se le resta un punto de vida

                if (ZumoDamageCounter == 1) StartCoroutine(damageZumoFloor());          //si la vida restante del sumo es 1 comienza la corutina del sumo siendo dañado
                
                if (ZumoDamageCounter == 0) StartCoroutine(muerteZumo());               //si la vida restante del sumo es 1 comienza la corutina del sumo muriendo
                
            }
            if (GameObject.FindWithTag("NinjaHitBox"))                                  //si a quien le pega es el ninja 
            {
                NinjaDamageCounter = NinjaDamageCounter - 1;
                if (NinjaDamageCounter == 2) StartCoroutine(DamageNinja());             //comienza la corutina del ninja recibiendo daño
                if (NinjaDamageCounter == 1) StartCoroutine(DamageNinja());             //comienza la corutina del ninja recibiendo daño
                if (NinjaDamageCounter == 0) StartCoroutine(muerteNinja());             //comienza la corutina del ninja muriendo

            }
            if (GameObject.FindWithTag("MatonHitBox")) StartCoroutine(muerteMaton());   //si a quien le pega es el maton comienza la corutina del maton muriendo


            Destroy(GameObject.FindWithTag("Bullet"));    //se elimina la flecha
        }

        if (o.gameObject.tag == "Carro")                   //si el trigger es el carro (llega a la posicion de la camara)
        {
            //Destroy(GameObject.FindWithTag("Ninja"));
            CharacterHealth = CharacterHealthObject.GetComponent<CharacterHealth>(); //se llama al componente characterhealth dentro de characterhealthobject(es el objeto que contiene el script de la vida)
            CharacterHealth.DealDamage(1);                                           //se hace 1 de daño         
            if (GameObject.FindWithTag("MatonHitBox"))                                 //si el que choca con el carro es el maton 
            {
               // StartCoroutine(MatonDamage());
               Destroy(GameObject.FindWithTag("MatonHitBox"));
                Application.LoadLevel("Perdiste");
            }
            if (GameObject.FindWithTag("ZumoHitBox"))                                 //si el que choca con el carro es el sumo
            {
              //StartCoroutine(ZumoDamage());
               Destroy(GameObject.FindWithTag("ZumoHitBox"));
                Application.LoadLevel("Perdiste");
            }
            if (GameObject.FindWithTag("NinjaHitBox"))                                 //si el que choca con el carro es el sumo
            {
                //StartCoroutine(ZumoDamage());
                Destroy(GameObject.FindWithTag("NinjaHitBox"));
                Application.LoadLevel("Perdiste");
            }


        }

        if (o.gameObject.tag == "Primero") StartCoroutine(Primero()); //si el trigger es el cubo llamado "primero" (solo en el nivel de ryu) comienza la corutina llamada "primero"

        if (o.gameObject.tag == "Segundo") StartCoroutine(Segundo()); //si el trigger es el cubo llamado "segundo" (solo en el nivel de ryu) comienza la corutina llamada "segundo"

        if (o.gameObject.tag == "Tercero") StartCoroutine(Tercero()); //si el trigger es el cubo llamado "tercero" (solo en el nivel de ryu) comienza la corutina llamada "tercero"

        if (o.gameObject.tag == "Cuarto") StartCoroutine(Cuarto()); //si el trigger es el cubo llamado "cuarto" (solo en el nivel de ryu) comienza la corutina llamada "cuarto"

        if (o.gameObject.tag == "Quinto") StartCoroutine(Quinto()); //si el trigger es el cubo llamado "quinto" (solo en el nivel de ryu) comienza la corutina llamada "quinto"

        if (o.gameObject.tag == "Sexto") StartCoroutine(Sexto()); //si el trigger es el cubo llamado "sexto" (solo en el nivel de ryu) comienza la corutina llamada "sexto"

        if (o.gameObject.tag == "Septimo") StartCoroutine(Septimo()); //si el trigger es el cubo llamado "septimo" (solo en el nivel de ryu) comienza la corutina llamada "septimo"

        if (o.gameObject.tag == "Octavo") StartCoroutine(Octavo()); //si el trigger es el cubo llamado "octavo" (solo en el nivel de ryu) comienza la corutina llamada "octavo"

    }

    IEnumerator muerteMaton()
    {
        stop = true;
        anim2 = animObject2.GetComponent<Animator>();
        anim2.SetTrigger("Explosion");
        yield return new WaitForSeconds(0.50f);
        Destroy(GameObject.FindWithTag("MatonHitBox"));
        
        Application.LoadLevel("SegundoNivel");
        


    }

    IEnumerator MatonDamage()
    {
        //anim4 = animObject4.GetComponent<Animator>();
        //anim4.SetTrigger("Maton");
        yield return new WaitForSeconds(0f);
        //anim4.SetTrigger("Null");
    }
    IEnumerator ZumoDamage()
    {
        //anim4 = animObject4.GetComponent<Animator>();
        anim4.SetTrigger("Zumo");
        yield return new WaitForSeconds(0f); //tiempo que dure la animacion 
        anim4.SetTrigger("Null");            //vuelve a ser transparente
    }
   
    
    
    IEnumerator muerteNinja() //esto ocurre cuando comienza la corutina llamada "muerteNinja"
    {
        stop = true;
        anim2 = animObject2.GetComponent<Animator>();
        anim2.SetTrigger("Explosion");
        yield return new WaitForSeconds(0.80f);
        Destroy(GameObject.FindWithTag("NinjaHitBox"));
        
        Application.LoadLevel("CuartoNivel");


    }
    IEnumerator damageZumoFloor() //esto ocurre cuando el sumo recibe daño
    {
        stop = true;
        anim2 = animObject2.GetComponent<Animator>();
        anim2.SetTrigger("Explosion");
        yield return new WaitForSeconds(1f);
        stop = false;
    }

    IEnumerator DamageNinja() //esto ocurre cuando comienza la corutina llamada "damageNinja"
    {

        stop = true;
        anim5 = animObject5.GetComponent<Animator>();
        anim5.SetTrigger("Tronco");
        anim2 = animObject2.GetComponent<Animator>();
        anim2.SetTrigger("Humo");
        yield return new WaitForSeconds(1.61f);
        ram = ram + 1;
        anim2 = animObject2.GetComponent<Animator>();
        anim2.SetTrigger("Humo");

        if ((ram % 2) == 0)
        {
            transform.Translate(2f, 0, 0);
            
            
        }
        else
        {
            transform.Translate(-4f, 0, 0);
            
            
        }
        
        stop = false;

    }
    
    IEnumerator muerteZumo() //esto ocurre cuando comienza la corutina llamada "muerteZumo"
    {

        //anim3 = animObject3.GetComponent<Animator>();
        anim3.SetTrigger("Death");
        stop = true;
        yield return new WaitForSeconds(2.50f);
        Destroy(GameObject.FindWithTag("ZumoHitBox"));
        stop = false;
        
        Application.LoadLevel("TercerNivel");
    }
    IEnumerator damageRyuFloor() //esto ocurre cuando ryu recibe daño estando en el suelo  
    {
        stop = true;
        //anim2 = animObject2.GetComponent<Animator>();
        anim2.SetTrigger("Explosion");
        yield return new WaitForSeconds(1f);
        stop = false;
        


    }
    IEnumerator damageRyuAir() //esto ocurre cuando ryu recibe daño estando en el aire
    {
        stop = true;

        yield return new WaitForSeconds(1f);
        stop = false;
    }
    IEnumerator muerteRyu() //esto ocurre cuando comienza la corutina llamada "muerteRyu"
    {
        stop = true;
        anim.SetTrigger("Death");
        yield return new WaitForSeconds(4f);
        Application.LoadLevel("Ganaste");
        yield return new WaitForSeconds(29f);
        stop = true;
        Destroy(GameObject.FindWithTag("RyuHitBox"));    
    }

    IEnumerator Primero() //esto ocurre cuando comienza la corutina llamada "primero"
    {
        //anim = animObject.GetComponent<Animator>();
        stop = true;
        anim.SetTrigger("Jump");
        yield return new WaitForSeconds(0.40f);
        stop = false;
        up = true;
        yield return new WaitForSeconds(0.80f);
        up = false;
        anim.SetTrigger("Air");
        yield return new WaitForSeconds(1f);
        anim.SetTrigger("Fall");
        down = true;
        yield return new WaitForSeconds(0.80f);
        down = false;
        stop = true;
        yield return new WaitForSeconds(0.40f);
        stop = false;
    }
    IEnumerator Segundo() //esto ocurre cuando comienza la corutina llamada "segundo"
    {
        //anim = animObject.GetComponent<Animator>();
        stop = true;
        anim.SetTrigger("Jump");
        yield return new WaitForSeconds(0.40f);
        stop = false;
        up = true;
        yield return new WaitForSeconds(0.80f);
        up = false;
        anim.SetTrigger("Attack");
        yield return new WaitForSeconds(1.20f);
        anim.SetTrigger("Fall");
        down = true;
        yield return new WaitForSeconds(0.80f);
        down = false;
        stop = true;
        yield return new WaitForSeconds(0.40f);
        stop = false;
    }
    IEnumerator Tercero() //esto ocurre cuando comienza la corutina llamada "tercero"
    {
        //correr hacia la derecha (o izquierda)
        //anim = animObject.GetComponent<Animator>();
        //anim.SetTrigger("Salto");
        ran = ran + 1;

        if ((ran % 2) == 0)
        {
            mueveIzquierda = true;
            yield return new WaitForSeconds(2.51f);
            mueveIzquierda = false;
        }
        else
        {
            mueveDerecha = true;
            yield return new WaitForSeconds(2.51f);
            mueveDerecha = false;
        }
    }
    IEnumerator Cuarto() //esto ocurre cuando comienza la corutina llamada "cuarto"
    {
        //saltar + correr hacia la izquierda + subir el hitbox
        mueveDerecha = true;
        mueveIzquierda = false;
        stop = true;
        anim.SetTrigger("Jump");
        yield return new WaitForSeconds(0.40f);
        stop = false;
        mueveIzquierda = true;
        up = true;
        yield return new WaitForSeconds(0.80f);
        up = false;
        anim.SetTrigger("Air");
        yield return new WaitForSeconds(0.45f);
        anim.SetTrigger("Fall");
        down = true;
        yield return new WaitForSeconds(0.80f);
        stop = true;
        down = false;
        yield return new WaitForSeconds(0.40f);
        stop = false;

    }
    IEnumerator Quinto() //esto ocurre cuando comienza la corutina llamada "quinto"
    {
        //saltar + correr hacia la izquierda + subir el hitbox
        mueveDerecha = true;
        mueveIzquierda = false;
        stop = true;
        anim.SetTrigger("Jump");
        yield return new WaitForSeconds(0.40f);
        stop = false;
        mueveDerecha = true;
        up = true;
        yield return new WaitForSeconds(0.80f);
        up = false;
        anim.SetTrigger("Air");
        yield return new WaitForSeconds(0.45f);
        anim.SetTrigger("Fall");
        down = true;
        yield return new WaitForSeconds(0.80f);
        stop = true;
        down = false;
        yield return new WaitForSeconds(0.40f);
        stop = false;


    }
    IEnumerator Sexto() //esto ocurre cuando comienza la corutina llamada "quinto"
    {
        stop = true;
        anim.SetTrigger("Jump");
        yield return new WaitForSeconds(0.40f);
        stop = false;
        up = true;
        yield return new WaitForSeconds(0.80f);
        up = false;
        anim.SetTrigger("Attack");
        yield return new WaitForSeconds(1.20f);
        anim.SetTrigger("Fall");
        down = true;
        yield return new WaitForSeconds(0.80f);
        down = false;
        stop = true;
        yield return new WaitForSeconds(0.40f);
        stop = false;

    }
    IEnumerator Septimo() //esto ocurre cuando comienza la corutina llamada "septimo"
    {
        //anim = animObject.GetComponent<Animator>();
        mueveIzquierda = false;
        mueveDerecha = false;
        stop = true;
        anim.SetTrigger("Jump");
        yield return new WaitForSeconds(0.40f);
        up = true;
        yield return new WaitForSeconds(0.80f);
        transform.Translate(0, 25, 40);
        up = false;
        yield return new WaitForSeconds(2.00f);
        anim.SetTrigger("Air");
        down = true;
        yield return new WaitForSeconds(0.45f);
        stop = true;
        down = true;
        anim.SetTrigger("Fall");
        yield return new WaitForSeconds(1.00f);
        down = false;
        stop = false;


    }
    IEnumerator Octavo() //esto ocurre cuando comienza la corutina llamada "octavo"
    {
        //salta + teletransportar hacia atras (hasta el "segundo") + dejar bomba + subir hitbox
        //anim = animObject.GetComponent<Animator>();
        stop = true;
        transform.Translate(0, 6f, 0);
        anim.SetTrigger("Salto");
        yield return new WaitForSeconds(0.4f);
        transform.Translate(0, 25, 40);
        yield return new WaitForSeconds(0.1f);
        transform.Translate(0, -5f, 0);
        yield return new WaitForSeconds(0.1f);
        transform.Translate(0, -5f, 0);
        yield return new WaitForSeconds(0.1f);
        transform.Translate(0, -5f, 0);
        yield return new WaitForSeconds(0.1f);
        transform.Translate(0, -5f, 0);
        yield return new WaitForSeconds(0.1f);
        transform.Translate(0, -5f, 0);
        yield return new WaitForSeconds(0.1f);
        transform.Translate(0, 0, 0);
        yield return new WaitForSeconds(1.30f);
        transform.Translate(0, -5f, 0);
        stop = false;
        //dejar bomba

    }
}


