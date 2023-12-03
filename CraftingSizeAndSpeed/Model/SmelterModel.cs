// ReSharper disable MemberCanBePrivate.Global

namespace CraftingSizeAndSpeed.Model;

public class SmelterModel : Configurable<Smelter>
{
    public int MaxFuel;
    public int MaxOre;
    public int FuelPerProduct;
    public float SecondsPerProduct;

    public static SmelterModel From(Smelter original)
    {
        return new SmelterModel
        {
            MaxFuel = original.m_maxFuel,
            MaxOre = original.m_maxOre,
            FuelPerProduct = original.m_fuelPerProduct,
            SecondsPerProduct = original.m_secPerProduct,
        };
    }

    public override void Configure(Smelter original)
    {
        original.m_maxFuel = MaxFuel;
        original.m_maxOre = MaxOre;
        original.m_fuelPerProduct = FuelPerProduct;
        original.m_secPerProduct = SecondsPerProduct;
    }
}