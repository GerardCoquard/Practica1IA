using UnityEngine;

namespace Steerings
{

    public class KeepPosition : SteeringBehaviour
    {

        public GameObject target;
        public float requiredDistance;
        public float requiredAngle;

        public override GameObject GetTarget()
        {
            return target;
        }

        public override Vector3 GetLinearAcceleration()
        {
            return KeepPosition.GetLinearAcceleration(Context,target,requiredDistance,requiredAngle);
        }

        public static Vector3 GetLinearAcceleration (SteeringContext me, GameObject target,
                                                     float distance, float angle)
        {
            float desiredAngle = target.transform.eulerAngles.z + angle;
            Vector3 desiredPointFromTarget =  Utils.OrientationToVector(desiredAngle).normalized * distance;
            SURROGATE_TARGET.transform.position = target.transform.position + desiredPointFromTarget;
            
            return Arrive.GetLinearAcceleration(me,SURROGATE_TARGET);
        }

    }
}