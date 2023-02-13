using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Steerings {
    public class Food : MonoBehaviour
    {
        public FoodSpawner mySpawner;
        private void OnEnable() {
            StartCoroutine(SetPos());
        }
        
        IEnumerator SetPos()
        {
            while(mySpawner == null) yield return null;
            mySpawner.SetPosition(gameObject);
        }
    }
}
