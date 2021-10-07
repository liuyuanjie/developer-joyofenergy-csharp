using System;
using System.Collections.Generic;
using System.Data;
using Dapper;
using JOIEnergy.Application;
using JOIEnergy.Application.Model;
using JOIEnergy.Application.Services;
using JOIEnergy.Domain.Enums;
using Moq;
using Moq.Dapper;
using Xunit;

namespace JOIEnergy.Tests
{
    public class AccountServiceTest
    {
        private const Supplier PRICE_PLAN_ID = Supplier.PowerForEveryone;
        private const string SMART_METER_ID = "smart-meter-id";

        private readonly AccountService _accountService;
        private readonly Mock<IConnection> _mockConnection;
        private readonly Mock<IDbConnection> _mockDbConnection;

        public AccountServiceTest()
        {
            _mockConnection = new Mock<IConnection>();
            _mockDbConnection = new Mock<IDbConnection>();
            _mockConnection.Setup(x => x.OpenConnection())
                .Returns(_mockDbConnection.Object);

            _accountService = new AccountService(_mockConnection.Object);
        }

        [Fact]
        public void GivenTheSmartMeterIdReturnsThePricePlanId()
        {
            //Arrange
            _mockDbConnection.SetupDapper(x => x.ExecuteScalar<AccountEnergyCompanyPricePlanModel>(It.IsAny<string>(),
                    It.IsAny<object>(),
                    It.IsAny<IDbTransaction>(), It.IsAny<int?>(), It.IsAny<CommandType?>()))
                .Returns(new AccountEnergyCompanyPricePlanModel
                {
                    CompanySupplier = PRICE_PLAN_ID
                });

            //Act
            var result = _accountService.GetAccountEnergyCompanyPricePlanForSmartMeterId("smart-meter-id");

            //Assert
            Assert.Equal(Supplier.PowerForEveryone, result.CompanySupplier);
        }

        [Fact]
        public void GivenAnUnknownSmartMeterIdReturnsANullSupplier()
        {
            //Arrange
            _mockDbConnection.SetupDapper(x => x.ExecuteScalar<AccountEnergyCompanyPricePlanModel>(It.IsAny<string>(),
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
