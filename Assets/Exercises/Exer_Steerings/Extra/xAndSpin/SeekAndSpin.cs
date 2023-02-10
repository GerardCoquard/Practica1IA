using UnityEngine;

namespace Steerings
{

    public class SeekAndSpin : SteeringBehaviour
    {
        public GameObject target;

        public override GameObject GetTarget()
        {
            return target;
        }
        public override Vector3 GetLinearAcceleration()
        {
            return Seek.GetLinearAcceleration(Context,target);
        }
        public override float GetAngularAcceleration()
        {
            return SeekAndSpin.GetAngularAcceleration(Context);
        }
        public static float GetAngularAcceleration(SteeringContext me)
        {
            return me.maxAngularAcceleration;
        }
    }
}