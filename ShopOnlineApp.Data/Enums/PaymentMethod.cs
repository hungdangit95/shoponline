using System.ComponentModel;

namespace ShopOnlineApp.Data.Enums
{
    public enum PaymentMethod
    {
        [Description("Cash on delivery")]
        CashOnDelivery,
        [Description("Online Banking")]
        OnlinBanking,
        [Description("Payment Gateway")]
        PaymentGateway,
        [Description("Visa")]
        Visa,
        [Description("Master Card")]
        MasterCard,
        [Description("PayPal")]
        PayPal,
        [Description("Atm")]
        Atm
    }
}
