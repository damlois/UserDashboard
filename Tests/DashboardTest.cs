using Microsoft.AspNetCore.Mvc;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UserDashboard.Controllers;
using UserDashboard.Models;
using UserDashboard.Repository;
using Xunit;

namespace Tests
{
    public class DashboardTest
    {
        [Fact]
        public void Index_ReturnsAViewResult_WithAListOfUsers()
        {
            // Arrange
            var mockRepo = new Mock<IDataRepository>();
            mockRepo.Setup(repo => repo.GetMultipleUsers(3, null, 1))
                .Returns(GetTestUsers());
            var controller = new HomeController(mockRepo.Object);

            // Act
            var result = controller.Index(null, 1).GetAwaiter().GetResult();

            // Assert
            var viewResult = Assert.IsType<ViewResult>(result);
            var model = Assert.IsAssignableFrom<List<User>>(
                viewResult.ViewData.Model);
            Assert.Equal(3, model.Count());
        }

        private async  Task<List<User>> GetTestUsers()
        {
            var mockRepo = new Mock<IDataRepository>();
            return await mockRepo.Object.GetMultipleUsers(3, "female", 1);
            //var randomUsers = new List<User>
            //{
            //    new User
            //    {
            //        Name = new UserName { First = "Kayode", Last = "Tola" },
            //        Email = "kayodetola@gmail.com",
            //        Gender = "Male",
            //        Phone = "08100237845",
            //        Picture = new UserPicture { Large = "kayode@url.com" }
            //    },
            //    new User
            //    {
            //        Name = new UserName { First = "Tunji", Last = "Idowu" },
            //        Email = "tunjiidowu@gmail.com",
            //        Gender = "Female",
            //        Phone = "08100237843",
            //        Picture = new UserPicture { Large = "tunji@url.com" }
            //    },
            //    new User
            //    {
            //        Name = new UserName { First = "Tola", Last = "Eben" },
            //        Email = "tolaeben@gmail.com",
            //        Gender = "Male",
            //        Phone = "070123489422",
            //        Picture = new UserPicture { Large = "tola@url.com" }
            //    }
            //};

            //return randomUsers;
        }
    }
}
