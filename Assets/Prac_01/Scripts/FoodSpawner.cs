using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Steerings {
    public class FoodSpawner : MonoBehaviour
    {
        public float radius;
        public int amountOfFood;
        public GameObject prefab;
        private void Start() {
            for (int i = 0; i < amountOfFood; i++)
            {
                Instantiate(prefab).GetComponent<Food>().mySpawner = this;
            }
        }
        public void SetPosition(GameObject target)
        {
            int angle = Random.Range(0,361);
            float distance = Random.Range(0f,radius);
            Vector3 desiredPointFromTarget =  Utils.OrientationToVector(angle).normalized * distance;
            target.transform.position = transform.position + desiredPointFromTarget;
        }
    }
}
