using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrassPresenter : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] private Transform grassContainer;
    [SerializeField] private GameObject g1;
    [SerializeField] private GameObject g2;
    [SerializeField] private GameObject g3;
    [SerializeField] private GameObject g4;
    [SerializeField] private GameObject g5;
    [SerializeField] private GameObject g6;
    
    
    void Start()
    {
        spawn(g1, grassContainer, 30, 1);
        spawn(g2, grassContainer, 30, 1);
        spawn(g3, grassContainer, 30, 1);
        spawn(g4, grassContainer, 30, 1);
        spawn(g5, grassContainer, 30, 1);
        spawn(g6, grassContainer, 30, 1);
    }

    private void spawn(GameObject go, Transform container, int maxAmount, int maxGroup)
    {
        for (int i = 0; i < maxAmount;)
        {
            float x = Random.Range(-45, 45);
            float y = Random.Range(10, 90);
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
