using UnityEngine;

namespace Weapon
{
    public interface IWeapon
    {
        void Attack();

        void SetWeapon(WeaponType weaponType);

        void SetSprite(Sprite sprite);
    }
}