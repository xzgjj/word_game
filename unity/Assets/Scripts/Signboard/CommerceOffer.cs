using StarryForest.Core;

namespace StarryForest.Signboard
{
    public sealed class CommerceOffer
    {
        public CommerceOffer(string id, ItemId itemId, int itemCount, int coinCount)
        {
            Id = id;
            ItemId = itemId;
            ItemCount = itemCount;
            CoinCount = coinCount;
        }

        public string Id { get; }
        public ItemId ItemId { get; }
        public int ItemCount { get; }
        public int CoinCount { get; }
    }

    public sealed class CommerceOfferAvailability
    {
        public CommerceOfferAvailability(CommerceOffer offer, bool canUse)
        {
            Offer = offer;
            CanUse = canUse;
        }

        public CommerceOffer Offer { get; }
        public bool CanUse { get; }
    }
}
