using InGameDemo.WebApi.Data;
using InGameDemo.WebApi.Data.Interfaces;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace InGameDemo.WebApiTest
{
    public class CategoryRepositoryTest
    {
        public readonly IGenericRepository<Categories> _mockCategoryRepository;

        public CategoryRepositoryTest()
        {
            var categoryList = new List<Categories>
            {
                new Categories { Id = 90, Name = "Kategori 1", IsParent = true, ParentId = null, CreateDate = DateTime.Now },
                new Categories { Id = 91, Name = "Kategori 2", IsParent = true, ParentId = 90, CreateDate = DateTime.Now },
                new Categories { Id = 92, Name = "Kategori 2", IsParent = false, ParentId = 91, CreateDate = DateTime.Now }
            };

            var mockCategoryRepository = new Mock<IGenericRepository<Categories>>();

            mockCategoryRepository.Setup(s => s.GetAll()).Returns(Task.FromResult(categoryList));

            mockCategoryRepository.Setup(s => s.GetById(It.IsAny<int>())).Returns(Task.FromResult(categoryList.Single(s => s.Id == 90)));

            mockCategoryRepository.Setup(s => s.Add(It.IsAny<Categories>())).Callback((Categories target) => { categoryList.Add(target); }).Returns(Task.FromResult(true));

            _mockCategoryRepository = mockCategoryRepository.Object;
        }
    }
}
