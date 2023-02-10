using UnityEngine;

namespace Steerings
{

    public class ArriveAndSpin : SteeringBehaviour
    {
        public GameObject target;
        public override GameObject GetTarget()
        {
            return target;
        }
        public override Vector3 GetLinearAcceleration()
        {
            return Arrive.GetLinearAcceleration(Context,target);
        }
        public override float GetAngularAcceleration()
        {
            return ArriveAndSpin.GetAngularAcceleration(Context,target);
        }
        public static float GetAngularAcceleration(SteeringContext me, GameObject target)
        {
            Vector3 directionToTarget = target.transform.position - me.transform.position;
            float distanceToTarget = directionToTarget.magnitude;

            if (distanceToTarget < me.closeEnoughRadius) return 0;

            if (distanceToTarget > me.slowdownRadius) return SeekAndSpin.GetAngularAcceleration(me);

            float desiredSpeed = me.maxAngularSpeed * (distanceToTarget / me.slowdownRadius);
            float requiredAcceleration = (desiredSpeed - me.angularSpeed) / me.timeToDesiredSpeed;

            if (requiredAcceleration > me.maxAngularAcceleration)
                requiredAcceleration = me.maxAngularAcceleration;

            return requiredAcceleration;
        }

    }
}