using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Steerings
{

    public class SteeringsCombinationBase : SteeringBehaviour
    {
        List<SteeringBehaviour> steerings;
        private void Awake() {
            steerings =  new List<SteeringBehaviour>(GetComponents<SteeringBehaviour>());
            steerings.Remove(this);
            foreach (SteeringBehaviour behaviour in steerings) {behaviour.enabled = false;}
        }

        public override GameObject GetTarget()
        {
            return GetCurrentBehaviour().GetTarget();
        }

        public override Vector3 GetLinearAcceleration()
        {
            return GetCurrentBehaviour().GetLinearAcceleration();
        }

        public override float GetAngularAcceleration()
        {
            return GetCurrentBehaviour().GetAngularAcceleration();
        }

        public virtual SteeringBehaviour GetCurrentBehaviour()
        {
            for (int i = 0; i < steerings.Count; i++)
            {
                if(steerings[i].GetLinearAcceleration() != Vector3.zero) return steerings[i];
            }
            return steerings[steerings.Count-1];
        }
    }
}
