using EmployeeProject.Data;
using EmployeeProject.Models;
using EmployeeProject.Services;
using Microsoft.EntityFrameworkCore;
using Moq;
using NUnit.Framework;

namespace EmployeeProjectTests
{
    [TestFixture]
    public class EmployeeControllerTests
    {
        private Mock<ApplicationDbContext> _mockContext;
        private readonly IWebHostEnvironment _webHostEnvironment;
        private EmployeeServices _employeeService;

        [SetUp]
        public void SetUp()
        {
            _mockContext = new Mock<ApplicationDbContext>();
            _employeeService = new EmployeeServices(_mockContext.Object, _webHostEnvironment);
        }

        [Test]
        public void IsValidUser_ValidUser_ReturnsTrue()
        {
            // Arrange
            var model = new LoginModel { Username = "Tesla", Password = "password@123" };

            var mockSet = new Mock<DbSet<EmployeeModel>>();
            //mockSet.As<IQueryable<EmployeeModel>>().Setup(m => m.Provider).Returns(users.AsQueryable().Provider);
            //mockSet.As<IQueryable<EmployeeModel>>().Setup(m => m.Expression).Returns(users.AsQueryable().Expression);
            //mockSet.As<IQueryable<EmployeeModel>>().Setup(m => m.ElementType).Returns(users.AsQueryable().ElementType);
            //mockSet.As<IQueryable<EmployeeModel>>().Setup(m => m.GetEnumerator()).Returns(users.AsQueryable().GetEnumerator());

            _mockContext.Setup(c => c.Employees).Returns(mockSet.Object);
            var result = _employeeService.IsValidUser(model);
            Assert.That(result, Is.True);
        }

       
    }
}
