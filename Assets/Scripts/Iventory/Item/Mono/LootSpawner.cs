using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LootSpawner : MonoBehaviour
{        [System.Serializable]
    public class LootItem
    {   
        public GameObject item;
        //随机概率
        [Range(0,1)]
        public float weight;
    }
    public LootItem[] lootItems;

    public void SpawnLoot()
    {
        //循环列表物品 判断权重     返回值为0-1的小数， 当小于某个权重时就将其生成
        float currentValue = Random.value;
        for( int i=0;i<lootItems.Length ; i++)
        {
            if(currentValue<=lootItems[i].weight)
            {
                //TODO:对象池中调用
                GameObject obj = Instantiate(lootItems[i].item);
                //从天而降
                obj.transform.position = transform.position + Vector3.up*2;
                //确保一次只掉落一个物品
                break;
            }
        }
    }
}
