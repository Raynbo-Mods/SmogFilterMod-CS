using Eco.Gameplay.Components;
using Eco.Gameplay.Items;
using Eco.Gameplay.Objects;
using Eco.Gameplay.Skills;
using Eco.Shared.Localization;
using Eco.Shared.Serialization;
using Eco.Core.Items;
using Eco.World.Blocks;
using Eco.Gameplay.Pipes.LiquidComponents;
using Eco.Shared.Math;
using Eco.Gameplay.Components.Auth;
using Eco.Gameplay.Pipes.Gases;

namespace Eco.Mods.TechTree
{
    [Serialized]
    [Constructed]
    [RequireComponent(typeof(AirPollutionComponent))]
    [RequireComponent(typeof(PropertyAuthComponent))]
    [RequireComponent(typeof(OnOffComponent))]
    [RequireComponent(typeof(CraftingComponent))]
    [RequireComponent(typeof(LiquidProducerComponent))]
    [RequireComponent(typeof(LiquidConsumerComponent))]
    [RequireComponent(typeof(MinimapComponent))]
    [RequireComponent(typeof(LinkComponent))]
    public partial class SmogCapObject : WorldObject, IRepresentsItem
    {
        public override LocString DisplayName { get { return Localizer.DoStr("Smog Cap"); } }
        public virtual Type RepresentedItemType { get { return typeof(SmogCapItem); } }
        [Serialized] int fails = 0;
        [Serialized] int clock = 0;
        [Serialized] int once = 0;
        protected override void Initialize()
        {
            this.GetComponent<MinimapComponent>().SetCategory(Localizer.DoStr("Crafting"));
            this.GetComponent<LiquidConsumerComponent>().Setup(typeof(SmogItem), 1f, this.GetOccupancyType(BlockOccupancyType.InputPort), 0.3f);
            this.GetComponent<LiquidProducerComponent>().Setup(typeof(SmogItem), 0.3f, this.GetOccupancyType(BlockOccupancyType.ChimneyOut));
            this.GetOrCreateComponent<AirPollutionComponent>().Initialize(0.3f);
            once = 0;
        }
        static SmogCapObject()
        {
            WorldObject.AddOccupancy<SmogCapObject>(new List<BlockOccupancy>{
                new BlockOccupancy(new Vector3i(0, 0, 0), typeof(PipeSlotBlock), new Quaternion(0.7071071f, -2.634177E-07f, -2.634179E-07f, 0.7071065f), BlockOccupancyType.InputPort),
                new BlockOccupancy(new Vector3i(0, 1, 0), typeof(PipeSlotBlock), new Quaternion(-0.7071071f, 2.634177E-07f, 2.634179E-07f, 0.7071065f), BlockOccupancyType.ChimneyOut),
            });
        }
        public override void OnPermanentDestroy()
        {
            base.Destroy();
        }
        public override void Tick()
        {
            base.Tick();
            once += 1;
            if (once <= 10)
            {
                this.GetComponent<OnOffComponent>().SetOnOff(null, true);
            }
            if (this.GetComponent<CraftingComponent>().ActiveWorkOrder != null && !this.GetComponent<OnOffComponent>().On)
            {
                this.GetComponent<OnOffComponent>().SetOnOff(null, true);
            }
            if (this.GetComponent<OnOffComponent>().On && once > 10)
            {
                if (this.GetComponent<LiquidProducerComponent>().OutputPipe.AverageFlow.Quantity/1000f <= 0.2f && this.GetComponent<CraftingComponent>().Operating == true)
                {
                    fails += 1;
                    if (fails >= 4)
                    {
                        this.GetComponent<OnOffComponent>().SetOnOff(null, false);
                        fails = 0;
                    }
                }
                else if (this.GetComponent<CraftingComponent>().Operating == false && this.GetComponent<CraftingComponent>().ActiveWorkOrder == null)
                {
                    this.GetComponent<OnOffComponent>().SetOnOff(null, false);
                }
                else
                {
                    clock += 1;
                    if (clock >= 10)
                    {
                        this.GetOrCreateComponent<AirPollutionComponent>().Destroy();
                        this.GetOrCreateComponent<AirPollutionComponent>().Initialize((this.GetComponent<LiquidProducerComponent>().OutputPipe.AverageFlow.Quantity / 1000f - this.GetComponent<LiquidConsumerComponent>().InputPipe.AverageFlow.Quantity / 1000f)*3);
                        clock = 0;
                    }
                    fails = 0;
                }
            }
        }
    }

    [Serialized]
    [LocDisplayName("Smog Cap")]
    [Weight(100)]
    [MaxStackSize(10)]
    [Carried]
    [Tag("Utility")]
    [Ecopedia("Items", "Products", createAsSubPage: true)]
    [LiquidProducer(typeof(SmogItem), 0.3f)]
    public partial class SmogCapItem : WorldObjectItem<SmogCapObject>
    {
        public override LocString DisplayDescription => Localizer.DoStr("Used filters to remove impurities from smog. NOTE: some particals are too small to remove");
    }

    public partial class WeakSmogRecipe : RecipeFamily
    {
        public WeakSmogRecipe()
        {
            var recipe1 = new Recipe();
            recipe1.Init(
                "Weak Smog",
                Localizer.DoStr("Weak Smog"),
                new List<IngredientElement> { new IngredientElement(typeof(FilterItem), 1, true) },
                new List<CraftingElement> { new CraftingElement<UsedFilterItem>(1) });
            this.Recipes = new List<Recipe> { recipe1 };
            this.LaborInCalories = CreateLaborInCaloriesValue(50f);
            this.CraftMinutes = CreateCraftTimeValue(10f);
            this.ModsPreInitialize();
            this.Initialize(Localizer.DoStr("Weak Smog"), typeof(WeakSmogRecipe));
            this.ModsPostInitialize();
            CraftingComponent.AddRecipe(typeof(SmogCapObject), this);
        }
        partial void ModsPreInitialize();
        partial void ModsPostInitialize();
    }

    [RequiresSkill(typeof(MechanicsSkill), 3)]
    public partial class SmogCapRecipe : RecipeFamily
    {
        public SmogCapRecipe()
        {
            var recipe1 = new Recipe();
            recipe1.Init(
                "Smog Cap",
                Localizer.DoStr("Smog Cap"),
                new List<IngredientElement> { new IngredientElement(typeof(IronPipeItem), 5), new IngredientElement(typeof(IronPlateItem), 10), new IngredientElement(typeof(ScrewsItem), 20) },
                new List<CraftingElement> { new CraftingElement<SmogCapItem>(1) });
            this.Recipes = new List<Recipe> { recipe1 };
            this.ExperienceOnCraft = 1f;
            this.LaborInCalories = CreateLaborInCaloriesValue(50f, typeof(MechanicsSkill));
            this.CraftMinutes = CreateCraftTimeValue(typeof(MechanicsSkill), 20f, typeof(MechanicsSkill));
            this.ModsPreInitialize();
            this.Initialize(Localizer.DoStr("Smog Cap"), typeof(WeakSmogRecipe));
            this.ModsPostInitialize();
            CraftingComponent.AddRecipe(typeof(AssemblyLineObject), this);
        }
        partial void ModsPreInitialize();
        partial void ModsPostInitialize();
    }
}