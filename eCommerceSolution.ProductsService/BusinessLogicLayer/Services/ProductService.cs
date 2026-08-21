

using AutoMapper;
using DataAccessLayer.Entities;
using eCommerce.BusinessLogicLayer.DTO;
using eCommerce.BusinessLogicLayer.ServiceContracts;
using eCommerce.DataAccessLayer.RepositoryContracts;
using FluentValidation;
using FluentValidation.Results;
using System.Linq.Expressions;

namespace eCommerce.BusinessLogicLayer.Services;

public class ProductService : IProductService
{
    private readonly IValidator<ProductAddRequest> _productAddRequestValidator;
    private readonly IValidator<ProductUpdateRequest> _productUpdateRequestValidator;
    private readonly IMapper _mapper;
    private readonly IProductRepository _productRepostory;


    public ProductService(IValidator<ProductAddRequest> productAddRequestValidator, IValidator<ProductUpdateRequest> productUpdateRequestValidator, IMapper mapper, IProductRepository productRepostory)
    {
        _productAddRequestValidator = productAddRequestValidator;
        _productUpdateRequestValidator = productUpdateRequestValidator;
        _mapper = mapper;
        _productRepostory = productRepostory;
    }

    public async Task<ProductResponse?> AddProduct(ProductAddRequest productAddRequest)
    {

        if(productAddRequest is null)
        {
            throw new ArgumentNullException(nameof(productAddRequest));

        }
        // validate
      ValidationResult validationResult =  await _productAddRequestValidator.ValidateAsync(productAddRequest);
        if (!validationResult.IsValid)
        {
           string errors =  string.Join(",", validationResult.Errors.Select(temp => temp.ErrorMessage)); //Error1, Error2
            throw new ArgumentException(errors);
        }

       Product product =  _mapper.Map<Product>(productAddRequest);
       Product?  addedProduct=   await _productRepostory.AddProduct(product);
         if(addedProduct is null)
        {
            return null;
        }
       ProductResponse productResponse =  _mapper.Map<ProductResponse>(addedProduct);
        return productResponse;


    }

    public async Task<bool> DeleteProduct(Guid productID)
    {
      Product? existingProduct =  await _productRepostory.GetProductByCondition(temp=> temp.ProductID == productID);
        if(existingProduct is null)
        {
            return false;
        }

       bool isDeleted = await _productRepostory.DeleteProduct(productID);
        return isDeleted;
        
    }

    public async Task<ProductResponse?> GetProductByCondition(Expression<Func<Product, bool>> conditionExpression)
    {
       Product? product =  await _productRepostory.GetProductByCondition(conditionExpression);
        if(product is null)
        {
            return null;
        }

        ProductResponse productResponse = _mapper.Map<ProductResponse>(product);

        return productResponse;
    }

    public async Task<List<ProductResponse>> GetProducts()
    {
        var products = await _productRepostory.GetProducts();
        
        var productResponses = _mapper.Map<List<ProductResponse>>(products);

        return productResponses;
    }

    public async Task<List<ProductResponse?>> GetProductsByCondition(Expression<Func<Product, bool>> conditionExpression)
    {
        var products = await _productRepostory.GetProductsByCondition(conditionExpression);

        var productResponses = _mapper.Map<List<ProductResponse?>>(products);

        return productResponses;
    }

    public async Task<ProductResponse?> UpdateProduct(ProductUpdateRequest productUpdateRequest)
    {
       Product? existingProduct = await _productRepostory.GetProductByCondition(temp => temp.ProductID == productUpdateRequest.ProductID);
        if(existingProduct == null)
        {
            throw new ArgumentException("Invalid Product ID");
        }

        ValidationResult validationResult = await _productUpdateRequestValidator.ValidateAsync(productUpdateRequest);
        if (!validationResult.IsValid)
        {
            string errors = string.Join(",", validationResult.Errors.Select(temp => temp.ErrorMessage)); //Error1, Error2
            throw new ArgumentException(errors);
        }

        Product product = _mapper.Map<Product>(productUpdateRequest);
        Product? updatedproduct = await _productRepostory.UpdateProduct(product);
        ProductResponse? updatedProductResponse= _mapper.Map<ProductResponse>(updatedproduct);
        return updatedProductResponse;
    }
}

