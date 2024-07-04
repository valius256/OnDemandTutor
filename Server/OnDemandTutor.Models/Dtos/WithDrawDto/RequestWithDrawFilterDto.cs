namespace OnDemandTutor.Models.Dtos.WithDrawDto;

public class RequestWithDrawFilterDto
{
    public DateTime? FromDate { get; set; }
    public DateTime? ToDate { get; set; }
    public decimal? MinAmount { get; set; }
    public decimal? MaxAmount { get; set; }
    public int Limit { get; set; }
    public int Page { get; set; }
}