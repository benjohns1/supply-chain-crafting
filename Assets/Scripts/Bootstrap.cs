using UnityEngine;
using UnityEngine.SceneManagement;
using Unity.Entities;

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

            FactorySettings[] factories = GameObject.FindObjectsOfType<FactorySettings>();
            EntityManager em = World.Active.GetOrCreateManager<EntityManager>();
            foreach (FactorySettings factory in factories)
            {
                GameObject go = factory.gameObject;
                go.AddComponent<EnabledRecipesComponent>().Recipes = factory.EnabledRecipes;
                go.AddComponent<ProcessComponent>();
                go.AddComponent<ItemBufferInComponent>().ItemBuffer = Inventory.StackSettings.GetStacks(factory.StartingInputItems);
                go.AddComponent<ItemBufferOutComponent>().ItemBuffer = new Item.Stack[1]; // default buffer size of 1
                go.AddComponent<ConnectorInComponent>();
                go.AddComponent<ConnectorOutComponent>();
                GameObjectEntity.AddToEntityManager(em, go); // hybrid ECS
            }
        }
    }
}
