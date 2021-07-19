using Catalog.Controllers;
using Catalog.Dtos;
using Catalog.Entities;
using Catalog.Repositories;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Moq;
using System;
using Xunit;

namespace UnitTest
{
    public class ItemsControllerTest
    {
        private readonly Mock<IItemsRepository> repositoryStub = new ();
        private readonly Random rand = new();
        [Fact]
        public async void GetItemAsync_WithUnexistingItem_ReturnNotFound()
        {
            //Arrange
            repositoryStub.Setup(repo => repo.GetItemAsync(It.IsAny<Guid>()))
                .ReturnsAsync((Item)null);

            var controller = new ItemsController(repositoryStub.Object);

            //Act

            var result = await controller.GetItemAsync(Guid.NewGuid());

            //Assert

            result.Result.Should().BeOfType<NotFoundResult>();

        }

        [Fact]
        public async void GetItemAsync_WithExistingItem_ReturnExpectedItem()
        {
            //Arrange
            var expectedItem = CreateRandomItem();
            repositoryStub.Setup(repo => repo.GetItemAsync(It.IsAny<Guid>()))
                .ReturnsAsync(expectedItem);

            var controller = new ItemsController(repositoryStub.Object);

            //Act

            var result = await controller.GetItemAsync(Guid.NewGuid());

            //Assert

            result.Value.Should().BeEquivalentTo(expectedItem,
                    options=> options.ComparingByMembers<Item>());
        }

        [Fact]
        public async void GetItemsAsync_WithExistingItems_ReturnAllItems()
        {
            //Arrange
            var expectedItems = new[] { CreateRandomItem(), CreateRandomItem(), CreateRandomItem() };

            repositoryStub.Setup(repo => repo.GetItemsAsync())
                    .ReturnsAsync(expectedItems);
            var controller = new ItemsController(repositoryStub.Object);

            //Act

            var actualItems = await controller.GetItemsAsync();

            //Assert

            actualItems.Should().BeEquivalentTo(expectedItems, 
                options => options.ComparingByMembers<Item>());
        }

        [Fact]
        public async void CreateItemAsync_WithItemToCreate_ReturnCreatedItem()
        {
            //Arrange
            var itemToCreate = new CreateItemDto() { 
                Name = Guid.NewGuid().ToString(),
                Price = rand.Next(1000)
            };

            var controller = new ItemsController(repositoryStub.Object);

            //Act

            var result = await controller.CreateItemAsync(itemToCreate);

            //Assert

            var createdItem = (result.Result as CreatedAtActionResult).Value as ItemDto;
            itemToCreate.Should().BeEquivalentTo(createdItem,
                options => options.ComparingByMembers<ItemDto>().ExcludingMissingMembers());

            createdItem.Id.Should().NotBeEmpty();
            createdItem.CreatedDate.Should().BeCloseTo(DateTimeOffset.UtcNow, 1000);
        }

        [Fact]
        public async void UpdateItemAsync_WithExistingItem_ReturnNoContent()
        {
            //Arrange
            var expectedItem = CreateRandomItem();
            repositoryStub.Setup(repo => repo.GetItemAsync(It.IsAny<Guid>()))
                .ReturnsAsync(expectedItem);

            var itemId = expectedItem.Id;
            var itemToUpdate = new UpdateItemDto()
            {
                Name = Guid.NewGuid().ToString(),
                Price = expectedItem.Price + 10
             };

        var controller = new ItemsController(repositoryStub.Object);

            //Act

            var result = await controller.UpdateItemAsync(itemId,itemToUpdate);

            //Assert

            result.Should().BeOfType<NoContentResult>();
           
        }

        [Fact]
        public async void DeleteItemAsync_WithExistingItem_ReturnNoContent()
        {
            //Arrange
            var expectedItem = CreateRandomItem();
            repositoryStub.Setup(repo => repo.GetItemAsync(It.IsAny<Guid>()))
                .ReturnsAsync(expectedItem);


            var controller = new ItemsController(repositoryStub.Object);

            //Act

            var result = await controller.DeleteItemAsync(expectedItem.Id);

            //Assert

            result.Should().BeOfType<NoContentResult>();

        }

        private Item CreateRandomItem() {

            return new()
            {
                Id = Guid.NewGuid(),
                Name = Guid.NewGuid().ToString(),
                Price = rand.Next(1000),
                CreatedDate = DateTimeOffset.UtcNow
            };
        }
    }
}
