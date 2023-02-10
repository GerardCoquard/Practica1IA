using UnityEngine;

namespace Steerings
{

    public class KeepDistance : SteeringBehaviour
    {
   
        public GameObject target;
        public float requiredDistance;

        public override GameObject GetTarget()
        {
            return target;
        }
        public override Vector3 GetLinearAcceleration()
        {
            return KeepDistance.GetLinearAcceleration(Context,target,requiredDistance);
        }

        
        public static Vector3 GetLinearAcceleration (SteeringContext me, GameObject target, float requiredDistance)
        {
            Vector3 directionFromTarget = me.transform.position - target.transform.position;
            Vector3 displacementFromTarget = directionFromTarget.normalized * requiredDistance;
            Vector3 desiredPosition = target.transform.position + displacementFromTarget;
            SURROGATE_TARGET.transform.position = desiredPosition;

            return Arrive.GetLinearAcceleration(me, SURROGATE_TARGET);
        
        }

    }
}