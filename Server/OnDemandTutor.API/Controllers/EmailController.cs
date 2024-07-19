using Microsoft.AspNetCore.Mvc;
using OnDemandTutor.BusinessLogic.Interfaces.Mail;
using OnDemandTutor.Models.Dtos.EmailTemplate;

namespace OnDemandTutor.API.Controllers;

[Route("api/[controller]")]
[ApiController]
public class EmailController : ControllerBase
{
    private readonly IEmailServices _emailServices;
    private readonly ILogger<EmailController> _logger;
    
    public EmailController(ILogger<EmailController> logger, IEmailServices emailServices)
    {
        _logger = logger;
        _emailServices = emailServices;
    }
    
    [HttpPost("send")]
    public async Task<IActionResult> SendEmail([FromBody] EmailRequestDto request)
    {
        if (request == null)
            return BadRequest("Request cannot be null.");

        await _emailServices.SendEmailAsync(
            request.ToAddresses,
            request.CcAddresses,
            request.Subject,
            request.Body,
            request.IsHtml, 
            false);

        return Ok("Email sent successfully.");
    }
}