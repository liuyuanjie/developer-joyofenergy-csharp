using System;
using FluentValidation;
using JOIEnergy.Application.Model;

namespace JOIEnergy.Application.Commands
{
    public class StoreReadingCommandValidator : AbstractValidator<ElectricityReadingCommand>
    {
        public StoreReadingCommandValidator()
        {
            RuleFor(c => c.SmartMeterId).NotEmpty();
            RuleFor(c => c.ElectricityReadingModels).NotEmpty();
            RuleForEach(c => c.ElectricityReadingModels).SetValidator(new ElectricityReadingValidator());
        }
    }

    public class ElectricityReadingValidator : AbstractValidator<ElectricityReadingModel>
    {
        public ElectricityReadingValidator()
        {
            RuleFor(c => c.Reading).GreaterThanOrEqualTo(0);
            RuleFor(c => c.Time).Must(s => s > new DateTime(2000, 1, 1));
        }
    }
}
