

using eCommerce.BusinessLogicLayer.DTO;
using eCommerce.BusinessLogicLayer.ServiceContracts;
using FluentValidation;
using FluentValidation.Results;


namespace eCommerce.ProductsService.API.APIEndPoints
{
    public static class ProductAPIEndpoints
    {
        public static IEndpointRouteBuilder MapProductAPIEndPoints(this  IEndpointRouteBuilder app)
        {

            app.MapGet("/api/products", async (IProductService productService) =>
            {
              
                
                List<ProductResponse?> products =  await productService.GetProducts();
                return Results.Ok(products);
            });
            app.MapGet("/api/products/search/product-id/{ProductID:guid}", async (IProductService productService,Guid ProductID) =>
            {
              ProductResponse? product =  await productService.GetProductByCondition(temp=> temp.ProductID == ProductID);

                if(product is null)
                {
                    return Results.NotFound();
                }
                return Results.Ok(product);
            });

            app.MapGet("/api/products/search/{search}", async (IProductService productService, string search) =>
            {
                string searchLower = search.ToLower();
                List<ProductResponse?> productsBYProductName = await productService.GetProductsByCondition(temp => temp.ProductName != null && temp.ProductName.ToLower().Contains(searchLower));
                List<ProductResponse?> productsBYProductCategory = await productService.GetProductsByCondition(temp => temp.Category != null && temp.Category.ToLower().Contains(searchLower));
                var products = productsBYProductName.Union(productsBYProductCategory);
                return Results.Ok(products);
            });

            app.MapPost("/api/products", async (IProductService productService,ProductAddRequest productAddRequest,IValidator<ProductAddRequest> productAddRequestValidator) =>
            {
                ValidationResult validationResult = await productAddRequestValidator.ValidateAsync(productAddRequest);

                if(!validationResult.IsValid)
                {
                    Dictionary<string, string[]> errors =  validationResult.Errors.GroupBy(temp => temp.PropertyName).ToDictionary(grp=> grp.Key,grp=>grp.Select(err=> err.ErrorMessage).ToArray());
                    return Results.ValidationProblem(errors);
                }
                var product = await productService.AddProduct(productAddRequest);
                if(product is not null)
                {
                    return Results.Created($"/api/products/search/product-id/{product.ProductID}",product);
                }
                else
                {
                    return Results.Problem("Error in adding Product");
                }
               
            });


            app.MapPut("/api/products", async (IProductService productService, ProductUpdateRequest productUpdateRequest, IValidator<ProductUpdateRequest> productUpdateRequestValidator) =>
            {
                ValidationResult validationResult = await productUpdateRequestValidator.ValidateAsync(productUpdateRequest);

                if (!validationResult.IsValid)
                {
                    Dictionary<string, string[]> errors = validationResult.Errors.GroupBy(temp => temp.PropertyName).ToDictionary(grp => grp.Key, grp => grp.Select(err => err.ErrorMessage).ToArray());
                    return Results.ValidationProblem(errors);
                }
                var product = await productService.UpdateProduct(productUpdateRequest);
                if (product is not null)
                {
                    return Results.Ok(product);
                }
                else
                {
                    return Results.Problem("Error in adding Product");
                }

            });


            app.MapDelete("/api/products/{productID:guid}", async (IProductService productService , Guid productID) =>
            {
               

               
                bool isDeleted = await productService.DeleteProduct(productID);
                if (isDeleted)
                {
                    return Results.Ok(isDeleted);
                }
                else
                {
                    return Results.Problem("Error in deleting Product");
                }

            });


            return app;
        }
    }
}
