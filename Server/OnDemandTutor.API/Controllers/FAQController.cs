using System;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OnDemandTutor.API.Middlesware;
using OnDemandTutor.BusinessLogic.Interfaces.FAQ;
using OnDemandTutor.Models.Dtos.FAQ;
using OnDemandTutor.Models.Paging;

namespace OnDemandTutor.API.Controllers
{
	public class FAQController : ControllerBase
    {
        private readonly IFAQService _faqService;

        public FAQController(IFAQService faqService)
        {
            _faqService = faqService;
        }

        [HttpGet]
        [ProducesResponseType(typeof(ApiErrorActionResult), 400)]
        [ProducesResponseType(typeof(PagedResult<GetFAQDtos>), 200)]
        public async Task<IActionResult> GetFAQs([FromQuery] PagingModel<GetFAQDtos> pagingModel)
        {
            var faqs = await _faqService.GetFAQsAsync(pagingModel);
            return Ok(faqs);
        }

        [HttpGet("{id}")]
        [ProducesResponseType(typeof(ApiErrorActionResult), 400)]
        [ProducesResponseType(typeof(GetFAQDtos), 200)]
        public async Task<IActionResult> GetFAQById(int id)
        {
            var faq = await _faqService.GetFAQByIdAsync(id);
            if (faq == null)
            {
                return NotFound();
            }
            return Ok(faq);
        }

        [Authorize]
        [HttpPost]
        [ProducesResponseType(typeof(ApiErrorActionResult), 400)]
        [ProducesResponseType(typeof(CreateFAQDtos), 200)]
        public async Task<IActionResult> CreateFAQ([FromBody] CreateFAQDtos faqDto)
        {
            var createdFAQ = await _faqService.CreateFAQAsync(faqDto);
            return CreatedAtAction(nameof(GetFAQById), new { id = createdFAQ.Id }, createdFAQ);
        }

        [Authorize]
        [HttpPut("{id}")]
        [ProducesResponseType(typeof(ApiErrorActionResult), 400)]
        [ProducesResponseType(typeof(UpdateFAQDtos), 200)]
        public async Task<IActionResult> UpdateFAQ(int id, [FromBody] UpdateFAQDtos faqDto)
        {
            if (id != faqDto.Id)
            {
                return BadRequest("ID mismatch between route parameter and request body.");
            }
            var updatedFAQ = await _faqService.UpdateFAQAsync(faqDto);
            if (updatedFAQ == null)
            {
                return NotFound();
            }
            return NoContent();
        }

        [Authorize]
        [HttpDelete("{id}")]
        [ProducesResponseType(typeof(ApiErrorActionResult), 400)]
        [ProducesResponseType(204)]
        public async Task<IActionResult> DeleteFAQ(int id)
        {
            var isDeleted = await _faqService.DeleteFAQAsync(id);
            if (!isDeleted)
            {
                return NotFound();
            }
            return NoContent();
        }
    }
}

