using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

using CardNSlash;

public class Spawner : MonoBehaviour {


    [Header("Positions")]
    [SerializeField] private float _top;
    [SerializeField] private float _topLength;
    [SerializeField] private float _right;
    [SerializeField] private float _rightLength;

    [Header("UI")]
    [SerializeField] private GameObject _waveText;
    [SerializeField] private GameObject _deathPanel;
    
    [Header("Prefabs")]
    [SerializeField] private List<GameObject> _enemies;


	private readonly int _minEnemiesWave = 5;
    private readonly int _maxEnemiesWave = 8;
    
    private int _currentWave = 0;
    private int _enemiesLeft = 0;
    private int _enemiesKilled = 0;


    void Start() {
        EventManager.OnEnemyKilled += OnEnemyKilled;
        EventManager.OnGameEnded += OnGameEnded;
    }

    void OnDestroy() {
        EventManager.OnEnemyKilled -= OnEnemyKilled;    
        EventManager.OnGameEnded -= OnGameEnded;
    }

    void OnDrawGizmos() {
        Gizmos.DrawLine(new Vector2(-_topLength, _top), new Vector2(_topLength, _top));
        Gizmos.DrawLine(new Vector2(_right, _rightLength), new Vector2(_right, -_rightLength));
        Gizmos.DrawLine(new Vector2(-_right, _rightLength), new Vector2(-_right, -_rightLength));
    }

    void Update() {
        if (Input.GetKeyDown(KeyCode.M)) {
            _waveText.GetComponent<Text>().text = "Wave " + _currentWave;
            _waveText.GetComponent<Animator>().Play("DropNExit");
            Debug.Log("Play");
        }
        if (_enemiesLeft <= 0) {
            SpawnWave();
        }    
    }


    private void OnEnemyKilled() {
        _enemiesKilled++;
        _enemiesLeft--;
    }

    private void OnGameEnded() {
        _deathPanel.transform.GetChild(0).GetComponent<Text>().text = "Final Wave " + _currentWave;
        _deathPanel.transform.GetChild(1).GetComponent<Text>().text = "Enemies Slaughtered " + _enemiesKilled;
        _deathPanel.GetComponent<Animator>().Play("Drop");
    }

    private void SpawnWave() {
        _currentWave++;
        _waveText.GetComponent<Text>().text = "Wave " + _currentWave;
        _waveText.GetComponent<Animator>().Play("DropNExit");
        var enemiesToSpawn = Random.Range(_minEnemiesWave, _maxEnemiesWave + _currentWave);
        _enemiesLeft = enemiesToSpawn;

        for (var x = 0; x < enemiesToSpawn; x++) {
            Vector2 spawnPosition = new Vector2();
            var spawnLocation = (int) Random.Range(0, 3);
            switch (spawnLocation) {
                case 0:
                    spawnPosition = new Vector2(Random.Range(-_topLength, _topLength), _top);
                    break;
                case 1:
                    spawnPosition = new Vector2(_right, Random.Range(-_rightLength, _rightLength));
                    break;
                case 2:
                spawnPosition = new Vector2(-_right, Random.Range(-_rightLength, _rightLength));
                    break;
            }

            Instantiate(_enemies[Random.Range(0, _enemies.Count)], spawnPosition, Quaternion.identity);
        }
    }
}
