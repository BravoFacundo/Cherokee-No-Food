using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [Header("Config")]
    [SerializeField] int playerHealth = 11;
    [SerializeField] bool beingAttacked = false;
    private bool checkFirst = false;

    [Header("References")]
    [SerializeField] private ShootAtClickPosition shootAtClickPosition;
    [SerializeField] private ParticleController particleController;

    [Header("References 2")]
    [SerializeField] private Animator healthAnimator;
    [SerializeField] private Animator enemyAttackAnimator;
    [SerializeField] private Animator enemyImpactAnimator;
    [SerializeField] private Animator bowAnimator;

    void Start()
    {
        healthAnimator.SetInteger("Health", playerHealth);
    }

    private void Update()
    {
        Mathf.Clamp(playerHealth, 0, 12);

        if (Input.GetKeyDown("m"))
        {
            playerHealth++;
            healthAnimator.SetTrigger("ShakeBar");
            healthAnimator.SetInteger("Health", playerHealth);
        }
        if (Input.GetKeyDown("n"))
        {
            playerHealth--;
            healthAnimator.SetTrigger("ShakeBar");
            healthAnimator.SetInteger("Health", playerHealth);
        }

        if (Input.GetKeyDown("b"))
        {
            StartCoroutine(nameof(HealPlayer), 3);

        }
        if (Input.GetKeyDown("v"))
        {
            StartCoroutine(nameof(DamagePlayer), 3);

        }
    }

    private IEnumerator HealPlayer(int healPoints)
    {
        var newHealth = Mathf.Clamp(playerHealth + healPoints, 0, 12);
        for (int i = playerHealth; i < newHealth + 1; i++)
        {
            playerHealth = i;
            healthAnimator.SetInteger("Health", playerHealth);
            yield return new WaitForSeconds(.2f);
        }
    }
    public IEnumerator DamagePlayer(int damagePoints)
    {
        var newHealth = Mathf.Clamp(playerHealth - damagePoints, 0, 12);
        healthAnimator.SetTrigger("ShakeBar");
        yield return new WaitForSeconds(.6f);
        for (int i = playerHealth; i > newHealth -1; i--)
        {
            playerHealth = i;
            healthAnimator.SetInteger("Health", playerHealth);
            yield return new WaitForSeconds(.2f);
        }
    }
        
    public IEnumerator EnemyAttack(int damageReceived, string character, GameObject enemyObj)
    {
        if (beingAttacked && !checkFirst)
        {
            SetGameState("pause", true);
            SetGameState("Loose", true);
            checkFirst = true;
        }
        else
        switch (character)
        {
            case "Thug":                
                beingAttacked = true;
                enemyAttackAnimator.SetBool("Thug_Attacking", beingAttacked);
                Destroy(enemyObj);
                StartCoroutine(nameof(ReceivingDamage));
                break;

            case "Sumo":
                beingAttacked = true;
                enemyAttackAnimator.SetBool("Sumo_Attacking", beingAttacked);
                Destroy(enemyObj);
                StartCoroutine(nameof(ReceivingDamage));
                yield return new WaitForSeconds(1f);
                break;

            case "Ninja":
                Destroy(enemyObj);
                break;
            case "Boss":
                Destroy(enemyObj);
                break;

            case "ThugBackup":
                enemyAttackAnimator.SetTrigger("ThugDamage");
                Destroy(enemyObj);
                yield return new WaitForSeconds(1f);
                bowAnimator.SetTrigger("ShootArrow");
                enemyImpactAnimator.SetTrigger("ArrowExplosion");
                enemyAttackAnimator.SetTrigger("ThugDeath");
                break;
            case "SumoBackup":
                enemyAttackAnimator.SetTrigger("SumoDamage");
                Destroy(enemyObj);
                yield return new WaitForSeconds(1f);
                bowAnimator.SetTrigger("ShootArrow");
                enemyImpactAnimator.SetTrigger("ArrowExplosion");
                enemyAttackAnimator.SetTrigger("SumoDeath");
                break;            
        }
    }

    private IEnumerator ReceivingDamage()
    {
        //shootAtClickPosition.shootMode = ShootAtClickPosition.ShootMode.ShootToHUDPosition;
        for (int i = 0; i < 12; i++)
        {            
            if (beingAttacked)
            {                
                StartCoroutine(nameof(DamagePlayer), 1);
                yield return new WaitForSeconds(1.5f);
            }
            else
            {
                beingAttacked = false; checkFirst = false;
                enemyAttackAnimator.SetBool("SumoDoingDamage", beingAttacked);
                StopCoroutine(nameof(DamagePlayer));
                StopCoroutine(nameof(ReceivingDamage));
                //shootAtClickPosition.shootMode = ShootAtClickPosition.ShootMode.ShootFromBowPosition;
            }
        }
        beingAttacked = false; checkFirst = false;
        enemyAttackAnimator.SetBool("SumoDoingDamage", beingAttacked);
        StopCoroutine(nameof(DamagePlayer));
        StopCoroutine(nameof(ReceivingDamage));
        //shootAtClickPosition.shootMode = ShootAtClickPosition.ShootMode.ShootFromBowPosition;
    }

    public IEnumerator StopReceivingDamage()
    {
        yield return new WaitForSeconds(0);
        if (beingAttacked)
        {
            beingAttacked = false; checkFirst = false;
            yield return new WaitForSeconds(.5f);

            enemyAttackAnimator.SetBool("Sumo_Attacking", beingAttacked);
            enemyAttackAnimator.SetBool("Thug_Attacking", beingAttacked);
            StopCoroutine(nameof(DamagePlayer));
            StopCoroutine(nameof(ReceivingDamage));            
        }
    }

    public void SetGameState(string state, bool stateBool)
    {
        if (stateBool) Time.timeScale = 0f; else Time.timeScale = 1f;
        switch (state)
        {
            case "Pause":
                print("Pause");
                break;
            case "Loose":
                print("Loose");
                break;
        }
    }

}
