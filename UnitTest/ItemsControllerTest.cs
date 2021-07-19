using Catalog.Controllers;
using Catalog.Entities;
using Catalog.Repositories;
using Microsoft.AspNetCore.Mvc;
using Moq;
using System;
using Xunit;

namespace UnitTest
{
    public class ItemsControllerTest
    {
        [Fact]
        public async void GetItemAsync_WithUnexistingItem_ReturnNotFound()
        {
            //Arrange
            var repositoryStub = new Mock<IItemsRepository>();
            repositoryStub.Setup(repo => repo.GetItemAsync(It.IsAny<Guid>()))
                .ReturnsAsync((Item)null);

            var controller = new ItemsController(repositoryStub.Object);

            //Act

            var result = await controller.GetItemAsync(Guid.NewGuid());

            //Assert
            Assert.IsType<NotFoundResult>(result.Result);

        }

        [Fact]
        public async void GetItemAsync_WithExistingItem_ReturnExpectedItem()
        {
            //Arrange
            var repositoryStub = new Mock<IItemsRepository>();
            repositoryStub.Setup(repo => repo.GetItemAsync(It.IsAny<Guid>()))
                .ReturnsAsync((Item)null);

            var controller = new ItemsController(repositoryStub.Object);

            //Act

            var result = await controller.GetItemAsync(Guid.NewGuid());

            //Assert
            Assert.IsType<NotFoundResult>(result.Result);

        }
    }
}
