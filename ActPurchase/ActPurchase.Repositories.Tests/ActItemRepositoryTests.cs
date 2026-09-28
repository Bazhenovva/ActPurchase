using ActPurchase.Context.Tests;
using ActPurchase.Domain.Entities;
using ActPurchase.Repositories.Contracts;
using Ahatornn.TestGenerator;
using FluentAssertions;
using Xunit;

namespace ActPurchase.Repositories.Tests
{
    public class ActItemRepositoryTests : ActPurchaseContextInMemory
    {
        private readonly IActItemRepository repository;

        /// <summary>
        /// Тесты для <see cref="ActItemRepository"/>
        /// </summary>
        public ActItemRepositoryTests()
        {
            repository = new ActItemRepository(WriterContext, Context);
        }

        /// <summary>
        ///  Возвращает пустую коллекцию, если у акта нет позиций
        /// </summary>
        [Fact]
        public async Task GetAllActItemsShouldReturnNull()
        {
            // Act
            var result = await repository.GetAllActItemsAsync(Guid.NewGuid(), CancellationToken.None);

            // Assert
            result.Should().NotBeNull().And.BeEmpty();
        }

        /// <summary>
        /// Возвращает коллекцию позиций для указанного акта
        /// </summary>
        [Fact]
        public async Task GetAllActItemsShouldReturnValue()
        {
            // Arrange
            var actId = Guid.NewGuid();

            var item1 = TestEntityProvider.Shared.Create<ActItem>(x => x.ActId = actId);
            var item2 = TestEntityProvider.Shared.Create<ActItem>(x => x.ActId = actId);

            Context.AddRange(item1, item2);
            await UnitOfWork.SaveChangesAsync(CancellationToken.None);

            // Act
            var result = await repository.GetAllActItemsAsync(actId, CancellationToken.None);

            // Assert
            result.Should()
                .NotBeNull()
                .And.HaveCount(2)
                .And.ContainSingle(x => x.Id == item1.Id)
                .And.ContainSingle(x => x.Id == item2.Id);
        }

        /// <summary>
        /// Не возвращает удалённые позиции
        /// </summary>
        [Fact]
        public async Task GetAllActItemsShouldNotReturnDeleted()
        {
            // Arrange
            var actId = Guid.NewGuid();

            var item1 = TestEntityProvider.Shared.Create<ActItem>(x => x.ActId = actId);
            var item2 = TestEntityProvider.Shared.Create<ActItem>(x =>
            {
                x.ActId = actId;
                x.DeletedAt = DateTimeOffset.Now;
            });

            Context.AddRange(item1, item2);
            await UnitOfWork.SaveChangesAsync(CancellationToken.None);

            // Act
            var items = await repository.GetAllActItemsAsync(actId, CancellationToken.None);

            // Assert
            items.Should()
                .NotBeNull()
                .And.HaveCount(1)
                .And.ContainSingle(x => x.Id == item1.Id);
        }
        /// <summary>
        /// Возвращает позицию по идентификатору
        /// </summary>
        [Fact]
        public async Task GetActItemByIdShouldReturnValue()
        {
            // Arrange
            var item = TestEntityProvider.Shared.Create<ActItem>();
            Context.Add(item);
            await UnitOfWork.SaveChangesAsync(CancellationToken.None);

            // Act
            var result = await repository.GetActItemByIdAsync(item.Id, CancellationToken.None);

            // Assert
            result.Should().NotBeNull().And.BeEquivalentTo(item);
        }
        /// <summary>
        /// Возвращает null, если позиция не найдена
        /// </summary>
        [Fact]
        public async Task GetActItemByIdShouldReturnNull()
        {
            // Act
            var result = await repository.GetActItemByIdAsync(Guid.NewGuid(), CancellationToken.None);

            // Assert
            result.Should().BeNull();
        }
        /// <summary>
        /// Возвращает null, если позиция удалена
        /// </summary>
        [Fact]
        public async Task GetActItemByIdShouldReturnNullForDeleted()
        {
            // Arrange
            var item = TestEntityProvider.Shared.Create<ActItem>(x => x.DeletedAt = DateTimeOffset.Now);
            Context.Add(item);
            await UnitOfWork.SaveChangesAsync(CancellationToken.None);

            // Act
            var result = await repository.GetActItemByIdAsync(item.Id, CancellationToken.None);

            // Assert
            result.Should().BeNull();
        }
    }
}
