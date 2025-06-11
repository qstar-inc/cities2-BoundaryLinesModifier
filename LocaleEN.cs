using System.Collections.Generic;
using BoundaryLinesModifier.Systems;
using Colossal;
using Game.UI;

namespace BoundaryLinesModifier
{
    public class LocaleEN : IDictionarySource
    {
        private readonly Setting setting;

        public LocaleEN(Setting setting)
        {
            this.setting = setting;
        }

        private readonly Setting m_Setting;
        private static VanillaData VanillaDataFromStorage => VanillaDataStorage.VanillaData;

        public IEnumerable<KeyValuePair<string, string>> ReadEntries(
            IList<IDictionaryEntryError> errors,
            Dictionary<string, int> indexCounts
        )
        {
            static string Default(string value) => $"\r\n- Default: <{value}>";
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
                {
                    setting.GetOptionDescLocaleID(nameof(Setting.Width)),
                    $"The width for both tile and map borders. {Default(VanillaDataFromStorage.m_Width.ToString())}"
                },
                {
                    setting.GetOptionLabelLocaleID(nameof(Setting.Length)),
                    "Boundary Line Tiling Length"
                },
                {
                    setting.GetOptionDescLocaleID(nameof(Setting.Length)),
                    $"The tiling length for both tile and map borders. The tiling length distance is the distance of one line and one empty space. Should be greater than Boundary Line Width. {Default(VanillaDataFromStorage.m_TilingLength.ToString())}"
                },
                {
                    setting.GetOptionLabelLocaleID(nameof(Setting.CityBorderColor)),
                    "Tile Border Colour"
                },
                {
                    setting.GetOptionDescLocaleID(nameof(Setting.CityBorderColor)),
                    $"The color for purchased/unlocked map tiles border. Supports hex color code with optional alpha. {Default(VanillaDataFromStorage.m_CityBorderColor.ToHexCode())}"
                },
                {
                    setting.GetOptionLabelLocaleID(nameof(Setting.MapBorderColor)),
                    "Map Border Colour"
                },
                {
                    setting.GetOptionDescLocaleID(nameof(Setting.MapBorderColor)),
                    $"The color for complete map's border. Supports hex color code with optional alpha. {Default(VanillaDataFromStorage.m_MapBorderColor.ToHexCode())}"
                },
                //{ setting.GetOptionLabelLocaleID(nameof(Setting.ApplyButton)), "Apply" },
                //{ setting.GetOptionDescLocaleID(nameof(Setting.ApplyButton)), "Apply the set settings above." },

                { setting.GetOptionLabelLocaleID(nameof(Setting.ResetButton)), "Reset" },
                {
                    setting.GetOptionDescLocaleID(nameof(Setting.ResetButton)),
                    "Reset to vanilla values."
                },
                { setting.GetOptionLabelLocaleID(nameof(Setting.NameText)), "Mod Name" },
                { setting.GetOptionDescLocaleID(nameof(Setting.NameText)), "" },
                { setting.GetOptionLabelLocaleID(nameof(Setting.VersionText)), "Mod Version" },
                { setting.GetOptionDescLocaleID(nameof(Setting.VersionText)), "" },
                { setting.GetOptionLabelLocaleID(nameof(Setting.AuthorText)), "Author" },
                { setting.GetOptionDescLocaleID(nameof(Setting.AuthorText)), "" },
                { setting.GetOptionLabelLocaleID(nameof(Setting.BMaCLink)), "Buy Me a Coffee" },
                { setting.GetOptionDescLocaleID(nameof(Setting.BMaCLink)), "Support the author." },
            };
        }

        public void Unload() { }
    }
}
