using System;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;
using Unrez.Essential;
using Unrez.Networking;
using Unrez.Pets;
using Unrez.Pets.Cats;
using Unrez.Pets.Dogs;


namespace Unrez
{

    public class ChaseManager : NetworkBehaviour
    {
        public Map _map;
        public List<Cat> _cats;
        public Dog _dog;
        private PlayerSpawner _playerSpawner;

        //-----------------------------------------------------------------------------------------------------------
        #region SINGLETON
        private static bool _shuttingDown = false;
        private static readonly object _lock = new();
        private static ChaseManager _instance;

        public static ChaseManager Instance
        {
            get
            {
                if (_shuttingDown)
                {
                    Debug.LogAssertion($"[Singleton] Instance ' {typeof(ChaseManager)} already destroyed. Returning null.");
                    return null;
                }
                lock (_lock)
                {
                    if (_instance == null)
                    {
                        _instance = (ChaseManager)FindAnyObjectByType(typeof(ChaseManager));
                        if (_instance == null)
                        {
                            GameObject singletonObject = new GameObject();
                            _instance = singletonObject.AddComponent<ChaseManager>();
                            singletonObject.name = typeof(ChaseManager).ToString() + "_SingletonInstance";
                            DontDestroyOnLoad(singletonObject);
                        }
                    }
                    return _instance;
                }
            }
        }
        #endregion
        //-----------------------------------------------------------------------------------------------------------

        private void Awake()
        {
            _playerSpawner = GetComponent<PlayerSpawner>();
            _playerSpawner.OnPlayerSpawn += OnPlayerSpawnHandler;
            _cats = new List<Cat>();
        }

        private void OnApplicationQuit()
        {
            _shuttingDown = true;
        }

        public override void OnDestroy()
        {
            _shuttingDown = true;
            base.OnDestroy();
        }

        private void Update()
        {
            if (!IsServer)
            {
                return;
            }
            if (_dog != null & _cats.Count > 0)
            {
                CheckPetStatus();
            }
        }

        public override void OnNetworkSpawn()
        {
            base.OnNetworkSpawn();
            if (IsServer)
            {
                // Inicialização do jogo, spawn de jogadores, etc.
                foreach (BoneSpot boneSpot in _map.BoneSpots)
                {
                    boneSpot.OnBoneSpotDigged += BoneSpotDiggedHandle;
                }
            }
        }

        public override void OnNetworkDespawn()
        {
            base.OnNetworkDespawn();
        }

        private void OnPlayerSpawnHandler(ulong ownerId, Pet pet)
        {
            if (pet is Cat cat)
            {
                Debug.Log($"OnPlayerSpawnHandler - OwnerId {ownerId} Cat{cat.name}");
            }
            else
            {
                Debug.Log($"OnPlayerSpawnHandler - OwnerId {ownerId} DOG{pet.name}");
            }
        }

        [SerializeField]
        private bool dynamicFOV = false;
        private Cat closerCat = null;
        private float _distance;
        private float _lastDistance;
        private void CheckPetStatus()
        {
            _distance = _lastDistance = float.MaxValue;
            foreach (Cat cat in _cats)
            {
                _distance = Vector3.Distance(_dog.transform.position, cat.transform.position);
                if (_distance < _lastDistance)
                {
                    _lastDistance = _distance;
                }
                if (dynamicFOV)
                {
                    cat.SetFOV(_distance + 2f);
                }
                else
                {
                    if (_distance > 24)
                    {
                        cat.SetFOV(24);
                    }
                    else if (_distance < 16)
                    {
                        cat.SetFOV(16);
                    }
                    else
                    {
                        if (_distance < 12)
                        {
                            Vector3 dir = (cat.transform.position - _dog.transform.position).normalized;
                            RaycastHit2D hit = Physics2D.Raycast(cat.transform.position, dir, 12);
                            Debug.DrawLine(cat.transform.position, _dog.transform.position, Color.magenta);
                            if (hit)
                            {
                                if (hit.transform.GetComponent<Dog>() != null)
                                {
                                    Debug.Log("INCHASE");
                                    cat.SetFOV(12);
                                    _dog.SetFOV(12);
                                }
                                else
                                {
                                    Debug.DrawLine(cat.transform.position, hit.point, Color.red);
                                }
                            }
                        }
                    }
                }
            }
            if (_lastDistance < 36)
            {
                _dog.SetFOV(Mathf.Clamp(_lastDistance, 24, 36));
            }
            else
            {
                _dog.SetFOV(36);
            }
        }

        private void BoneSpotDiggedHandle(BoneSpot boneSpot)
        {
            Debug.Log("BoneSpot Digged!!!");
        }

        public void SetDog(Dog dog)
        {
            Debug.Log($"SetDog");
            _dog = dog;
        }
        public void AddCat(Cat cat)
        {
            Debug.Log($"AddCat");
            _cats.Add(cat);
        }
    }

}