using UnityEngine;

namespace Actor
{
    public interface IActorMover
    {
        void SetVelocityVector(Vector2 direction, float speedFactor = 1);
        void Rotate(float angle);
        void SetBound(Vector2 leftTop, Vector2 rightBottom);
    }
}