using System;
using Moq;
using System.Collections.Generic;
using Xunit;
using System.Linq;
using SportsStore.Controllers;
using SportsStore.Models;
using SportsStore.Models.ViewModels;



namespace SportsStore.Tests
{
    public class ProductControllerTests
    {
        [Fact]
        public void Can_Paginate()
        {
            Mock<IProductRepository> mock = new Mock<IProductRepository>();
            mock.Setup(m => m.Products).Returns(new Product[]
            {
                new Product {ProductID = 1, Name = "P1"},
                   new Product {ProductID = 2, Name = "P2"},
                   new Product {ProductID = 3, Name = "P3"},
                   new Product {ProductID = 4, Name = "P4"},
                   new Product {ProductID = 5, Name = "P5"}
            });

            ProductController controller = new ProductController(mock.Object);
            controller.PageSize = 3;

            //Act
            var result = controller.List(2).ViewData.Model as ProductsListViewModel;

            //Assert can send pagination view model
            PageInfo pageInfo = result.PageInfo;
            Assert.Equal(2, pageInfo.CurrentPage);
            Assert.Equal(3, pageInfo.ItemsPerPage);
            Assert.Equal(5, pageInfo.TotalItems);
            Assert.Equal(2, pageInfo.TotalPages);


            Product[] prodArray = result.Products.ToArray();
            Assert.True(prodArray.Length == 2);
            Assert.Equal("P4", prodArray[0].Name);
            Assert.Equal("P5", prodArray[1].Name);

        }
    }
}
