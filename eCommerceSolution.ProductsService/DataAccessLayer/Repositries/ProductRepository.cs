using DataAccessLayer.Entities;
using eCommerce.DataAccessLayer.Context;
using eCommerce.DataAccessLayer.RepositoryContracts;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;


namespace DataAccessLayer.Repositries
{
    public class ProductRepository : IProductRepository
    {

        private readonly ApplicationDbContext _dbContext;

        public ProductRepository(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        public async Task<Product?> AddProduct(Product product)
        {
            _dbContext.Products.Add(product);
            await _dbContext.SaveChangesAsync();
            return product;
        }

        public async  Task<bool> DeleteProduct(Guid productID)
        {
          Product? existingProduct = await  _dbContext.Products.FirstOrDefaultAsync(temp => temp.ProductID == productID);
            if(existingProduct is null)
            {
                return false;
            }
            _dbContext.Products.Remove(existingProduct);
           int affectedRowsCount =  await _dbContext.SaveChangesAsync();
            return affectedRowsCount > 0;
        }

        public async Task<Product?> GetProductByCondition(Expression<Func<Product, bool>> conditionExpression)
        {
            return await _dbContext.Products.FirstOrDefaultAsync(conditionExpression);
        }

        public  async Task<IEnumerable<Product>> GetProducts()
        {
            return await _dbContext.Products.ToListAsync();

        }

        public async  Task<IEnumerable<Product>> GetProductsByCondition(Expression<Func<Product, bool>> conditionExpression)
        {
            return await _dbContext.Products.Where(conditionExpression).ToListAsync();

        }

        public async  Task<Product?> UpdateProduct(Product product)
        {
            Product? existingProduct = _dbContext.Products.FirstOrDefault(temp => temp.ProductID == product.ProductID);
            if(existingProduct is null)
            {
                return null;
            }
            existingProduct.ProductName = product.ProductName;
            existingProduct.UnitPrice = product.UnitPrice;
            existingProduct.QuantityInStock = product.QuantityInStock;
            existingProduct.Category = product.Category;
            await _dbContext.SaveChangesAsync();
            return existingProduct;
        }

      
    }
}
