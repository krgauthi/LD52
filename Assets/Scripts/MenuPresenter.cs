using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuPresenter : MonoBehaviour
{
    
    private static int sheepCount = 1000;
    [SerializeField] private GameObject sheepPrefab;
    [SerializeField] private Transform sheepContainer;
    // Start is called before the first frame update
    void Start()
    {
        //Spawn in Entities
        //spawn(sheepPrefab, sheepContainer, sheepCount, 15);
    }


    private void spawn(GameObject go, Transform container, int maxAmount, int maxGroup)
    {
        for (int i = 0; i < maxAmount;)
        {
            float x = Random.Range(-45, 45);
            float y = Random.Range(20, 90);
            var group = Random.Range(1, maxGroup) + i;
            for (; i < maxAmount && i <= group; i++)
            {
                float xMod = Random.Range(-4, 4);
                float yMod = Random.Range(-4, 4);
                GameObject instantiatedGameObject =
                    Instantiate(go, new Vector3(x + xMod, y + yMod, 0), Quaternion.identity);
                instantiatedGameObject.name = "Sheep " + i;
                instantiatedGameObject.transform.SetParent(container.transform);
            }
        }
    }
}
