using Game.Prefabs;
using Game;
using System;
using Unity.Collections;
using Unity.Entities;
using Colossal.Json;
using UnityEngine;
using System.Collections.Generic;
using Game.UI.InGame;
using Unity.Burst;

namespace BoundaryLinesModifier
{
    public partial class BoundaryLinesSystem : GameSystemBase
    {
        public static PrefabSystem prefabSystem;
        public static EntityQuery prefabQuery;
        private Setting settings = Mod.m_Setting;
        private readonly VanillaData VanillaData = new();
        //public Entity entity;

        protected override void OnCreate()
        {
            base.OnCreate();
            //Mod.log.Info("Initializing PrefabSystem...");
            prefabSystem = World.DefaultGameObjectInjectionWorld.GetOrCreateSystemManaged<PrefabSystem>();
            //Mod.log.Info("Initialized PrefabSystem...");

            if (prefabSystem == null)
            {
                Mod.log.Info("Failed to get or create PrefabSystem.");
                return;
            }
            prefabQuery = GetEntityQuery(new EntityQueryDesc()
            {
                All = [
                    ComponentType.ReadWrite<PrefabData>(),
                    ComponentType.ReadWrite<CityBoundaryData>()
                    ],
            });
            RequireForUpdate(prefabQuery);
        }

        protected override void OnUpdate()
        {
            var entities = prefabQuery.ToEntityArray(Allocator.Temp);
            Entity entity;
            if (entities.Length > 0)
            {
                entity = entities[0];
                //settings.index = entity.Index;
                //settings.version = entity.Version;
            }
            else
            {
                Mod.log.Info("No entities found in prefabQuery.");
                return;
            }
            
            if (prefabSystem.TryGetPrefab(entity, out PrefabBase prefab))
            {
                if (prefab != null)
                {
                    prefab.TryGet(out CityBoundaryPrefab boundaryPrefab);

                    boundaryPrefab.m_Width = settings.Width;
                    float width = Math.Max(Math.Max(settings.Width, settings.Length),0f);
                    boundaryPrefab.m_TilingLength = width;
                    
                    Color CityBorderColor = VanillaData.CityBorderColor;
                    if (settings.CityBorderColor.StartsWith("#") && (settings.CityBorderColor.Length == 9 || settings.CityBorderColor.Length == 7 ))
                    {
                        try
                        {
                            CityBorderColor = ColorParser.ParseColor(settings.CityBorderColor);
                        }
                        catch (Exception e)
                        {
                            CityBorderColor = ColorParser.ParseColor("#000000");
                            Mod.log.Info($"Invalid colour format for CityBorderColor ({settings.CityBorderColor}): {e}");
                        }
                    }

                    Color MapBorderColor = VanillaData.MapBorderColor;
                    if (settings.MapBorderColor.StartsWith("#") && (settings.MapBorderColor.Length == 9 || settings.MapBorderColor.Length == 7))
                    {
                        try
                        {
                            MapBorderColor = ColorParser.ParseColor(settings.MapBorderColor);
                        }
                        catch (Exception e)
                        {
                            MapBorderColor = ColorParser.ParseColor("#000000");
                            Mod.log.Info($"Invalid colour format for MapBorderColor ({settings.MapBorderColor}): {e}");
                        }
                    }

                    boundaryPrefab.m_CityBorderColor = CityBorderColor;
                    boundaryPrefab.m_MapBorderColor = MapBorderColor;
                }
            }
            //}
            //catch (Exception e)
            //{
            //    Mod.log.Error(e);
            //}
        }
    }
}
