

using DataAccessLayer.Entities;
using eCommerce.BusinessLogicLayer.DTO;
using System.Linq.Expressions;

namespace eCommerce.BusinessLogicLayer.ServiceContracts;
    public interface IProductService
    {
    Task<List<ProductResponse>> GetProducts();
    Task<List<ProductResponse?>> GetProductsByCondition(Expression<Func<Product, bool>> conditionExpression); // Can also use productresponse instead of prduct but some additional code has to be written
    Task<ProductResponse?> AddProduct(ProductAddRequest productAddRequest);
    Task<ProductResponse?> UpdateProduct(ProductUpdateRequest productUpdateRequest);
    Task<bool> DeleteProduct(Guid productID);
    Task<ProductResponse?> GetProductByCondition(Expression<Func<Product, bool>> conditionExpression);
}

