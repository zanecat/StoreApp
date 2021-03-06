﻿using Microsoft.AspNetCore.Mvc;
using Moq;
using SportsStore.Controllers;
using SportsStore.Models;
using SportsStore.Models.ViewModels;
using System;
using System.Linq;
using Xunit;



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
                new Product {ProductID = 1, Name = "P1", Category = "Cat1"},
                new Product {ProductID = 2, Name = "P2", Category = "Cat2"},
                new Product {ProductID = 3, Name = "P3", Category = "Cat1"},
                new Product {ProductID = 4, Name = "P4", Category = "Cat2"},
                new Product {ProductID = 5, Name = "P5", Category = "Cat3"}
            });

            ProductController controller = new ProductController(mock.Object) {PageSize = 3};

            //Act
            var result = controller.List(null, 2).ViewData.Model as ProductsListViewModel;

            //Assert can send pagination view model
            PageInfo pageInfo = result.PageInfo;
            Assert.Equal(2, pageInfo.CurrentPage);
            Assert.Equal(3, pageInfo.ItemsPerPage);
            Assert.Equal(5, pageInfo.TotalItems);
            Assert.Equal(3, pageInfo.TotalPages);
        }
        [Fact]
        public void Generate_Category_Specific_Product_Count()
        {
            // Arrange
            Mock<IProductRepository> mock = new Mock<IProductRepository>();
            mock.Setup(m => m.Products).Returns(new Product[] {
           new Product {ProductID = 1, Name = "P1", Category = "Cat1"},
           new Product {ProductID = 2, Name = "P2", Category = "Cat2"},
           new Product {ProductID = 3, Name = "P3", Category = "Cat1"},
           new Product {ProductID = 4, Name = "P4", Category = "Cat2"},
           new Product {ProductID = 5, Name = "P5", Category = "Cat3"}
});
            ProductController target = new ProductController(mock.Object);
            target.PageSize = 3;
            Func<ViewResult, ProductsListViewModel> GetModel = result =>
                result?.ViewData?.Model as ProductsListViewModel;
            // Action
            int? res1 = GetModel(target.List("Cat1"))?.PageInfo.TotalItems;
            int? res2 = GetModel(target.List("Cat2"))?.PageInfo.TotalItems;
            int? res3 = GetModel(target.List("Cat3"))?.PageInfo.TotalItems;
            int? resAll = GetModel(target.List(null))?.PageInfo.TotalItems;
            // Assert
            Assert.Equal(2, res1);
            Assert.Equal(2, res2);
            Assert.Equal(1, res3);
            Assert.Equal(5, resAll);
        }
    }
}
