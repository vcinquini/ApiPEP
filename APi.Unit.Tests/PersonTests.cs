using AutoFixture;
using Domain.Entities;
using Domain.Repository;
using FluentAssertions;
using Moq;
using System;
using System.Threading.Tasks;
using Xunit;

namespace APi.Unit.Tests
{
    public class PersonTests
    {
        private readonly Fixture fix;
        private readonly Mock<IPersonRepository> _mockPessoaRepository;

        public PersonTests()
        {
            fix = new Fixture();
            _mockPessoaRepository = new Mock<IPersonRepository>();
        }

        [Fact]
        public void Person_GetPerson_ReturnsPerson_When_IdIsValid()
        {
            long cpf = fix.Create<long>();
            Person pessoa = null;
            Action action = () => { pessoa = new Person(cpf, DateTime.MinValue, DateTime.MinValue, 0, 0); };

            _mockPessoaRepository
                .Setup(m => m.GetByIdAsync(cpf))
                .Returns(Task.FromResult(pessoa));

            action
                .Should()
                .NotThrow<ArgumentException>();

            pessoa
                .Should()
                .NotBeNull();

            pessoa
                .CPF
                .Should()
                .Be(cpf);

            pessoa
                .PEP
                .Should()
                .BeInRange(0,2);

            pessoa
                .PEPType
                .Should()
                .Be(0);
        }

        [Fact]
        public void Person_ThrowsException_When_IdIsZero()
        {
            Action action = () => new Person(0, DateTime.MinValue, DateTime.MinValue, 0, 0);

            action
                .Should()
                .Throw<ArgumentException>();

        }

    }
}
