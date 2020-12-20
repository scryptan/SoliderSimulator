using Weapon;

namespace Actor
{
    public interface IActorInfo
    {
        PlayerModel PlayerModel { get; set; }
        ActorView ActorView { get; set; }
        ActorMover ActorMover { get; set; }
        IWeapon Weapon { get; set; }
    }
}