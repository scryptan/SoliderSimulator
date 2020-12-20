using Actor;
using UnityEngine;

namespace Weapon
{
    public class Bullet : MonoBehaviour
    {
        public float Speed = 2f;
        public float Damage = 10f;

        private void Update()
        {
            transform.position += transform.right * (Speed * Time.deltaTime);
        }

        private void OnCollisionEnter2D(Collision2D other)
        {
            var actor = other.collider.GetComponent<ActorTag>();
            if (actor != null)
                actor.PlayerModel.GetDamage(Damage);
            Destroy(gameObject);
        }
    }
}