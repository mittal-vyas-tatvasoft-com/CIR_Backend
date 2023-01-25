namespace CIR.Common.Enums
{
    public class GlobalConfigurationEnums
    {
        public enum CutOffDays
        {
            SameDay = 0,

            PreviousDay = 1
        }

        public enum DynamicDataFields
        {
            Reference = 0,
            BookingId = 1,
            OrderNumber = 2,
            CustomerEmail = 3,
            CustomerName = 4,
            CollectionDate = 5,
            CollectionAddress = 6,
            TrackingURL = 7,
            LabelURL = 8,
            BookingURL = 9
        }

        public enum StyleOrder
        {
            Ascending = 0,
            Descending = 1
        }

        public enum StyleType
        {
            Textbox = 0,
            Colorpicker = 1
        }

        public enum Type
        {
            Refund = 1,
            Return = 2,
            ProductType = 3
        }
    }
}
