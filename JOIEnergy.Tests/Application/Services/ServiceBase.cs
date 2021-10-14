using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using JOIEnergy.Application;
using JOIEnergy.Application.Interfaces;
using JOIEnergy.Domain.Entity;
using Moq;

namespace JOIEnergy.Tests.Application.Services
{
    public abstract class ServiceBase
    {
        protected readonly Mock<IConnection> ConnectionMock;
        protected readonly Mock<IDbConnection> DbConnectionMock;
        protected readonly Mock<IRepository<SmartMeter>> SmartMeterRepositoryMock;
        protected readonly Mock<IUnitOfWork> UnitOfWorkMock;

        protected ServiceBase()
        {
            ConnectionMock = new Mock<IConnection>();
            DbConnectionMock = new Mock<IDbConnection>();
            ConnectionMock.Setup(x => x.OpenConnection())
                .Returns(DbConnectionMock.Object);

            UnitOfWorkMock = new Mock<IUnitOfWork>();
            SmartMeterRepositoryMock = new Mock<IRepository<SmartMeter>>();
            SmartMeterRepositoryMock.Setup(x => x.UnitOfWork)
                .Returns(UnitOfWorkMock.Object);
        }
    }
}
