using Colossal.Json;
using Game;
using Game.Prefabs;
using Game.SceneFlow;
using Unity.Collections;
using Unity.Entities;
using UnityEngine;

namespace BoundaryLinesModifier.Systems
{
    public static class VanillaDataStorage
    {
        public static VanillaData VanillaData { get; set; } = new VanillaData();
    }

    public struct VanillaData
    {
        public float m_Width;
        public float m_TilingLength;
        public Color m_CityBorderColor;
        public Color m_MapBorderColor;
    }

    public partial class VanillaDataSystem : GameSystemBase
    {
        private PrefabSystem prefabSystem;
        private EntityQuery prefabQuery;

        protected override void OnCreate()
        {
            base.OnCreate();

            prefabSystem = World.GetOrCreateSystemManaged<PrefabSystem>();
            prefabQuery = SystemAPI.QueryBuilder().WithAll<CityBoundaryData>().Build();
            RequireForUpdate(prefabQuery);
        }

        protected override void OnUpdate()
        {
            var entities = prefabQuery.ToEntityArray(Allocator.Temp);
            foreach (Entity entity in entities)
            {
                if (!prefabSystem.TryGetPrefab(entity, out PrefabBase prefabBase))
                {
                    continue;
                }

                if (prefabBase != null)
                {
                    if (
                        prefabBase.TryGet(out CityBoundaryPrefab data)
                        && prefabSystem.GetPrefabName(entity).Contains("City Boundary")
                    )
                    {
                        VanillaDataStorage.VanillaData = new VanillaData
                        {
                            m_Width = data.m_Width,
                            m_TilingLength = data.m_TilingLength,
                            m_CityBorderColor = data.m_CityBorderColor,
                            m_MapBorderColor = data.m_MapBorderColor,
                        };
#if DEBUG
                        Mod.log.Info(
                            $"Vanilla data saved: {VanillaDataStorage.VanillaData.ToJSONString()}"
                        );
#endif
                    }
                }
            }
            Mod.m_Setting.VanillaDataFromStorage = VanillaDataStorage.VanillaData;
            GameManager.instance.localizationManager.AddSource(
                "en-US",
                new LocaleEN(Mod.m_Setting)
            );
            Enabled = false;
        }
    }
}
