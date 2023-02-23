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
    [LocDisplayName("Filter")]
    [Weight(100)]
    [MaxStackSize(10)]
    [Tag("Utility")]
    [Ecopedia("Items", "Products", createAsSubPage: true)]
    public partial class FilterItem : Item
    {
        public override LocString DisplayDescription => Localizer.DoStr("An item used to filter impurities from gasses. NOTE: some particals are to small to remove");
    }
    [Serialized]
    [RequiresSkill(typeof(MechanicsSkill), 3)]
    public partial class FilterRecipe : RecipeFamily
    {
        public FilterRecipe()
        {
            var recipe1 = new Recipe();
            recipe1.Init(
                "Paper Filters",
                Localizer.DoStr("Paper Filters"),
                new List<IngredientElement> { new IngredientElement(typeof(PaperItem), 30, false), new IngredientElement(typeof(CharcoalItem), 10, true), new IngredientElement(typeof(SandItem), 10, true) },
                new List<CraftingElement> { new CraftingElement<FilterItem>(15) });
            var recipe2 = new Recipe();
            recipe2.Init(
                "Plant Based Filters",
                Localizer.DoStr("Plant Based Filters"),
                new List<IngredientElement> { new IngredientElement("NaturalFiber", 400, false), new IngredientElement(typeof(CharcoalItem), 10, true), new IngredientElement(typeof(SandItem), 10, true) },
                new List<CraftingElement> { new CraftingElement<FilterItem>(15) });
            this.Recipes = new List<Recipe> { recipe1, recipe2 };
            this.ExperienceOnCraft = 0.25f;
            this.LaborInCalories = CreateLaborInCaloriesValue(50f, typeof(MechanicsSkill));
            this.CraftMinutes = CreateCraftTimeValue(typeof(MechanicsSkill), 5f, typeof(MechanicsSkill));
            this.ModsPreInitialize();
            this.Initialize(Localizer.DoStr("Filters"), typeof(FilterRecipe));
            this.ModsPostInitialize();
            CraftingComponent.AddRecipe(typeof(AssemblyLineObject), this);
        }
        partial void ModsPreInitialize();
        partial void ModsPostInitialize();
    }
}
