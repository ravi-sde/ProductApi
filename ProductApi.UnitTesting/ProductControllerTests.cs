using FakeItEasy;
using Microsoft.AspNetCore.Mvc;
using Moq;
using ProductApi.Controllers;
using ProductApi.DAL;
using ProductApi.Models;
using ProductApi.Service.ServiceInterface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace ProductApi.UnitTesting
{
    public class ProductControllerTests
    {
        [Fact]
        public async Task GetProducts_Returns_All_ProductsAsync()
        {
            //Arrange 
            var mockRepo = new Mock<IProductService>();
            mockRepo.Setup(repo => repo.GetAllProducts())
                    .ReturnsAsync(GetProducts());
            var controller = new ProductController(mockRepo.Object);

            //Act
            var actionResult = await controller.GetAllProducts();

            //Assert

            var result = actionResult.Result as OkObjectResult;
            var returnProduct = result.Value as IEnumerable<Product>;
            Assert.NotNull(returnProduct);
            Assert.True(returnProduct.Count() > 0, "Passed");
        }

        [Fact]
        public async Task GetProductById_Return_A_Product()
        {
            int id = 1;
            var mockRepo = new Mock<IProductService>();
            mockRepo.Setup(repo => repo.GetProductById(id))
                    .ReturnsAsync(new Product
                    {
                        Id = 1,
                        ProductName = "Test1",
                        ProductDescription = "Test Description 1",
                        Units = "11"
                    });

            var controller = new ProductController(mockRepo.Object);


            var result = await controller.GetProductById(id);

            var resultData = result.Result as OkObjectResult;

            Assert.Equal(200, resultData.StatusCode);
            Assert.IsType<Product>(resultData.Value);

        }

        [Fact]
        public async Task PostProduct_Return_Created_Product()
        {
            // Arrange
            
            var mockRepo = new Mock<IProductService>();
            mockRepo.Setup(repo => repo.PostProduct(NewProduct()))
                   .ReturnsAsync(new Product
                   {
                       Id = 1,
                       ProductName = "Test1",
                       ProductDescription = "Test Description 1",
                       Units = "11"
                   });

            var newProduct = NewProduct();
            var controller = new ProductController(mockRepo.Object);

           
            // Act
            var result = await controller.PostProduct(newProduct);

            // Assert
           // var resultData = result.Result as CreatedAtActionResult;

            //mockRepo.Verify();
            Assert.IsType<CreatedAtActionResult>(result);
           
        }

        [Fact]
        public async Task DeleteProduct_Remove_A_Product()
        {
            // Arrange
            int id = 2;

            var mockRepo = new Mock<IProductService>();
            mockRepo.Setup(repo => repo.DeleteProduct(id))
                  .ReturnsAsync((Product)null);

            //Act
            var controller = new ProductController(mockRepo.Object);


            var result = await controller.DeleteProduct(id);

            // Assert
            Assert.IsType<NoContentResult>(result);
        }
    

        [Fact]

        public async Task UpdatePrdocuct_Return_UpdatedProduct()
        {
            // Arrange
            int id = 1;
            var mockRepo = new Mock<IProductService>();
            mockRepo.Setup(repo => repo.UpdateProduct(id,NewProduct()))
                   .ReturnsAsync(new Product
                   {
                       Id = 1,
                       ProductName = "Test2",
                       ProductDescription = "Test Description 1",
                       Units = "11"
                   });

            var newProduct = NewProduct();
            newProduct.ProductName = "Test2";
            var controller = new ProductController(mockRepo.Object);


            // Act
            var result = await controller.UpdateProduct(id,newProduct);

            // Assert
            Assert.IsType<NoContentResult>(result);

        }

        private List<Product> GetProducts()
        {
            var products = new List<Product>();
            products.Add(new Product()
            {
                Id = 1,
                ProductName ="Test1",
                ProductDescription = "Test Description 1",
                Units = "11"
            });
            products.Add(new Product()
            {
                Id = 2,
                ProductName = "Test2",
                ProductDescription = "Test Description 2",
                Units = "12"
            });
            return products;
        }

        private static Product NewProduct()
        {
            return new Product
            {
                Id = 1,
                ProductName = "Test1",
                ProductDescription = "Test Description 1",
                Units = "11"
            };
        }

       
    }
}
