using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Steerings {
    public class FoodSpawner : MonoBehaviour
    {
        public float radius;
        public int amountOfFood;
        public GameObject prefab;
        private void OnDrawGizmos() {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(transform.position,radius);
        }
        private void Start() {
            List<GameObject> foods = new List<GameObject>();
            for (int i = 0; i < amountOfFood; i++)
            {
                GameObject f = Instantiate(prefab);
                Food food = f.GetComponent<Food>();
                food.mySpawner = this;
                foods.Add(f);
            }
            StartCoroutine(SetAll(foods));
        }
        static bool Overlapping(Vector3 point, float _radius, LayerMask foodLayer)
        {
            return Physics.CheckSphere(point,_radius, foodLayer);
        }
        IEnumerator SetAll(List<GameObject> foods)
        {
            bool firstFrame = false;
            while(firstFrame) {firstFrame = false; yield return null;}

            while(foods.Count > 0)
            {
                GameObject item = foods[foods.Count-1];
                Food f = item.GetComponent<Food>();
                StartCoroutine(SetPosition(item,f.mySpawner,f.radius,f.whatIsFood));
                foods.Remove(item);
                yield return null;
            }
        }
        public static IEnumerator SetPosition(GameObject target, FoodSpawner spawner, float _radius, LayerMask foodLayer)
        {
            yield return new WaitForEndOfFrame();
            int angle = Random.Range(0,361);
            float distance = Random.Range(0f,spawner.radius);
            Vector3 desiredPosition =  (Utils.OrientationToVector(angle).normalized * distance) + spawner.transform.position;
            while(Overlapping(desiredPosition,_radius,foodLayer))
            {
                angle = Random.Range(0,361);
                distance = Random.Range(0f,spawner.radius);
                desiredPosition =  Utils.OrientationToVector(angle).normalized * distance + spawner.transform.position;
            }
            target.transform.position = desiredPosition;
            target.SetActive(true);
            target.transform.SetParent(spawner.transform);
        }
    }
}
