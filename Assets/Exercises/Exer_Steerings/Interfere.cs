using UnityEngine;

namespace Steerings
{

    public class Interfere : SteeringBehaviour
    {
        public GameObject target;
        public float requiredDistance;
        public override GameObject GetTarget()
        {
            return target;
        }
        public override Vector3 GetLinearAcceleration()
        {
            return Interfere.GetLinearAcceleration(Context,target,requiredDistance);
        }
        public static Vector3 GetLinearAcceleration (SteeringContext me, GameObject target, float distance)
        {
            if(target.GetComponent<SteeringContext>() == null)
            {
                Debug.LogWarning("Target doesn't have a SteeringContext");
                return Vector3.zero;
            }

            SteeringContext targetContext = target.GetComponent<SteeringContext>();
            Vector3 distanceFromTarget = targetContext.velocity.normalized * distance;
            SURROGATE_TARGET.transform.position = target.transform.position + distanceFromTarget;
            return Arrive.GetLinearAcceleration(me,SURROGATE_TARGET);
        }

    }
}