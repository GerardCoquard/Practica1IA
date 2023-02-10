using UnityEngine;

namespace Steerings
{

    public class Interpose : SteeringBehaviour
    {
        public GameObject target;
        public GameObject secondTarget;

        public override GameObject GetTarget()
        {
            return target;
        }
        public override Vector3 GetLinearAcceleration()
        {
            return Interpose.GetLinearAcceleration(Context, target,secondTarget);
        }
        public static Vector3 GetLinearAcceleration(SteeringContext me, GameObject target1, GameObject target2)
        {
            Vector3 desiredMiddlePosition = (target1.transform.position + target2.transform.position)/2;
            SURROGATE_TARGET.transform.position = desiredMiddlePosition;
            return Arrive.GetLinearAcceleration(me, SURROGATE_TARGET);
        }
    }
}