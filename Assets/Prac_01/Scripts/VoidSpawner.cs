using UnityEngine;

namespace Steerings
{
    public class VoidSpawner : Steerings.GroupManager
    {
        public int numInstances = 20;
        public float delay = 0.5f;
        public GameObject prefab;
        public GameObject atractor;

        private int created = 0;
        private float elapsedTime = 0f;

        void Update()
        {
            Spawn();
        }

        private void Spawn ()
        {
            if (created == numInstances) return;

            if (elapsedTime < delay)
            {
                elapsedTime += Time.deltaTime;
                return;
            }

            // if this point is reached, it's time to spawn a new instance
            GameObject clone = Instantiate(prefab);
            clone.transform.position = transform.position;
            clone.GetComponent<FlockingAround>().attractor = atractor;

            AddBoid(clone);
            created++;
            elapsedTime = 0f;
        }
    }
}

