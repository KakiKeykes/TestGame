using Assets.Scripts.GamePlay;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.Save
{
    public class SaveManager : MonoBehaviour
    {
        [SerializeField] GameController _gameController;

        private List<ISaveable> saveableObjects = new();

        private void Start()
        {
            saveableObjects.Add(_gameController);
        }

        private void Update()
        {
            if (Input.GetKeyUp(KeyCode.F5))
            {
                SaveSystem.SaveGame(saveableObjects);
            }
            if (Input.GetKeyUp(KeyCode.F6))
            {
                SaveSystem.LoadGame(saveableObjects);
            }
        }
    }
}