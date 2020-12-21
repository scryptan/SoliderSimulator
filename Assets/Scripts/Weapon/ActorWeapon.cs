using System;
using System.Collections.Generic;
using Actor;
using UnityEngine;

namespace Weapon
{
    public class ActorWeapon : MonoBehaviour, IWeapon
    {
        private WeaponType currentWeapon;

        private Dictionary<WeaponType, WeaponInfo> _weaponInfos = new Dictionary<WeaponType, WeaponInfo>
        {
            {
                WeaponType.Pistol, new WeaponInfo
                {
                    Damage = 5,
                    BulletSpeed = 5,
                    FireRate = 2
                }
            },
            {
                WeaponType.Automata, new WeaponInfo
                {
                    Damage = 10,
                    BulletSpeed = 15,
                    FireRate = 0.5f
                }
            },
            {
                WeaponType.Sniper, new WeaponInfo
                {
                    Damage = 30,
                    BulletSpeed = 30,
                    FireRate = 4
                }
            },
        };

        private float _lastTimeFire;
        private Sprite _sprite;
        private PlayerModel _model;

        private void Start()
        {
            _model = GetComponent<ActorTag>().PlayerModel;
        }

        private void Update()
        {
            _lastTimeFire += Time.deltaTime;
        }

        public void SetWeapon(WeaponType weaponType)
        {
            currentWeapon = weaponType;
        }

        public void SetSprite(Sprite sprite)
        {
            _sprite = sprite;
        }

        public void Attack()
        {
            var currWeapon = _weaponInfos[currentWeapon];
            if (_lastTimeFire > currWeapon.FireRate && _model.Ammo > 0)
            {
                _model.Ammo--;
                SpawnBullet(currWeapon);
                _lastTimeFire = 0;
            }
        }

        private void SpawnBullet(WeaponInfo currWeapon)
        {
            var bullet = new GameObject("bullet");
            bullet.transform.rotation = transform.rotation;
            bullet.transform.position = transform.position + transform.right * 0.5f;
            
            bullet.transform.localScale = Vector3.one * 0.02f;
            var spriteRenderer = bullet.AddComponent<SpriteRenderer>();
            spriteRenderer.sprite = _sprite;
            spriteRenderer.color = Color.yellow;

            var bulletComponent = bullet.AddComponent<Bullet>();
            bulletComponent.Damage = currWeapon.Damage;
            bulletComponent.Speed = currWeapon.BulletSpeed;

            bullet.AddComponent<CircleCollider2D>();
        }
    }
}