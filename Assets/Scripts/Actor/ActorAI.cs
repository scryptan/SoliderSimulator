using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Actor
{
    public class ActorAI : MonoBehaviour
    {
        private List<ActorTag> _actors = new List<ActorTag>();
        private List<ActorTag> _actorsToRemove = new List<ActorTag>();

        private void FixedUpdate()
        {
            foreach (var actor in _actors.Where(x => x != null))
            {
                var actorsInViewPoint = actor.ActorView.GetUnitsInViewPoint()
                    .Where(x => x.PlayerModel.Color != actor.PlayerModel.Color &&
                                actor.PlayerModel.State != ActorState.Finished).ToList();
                switch (actor.PlayerModel.State)
                {
                    case ActorState.Created:
                        actor.PlayerModel.State = ActorState.Initial;
                        break;
                    case ActorState.Initial:
                        actor.ActorMover.SetVelocityVector(actor.ActorMover.transform.right);
                        var a = _actors.Count(x => x != null &&
                                                   x.PlayerModel.Color != actor.PlayerModel.Color &&
                                                   x.PlayerModel.State != ActorState.Finished);
                        if (a > 0)
                            LookOnEnemy(actor,
                                _actors.Where(x => x.PlayerModel.Color != actor.PlayerModel.Color &&
                                                   x.PlayerModel.State != ActorState.Finished).ToList());
                        else
                        {
                            actor.PlayerModel.State = ActorState.RunHome;
                            break;
                        }

                        ChangeStateByCountOfEnemy(actor, actorsInViewPoint);
                        ChangeStateByCountOfHealth(actor);
                        break;
                    case ActorState.FullyAttack:
                        LookOnEnemy(actor, actorsInViewPoint);
                        actor.ActorMover.SetVelocityVector(actor.ActorMover.transform.right);
                        actor.Weapon.Attack();
                        ChangeStateByCountOfEnemy(actor, actorsInViewPoint);
                        ChangeStateByCountOfHealth(actor);
                        break;
                    case ActorState.StopAndAttack:
                        LookOnEnemy(actor, actorsInViewPoint);
                        actor.Weapon.Attack();
                        ChangeStateByCountOfEnemy(actor, actorsInViewPoint);
                        ChangeStateByCountOfHealth(actor);
                        break;
                    case ActorState.Defence:
                        LookOnEnemy(actor, actorsInViewPoint);
                        actor.ActorMover.SetVelocityVector(-actor.ActorMover.transform.right);
                        actor.Weapon.Attack();
                        ChangeStateByCountOfEnemy(actor, actorsInViewPoint);
                        ChangeStateByCountOfHealth(actor);
                        break;
                    case ActorState.RunHome:
                        actor.ActorMover.SetVelocityVector(actor.PlayerModel.Color == ActorColor.Blue
                            ? -Vector2.right
                            : Vector2.right);
                        ChangeStateByCountOfHealth(actor);
                        break;
                    case ActorState.RunHomeSlowly:
                        actor.ActorMover.SetVelocityVector(
                            actor.PlayerModel.Color == ActorColor.Blue ? -Vector2.right : Vector2.right, 2 / 3f);
                        ChangeStateByCountOfHealth(actor);
                        break;
                    case ActorState.MakeMedicineHelp:
                        break;
                    case ActorState.MakeArmoryHelp:
                        break;
                    case ActorState.Finished:
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }

            foreach (var actor in _actorsToRemove)
            {
                _actors.Remove(actor);
            }

            _actorsToRemove.Clear();
        }

        public void AddActors(params ActorTag[] actorInfos)
        {
            _actors = _actors.Union(actorInfos).ToList();
        }

        private void LookOnEnemy(IActorInfo actor, List<ActorTag> actorTags)
        {
            actorTags = actorTags.Where(x => x != null).ToList();
            if (actorTags.Count > 0)
            {
                var dir = actorTags[Random.Range(0, actorTags.Count)].transform.position -
                          actor.ActorMover.transform.position;

                var angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
                actor.ActorMover.Rotate(angle);
            }
        }

        private void ChangeStateByCountOfEnemy(ActorTag actor, List<ActorTag> actorTags)
        {
            switch (actorTags.Count)
            {
                case 0:
                    ChangeState(actor, ActorState.Initial);
                    return;
                case 1:
                    ChangeState(actor, ActorState.FullyAttack);
                    return;
                case 2:
                    ChangeState(actor, ActorState.StopAndAttack);
                    return;
                default:
                    ChangeState(actor, ActorState.Defence);
                    break;
            }
        }

        private void ChangeStateByCountOfHealth(ActorTag actor)
        {
            if (actor.PlayerModel.Health <= 0)
            {
                _actorsToRemove.Add(actor);
                ChangeState(actor, ActorState.Finished);
                Destroy(actor.ActorMover.gameObject);
            }
            else if (actor.PlayerModel.Health < 30)
                ChangeState(actor, ActorState.RunHomeSlowly);
            else if (actor.PlayerModel.Health < 60)
                ChangeState(actor, ActorState.RunHome);
        }

        private void ChangeState(ActorTag actor, ActorState newState)
        {
            actor.PlayerModel.State = newState;
        }
    }
}