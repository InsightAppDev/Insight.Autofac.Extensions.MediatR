using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Insight.Autofac.Extensions.MediatR.Samples.Application.Queries;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace Insight.Autofac.Extensions.MediatR.Samples.WebApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class CurrentUtcDateController : ControllerBase
    {
        private readonly IMediator _mediator;

        public CurrentUtcDateController(IMediator mediator)
        {
            if (mediator == null)
                throw new ArgumentNullException(nameof(mediator));
           
            _mediator = mediator;
        }

        [HttpGet]
        public async Task<IActionResult> Get(string format, CancellationToken cancellationToken)
        {
            var result = await _mediator.Send(new GetCurrentUtcDateTimeStringQuery(format), cancellationToken);
            if (string.IsNullOrWhiteSpace(result))
                return BadRequest();
            
            return Ok(result);
        }
    }
}