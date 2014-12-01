//using Ninject;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using SwipeBox.DAL.Context;
using SwipeBox.DAL.Repositories;
using SwipeBox.Shared.Entities;
using System.Collections.Generic;
using System.Linq;

namespace SwipeBox.DAL.Test
{
    [TestClass]
    public class ClientRepositoryTest
    {

        private IClientRepository m_repo;
        private Mock<ISwipeBoxContext> m_context;

        [TestInitialize]
        public void Initialize()
        {
            // Set up mock DbContext
            m_context = new Mock<ISwipeBoxContext>();

            var data = new List<Client>
            {
                new Client { Name = "Dan", Email = "danblackmore@gmail.com", PhoneNumber = "01242266200" },
                new Client { Name = "Joe", Email = "Joebloggs@gmail.com", PhoneNumber = "22334455" },
                new Client { Name = "Ben", Email = "Benbloggs@gmail.com", PhoneNumber = "22334455" }

            }.AsQueryable();

            var clients = new Mock<System.Data.Entity.DbSet<Client>>();
            clients.As<IQueryable<Client>>().Setup(c => c.Provider).Returns(data.Provider);
            clients.As<IQueryable<Client>>().Setup(c => c.Expression).Returns(data.Expression);
            clients.As<IQueryable<Client>>().Setup(c => c.ElementType).Returns(data.ElementType);
            clients.As<IQueryable<Client>>().Setup(c => c.GetEnumerator()).Returns(data.GetEnumerator());

            m_context.Setup(c => c.Clients).Returns(clients.Object);
            m_context.Setup(c => c.SaveChanges()).Callback(() =>
            {
                m_context.Object.Clients = clients.Object;
            });

            m_repo = new EFClientRepository(m_context.Object);
        }


        [TestMethod]
        public void GetClients_IsPopulated()
        {
            // Arrange
            var numberOfClients = 3;

            // Act
            var clients = m_repo.Get;

            // Assert
            Assert.IsNotNull(clients);
            Assert.AreEqual(typeof(Client), clients.First().GetType());
            Assert.AreEqual(numberOfClients, clients.Count());

        }

        [TestMethod]
        public void SaveClient_NewClient_IsAdded()
        {
            // Arrange
            var client = new Client()
            {
                Name = "Barry",
                Email = "Barry@hotmail.com",
                PhoneNumber = "01242233423"
            };

            // Act
            var returnValue = m_repo.Save(client);
            
            // Assert
            m_context.Verify(m => m.SaveChanges(), Times.Once);
            
        }
    }
}
