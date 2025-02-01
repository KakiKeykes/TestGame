using Assets.Scripts.Character.Enemy;
using Assets.Scripts.Character.Player;
using Assets.Scripts.Save;
using Assets.Scripts.UI;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Assets.Scripts.GamePlay
{
    public class GameController : MonoBehaviour, ISaveable
    {
        [SerializeField] private EnemyController _enemyPrefab;
        [SerializeField] private int _enemySpawnCount;
        [SerializeField] private PlayerController _player;
        [SerializeField] private Canvas _canvas;
        [SerializeField] private HPBar _hpBar;
        [SerializeField] private GameObject _gameOverWindow;
        private bool _gameOver;

        [SerializeField] private List<EnemyController> _enemyList = new();
        [SerializeField] private List<ItemObject> _droppedItem = new();

        private void OnEnable()
        {
            EventManager.onEnemyDie += HandleEnemyDie;

        }

        private void OnDisable()
        {
            EventManager.onEnemyDie -= HandleEnemyDie;

        }

        void Start()
        {
            for(int i = 0;  i < _enemySpawnCount; i++)
            {
                var spawnPos = new Vector3(Random.Range(-10, 10), Random.Range(-10, 10));
                _enemyList.Add(Instantiate(_enemyPrefab, spawnPos, Quaternion.identity));

                _enemyList[i].HPBar = Instantiate(_hpBar, _canvas.transform.position, Quaternion.identity);
                _enemyList[i].HPBar.transform.SetParent(_canvas.transform);
                _enemyList[i].InitializeHPBar();
            }

            _player.HPBar = Instantiate(_hpBar, _canvas.transform.position, Quaternion.identity);
            _player.HPBar.transform.SetParent(_canvas.transform);
            _player.InitializeHPBar();
        }

        void Update()
        {
            if (_gameOver) return;

            if (_player == null)
            {
                GameOver();
                return;
            }

            foreach (var enemy in _enemyList)
            {
                var distance = (enemy.transform.position - _player.transform.position).magnitude;

                if (enemy.TargetDistance > distance)
                    enemy.Target = _player;
                else
                    enemy.Target = null;

                if (_player.TargetDistance > distance)
                {
                    if (_player.Target != null)
                    {
                        var targetDistance = (_player.Target.transform.position - _player.transform.position).magnitude;
                        if (targetDistance < distance)
                            continue;
                    }
                    _player.Target = enemy;
                }
            }
        }

        private void GameOver()
        {
            _gameOver = true;
            _gameOverWindow.SetActive(true);
            _gameOverWindow.GetComponent<RectTransform>().SetAsLastSibling();
        }

        private void HandleEnemyDie(EnemyController enemy)
        {
            _enemyList.Remove(enemy);
        }

        public void SaveData(SaveData data)
        {
            _player.SaveData(data);

            data.Data["EnemyCount"] = _enemyList.Count;

            for (int i = 0; i < _enemyList.Count; i++)
            {
                var enemy = _enemyList[i];
                data.Data[$"Enemy_{i}_Health"] = enemy.CurrentHealth;
                data.Data[$"Enemy_{i}_Position"] = new float[] { enemy.transform.position.x, enemy.transform.position.y, enemy.transform.position.z };
                data.Data[$"Enemy_{i}_Rotation"] = new float[] { enemy.transform.rotation.x, enemy.transform.rotation.y, enemy.transform.rotation.z, enemy.transform.rotation.w };
            }
        }

        public void LoadData(SaveData data)
        {
            _player.LoadData(data);

            if (data.Data.TryGetValue("EnemyCount", out var enemyCount))
            {
                int count = Convert.ToInt32(enemyCount);

                foreach (var enemy in _enemyList)
                {
                    Destroy(enemy.gameObject);
                }
                _enemyList.Clear();

                for (int i = 0; i < count; i++)
                {
                    var enemy = Instantiate(_enemyPrefab);
                    _enemyList.Add(enemy);

                    if (data.Data.TryGetValue($"Enemy_{i}_Health", out var health))
                    {
                        enemy.CurrentHealth = (float)(double)health;
                    }

                    if (data.Data.TryGetValue($"Enemy_{i}_Position", out var position))
                    {
                        var jArray = (JArray)position;

                        var positionArray = jArray.Select(t => (float)t).ToArray();

                        enemy.transform.position = new Vector3(positionArray[0], positionArray[1], positionArray[2]);
                    }

                    if (data.Data.TryGetValue($"Enemy_{i}_Rotation", out var rotation))
                    {
                        var jArray = (JArray)rotation;

                        var rotationArray = jArray.Select(t => (float)t).ToArray();

                        enemy.transform.rotation = new Quaternion(rotationArray[0], rotationArray[1], rotationArray[2], rotationArray[3]);
                    }

                    enemy.HPBar = Instantiate(_hpBar, _canvas.transform.position, Quaternion.identity);
                    enemy.HPBar.transform.SetParent(_canvas.transform);
                    enemy.InitializeHPBar();
                }
            }
        }
    }
}