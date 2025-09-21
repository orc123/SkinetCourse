using Core.Entities;
using System.Linq.Expressions;

namespace Core.Specifications;

public class BrandSpecification : BaseSpecification<Product, string>
{
    public BrandSpecification()
    {
        AddSelect(x => x.Brand);
        ApplyDistinct();
    }
}
