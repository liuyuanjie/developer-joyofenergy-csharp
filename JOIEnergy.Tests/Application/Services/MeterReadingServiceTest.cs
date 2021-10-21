using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading;
using Dapper;
using JOIEnergy.Application.Commands;
using JOIEnergy.Application.Model;
using JOIEnergy.Application.Services;
using JOIEnergy.Domain.Entity;
using Moq;
using Moq.Dapper;
using Xunit;

namespace JOIEnergy.Tests.Application.Services
{
    public class MeterReadingServiceTest : ServiceBase
    {
        private static string SMART_METER_ID = "smart-meter-id";
        private readonly List<ElectricityReadingModel> _electricityReadingModels;

        private readonly MeterReadingService _meterReadingService;

        public MeterReadingServiceTest()
        {
            _meterReadingService = new MeterReadingService(ConnectionMock.Object, SmartMeterRepositoryMock.Object);

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
            DbConnectionMock.SetupDapper(x => x.Query<ElectricityReadingModel>(It.IsAny<string>(),
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
            DbConnectionMock.SetupDapper(x => x.Query<ElectricityReadingModel>(It.IsAny<string>(),
                    It.IsAny<object>(),
                    It.IsAny<IDbTransaction>(), It.IsAny<bool>(), It.IsAny<int?>(), It.IsAny<CommandType?>()))
                .Returns(_electricityReadingModels);

            var electricityReadingModel = new ElectricityReadingModel() { Time = DateTime.Now, Reading = 25m };

            SmartMeterRepositoryMock.Setup(x => x.Query())
                .Returns(new List<SmartMeter>(){ new SmartMeter
                {
                    SmartMeterId = SMART_METER_ID,
                    ElectricityReadings = _electricityReadingModels
                        .Select(x => new ElectricityReading {Time = x.Time, Reading = x.Reading}).ToList()
                }}.AsQueryable());

            UnitOfWorkMock.Setup(x => x.CommitAsync(It.IsAny<CancellationToken>()))
                .Callback(() =>
                {
                    _electricityReadingModels.Add(electricityReadingModel);
                });

            //Act
            _meterReadingService.StoreReadings(new ElectricityReadingCommand
            {
                SmartMeterId = SMART_METER_ID,
                ElectricityReadings = new List<ElectricityReadingModel>() {
                    electricityReadingModel
                }
            }).Wait();

            //Assert
            var electricityReadings = _meterReadingService.GetReadings(SMART_METER_ID);

            Assert.Equal(3, electricityReadings.Count);
        }

        [Fact]
        public void GivenValidSmartMeterIdAndOneDateShouldReturnLastWeekReadingWithoutBeforeLastMondayReadings()
        {
            var dateTime = DateTime.Today;
            var currentWeekMonday = dateTime.AddDays(-(byte)DateTime.Now.DayOfWeek);
            var lastWeekMonday = currentWeekMonday.AddDays(-7);
            _electricityReadingModels.AddRange(new List<ElectricityReadingModel>()
            {
                new ElectricityReadingModel()
                {
                    Reading = 10m,
                    Time = lastWeekMonday.AddDays(-1)
                },
                new ElectricityReadingModel()
                {
                    Reading = 10m,
                    Time = lastWeekMonday.AddDays(-7)
                },
                new ElectricityReadingModel()
                {
                    Reading = 10m,
                    Time = lastWeekMonday.AddDays(-10)
                }
            });

            var smartMeter = new SmartMeter()
            {
                SmartMeterId = SMART_METER_ID,
                ElectricityReadings = _electricityReadingModels.Select(x => new ElectricityReading()
                {
                    Time = x.Time,
                    Reading = x.Reading
                }).ToList()
            };

            SmartMeterRepositoryMock.Setup(x => x.Query())
                .Returns(new List<SmartMeter>()
                {
                    smartMeter
                }.AsQueryable);

            var electricityReadingModels = _meterReadingService.GetLastWeekReadings(SMART_METER_ID, dateTime);

            Assert.Equal(2, electricityReadingModels.Count);
        }

        [Fact]
        public void GivenValidSmartMeterIdAndOneDateShouldReturnLastWeekReadingWithoutAfterLastSundayReadings()
        {
            var dateTime = DateTime.Today;
            var currentWeekMonday = dateTime.AddDays(-(byte)DateTime.Now.DayOfWeek);
            var lastWeekSunday = currentWeekMonday.AddDays(-1);
            _electricityReadingModels.AddRange(new List<ElectricityReadingModel>()
            {
                new ElectricityReadingModel()
                {
                    Reading = 10m,
                    Time = lastWeekSunday.AddDays(-1)
                },
                new ElectricityReadingModel()
                {
                    Reading = 10m,
                    Time = lastWeekSunday.AddDays(-2)
                },
                new ElectricityReadingModel()
                {
                    Reading = 10m,
                    Time = lastWeekSunday.AddDays(1)
                },
                new ElectricityReadingModel()
                {
                    Reading = 10m,
                    Time = lastWeekSunday.AddDays(7)
                },
                new ElectricityReadingModel()
                {
                    Reading = 10m,
                    Time = lastWeekSunday.AddDays(10)
                }
            });

            var smartMeter = new SmartMeter()
            {
                SmartMeterId = SMART_METER_ID,
                ElectricityReadings = _electricityReadingModels.Select(x => new ElectricityReading()
                {
                    Time = x.Time,
                    Reading = x.Reading
                }).ToList()
            };

            SmartMeterRepositoryMock.Setup(x => x.Query())
                .Returns(new List<SmartMeter>()
                {
                    smartMeter
                }.AsQueryable);

            var electricityReadingModels = _meterReadingService.GetLastWeekReadings(SMART_METER_ID, dateTime);

            Assert.Equal(2, electricityReadingModels.Count);
        }
    }
}