using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using JOIEnergy.Application;
using Moq;

namespace JOIEnergy.Tests.Application.UsageCostPricePlan
{
    public abstract class UsageCostPricePlanBase
    {
        protected readonly Mock<IMeterReadingService> MeterReadingServiceMock;
        protected readonly Mock<IPricePlanService> PricePlanServiceMock;

        protected UsageCostPricePlanBase()
        {
            MeterReadingServiceMock = new Mock<IMeterReadingService>();
            PricePlanServiceMock = new Mock<IPricePlanService>();
        }
    }
}
