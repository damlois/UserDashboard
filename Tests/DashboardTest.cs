using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
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
        #region Index
        [Fact]
        public void Index_ReturnsAViewResult_WithAListOfUsers()
        {
            var mockRepo = new Mock<IDataRepository>();
            mockRepo.Setup(repo => repo.GetMultipleUsers(It.IsAny<int>(), It.IsAny<string>()))
                .Returns<int, string>((x, y) => Task.FromResult(GetTestUsers()));
            var mockTempData = new Mock<ITempDataDictionary>();

            var controller = new HomeController(mockRepo.Object);
            controller.TempData = mockTempData.Object;

            var result = controller.Index(null, 1).GetAwaiter().GetResult();

            var viewResult = Assert.IsType<ViewResult>(result);
            var model = Assert.IsAssignableFrom<List<User>>(
                viewResult.ViewData.Model);
            Assert.Equal(3, model.Count());
        }

        private List<User> GetTestUsers()
        {
            var randomUsers = new List<User>
            {
                new User
                {
                    Name = new UserName { First = "Kayode", Last = "Tola" },
                    Email = "kayodetola@gmail.com",
                    Gender = "Male",
                    Phone = "08100237845",
                    Picture = new UserPicture { Large = "kayode@url.com" }
                },
                new User
                {
                    Name = new UserName { First = "Tunji", Last = "Idowu" },
                    Email = "tunjiidowu@gmail.com",
                    Gender = "Female",
                    Phone = "08100237843",
                    Picture = new UserPicture { Large = "tunji@url.com" }
                },
                new User
                {
                    Name = new UserName { First = "Tola", Last = "Eben" },
                    Email = "tolaeben@gmail.com",
                    Gender = "Male",
                    Phone = "070123489422",
                    Picture = new UserPicture { Large = "tola@url.com" }
                }
            };

            return randomUsers;
        }

        [Fact]
        public void Index_ReturnsSameUser_CheckRandomUserAfterPagination()
        {
            var mockRepo = new Mock<IDataRepository>();
            mockRepo.Setup(repo => repo.GetMultipleUsers(It.IsAny<int>(), It.IsAny<string>()))
                .Returns<int, string>((x, y) => Task.FromResult(GetTestUsers()));
            var mockTempData = new Mock<ITempDataDictionary>();

            var controller = new HomeController(mockRepo.Object);
            controller.TempData = mockTempData.Object;

            var result1 = controller.Index(null, 1).GetAwaiter().GetResult();
            var viewResult1 = Assert.IsType<ViewResult>(result1);

            var result2 = controller.Index(null, 2).GetAwaiter().GetResult();
            var viewResult2 = Assert.IsType<ViewResult>(result2);

            var areEqual = viewResult1.TempData["randomUser"] == viewResult2.TempData["randomUser"];
            Assert.True(areEqual);
        }

        [Fact]
        public void Index_ReturnsAViewResult_WithNoUser()
        {
            var mockRepo = new Mock<IDataRepository>();
            mockRepo.Setup(repo => repo.GetMultipleUsers(It.IsAny<int>(), It.IsAny<string>()))
                .Returns<int, string>((x, y) => Task.FromResult<List<User>>(null));
            var mockTempData = new Mock<ITempDataDictionary>();

            var controller = new HomeController(mockRepo.Object);
            controller.TempData = mockTempData.Object;

            var result = controller.Index(null, 1).GetAwaiter().GetResult();

            var viewResult = Assert.IsType<ViewResult>(result);
            var model = Assert.IsAssignableFrom<List<User>>(
                viewResult.ViewData.Model);
            Assert.Empty(model);
        }
        #endregion

    }
}