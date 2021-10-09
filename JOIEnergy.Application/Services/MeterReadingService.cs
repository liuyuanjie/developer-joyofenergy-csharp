using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Dapper;
using JOIEnergy.Application.Commands;
using JOIEnergy.Application.Exception;
using JOIEnergy.Application.Interfaces;
using JOIEnergy.Application.Model;
using JOIEnergy.Domain.Entity;

namespace JOIEnergy.Application.Services
{
    public class MeterReadingService : IMeterReadingService
    {
        private readonly IConnection _connection;
        private readonly IRepository<SmartMeter> _repository;
        public MeterReadingService(IConnection connection, IRepository<SmartMeter> repository)
        {
            _connection = connection;
            _repository = repository;
        }

        public List<ElectricityReadingModel> GetReadings(string smartMeterId)
        {
            var sql = "SELECT e.[Time], e.[Reading] FROM [ElectricityReadings] AS e " +
                      "INNER JOIN [SmartMeters] AS s ON s.[Id] = e.[SmartMeterId] "+
                      "WHERE s.[SmartMeterId] = @smartMeterId";
            using (var conn = _connection.OpenConnection())
            {
                return conn.Query<ElectricityReadingModel>(sql, new { smartMeterId }).ToList();
            }
        }

        public async Task<int>  StoreReadings(ElectricityReadingCommand command)
        {
            if (!IsMeterReadingsValid(command))
            {
                throw new ApiException("Can not be empty.");
            }

            var smartMeter = _repository.Query().FirstOrDefault(x => x.SmartMeterId == command.SmartMeterId)
                             ?? SmartMeter.Create(command.SmartMeterId);

            command.ElectricityReadings
                .ForEach(electricityReading => smartMeter.CreateElectricityReading(electricityReading.Time, electricityReading.Reading));

            _repository.Update(smartMeter);
            return await _repository.UnitOfWork.CommitAsync();
        }

        private bool IsMeterReadingsValid(ElectricityReadingCommand electricityReadingCommand)
        {
            var smartMeterId = electricityReadingCommand.SmartMeterId;
            List<ElectricityReadingModel> electricityReadings = electricityReadingCommand.ElectricityReadings;
            return smartMeterId != null && smartMeterId.Any()
                                        && electricityReadings != null && electricityReadings.Any();
        }
    }
}
