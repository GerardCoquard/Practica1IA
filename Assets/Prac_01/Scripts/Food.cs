using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Steerings {
    public class Food : MonoBehaviour
    {
        public FoodSpawner mySpawner;
        private void OnDisable() {
            if(!mySpawner.Equals(null))
            {
                FoodSpawner.SetPosition(gameObject,mySpawner);
                transform.SetParent(mySpawner.transform);
            }
        }
    }
}
