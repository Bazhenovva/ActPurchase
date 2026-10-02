using FluentAssertions;
using Xunit;
using ActPurchase.Repositories.Contracts;
using ActPurchase.Context.Tests;
using ActPurchase.Domain.Entites;
using Ahatornn.TestGenerator;

namespace ActPurchase.Repositories.Tests
{
    public class ActRepositoryTests : ActPurchaseContextInMemory
    {
        private readonly IActRepository repository;

        /// <summary>
        /// Тесты для <see cref="ActRepository"/>
        /// </summary>
        public ActRepositoryTests()
        {
            repository = new ActRepository(WriterContext, Context);
        }

        /// <summary>
        /// Возвращает пустую коллекцию актов
        /// </summary>
        [Fact]
        public async Task GetAllActsShouldReturnEmptyCollection()
        {
            //Act
            var result = await repository.GetAllActsAsync(CancellationToken.None);

            // Assert
            result.Should().BeEmpty().And.NotBeNull();

        }

        /// <summary>
        /// Возвращает акт по идентификатору если он существут
        /// </summary>
        [Fact]
        public async Task GetActByIdShouldReturnValue()
        {
            // Arrange
            var item = TestEntityProvider.Shared.Create<Act>();
            Context.Add(item);
            await UnitOfWork.SaveChangesAsync(CancellationToken.None);

            // Act
            var result = await repository.GetActByIdAsync(item.Id, CancellationToken.None);

            // Assert
            result.Should().NotBeNull();
            result.Id.Should().Be(item.Id);
        }

        /// <summary>
        ///  Если акта нет, возвращает null
        /// </summary>
        [Fact]
        public async Task GetActByIdShouldReturnNull()
        {
            // Act
            var result = await repository.GetActByIdAsync(Guid.NewGuid(), CancellationToken.None);

            // Assert
            result.Should().BeNull();
        }

        /// <summary>
        /// Если акт удален, возвращает null
        /// </summary>
        [Fact]
        public async Task GetActByIdShouldReturnNullForDeleted()
        {
            // Arrange
            var item = TestEntityProvider.Shared.Create<Act>(x => x.DeletedAt = DateTimeOffset.Now);
            Context.Add(item);
            await UnitOfWork.SaveChangesAsync(CancellationToken.None);

            // Act
            var result = await repository.GetActByIdAsync(item.Id, CancellationToken.None);

            // Assert
            result.Should().BeNull();
        }

        /// <summary>
        /// возвращает акт по номеру
        /// </summary>
        [Fact]
        public async Task GetActByNumberShouldReturnValue()
        {
            // Arrange
            var item = TestEntityProvider.Shared.Create<Act>(x => x.Number = "1");
            Context.Add(item);
            await UnitOfWork.SaveChangesAsync(CancellationToken.None);

            // Act
            var result = await repository.GetActByNumberAsync("1", CancellationToken.None);

            // Assert
            result.Should().NotBeNull();
            result.Id.Should().Be(item.Id);
        }

        /// <summary>
        ///  Если акта нет, возвращает null
        /// </summary>
        [Fact]
        public async Task GetActByNumberShouldReturnNull()
        {
            // Act
            var result = await repository.GetActByNumberAsync("12345", CancellationToken.None);

            // Assert
            result.Should().BeNull();
        }

        /// <summary>
        /// Если акт удален, возвращает null
        /// </summary>
        [Fact]
        public async Task GetActByNumberShouldReturnNullForDeleted()
        {
            // Arrange
            var item = TestEntityProvider.Shared.Create<Act>(x =>
            {
                x.DeletedAt = DateTimeOffset.Now;
                x.Number = "2";
            });
            Context.Add(item);
            await UnitOfWork.SaveChangesAsync(CancellationToken.None);

            // Act
            var result = await repository.GetActByNumberAsync("2", CancellationToken.None);

            // Assert
            result.Should().BeNull();
        }

        /// <summary>
        /// возвращает все неудалённые акты
        /// </summary>
        [Fact]
        public async Task GetAllActsShouldReturnValue()
        {
            // Arrange
            var item1 = TestEntityProvider.Shared.Create<Act>();
            var item2 = TestEntityProvider.Shared.Create<Act>(x => x.DeletedAt = DateTimeOffset.Now);
            var item3 = TestEntityProvider.Shared.Create<Act>();

            Context.AddRange(item1, item2, item3);
            await UnitOfWork.SaveChangesAsync(CancellationToken.None);

            // Act
            var result = await repository.GetAllActsAsync(CancellationToken.None);

            // Assert
            result.Should()
                .NotBeNull()
                .And.HaveCount(2)
                .And.ContainSingle(x => x.Id == item1.Id)
                .And.ContainSingle(x => x.Id == item3.Id);
        }

        /// <summary>
        /// сортирует акты по номеру
        /// </summary>
        [Fact]
        public async Task GetAllActsShouldReturnOrderedByNumber()
        {
            // Arrange
            var item1 = TestEntityProvider.Shared.Create<Act>(x => x.Number = "1");
            var item2 = TestEntityProvider.Shared.Create<Act>(x => x.Number = "2");
            var item3 = TestEntityProvider.Shared.Create<Act>(x => x.Number = "3");

            Context.AddRange(item1, item2, item3);
            await UnitOfWork.SaveChangesAsync(CancellationToken.None);

            // Act
            var result = await repository.GetAllActsAsync(CancellationToken.None);

            // Assert
            result.Should()
                .NotBeNull()
                .And.HaveCount(3)
                .And.BeInAscendingOrder(x => x.Number);
        }
    }
}
