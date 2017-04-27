using Castle.Core.Internal;
using Microsoft.AspNetCore.Mvc;
using SportsStore.Controllers;
using SportsStore.Models;
using Moq;
using Xunit;
using System;
using System.Collections.Generic;
using System.Text;

namespace SportsStore.Tests
{
    class OrderControllerTests
    {
        [Fact]
        public void Cannot_checkout_empty_cart()
        {
            Mock<IOrderRepository> mock = new Mock<IOrderRepository>();
            Cart cart = new Cart();
            Order order = new Order();
            OrderController target = new OrderController(mock.Object, cart);

            ViewResult result = target.Checkout(order) as ViewResult;

            mock.Verify(m => m.SaveOrder(It.IsAny<Order>()), Times.Never);

            Assert.True(string.IsNullOrEmpty(result.ViewName));
            Assert.False(result.ViewData.ModelState.IsValid);
        }

        [Fact]
        public void Can_checkout_and_submit_order()
        {
            Mock<IOrderRepository> mock = new Mock<IOrderRepository>();
            Cart cart = new Cart();
            cart.AddItem(new Product(), 1);
            OrderController target = new OrderController(mock.Object, cart);
            RedirectToActionResult result =
               (RedirectToActionResult)target.Checkout(new Order());
            mock.Verify(m => m.SaveOrder(It.IsAny<Order>()), Times.Once);
            Assert.Equal("Completed", result.ActionName);
        }
    }
}
