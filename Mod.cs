using System.Reflection;
using BoundaryLinesModifier.Systems;
using Colossal.IO.AssetDatabase;
using Colossal.Logging;
using Game;
using Game.Modding;
using Game.SceneFlow;
using Unity.Entities;

namespace BoundaryLinesModifier
{
    public class Mod : IMod
    {
        public static string Name = Assembly
            .GetExecutingAssembly()
            .GetCustomAttribute<AssemblyTitleAttribute>()
            .Title;
        public static string Version = Assembly
            .GetExecutingAssembly()
            .GetName()
            .Version.ToString(3);

        public static ILog log = LogManager
            .GetLogger($"{nameof(BoundaryLinesModifier)}")
            .SetShowsErrorsInUI(false);
        public static Setting m_Setting;

        public void OnLoad(UpdateSystem updateSystem)
        {
            m_Setting = new Setting(this);
            m_Setting.RegisterInOptionsUI();

            AssetDatabase.global.LoadSettings(
                nameof(BoundaryLinesModifier),
                m_Setting,
                new Setting(this)
            );

            updateSystem.UpdateAfter<VanillaDataSystem>(SystemUpdatePhase.PrefabUpdate);
            World.DefaultGameObjectInjectionWorld.GetOrCreateSystemManaged<BoundaryLinesSystem>();
            GameManager.instance.localizationManager.AddSource("en-US", new LocaleEN(m_Setting));
        }

        public void OnDispose()
        {
            if (m_Setting != null)
            {
                m_Setting.UnregisterInOptionsUI();
                m_Setting = null;
            }
        }
    }
}
