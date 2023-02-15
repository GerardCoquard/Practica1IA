using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Steerings {
    public class FoodSpawner : MonoBehaviour
    {
        public float radius;
        public int amountOfFood;
        public GameObject prefab;
        public LayerMask whatIsFood;
        private void Start() {
            for (int i = 0; i < amountOfFood; i++)
            {
                GameObject f = Instantiate(prefab,transform);
                f.GetComponent<Food>().mySpawner = this;
                FoodSpawner.SetPosition(f,this);
            }
        }
        public static void SetPosition(GameObject target, FoodSpawner spawner)
        {
            int angle = Random.Range(0,361);
            float distance = Random.Range(0f,spawner.radius);
            Vector3 desiredPosition =  Utils.OrientationToVector(angle).normalized * distance + spawner.transform.position;
            while(Overlapping(desiredPosition))
            {
                angle = Random.Range(0,361);
                distance = Random.Range(0f,spawner.radius);
                desiredPosition =  Utils.OrientationToVector(angle).normalized * distance + spawner.transform.position;
            }
            if(spawner.gameObject.activeSelf) spawner.StartCoroutine(spawner.SetActive(target));
        }
        static bool Overlapping(Vector3 point)
        {
            Vector3 cameraPos = Camera.main.gameObject.transform.position;
            Vector3 direction = point - cameraPos;
            return Physics.Raycast(cameraPos,direction,Mathf.Infinity, LayerMask.NameToLayer("Food"));
        }
        IEnumerator SetActive(GameObject target)
        {
            yield return new WaitForEndOfFrame();
            target.SetActive(true);
        }
    }
}
