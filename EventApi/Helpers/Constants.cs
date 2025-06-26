namespace EventApi.Helpers
{
    public static class Constants
    {
        public const string PurchaseStatusPending = "Pending";
        public const string TicketStatusPending = "Pending";
        public const string TicketStatusBooked = "Booked";
        public const string TicketStatusSold = "Sold";
        public const string TicketStatusAvailable = "Available";
        public const string PaymentStatusCompleted = "Completed";
        public const string PaymentStatusPending = "Pending";
        public const string PaymentStatusFailed = "Failed";
        public const string SeatTypeSold = "Sold";
        public const string SeatTypeAvailable = "Available ";

        //UserRoles
        public const string CustomerUserRole = "Customer";
        public const string ManagerUserRole = "Manager";

        //EventStatus
        public const string EventStatusActive = "Active";
        public const string EventStatusInactive = "Inactive";
        public const string EventStatusPreSale = "PreSale";

        //Cache keys
        public const string AllEventsCacheKey = "AllEvents";
        public const string MinimizedEventsCacheKey = "MinimizedEvents";
        public const string SeatsWithStatusAndPriceCacheKey = "SeatsWithStatusAndPrice";

        //Operation Statuses
        public const string TicketBooked = "Sucess booking";


    }
}
