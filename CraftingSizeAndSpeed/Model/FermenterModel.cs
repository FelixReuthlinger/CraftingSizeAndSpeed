// ReSharper disable MemberCanBePrivate.Global

namespace CraftingSizeAndSpeed.Model;

public class FermenterModel : Configurable<Fermenter>
{
    public float FermentationDuration;

    public static FermenterModel From(Fermenter original)
    {
        return new FermenterModel
        {
            FermentationDuration = original.m_fermentationDuration,
        };
    }

    public override void Configure(Fermenter original)
    {
        original.m_fermentationDuration = FermentationDuration;
    }
}