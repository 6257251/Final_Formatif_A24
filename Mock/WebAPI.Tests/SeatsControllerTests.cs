using Microsoft.AspNetCore.Cors.Infrastructure;
using Microsoft.AspNetCore.Mvc;
using Moq;
using WebAPI.Controllers;
using WebAPI.Exceptions;
using WebAPI.Models;
using WebAPI.Services;

namespace WebAPI.Tests;

[TestClass]
public class SeatsControllerTests
{

    Mock<SeatsService> seatsServiceMock;
    Mock<SeatsController> seatsControllerMock;

    public SeatsControllerTests()
    {
        seatsServiceMock = new Mock<SeatsService>();
        seatsControllerMock = new Mock<SeatsController>(seatsServiceMock.Object) { CallBase = true };

        seatsControllerMock.Setup(s => s.UserId).Returns("2");
    }

    [TestMethod]
    public void ReserveSeat()
    {
        Seat seat = new Seat()
        {
            Id = 1,
            Number = 1,
        };

        seatsServiceMock.Setup(s => s.ReserveSeat(It.IsAny<String>(), It.IsAny<int>())).Returns(seat);

        var actionResult = seatsControllerMock.Object.ReserveSeat(seat.Number);
        var result = actionResult.Result as OkObjectResult;
        Assert.IsNotNull(result);
    }

    [TestMethod]
    public void ReserveAlreadyTakenSeat()
    {
        //Seat seat = new Seat()
        //{
        //    Id = 1,
        //    Number = 1,
        //};

        seatsServiceMock.Setup(s => s.ReserveSeat(It.IsAny<String>(), It.IsAny<int>())).Throws(new SeatAlreadyTakenException());

        var actionResult = seatsControllerMock.Object.ReserveSeat(1);
        var result = actionResult.Result as UnauthorizedResult;
        Assert.IsNotNull(result);
    }

    [TestMethod]
    public void ReserveHigherNumberThanMaximumSeat()
    {
        seatsServiceMock.Setup(s => s.ReserveSeat(It.IsAny<String>(), It.IsAny<int>())).Throws(new SeatOutOfBoundsException());

        var actionResult = seatsControllerMock.Object.ReserveSeat(1);
        var result = actionResult.Result as NotFoundObjectResult;

        Assert.IsNotNull(result);
        Assert.AreEqual("Could not find 1", result.Value);
    }

    [TestMethod]
    public void ReserveSeatWhenUserAlreadyReservedSeat()
    {
        seatsServiceMock.Setup(s => s.ReserveSeat(It.IsAny<String>(), It.IsAny<int>())).Throws(new UserAlreadySeatedException());

        var actionResult = seatsControllerMock.Object.ReserveSeat(1);
        var result = actionResult.Result as BadRequestResult;

        Assert.IsNotNull(result);
    }
}
