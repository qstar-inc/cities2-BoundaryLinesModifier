using Colossal;
using Colossal.IO.AssetDatabase;
using Game.Modding;
using Game.Settings;
using Game.UI;
using System.Collections.Generic;

namespace BoundaryLinesModifier
{
    
    [FileLocation($"ModsSettings\\StarQ\\{nameof(BoundaryLinesModifier)}")]
    [SettingsUITabOrder(GeneralTab, AboutTab)]
    [SettingsUIGroupOrder(GeneralGroup, InfoGroup)]
    //[SettingsUIShowGroupName(GeneralGroup, InfoGroup)]
    public class Setting : ModSetting
    {
        private readonly VanillaData VanillaData = new();
        private readonly BoundaryLinesSystem boundaryLinesSystem = new();

        public const string GeneralTab = "General";
        public const string GeneralGroup = "General";

        public const string AboutTab = "About";
        public const string InfoGroup = "Info";

        public Setting(IMod mod) : base(mod) => SetDefaults();

        [SettingsUISection(GeneralTab, GeneralGroup)]
        [SettingsUISlider(min = 0, max = 200, step = 2, scalarMultiplier = 1, unit = Unit.kLength)]
        public float Width { get; set; }
        [SettingsUISection(GeneralTab, GeneralGroup)]
        [SettingsUISlider(min = 0, max = 100, step = 2, scalarMultiplier = 1, unit = Unit.kLength)]
        public float Length { get; set; }
        [SettingsUISection(GeneralTab, GeneralGroup)]
        [SettingsUITextInput]
        public string CityBorderColor { get; set; }
        [SettingsUISection(GeneralTab, GeneralGroup)]
        [SettingsUITextInput]
        public string MapBorderColor { get; set; }
        [SettingsUISection(GeneralTab, GeneralGroup)]
        //[SettingsUIButton]
        //public bool ApplyButton { set { ApplySettings(); } }

        [SettingsUISection(GeneralTab, GeneralGroup)]
        [SettingsUIButton]
        public bool ResetButton { set { SetDefaults(); } }
        [SettingsUISection(GeneralTab, GeneralGroup)]
        public string Tips => "Reload the save to apply the changes";


        [SettingsUISection(AboutTab, InfoGroup)]
        public string NameText => Mod.Name;

        [SettingsUISection(AboutTab, InfoGroup)]
        public string VersionText => Mod.Version;

        [SettingsUISection(AboutTab, InfoGroup)]
        public string AuthorText => Mod.Author;

        public override void SetDefaults()
        {
            Width = VanillaData.Width;
            Length = VanillaData.Length;
            CityBorderColor = VanillaData.CityBorderColor.ToHexCode();
            MapBorderColor = VanillaData.MapBorderColor.ToHexCode();
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

    public class LocaleEN(Setting setting) : IDictionarySource
    {
        public IEnumerable<KeyValuePair<string, string>> ReadEntries(IList<IDictionaryEntryError> errors, Dictionary<string, int> indexCounts)
        {
            return new Dictionary<string, string>
            {
                { setting.GetSettingsLocaleID(), Mod.Name },
                { setting.GetOptionTabLocaleID(Setting.GeneralTab), Setting.GeneralTab },
                { setting.GetOptionTabLocaleID(Setting.AboutTab), Setting.AboutTab },

                { setting.GetOptionGroupLocaleID(Setting.GeneralGroup), Setting.GeneralGroup },
                { setting.GetOptionGroupLocaleID(Setting.InfoGroup), Setting.InfoGroup },

                { setting.GetOptionLabelLocaleID(nameof(Setting.Tips)), "" },
                { setting.GetOptionDescLocaleID(nameof(Setting.Tips)), "" },

                { setting.GetOptionLabelLocaleID(nameof(Setting.Width)), "Boundary Line Width" },
                { setting.GetOptionDescLocaleID(nameof(Setting.Width)), "The width for both tile and map borders. (Default: 10)"},

                { setting.GetOptionLabelLocaleID(nameof(Setting.Length)), "Boundary Line Tiling Length" },
                { setting.GetOptionDescLocaleID(nameof(Setting.Length)), "The tiling length for both tile and map borders. The tiling length distance is the distance of one line and one empty space. Should be greater than Boundary Line Width (Default: 80)" },

                { setting.GetOptionLabelLocaleID(nameof(Setting.CityBorderColor)), "Tile Border Colour" },
                { setting.GetOptionDescLocaleID(nameof(Setting.CityBorderColor)), "The color for purchased/unlocked map tiles border. Supports hex color code with optional alpha." },

                { setting.GetOptionLabelLocaleID(nameof(Setting.MapBorderColor)), "Map Border Colour" },
                { setting.GetOptionDescLocaleID(nameof(Setting.MapBorderColor)), "The color for complete map's border. Supports hex color code with optional alpha." },

                //{ setting.GetOptionLabelLocaleID(nameof(Setting.ApplyButton)), "Apply" },
                //{ setting.GetOptionDescLocaleID(nameof(Setting.ApplyButton)), "Apply the set settings above." },

                { setting.GetOptionLabelLocaleID(nameof(Setting.ResetButton)), "Reset" },
                { setting.GetOptionDescLocaleID(nameof(Setting.ResetButton)), "Reset to vanilla values." },

                { setting.GetOptionLabelLocaleID(nameof(Setting.NameText)), "Mod Name" },
                { setting.GetOptionDescLocaleID(nameof(Setting.NameText)), "" },
                { setting.GetOptionLabelLocaleID(nameof(Setting.VersionText)), "Mod Version" },
                { setting.GetOptionDescLocaleID(nameof(Setting.VersionText)), "" },
                { setting.GetOptionLabelLocaleID(nameof(Setting.AuthorText)), "Author" },
                { setting.GetOptionDescLocaleID(nameof(Setting.AuthorText)), "" },
            };
        }

        public void Unload()
        {

        }
    }
}
