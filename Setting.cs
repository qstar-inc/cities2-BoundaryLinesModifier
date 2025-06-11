using System;
using System.Collections.Generic;
using BoundaryLinesModifier.Systems;
using Colossal.IO.AssetDatabase;
using Colossal.Json;
using Game.Modding;
using Game.Settings;
using Game.UI;
using Unity.Entities;
using UnityEngine.Device;

namespace BoundaryLinesModifier
{
    [FileLocation("ModsSettings\\StarQ\\" + nameof(BoundaryLinesModifier))]
    [SettingsUITabOrder(GeneralTab, AboutTab)]
    [SettingsUIGroupOrder(GeneralGroup, InfoGroup)]
    //[SettingsUIShowGroupName(GeneralGroup, InfoGroup)]
    public class Setting : ModSetting
    {
        public Setting(IMod mod)
            : base(mod)
        {
            SetDefaults();
        }

        private readonly BoundaryLinesSystem blm =
            World.DefaultGameObjectInjectionWorld.GetOrCreateSystemManaged<BoundaryLinesSystem>();

        private readonly Dictionary<string, object> _values = new();

        private T GetValue<T>(string propertyName, T defaultValue = default)
        {
            if (_values.TryGetValue(propertyName, out var value))
            {
                try
                {
                    return (T)Convert.ChangeType(value, typeof(T));
                }
                catch (InvalidCastException)
                {
                    Mod.log.Info(
                        $"Warning: Unable to cast setting '{propertyName}' to {typeof(T)}. Returning default."
                    );
                }
            }
            return defaultValue;
        }

        private void SetValue<T>(string propertyName, T value, Action onChanged = null)
        {
            _values[propertyName] = value;
            onChanged?.Invoke();
        }

        [Exclude]
        public VanillaData VanillaDataFromStorage = new();

        public const string GeneralTab = "General";
        public const string GeneralGroup = "General";

        public const string AboutTab = "About";
        public const string InfoGroup = "Info";

        [SettingsUISection(GeneralTab, GeneralGroup)]
        [SettingsUISlider(min = 0, max = 200, step = 2, scalarMultiplier = 1, unit = Unit.kLength)]
        public float Width
        {
            get => GetValue(nameof(Width), 0f);
            set => SetValue(nameof(Width), value, ApplyChanges);
        }

        [SettingsUISection(GeneralTab, GeneralGroup)]
        [SettingsUISlider(min = 0, max = 100, step = 2, scalarMultiplier = 1, unit = Unit.kLength)]
        public float Length
        {
            get => GetValue(nameof(Length), VanillaDataFromStorage.m_TilingLength);
            set => SetValue(nameof(Length), value, () => ApplyChanges());
        }

        [SettingsUISection(GeneralTab, GeneralGroup)]
        [SettingsUITextInput]
        public string CityBorderColor
        {
            get =>
                GetValue(
                    nameof(CityBorderColor),
                    VanillaDataFromStorage.m_CityBorderColor.ToHexCode()
                );
            set => SetValue(nameof(CityBorderColor), value, () => ApplyChanges());
        }

        [SettingsUISection(GeneralTab, GeneralGroup)]
        [SettingsUITextInput]
        public string MapBorderColor
        {
            get =>
                GetValue(
                    nameof(MapBorderColor),
                    VanillaDataFromStorage.m_MapBorderColor.ToHexCode()
                );
            set => SetValue(nameof(MapBorderColor), value, () => ApplyChanges());
        }

        [SettingsUISection(GeneralTab, GeneralGroup)]
        //[SettingsUIButton]
        //public bool ApplyButton { set { ApplySettings(); } }

        [SettingsUISection(GeneralTab, GeneralGroup)]
        [SettingsUIButton]
        public bool ResetButton
        {
            set { SetDefaults(); }
        }

        [SettingsUISection(GeneralTab, GeneralGroup)]
        public string Tips => "Reload the save to apply the changes";

        [SettingsUISection(AboutTab, InfoGroup)]
        public string NameText => Mod.Name;

        [SettingsUISection(AboutTab, InfoGroup)]
        public string VersionText => Mod.Version;

        [SettingsUISection(AboutTab, InfoGroup)]
        public string AuthorText => "StarQ";

        [SettingsUIButtonGroup("Social")]
        [SettingsUIButton]
        [SettingsUISection(AboutTab, InfoGroup)]
        public bool BMaCLink
        {
            set
            {
                try
                {
                    Application.OpenURL($"https://buymeacoffee.com/starq");
                }
                catch (Exception e)
                {
                    Mod.log.Info(e);
                }
            }
        }

        [Exclude]
        public bool Changes = false;

        public void ApplyChanges()
        {
            Changes = true;
        }

        public override void SetDefaults()
        {
            Changes = false;
            Width = VanillaDataFromStorage.m_Width;
            Length = VanillaDataFromStorage.m_TilingLength;
            CityBorderColor = VanillaDataFromStorage.m_CityBorderColor.ToHexCode();
            MapBorderColor = VanillaDataFromStorage.m_MapBorderColor.ToHexCode();
        }

        //public void ApplySettings()
        //{
        //    boundaryLinesSystem.InitializeSystem();
        //}

        //[SettingsUIHidden]
        //public int index;
        //[SettingsUIHidden]
        //public int version;
    }
}
