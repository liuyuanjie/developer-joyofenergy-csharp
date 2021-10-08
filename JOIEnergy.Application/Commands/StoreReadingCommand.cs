using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using JOIEnergy.Application.Exception;
using JOIEnergy.Application.Model;
using JOIEnergy.Domain.Entity;
using JOIEnergy.Domain.Interfaces;
using MediatR;

namespace JOIEnergy.Application.Commands
{
    public class ElectricityReadingCommand : IRequest<int>
    {
        public string SmartMeterId { get; set; }
        public List<ElectricityReadingModel> ElectricityReadingModels { get; set; }
    }

    public class StoreReadingCommandHandler : IRequestHandler<ElectricityReadingCommand, int>
    {
        private readonly IRepository<SmartMeter> _repository;
        public StoreReadingCommandHandler(IRepository<SmartMeter> repository)
        {
            _repository = repository;
        }

        public async Task<int> Handle(ElectricityReadingCommand request, CancellationToken cancellationToken)
        {
            var smartMeter = _repository.Query().FirstOrDefault(x => x.SmartMeterId == request.SmartMeterId)
                             ?? SmartMeter.Create(request.SmartMeterId);

            request.ElectricityReadingModels
                .ForEach(electricityReading => smartMeter.CreateElectricityReading(electricityReading.Time.ToUniversalTime(), electricityReading.Reading));

            _repository.Update(smartMeter);
            return await _repository.UnitOfWork.CommitAsync();
        }
    }
}
