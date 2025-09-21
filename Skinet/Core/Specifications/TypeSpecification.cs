using Core.Entities;

namespace Core.Specifications;

public class TypeSpecification : BaseSpecification<Product, string>
{
    public TypeSpecification()
    {
        AddSelect(x => x.Type);
        ApplyDistinct();
    }
}
