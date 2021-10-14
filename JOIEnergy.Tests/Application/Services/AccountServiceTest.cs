using System.Data;
using Dapper;
using JOIEnergy.Application.Model;
using JOIEnergy.Application.Services;
using JOIEnergy.Domain.Enums;
using Moq;
using Moq.Dapper;
using Xunit;

namespace JOIEnergy.Tests.Application.Services
{
    public class AccountServiceTest: ServiceBase
    {
        private const Supplier PRICE_PLAN_ID = Supplier.PowerForEveryone;
        private const string SMART_METER_ID = "smart-meter-id";

        private readonly AccountService _accountService;

        public AccountServiceTest()
        {
            _accountService = new AccountService(ConnectionMock.Object);
        }

        [Fact]
        public void GivenTheSmartMeterIdReturnsThePricePlanId()
        {
            //Arrange
            DbConnectionMock.SetupDapper(x => x.ExecuteScalar<AccountEnergyCompanyPricePlanModel>(It.IsAny<string>(),
                    It.IsAny<object>(),
                    It.IsAny<IDbTransaction>(), It.IsAny<int?>(), It.IsAny<CommandType?>()))
                .Returns(new AccountEnergyCompanyPricePlanModel
                {
                    CompanySupplier = PRICE_PLAN_ID
                });

            //Act
            var result = _accountService.GetAccountEnergyCompanyPricePlanForSmartMeterId(SMART_METER_ID);

            //Assert
            Assert.Equal(Supplier.PowerForEveryone, result.CompanySupplier);
        }

        [Fact]
        public void GivenAnUnknownSmartMeterIdReturnsANullSupplier()
        {
            //Arrange
            DbConnectionMock.SetupDapper(x => x.ExecuteScalar<AccountEnergyCompanyPricePlanModel>(It.IsAny<string>(),
                    It.IsAny<object>(),
                    It.IsAny<IDbTransaction>(), It.IsAny<int?>(), It.IsAny<CommandType?>()))
                .Returns(new AccountEnergyCompanyPricePlanModel());

            //Act
            var result = _accountService.GetAccountEnergyCompanyPricePlanForSmartMeterId("bob-carolgees");

            //Assert
            Assert.Equal(Supplier.NullSupplier, result.CompanySupplier);
        }
    }
}
