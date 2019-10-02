using System;
using System.Linq;
using Xunit;

namespace InGameDemo.WebApiTest
{
    public class WebApiDataTest
    {
        [Fact]
        public void GetAll_Test()
        {
            CategoryRepositoryTest categoryRepositoryTest = new CategoryRepositoryTest();
            var expected = categoryRepositoryTest._mockCategoryRepository.GetAll().Result.Count();

            Assert.NotNull(expected);
            Assert.True(expected > 0);
        }

        [Fact]
        public void GetById_Test()
        {
            CategoryRepositoryTest categoryRepositoryTest = new CategoryRepositoryTest();
            var expected = categoryRepositoryTest._mockCategoryRepository.GetById(90).Result;

            Assert.NotNull(expected);
            Assert.True(expected != null);
        }

        [Fact]
        public void Add_Test()
        {
            CategoryRepositoryTest categoryRepositoryTest = new CategoryRepositoryTest();
            var expected = categoryRepositoryTest._mockCategoryRepository.Add(new WebApi.Data.Categories
            {
                Id = 95,
                Name = "Test",
                IsParent = true,
                ParentId = null,
                CreateDate = DateTime.Now
            }).Result;

            Assert.True(expected);
        }
    }
}
