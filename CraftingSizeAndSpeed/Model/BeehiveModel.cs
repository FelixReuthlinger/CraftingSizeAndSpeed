// ReSharper disable MemberCanBePrivate.Global

namespace CraftingSizeAndSpeed.Model;

public class BeehiveModel : Configurable<Beehive>
{
    public int MaxStorage;
    public float SecondsPerUnit;
    
    public static BeehiveModel From(Beehive original)
    {
        return new BeehiveModel
        {
            MaxStorage = original.m_maxHoney,
            SecondsPerUnit = original.m_secPerUnit
        };
    }

    public override void Configure(Beehive original)
    {
        original.m_maxHoney = MaxStorage;
        original.m_secPerUnit = SecondsPerUnit;
    }
}