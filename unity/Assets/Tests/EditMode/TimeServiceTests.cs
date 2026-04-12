using NUnit.Framework;
using StarryForest.Core;
using StarryForest.Inventory;
using StarryForest.World;

namespace StarryForest.Tests.EditMode
{
    public sealed class TimeServiceTests
    {
        [Test]
        public void SetTimeUpdatesPlayerStateWithoutChangingInventory()
        {
            PlayerState state = NewState();
            state.Items[ItemId.Wood] = 2;
            TimeService timeService = new TimeService();

            OperationResult result = timeService.SetTime(state, TimeOfDay.Night);

            Assert.IsTrue(result.Success, result.Message);
            Assert.AreEqual(TimeOfDay.Night, state.TimeOfDay);
            Assert.AreEqual(2, state.Items[ItemId.Wood]);
        }

        [Test]
        public void CycleNextMovesThroughMorningNoonNight()
        {
            PlayerState state = NewState();
            TimeService timeService = new TimeService();

            timeService.CycleNext(state);
            Assert.AreEqual(TimeOfDay.Noon, state.TimeOfDay);

            timeService.CycleNext(state);
            Assert.AreEqual(TimeOfDay.Night, state.TimeOfDay);

            timeService.CycleNext(state);
            Assert.AreEqual(TimeOfDay.Morning, state.TimeOfDay);
        }

        private static PlayerState NewState()
        {
            return new PlayerState(ItemCatalog.InventorySlotCount);
        }
    }
}
