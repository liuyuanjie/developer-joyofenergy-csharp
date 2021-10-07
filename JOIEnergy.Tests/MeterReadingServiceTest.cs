using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading;
using Dapper;
using JOIEnergy.Application;
using JOIEnergy.Application.Commands;
using JOIEnergy.Application.Model;
using JOIEnergy.Application.Services;
using JOIEnergy.Domain.Entity;
using JOIEnergy.Domain.Interfaces;
using Moq;
using Moq.Dapper;
using Xunit;

namespace JOIEnergy.Tests
{
    public class MeterReadingServiceTest
    {
        private static string SMART_METER_ID = "smart-meter-id";

        private readonly Mock<IConnection> _mockConnection;
        private readonly Mock<IDbConnection> _mockDbConnection;
        private readonly Mock<IRepository<SmartMeter>> _mockRepository;
        private readonly Mock<IUnitOfWork> _mockUnitOfWork;

        private readonly List<ElectricityReadingModel> _electricityReadingModels;

        private readonly MeterReadingService _meterReadingService;

        public MeterReadingServiceTest()
        {
            _mockConnection = new Mock<IConnection>();
            _mockDbConnection = new Mock<IDbConnection>();
            _mockConnection.Setup(x => x.OpenConnection())
                .Returns(_mockDbConnection.Object);

            _mockUnitOfWork = new Mock<IUnitOfWork>();
            _mockRepository = new Mock<IRepository<SmartMeter>>();
            _mockRepository.Setup(x => x.UnitOfWork)
                .Returns(_mockUnitOfWork.Object);

            _meterReadingService = new MeterReadingService(_mockConnection.Object, _mockRepository.Object);

            //Arrange
            _electricityReadingModels = new List<ElectricityReadingModel>
            {
                new ElectricityReadingModel() { Time = DateTime.Now.AddMinutes(-30), Reading = 35m },
                new ElectricityReadingModel() { Time = DateTime.Now.AddMinutes(-15), Reading = 30m }
            };
        }

        [Fact]
        public void GivenMeterIdThatDoesNotExistShouldReturnNull()
        {
            //Arrange
            _mockDbConnection.SetupDapper(x => x.Query<ElectricityReadingModel>(It.IsAny<string>(),
                    It.IsAny<object>(),
                    It.IsAny<IDbTransaction>(), It.IsAny<bool>(), It.IsAny<int?>(), It.IsAny<CommandType?>()))
                .Returns(new List<ElectricityReadingModel>());

            //Assert
            var electricityReadingModels = _meterReadingService.GetReadings("unknown-id");
            Assert.Empty(electricityReadingModels);
        }

        [Fact]
        public void GivenMeterReadingThatExistsShouldReturnMeterReadings()
        {
            //Arrange
            _mockDbConnection.SetupDapper(x => x.Query<ElectricityReadingModel>(It.IsAny<string>(),
                    It.IsAny<object>(),
                    It.IsAny<IDbTransaction>(), It.IsAny<bool>(), It.IsAny<int?>(), It.IsAny<CommandType?>()))
                .Returns(_electricityReadingModels);

            var electricityReadingModel = new ElectricityReadingModel() { Time = DateTime.Now, Reading = 25m };

            _mockRepository.Setup(x => x.Query())
                .Returns(new List<SmartMeter>(){ new SmartMeter
                {
                    SmartMeterId = SMART_METER_ID,
                    ElectricityReadings = _electricityReadingModels
                        .Select(x => new ElectricityReading {Time = x.Time, Reading = x.Reading}).ToList()
                }}.AsQueryable());

            _mockUnitOfWork.Setup(x => x.CommitAsync(It.IsAny<CancellationToken>()))
                .Callback(() =>
                {
                    _electricityReadingModels.Add(electricityReadingModel);
                });

            //Act
            _meterReadingService.StoreReadings(new ElectricityReadingCommand
            {
                SmartMeterId = SMART_METER_ID,
                ElectricityReadingModels = new List<ElectricityReadingModel>() {
                    electricityReadingModel
                }
            });

            //Assert
            var electricityReadings = _meterReadingService.GetReadings(SMART_METER_ID);

            Assert.Equal(3, electricityReadings.Count);
        }
    }
}