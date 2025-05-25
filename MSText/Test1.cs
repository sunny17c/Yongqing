using Moq;
using WebApplication1.Interfaces;
using Yongqing.Database;

namespace MSText;
[TestClass]
public sealed class Test1
{
    private Mock<IBooksService> _mockService = null!;

    [TestInitialize]
    public void Setup()
    {
        _mockService = new Mock<IBooksService>();
    }

    [TestMethod]
    public async Task GetAllAsync()
    {
        _mockService.Setup(s => s.GetAllAsync())
                    .ReturnsAsync(new List<Books>());
        var result = await _mockService.Object.GetAllAsync();

        Assert.IsNotNull(result); 
        _mockService.Verify(s => s.GetAllAsync(), Times.Once);
    }

    [TestMethod]
    public async Task GetByIdAsync()
    {
        _mockService.Setup(s => s.GetByIdAsync(1))
                    .ReturnsAsync(new Books { Id = 1, Title = "第一本", Author = "作者一" });
        var result = await _mockService.Object.GetByIdAsync(1);

        Assert.IsNotNull(result);
        _mockService.Verify(s => s.GetAllAsync(), Times.Once);
    }

    [TestMethod]
    public async Task CreateAsync()
    {
        _mockService.Setup(s => s.CreateAsync(It.Is<Books>(b => b.Title == "第一本" && b.Author == "作者一")))
                   .ReturnsAsync(true);
        var result = await _mockService.Object.CreateAsync(new Books { Title = "第一本", Author = "作者一" });

        Assert.IsTrue(result);
        _mockService.Verify(s => s.GetAllAsync(), Times.Once);
    }

    [TestMethod]
    public async Task UpdateAsync()
    {
        _mockService.Setup(s => s.UpdateAsync(It.Is<Books>(b => b.Id == 1 && b.Title == "第一本" && b.Author == "作者一")))
                   .ReturnsAsync(true);
        var result = await _mockService.Object.UpdateAsync(new Books { Id = 1, Title = "第一本", Author = "作者一" });

        Assert.IsTrue(result);
        _mockService.Verify(s => s.GetAllAsync(), Times.Once);
    }

    [TestMethod]
    public async Task DeleteAsync()
    {
        _mockService.Setup(s => s.DeleteAsync(1))
                   .ReturnsAsync(true);
        var result = await _mockService.Object.DeleteAsync(1);

        Assert.IsTrue(result);
        _mockService.Verify(s => s.GetAllAsync(), Times.Once);
    }
}
