using System;
using System.Collections.Generic;
using UnityEngine;

namespace Actor
{
    [Serializable]
    public class PlayerModel
    {
        public string Name;
        public float Speed;
        public float Health;
        public float PointOfView;
        public float RadiusOfView;
        public int RayCount;
        public ActorState State;
        public ActorColor Color;

        public void GetDamage(float damage)
        {
            Health -= damage;
            if (Health < 0)
                Health = 0;
        }
        
        public override string ToString()
        {
            return $"{nameof(Speed)}: {Speed}, {nameof(Health)}: {Health}, {nameof(PointOfView)}: {PointOfView}, " +
                   $"{nameof(RadiusOfView)}: {RadiusOfView}, {nameof(State)}: {State}, {nameof(Color)}: {Color}, {nameof(RayCount)}: {RayCount}";
        }
    }
}