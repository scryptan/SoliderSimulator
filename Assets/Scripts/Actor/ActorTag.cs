using System;
using UnityEngine;
using Weapon;

namespace Actor
{
    public class ActorTag: MonoBehaviour, IActorInfo
    {
        public PlayerModel PlayerModel { get; set; }
        public ActorView ActorView { get; set; }
        public ActorMover ActorMover { get; set; }
        public IWeapon Weapon { get; set; }
    }
}