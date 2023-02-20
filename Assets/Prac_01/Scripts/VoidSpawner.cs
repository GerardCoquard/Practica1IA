using UnityEngine;

namespace Steerings
{
    public class VoidSpawner : Steerings.GroupManager
    {
        public int numInstances = 20;
        public GameObject prefab;
        int created = 0;
        void Update()
        {
            Spawn();
        }
        private void Spawn ()
        {
            if (created == numInstances) return;

            GameObject clone = Instantiate(prefab);
            clone.transform.position = transform.position;
            AddBoid(clone);
            created++;
        }
    }
}

