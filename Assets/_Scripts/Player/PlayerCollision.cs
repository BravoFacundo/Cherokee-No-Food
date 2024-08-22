using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCollision : MonoBehaviour
{
    public PlayerController playerController;

    private void OnTriggerEnter(Collider col)
    {
        GameObject colObj = GetTaggedParentName(col.transform);
        string colName = colObj.name;

        switch (colName)
        {
            //Este caso debe ser borrado luego
            case string name when name.StartsWith("Projectile"):
                print("Projectile ataca");
                StartCoroutine(playerController.DamagePlayer(1));
                Destroy(colObj); //Provisional
                break;

            //---------------Deployable Damage---------------//

            case string name when name.StartsWith("Bomb"):
                print("Bomb Impact");
                StartCoroutine(playerController.DamagePlayer(3));
                Destroy(colObj); //Provisional
                break;

            //---------------Projectile Damage---------------//

            case string name when name.StartsWith("Shuriken"):
                print("Shuriken Impact");
                StartCoroutine(playerController.DamagePlayer(2));
                Destroy(colObj); //Provisional
                break;

            case string name when name.StartsWith("Knife"):
                print("Knife Impact");
                StartCoroutine(playerController.DamagePlayer(2));
                Destroy(colObj); //Provisional
                break;

            //---------------Imposibble Enemy Damage--------------------//

            case string name when name.StartsWith("Ninja"):
                StartCoroutine(playerController.EnemyAttack(0, "Ninja", colObj));
                break;

            case string name when name.StartsWith("Boss"):
                StartCoroutine(playerController.EnemyAttack(0, "Boss", colObj));
                break;

        }

    }

    private GameObject GetTaggedParentName(Transform child)
    {
        if (!child.CompareTag("Untagged")) return child.gameObject;

        Transform parent = child.parent;
        while (parent != null)
        {
            if (!parent.CompareTag("Untagged"))
            {
                return parent.gameObject;
            }
            parent = parent.parent;
        }
        return null;
    }
}
