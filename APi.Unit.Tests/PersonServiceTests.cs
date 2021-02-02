using Application.Services;
using AutoFixture;
using Domain.Entities;
using Domain.Repository;
using FluentAssertions;
using Microsoft.Extensions.Logging;
using Moq;
using System;
using System.Threading.Tasks;
using Xunit;

namespace Api.Unit.Tests
{
    public class PersonServiceTests
    {
        private readonly Fixture _fix;
        private readonly Mock<ILogger<PersonService>> _mockLogger;
        private readonly Mock<IPersonRepository> _mockPersonRepository;


        public PersonServiceTests()
        {
            _fix = new Fixture();
            _mockLogger = new Mock<ILogger<PersonService>>();
            _mockPersonRepository = new Mock<IPersonRepository>();
        }


        [Fact]
        public async void GetPerson_When_IdIsValid_Returns_Person()
        {
            Person person = _fix.Create<Person>();

            _mockPersonRepository
                .Setup(m => m.GetByIdAsync(person.CPF))
                .Returns(Task.FromResult(person));

            var service = new PersonService(_mockLogger.Object, _mockPersonRepository.Object);

            var result = await service.GetPerson(person.CPF);

            result
                .Should()
                .BeOfType(typeof(Person));

            result
                .CPF
                .Should()
                .Be(person.CPF);

            result
                .Creation
                .Should()
                .BeSameDateAs(person.Creation);
        }

        [Fact]
        public async void GetPerson_When_IdIsInvalid_Returns_NUll()
        {
            Person Person = null;

            _mockPersonRepository
                .Setup(m => m.GetByIdAsync(1))
                .Returns(Task.FromResult(Person));

            var service = new PersonService(_mockLogger.Object, _mockPersonRepository.Object);

            var result = await service.GetPerson(1);

            result
                .Should()
                .BeNull();
        }

        [Fact]
        public async void GetPerson_Throws_Exception()
        {
            Person Person = _fix.Create<Person>();

            _mockPersonRepository
                .Setup(m => m.GetByIdAsync(1))
                .Throws(new Exception("Error from unit test"));

            var service = new PersonService(_mockLogger.Object, _mockPersonRepository.Object);

            var result = await service.GetPerson(1);

            result
                .Should()
                .BeNull();
        }
    }
}
