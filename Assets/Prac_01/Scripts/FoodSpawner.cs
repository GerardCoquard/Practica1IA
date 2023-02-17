using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Steerings {
    public class FoodSpawner : MonoBehaviour
    {
        public float radius;
        public int amountOfFood;
        public GameObject prefab;
        static List<Vector3> points = new List<Vector3>();
        static List<float> radios = new List<float>();
        private void OnDrawGizmos() {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(transform.position,radius);
            Gizmos.color = Color.blue;
            for (int i = 0; i < points.Count; i++)
            {
                Gizmos.DrawWireSphere(points[i],radios[i]);
            }
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
            points.Add(point);
            radios.Add(_radius);
            Collider[] col = Physics.OverlapSphere(point,_radius, foodLayer);
            foreach (var item in col)
            {
                Debug.Log(item.name);
            }
            return col.Length > 0;
        }
        IEnumerator SetAll(List<GameObject> foods)
        {
            bool firstFrame = false;
            while(firstFrame) {firstFrame = false; yield return null;}

            foreach (GameObject item in foods)
            {
                Food f = item.GetComponent<Food>();
                SetPosition(item,f.mySpawner,f.radius,f.whatIsFood);
            }
        }
        public static void SetPosition(GameObject target, FoodSpawner spawner, float _radius, LayerMask foodLayer)
        {
            int n = 0;
            int angle = Random.Range(0,361);
            float distance = Random.Range(0f,spawner.radius);
            Vector3 desiredPosition =  (Utils.OrientationToVector(angle).normalized * distance) + spawner.transform.position;
            while(Overlapping(desiredPosition,_radius,foodLayer))
            {
                n++;
                Debug.Log(n);
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
