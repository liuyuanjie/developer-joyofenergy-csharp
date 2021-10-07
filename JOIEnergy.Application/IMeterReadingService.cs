using System.Collections.Generic;
using System.Threading.Tasks;
using JOIEnergy.Application.Commands;
using JOIEnergy.Application.Model;

namespace JOIEnergy.Application
{
    public interface IMeterReadingService
    {
        List<ElectricityReadingModel> GetReadings(string smartMeterId);
        Task<int> StoreReadings(ElectricityReadingCommand electricityReadingCommand);
    }
}