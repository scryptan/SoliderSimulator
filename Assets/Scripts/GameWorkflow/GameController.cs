using System.Collections.Generic;
using System.Linq;
using Actor;
using UnityEngine;
using UnityEngine.SceneManagement;
using Weapon;
using Random = UnityEngine.Random;

namespace GameWorkflow
{
    [RequireComponent(typeof(ActorAI))]
    public class GameController : MonoBehaviour
    {
        private List<ActorTag> _units;
        public List<PlayerModel> playerModels;
        [SerializeField] List<string> names = new List<string>();
        [SerializeField] private int actorCountOnStartGame = 1;
        [SerializeField] private int actorCountToSpawnOnStartGame = 1;
        [SerializeField] private PlayerModel defaultModel;

        [SerializeField] private Sprite sprite;
        [SerializeField] private GameObject panel;
        private ActorAI _ai;

        private float _verticalBound;
        private float _horizontalBound;
        private float _spawnVerticalBound;
        private float _spawnHorizontalBound;
        private bool _isGameEnd;

        private void Start()
        {
            playerModels = new List<PlayerModel>();
            panel.SetActive(false);
            // ReSharper disable once Unity.NoNullPropagation
            _verticalBound = Camera.main?.orthographicSize ?? 0;
            _horizontalBound = _verticalBound * Screen.width / Screen.height;

            _spawnVerticalBound = _verticalBound - 1f;
            _spawnHorizontalBound = _horizontalBound - 1f;

            _units = new List<ActorTag>();
            _ai = FindObjectOfType<ActorAI>();
            CreateActors(actorCountToSpawnOnStartGame, ActorColor.Red);
            CreateActors(actorCountToSpawnOnStartGame, ActorColor.Blue);

            StartGame(playerModels.ToArray());
            CreateActors(actorCountOnStartGame - actorCountToSpawnOnStartGame, ActorColor.Red);
            CreateActors(actorCountOnStartGame - actorCountToSpawnOnStartGame, ActorColor.Blue);
        }

        private void Update()
        {
            if (!_isGameEnd)
            {
                foreach (var unit in _units.Where(x => x != null))
                {
                    if (unit.PlayerModel.Color == ActorColor.Blue)
                    {
                        if (unit.transform.position.x + _horizontalBound < 0.1f)
                            MakeActorFinished(unit);
                    }
                    else
                    {
                        if (_horizontalBound - unit.transform.position.x < 0.1f)
                            MakeActorFinished(unit);
                    }
                }

                if (playerModels.All(x => x.State == ActorState.Finished || x.State == ActorState.Created))
                {
                    _isGameEnd = true;
                    panel.SetActive(true);
                }
            }
        }

        private void MakeActorFinished(ActorTag actor)
        {
            actor.PlayerModel.State = ActorState.Finished;
            Destroy(actor.gameObject);
        }

        private List<PlayerModel> CreateActors(int count, ActorColor color)
        {
            var actors = new List<PlayerModel>();
            for (int i = 0; i < count; i++)
            {
                actors.Add(new PlayerModel
                {
                    Name = $"{color} {names[Random.Range(0, names.Count)]}",
                    DefaultAmmo = defaultModel.DefaultAmmo,
                    Ammo = defaultModel.DefaultAmmo,
                    Color = color,
                    Health = defaultModel.Health,
                    Speed = defaultModel.Speed,
                    State = ActorState.Created,
                    PointOfView = defaultModel.PointOfView,
                    RadiusOfView = defaultModel.RadiusOfView,
                    RayCount = defaultModel.RayCount
                });
            }

            playerModels.AddRange(actors);
            return actors;
        }

        private void StartGame(params PlayerModel[] models)
        {
            foreach (var model in models)
            {
                _units.Add(SpawnUnit(model));
            }

            _ai.AddActors(_units.ToArray());
        }

        public ActorTag SpawnUnit(PlayerModel model)
        {
            var newGm = new GameObject(model.Name);
            newGm.transform.localScale = Vector3.one * 0.1f;
            newGm.transform.position = model.Color == ActorColor.Blue
                ? new Vector3(Random.Range(-_spawnHorizontalBound, -_spawnHorizontalBound + 0.2f),
                    Random.Range(-_spawnVerticalBound, _spawnVerticalBound))
                : new Vector3(Random.Range(_spawnHorizontalBound, _spawnHorizontalBound - 0.2f),
                    Random.Range(-_spawnVerticalBound, _spawnVerticalBound));
            newGm.transform.rotation = model.Color == ActorColor.Blue
                ? Quaternion.Euler(Vector3.zero)
                : Quaternion.Euler(0, 0, 180);

            var spriteRenderer = newGm.AddComponent<SpriteRenderer>();
            spriteRenderer.sprite = sprite;
            spriteRenderer.color = model.Color == ActorColor.Blue ? Color.blue : Color.red;

            var collider = newGm.AddComponent<CircleCollider2D>();
            var rigdbody = newGm.AddComponent<Rigidbody2D>();
            rigdbody.gravityScale = 0;

            var actorTag = newGm.AddComponent<ActorTag>();
            actorTag.PlayerModel = model;

            actorTag.ActorMover = newGm.AddComponent<ActorMover>();
            actorTag.ActorMover.SetBound(new Vector2(-_horizontalBound, _verticalBound),
                new Vector2(_horizontalBound, -_verticalBound));

            actorTag.ActorView = newGm.AddComponent<ActorView>();
            actorTag.Weapon = newGm.AddComponent<ActorWeapon>();
            actorTag.Weapon.SetSprite(sprite);
            actorTag.Weapon.SetWeapon(WeaponType.Automata);

            return actorTag;
        }

        public void RestartGame()
        {
            SceneManager.LoadScene(0);
        }

        public void SpawnRed()
        {
            if (_units.Count(x => x.PlayerModel.Color == ActorColor.Red) < actorCountOnStartGame)
            {
                var unit = SpawnUnit(playerModels.First(x => x.State == ActorState.Created && x.Color == ActorColor.Red));
                _ai.AddActors(unit);
                _units.Add(unit);
            }
                
        }

        public void SpawnBlue()
        {
            if (_units.Count(x => x.PlayerModel.Color == ActorColor.Blue) < actorCountOnStartGame)
            {
                var unit = SpawnUnit(playerModels.First(x => x.State == ActorState.Created && x.Color == ActorColor.Blue));
                _ai.AddActors(unit);
                _units.Add(unit);
            }
        }
    }
}