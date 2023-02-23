namespace Eco.Mods.TechTree
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using Eco.Gameplay.Blocks;
    using Eco.Gameplay.Components;
    using Eco.Gameplay.DynamicValues;
    using Eco.Gameplay.Items;
    using Eco.Gameplay.Objects;
    using Eco.Gameplay.Players;
    using Eco.Gameplay.Skills;
    using Eco.Gameplay.Systems;
    using Eco.Gameplay.Systems.TextLinks;
    using Eco.Shared.Localization;
    using Eco.Shared.Serialization;
    using Eco.Shared.Utils;
    using Eco.Core.Items;
    using Eco.World;
    using Eco.World.Blocks;
    using Eco.Gameplay.Pipes;
    using Eco.Gameplay.GameActions;
    using System.Runtime.Serialization;
    using Eco.Gameplay.Pipes.LiquidComponents;
    using System.IO.Pipes;
    using Eco.Shared.Math;
    using Eco.WebServer.Web.Controllers;
    using Eco.Core.Controller;
    using Eco.Gameplay.Components.Auth;
    using Eco.Gameplay.Disasters;

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