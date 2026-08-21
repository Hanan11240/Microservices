using System;
using System.Collections.Generic;
using AutoMapper;

public enum CategoryOptions { Electronics, HomeAppliance, Furniture, Accessories }

public class Product {
    public Guid ProductID { get; set; }
    public string? ProductName { get; set; }
    public string? Category { get; set; }
    public double? UnitPrice { get; set; }
    public int? QuantityInStock { get; set; }
}

public record ProductResponse(Guid ProductID, string? ProductName, CategoryOptions Category, double? UnitPrice, int? QuantityInStock)
{
    public ProductResponse() : this(default, default, default, default, default) { }
}

public class ProductToProductResponseMappingProfile : Profile {
    public ProductToProductResponseMappingProfile() {
        CreateMap<Product, ProductResponse>()
            .ForMember(dest => dest.Category, opt => opt.MapFrom(src => ParseCategory(src.Category)));
    }
    private static CategoryOptions ParseCategory(string? category) {
        if (string.IsNullOrEmpty(category)) return default;
        if (Enum.TryParse<CategoryOptions>(category, true, out var result)) return result;
        return default;
    }
}

class Program {
    static void Main() {
        var config = new MapperConfiguration(cfg => { cfg.AddProfile<ProductToProductResponseMappingProfile>(); });
        var mapper = config.CreateMapper();
        
        var products = new List<Product> { new Product { ProductID = Guid.NewGuid(), ProductName = "Test", Category = "Electronics" } };
        
        try {
            var response = mapper.Map<List<ProductResponse>>(products);
            Console.WriteLine("Success: " + response.Count);
        } catch (Exception ex) {
            Console.WriteLine("ERROR: " + ex.ToString());
        }
    }
}

