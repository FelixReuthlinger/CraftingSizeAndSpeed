// ReSharper disable MemberCanBePrivate.Global

namespace CraftingSizeAndSpeed.Model;

public class CookingStationModel : Configurable<CookingStation>
{
    public int MaxFuel;
    public int SecondsPerFuel;

    public static CookingStationModel From(CookingStation original)
    {
        return new CookingStationModel
        {
            MaxFuel = original.m_maxFuel,
            SecondsPerFuel = original.m_secPerFuel
        };
    }

    public override void Configure(CookingStation original)
    {
        original.m_maxFuel = MaxFuel;
        original.m_secPerFuel = SecondsPerFuel;
    }
}