using Application.Aplicacao.Queries;
using Domain.Entities;
using Domain.Services;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class PEPController : ControllerBase
    {
        private readonly ILogger<PEPController> _logger;
        private readonly IPersonService _pessoaService;
        private readonly IMediator _mediator;

        public PEPController(ILogger<PEPController> logger, IPersonService pessoaService, IMediator mediator)
        {
            _logger = logger;
            _pessoaService = pessoaService;
            _mediator = mediator;
        }



        [HttpGet]
        [Route("/Echo")]
        public IActionResult Echo()
        {
            return Content("Echo back...");
        }



        [HttpGet("{cpf}")]
        public async Task<IActionResult> Get(long cpf)
        {
            Person pessoa;

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                PersonQuery query = new PersonQuery(cpf);
                pessoa = await _mediator.Send(query);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao executar a busca de pessoa");
                return StatusCode(500, ex.Message);
            }

            return Ok(pessoa);
        }
    }

}

