namespace OnDemandTutor.Models.Dtos.Payment;

public class PaymentInformationModel
{
    public string OrderType { get; set; }
    public double Amount { get; set; }
    public string OrderDescription { get; set; }
    public string Email { get; set; }

}