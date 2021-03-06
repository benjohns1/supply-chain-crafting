﻿using UnityEngine;
using UnityEngine.SceneManagement;
using Unity.Entities;
using System.Collections.Generic;
using System;

namespace SupplyChain
{
    public class Bootstrap
    {
        public static Settings Settings { get; private set; }

        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.AfterSceneLoad)]
        public static void InitializeAfterSceneLoad()
        {
            var settingsGO = GameObject.Find("Settings");
            if (settingsGO == null)
            {
                SceneManager.sceneLoaded += OnSceneLoaded;
                return;
            }

            InitializeWithScene();
        }

        private static void OnSceneLoaded(Scene arg0, LoadSceneMode arg1)
        {
            InitializeWithScene();
        }

        public static void InitializeWithScene()
        {
            var settingsGO = GameObject.Find("Settings");
            Settings = settingsGO?.GetComponent<Settings>() ?? new Settings();
            EntityManager em = World.Active.GetOrCreateManager<EntityManager>();

            // Initialize hybrid factory archetype with components from initial factory settings
            FactorySettings[] factories = UnityEngine.Object.FindObjectsOfType<FactorySettings>();
            List<GameObject> outputConnectedGos = new List<GameObject>();
            foreach (FactorySettings factory in factories)
            {
                GameObject go = factory.gameObject;
                go.AddComponent<EnabledRecipesComponent>().Recipes = factory.EnabledRecipes;
                go.AddComponent<ProcessComponent>().ItemBufferIn = Inventory.StackSettings.GetStacks(factory.StartingInputItems);
                go.AddComponent<ConnectorInComponent>();
                if (factory.OutputConnected != null)
                {
                    outputConnectedGos.Add(go);
                }
            }

            // Connect inputs/outputs from initial factory settings
            foreach (GameObject go in outputConnectedGos)
            {
                ConnectorInComponent otherConnector = go.GetComponent<FactorySettings>()?.OutputConnected?.GetComponent<ConnectorInComponent>();
                if (otherConnector != null)
                {
                    int guid = Guid.NewGuid().GetHashCode();
                    otherConnector.Guid = guid;
                    go.AddComponent<ConnectorOutComponent>().OtherConnectorGuid = guid;
                }
            }

            // Setup players
            PlayerSettings[] players = UnityEngine.Object.FindObjectsOfType<PlayerSettings>();

            // Add game objects with initialized components to ECS
            foreach (FactorySettings factory in factories)
            {
                Entity factoryEntity = GameObjectEntity.AddToEntityManager(em, factory.gameObject); // hybrid ECS
                em.AddComponentData(factoryEntity, new Interactable { Focus = 0 });
            }

            foreach (PlayerSettings player in players)
            {
                Entity playerEntity = GameObjectEntity.AddToEntityManager(em, player.gameObject);
                em.AddComponentData(playerEntity, new Interactor());
            }
        }
    }
}
