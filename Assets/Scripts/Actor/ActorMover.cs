using UnityEngine;

namespace Actor
{
    public class ActorMover : MonoBehaviour, IActorMover
    {
        private float defaultSpeed;
        private float speed;
        private Vector3 velocity;
        private Vector2 leftTopBoundPosition;
        private Vector2 rightBottomBoundPosition;

        private void Awake()
        {
            defaultSpeed = 2f;
            speed = defaultSpeed;
        }

        void FixedUpdate()
        {
            var nextPos = transform.localPosition + velocity * (speed * Time.deltaTime);
            if (IsInBound(nextPos))
                transform.localPosition = nextPos;
        }

        public void SetVelocityVector(Vector2 direction, float speedFactor = 1)
        {
            speed = defaultSpeed * speedFactor;
            velocity = direction;
        }

        public void Rotate(float angle)
        {
            transform.rotation = Quaternion.Euler(0f, 0f, angle);
        }

        public void SetBound(Vector2 leftTop, Vector2 rightBottom)
        {
            leftTopBoundPosition = leftTop;
            rightBottomBoundPosition = rightBottom;
        }

        private bool IsInBound(Vector2 nextPos)
        {
            return nextPos.x >= leftTopBoundPosition.x &&
                   nextPos.x <= rightBottomBoundPosition.x &&
                   nextPos.y >= rightBottomBoundPosition.y &&
                   nextPos.y <= leftTopBoundPosition.y;
        }
    }
}