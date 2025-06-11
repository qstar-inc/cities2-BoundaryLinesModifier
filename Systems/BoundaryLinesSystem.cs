using System;
using Colossal.Json;
using Colossal.Serialization.Entities;
using Game;
using Game.Prefabs;
using Unity.Collections;
using Unity.Entities;
using UnityEngine;

namespace BoundaryLinesModifier.Systems
{
    public partial class BoundaryLinesSystem : GameSystemBase
    {
        public static PrefabSystem prefabSystem;
        public static EntityQuery prefabQuery;
        public static Setting settings;

        protected override void OnCreate()
        {
            base.OnCreate();
            prefabSystem =
                World.DefaultGameObjectInjectionWorld.GetOrCreateSystemManaged<PrefabSystem>();

            prefabQuery = SystemAPI.QueryBuilder().WithAll<CityBoundaryData>().Build();
            RequireForUpdate(prefabQuery);
        }

        protected override void OnGameLoadingComplete(Purpose purpose, GameMode mode)
        {
            base.OnGameLoadingComplete(purpose, mode);

            settings = Mod.m_Setting;
            if (settings.Changes && mode == GameMode.Game)
            {
                VanillaData vanillaData = VanillaDataStorage.VanillaData;
                try
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
                                prefabBase.TryGet(out CityBoundaryPrefab boundaryPrefab)
                                && prefabSystem.GetPrefabName(entity).Contains("City Boundary")
                            )
                            {
                                boundaryPrefab.m_Width = settings.Width;
                                float tiling = Math.Max(
                                    Math.Max(settings.Width, settings.Length),
                                    0f
                                );
                                boundaryPrefab.m_TilingLength = tiling;

                                Color CityBorderColor = vanillaData.m_CityBorderColor;
                                if (
                                    settings.CityBorderColor.StartsWith("#")
                                    && (
                                        settings.CityBorderColor.Length == 9
                                        || settings.CityBorderColor.Length == 7
                                    )
                                )
                                {
                                    try
                                    {
                                        CityBorderColor = ColorParser.ParseColor(
                                            settings.CityBorderColor
                                        );
                                        boundaryPrefab.m_CityBorderColor = CityBorderColor;
                                    }
                                    catch (Exception)
                                    {
                                        //CityBorderColor = ColorParser.ParseColor("#000000");
                                        //Mod.log.Info(
                                        //    $"Invalid colour format for CityBorderColor ({settings.CityBorderColor}): {e}"
                                        //);
                                    }
                                }

                                Color MapBorderColor = vanillaData.m_MapBorderColor;
                                if (
                                    settings.MapBorderColor.StartsWith("#")
                                    && (
                                        settings.MapBorderColor.Length == 9
                                        || settings.MapBorderColor.Length == 7
                                    )
                                )
                                {
                                    try
                                    {
                                        MapBorderColor = ColorParser.ParseColor(
                                            settings.MapBorderColor
                                        );
                                        boundaryPrefab.m_MapBorderColor = MapBorderColor;
                                    }
                                    catch (Exception)
                                    {
                                        //MapBorderColor = ColorParser.ParseColor("#000000");
                                        //Mod.log.Info(
                                        //    $"Invalid colour format for MapBorderColor ({settings.MapBorderColor}): {e}"
                                        //);
                                    }
                                }

                                //prefabBase.ReplaceComponentWith(
                                //    boundaryPrefab,
                                //    typeof(CityBoundaryPrefab)
                                //);
                            }
                        }
                    }
                }
                catch (Exception e)
                {
                    Mod.log.Error(e);
                }
                settings.Changes = false;
            }
        }

        protected override void OnUpdate() { }
    }
}
