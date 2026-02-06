using Xunit;
using Moq;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

public class HospitalControllerTests
{
    [Fact]
    public async Task GetHospitalById_ReturnsOkResult_WhenHospitalExists()
    {
        // Arrange
        var mockService = new Mock<IHospitalService>();
        mockService.Setup(s => s.GetHospitalAsync(1))
                   .ReturnsAsync(new Hospital { Id = 1, Name = "City Hospital" });

        var controller = new HospitalController(mockService.Object);

        // Act
        var result = await controller.GetHospitalById(1);

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result);
        var hospital = Assert.IsType<Hospital>(okResult.Value);
        Assert.Equal(1, hospital.Id);
        Assert.Equal("City Hospital", hospital.Name);
    }

    [Fact]
    public async Task GetHospitalById_ReturnsNotFound_WhenHospitalDoesNotExist()
    {
        // Arrange
        var mockService = new Mock<IHospitalService>();
        mockService.Setup(s => s.GetHospitalAsync(99))
                   .ReturnsAsync((Hospital)null);

        var controller = new HospitalController(mockService.Object);

        // Act
        var result = await controller.GetHospitalById(99);

        // Assert
        Assert.IsType<NotFoundResult>(result);
    }
}

// Example Hospital model
public class Hospital
{
    public int Id { get; set; }
    public string Name { get; set; }
}

// Example service interface
public interface IHospitalService
{
    Task<Hospital> GetHospitalAsync(int id);
}
