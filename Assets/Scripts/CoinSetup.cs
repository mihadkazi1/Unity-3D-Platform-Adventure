using UnityEngine;

public class CoinSetup : MonoBehaviour
{
    void Start()
    {
        GameObject[] coins = GameObject.FindGameObjectsWithTag("Coin");

        foreach (GameObject coin in coins)
        {
            Coin coinScript = coin.GetComponent<Coin>();

            if (coinScript == null)
            {
                coinScript = coin.AddComponent<Coin>();
            }

            Collider col = coin.GetComponent<Collider>();

            if (col == null)
            {
                SphereCollider sphere = coin.AddComponent<SphereCollider>();
                sphere.isTrigger = true;
            }
            else
            {
                col.isTrigger = true;
            }
        }

        Debug.Log("All coins have been configured.");
    }
}