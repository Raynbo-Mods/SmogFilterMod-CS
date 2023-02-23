using Eco.Gameplay.Items;
using Eco.Shared.Localization;
using Eco.Shared.Serialization;
using Eco.Core.Items;
using Eco.World.Blocks;

namespace Eco.Mods.TechTree
{
    [Serialized]
    [LocDisplayName("Used Filter")]
    [Weight(100)]
    [MaxStackSize(10)]
    [Tag("Utility")]
    [Ecopedia("Items", "Products", createAsSubPage: true)]
    [PolluteGround(0.05f)]
    public partial class UsedFilterItem : Item
    {
        public override LocString DisplayDescription => Localizer.DoStr("A used filter if stored improperly it will cause ground pollution.");
    }
}