using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCollision : MonoBehaviour
{
    [SerializeField] private PlayerController playerController;

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

            //---------------Enemy Damage--------------------//

            case string name when name.StartsWith("Thug"):
                StartCoroutine(playerController.EnemyAttack(1, "Thug", colObj));
                break;

            case string name when name.StartsWith("Sumo"):
                StartCoroutine(playerController.EnemyAttack(1, "Sumo", colObj));
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
