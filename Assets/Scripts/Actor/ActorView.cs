using System;
using System.Collections.Generic;
using Root;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Actor
{
    public class ActorView: MonoBehaviour
    {
        private PlayerModel _playerModel = new PlayerModel();
        private List<ActorTag> _actorsInView = new List<ActorTag>();
        private void Start()
        {
            _playerModel = GetComponent<ActorTag>().PlayerModel;
        }

        public List<ActorTag> GetUnitsInViewPoint()
        {
            _actorsInView.Clear();
            var angleIncrease = _playerModel.PointOfView / _playerModel.RayCount;
            for (int i = -_playerModel.RayCount/2; i <= _playerModel.RayCount/2; i++)
            {
                var hits = Physics2D.RaycastAll(transform.position, Utils.GetVectorFromAngle(angleIncrease * i + transform.rotation.eulerAngles.z),
                    _playerModel.RadiusOfView);
                foreach (var hit in hits)
                {
                    var unitModel = hit.collider.GetComponent<ActorTag>();
                    // ReSharper disable once Unity.NoNullPropagation
                    if(unitModel?.PlayerModel != null && unitModel.PlayerModel != _playerModel && !_actorsInView.Contains(unitModel))
                        _actorsInView.Add(unitModel);
                }
            }

            return _actorsInView;
        }

        private void OnDrawGizmos()
        {
            var angleIncrease = _playerModel.PointOfView / _playerModel.RayCount;
            for (int i = -_playerModel.RayCount / 2; i <= _playerModel.RayCount/2; i++)
            {
                Debug.DrawRay(transform.position, Utils.GetVectorFromAngle(angleIncrease * i + transform.rotation.eulerAngles.z)* _playerModel.RadiusOfView);
            }
        }
    }
}