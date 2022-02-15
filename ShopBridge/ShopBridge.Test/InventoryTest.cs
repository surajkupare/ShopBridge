using AutoMapper;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using ShopBridge.Controllers;
using ShopBridge.DTOs;
using ShopBridge.Interfaces;
using ShopBridge.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace ShopBridge.Test
{
    [TestClass]
    public class InventoryTest
    {
        [TestMethod]
        [DataRow(true)] //Happy flow
        [DataRow(false)] //return null from repo
        public async Task GetInventories(bool returnNull)
        {
            //1. Arrange
            Mock<IInventoryRepository> mockRepository = new Mock<IInventoryRepository>();
            Mock<IMapper> mapperMock = new Mock<IMapper>();

            if (!returnNull)
            {
                IList<Inventory> inventories;
                inventories = new List<Inventory>
                {
                    new Inventory
                    {
                        Id = 1,
                        Name = "Test",
                        Price = 1000
                    },
                    new Inventory
                    {
                        Id = 2,
                        Name = "demo",
                        Price = 5000
                    }
                };


                IList<InventoryDto> inventoryDtos;
                inventoryDtos = new List<InventoryDto>
                {
                    new InventoryDto
                    {
                        Id = 1,
                        Name = "Test",
                        Price = 1000
                    },
                    new InventoryDto
                    {
                        Id = 2,
                        Name = "demo",
                        Price = 5000
                    }
                };
                mapperMock.Setup(m => m.Map<IList<InventoryDto>>(It.IsAny<IList<InventoryDto>>())).Returns(inventoryDtos);
                mockRepository.Setup(x => x.GetInventories()).ReturnsAsync(() => inventories);
            }
            else
                mockRepository.Setup(x => x.GetInventories()).ReturnsAsync(() => null);


            InventoryController inventoryController = new InventoryController(mockRepository.Object, mapperMock.Object);

            //2. Act
            var actualObjResult = await inventoryController.GetInventories();

            //3. Assert
            actualObjResult.Should().NotBeNull();

            int? status;
            if (actualObjResult as NotFoundResult != null || actualObjResult as BadRequestResult != null || actualObjResult as OkResult != null)
                status = ((StatusCodeResult)actualObjResult).StatusCode;
            else
                status = ((ObjectResult)actualObjResult).StatusCode;

            if (returnNull)
                Assert.IsTrue(status == 404);
            else
                Assert.IsTrue(status == 200);
        }


        [TestMethod]
        [DataRow(1)]
        [DataRow(null)]
        public async Task GetInventory(int? id)
        {
            //1. Arrange
            Mock<IInventoryRepository> mockRepository = new Mock<IInventoryRepository>();
            Mock<IMapper> mapperMock = new Mock<IMapper>();

            if (id != null)
            {
                Inventory inventory = new Inventory
                {
                    Id = 1,
                    Description = "Test data",
                    Name = "TestName",
                    Price = 2000
                };

                mockRepository.Setup(x => x.GetInventory(It.IsAny<int>())).ReturnsAsync(() => inventory);
                mapperMock.Setup(m => m.Map<Inventory>(It.IsAny<Inventory>())).Returns(inventory);

            }
            else
                mockRepository.Setup(x => x.GetInventory(It.IsAny<int>())).ReturnsAsync(() => null);

            InventoryController inventoryController = new InventoryController(mockRepository.Object, mapperMock.Object);

            //2. Act
            var actualObjResult = await inventoryController.GetInventory(id == null ? 0 : 1);

            //3. Assert
            actualObjResult.Should().NotBeNull();

            int? status;
            if (actualObjResult as NotFoundResult != null || actualObjResult as BadRequestResult != null)
                status = ((StatusCodeResult)actualObjResult).StatusCode;
            else
                status = ((ObjectResult)actualObjResult).StatusCode;

            if (id == null)
                Assert.IsTrue(status == 404);
            else
                Assert.IsTrue(status == 200);
        }


        [TestMethod]
        [DataRow(true)] //Happy flow
        [DataRow(false)] //return null from repo
        public async Task UpdateInventory(bool isSavedAllSuccessed)
        {
            //1. Arrange
            Mock<IInventoryRepository> mockRepository = new Mock<IInventoryRepository>();
            Mock<IMapper> mapperMock = new Mock<IMapper>();

            Inventory inventory = new Inventory
            {
                Id = 1,
                Description = "Test data",
                Name = "TestName",
                Price = 2000
            };

            InventoryDto inventoryDto = new InventoryDto
            {
                Id = 1,
                Description = "Test data",
                Name = "TestName",
                Price = 2000
            };

            if (isSavedAllSuccessed)
                mockRepository.Setup(x => x.SaveAll()).ReturnsAsync(() => true);
            else
                mockRepository.Setup(x => x.SaveAll()).ReturnsAsync(() => false);

            mockRepository.Setup(x => x.GetInventory(It.IsAny<int>())).ReturnsAsync(() => inventory);
            mapperMock.Setup(m => m.Map<Inventory>(It.IsAny<Inventory>())).Returns(inventory);


            InventoryController inventoryController = new InventoryController(mockRepository.Object, mapperMock.Object);

            //2. Act
            var actualObjResult = await inventoryController.UpdateInventory(1, inventoryDto);

            //3. Assert
            actualObjResult.Should().NotBeNull();

            int? status;
            if (actualObjResult as NotFoundResult != null || actualObjResult as BadRequestResult != null || actualObjResult as NoContentResult != null)
                status = ((StatusCodeResult)actualObjResult).StatusCode;
            else
                status = ((ObjectResult)actualObjResult).StatusCode;

            if (!isSavedAllSuccessed)
                Assert.IsTrue(status == 400);
            else
                Assert.IsTrue(status == 204);
        }


        [TestMethod]
        [DataRow(true)] //Happy flow
        [DataRow(false)] //return null from repo
        public async Task AddInventory(bool isSavedAllSuccessed)
        {
            //1. Arrange
            Mock<IInventoryRepository> mockRepository = new Mock<IInventoryRepository>();
            Mock<IMapper> mapperMock = new Mock<IMapper>();

            Inventory inventory = new Inventory
            {
                Id = 1,
                Description = "Test data",
                Name = "TestName",
                Price = 2000
            };

            InventoryDto inventoryDto = new InventoryDto
            {
                Id = 1,
                Description = "Test data",
                Name = "TestName",
                Price = 2000
            };

            mockRepository.Setup(x => x.Add(It.IsAny<Inventory>()));

            if (isSavedAllSuccessed)
                mockRepository.Setup(x => x.SaveAll()).ReturnsAsync(() => true);
            else
                mockRepository.Setup(x => x.SaveAll()).ReturnsAsync(() => false);

            mapperMock.Setup(m => m.Map<Inventory>(It.IsAny<Inventory>())).Returns(inventory);

            InventoryController inventoryController = new InventoryController(mockRepository.Object, mapperMock.Object);

            //2. Act
            var actualObjResult = await inventoryController.AddInventory(inventoryDto);

            //3. Assert
            actualObjResult.Should().NotBeNull();

            int? status;
            if (actualObjResult as NotFoundResult != null || actualObjResult as BadRequestResult != null || actualObjResult as OkResult != null)
                status = ((StatusCodeResult)actualObjResult).StatusCode;
            else
                status = ((ObjectResult)actualObjResult).StatusCode;

            if (!isSavedAllSuccessed)
                Assert.IsTrue(status == 400);
            else
                Assert.IsTrue(status == 200);
        }

        [TestMethod]
        [DataRow(true)] //Happy flow
        [DataRow(false)] //return null from repo
        public async Task DeleteInventory(bool isSavedAllSuccessed)
        {
            //1. Arrange
            Mock<IInventoryRepository> mockRepository = new Mock<IInventoryRepository>();
            Mock<IMapper> mapperMock = new Mock<IMapper>();

            Inventory inventory = new Inventory
            {
                Id = 1,
                Description = "Test data",
                Name = "TestName",
                Price = 2000
            };

            mockRepository.Setup(x => x.Delete(It.IsAny<Inventory>()));
            mockRepository.Setup(x => x.GetInventory(It.IsAny<int>())).ReturnsAsync(() => inventory);

            if (isSavedAllSuccessed)
                mockRepository.Setup(x => x.SaveAll()).ReturnsAsync(() => true);
            else
                mockRepository.Setup(x => x.SaveAll()).ReturnsAsync(() => false);

            mapperMock.Setup(m => m.Map<Inventory>(It.IsAny<Inventory>())).Returns(inventory);

            InventoryController inventoryController = new InventoryController(mockRepository.Object, mapperMock.Object);

            //2. Act
            var actualObjResult = await inventoryController.DeleteInventory(1);

            //3. Assert
            actualObjResult.Should().NotBeNull();

            int? status;
            if (actualObjResult as NotFoundResult != null || actualObjResult as BadRequestResult != null || actualObjResult as OkResult != null)
                status = ((StatusCodeResult)actualObjResult).StatusCode;
            else
                status = ((ObjectResult)actualObjResult).StatusCode;

            if (!isSavedAllSuccessed)
                Assert.IsTrue(status == 400);
            else
                Assert.IsTrue(status == 200);
        }

    }
}
