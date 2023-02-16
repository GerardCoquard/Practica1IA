using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Steerings {
    public class Food : MonoBehaviour
    {
        public float radius;
        bool quitting;
        public FoodSpawner mySpawner;
        public LayerMask whatIsFood;
        private void OnDisable() {
            if(!mySpawner.Equals(null) && !quitting)
            {
                StartCoroutine(FoodSpawner.SetPosition(gameObject,mySpawner,radius,whatIsFood));
            }
        }
        private void OnApplicationQuit() {
            quitting = true;
        }
    }
}
