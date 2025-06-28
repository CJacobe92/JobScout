using System;
using Microsoft.AspNetCore.Mvc;
using MediatR;
using JobScout.AppService.Common;

using AutoMapper;

namespace JobScout.API.Common
{
    [Route("api/[controller]")]
    public class BaseController(IMediator mediator, IMapper mapper) : Controller
    {
        protected readonly IMediator _mediator = mediator;
        protected readonly IMapper _mapper = mapper;

        //Gets the incoming DTO via T1 then maps it to a commands via Auto mapper and returns the result via T3
        protected async Task<IActionResult> Handle<TDto, TCommand, TResponse>(TDto dto)
            where TCommand : IRequest<TResponse>
        {
            var queryOrCommand = _mapper.Map<TCommand>(dto);
            return await Handle(queryOrCommand);
        }

        protected async Task<IActionResult> Handle<T>(IRequest<T> queryOrCommand)
        {

            if (queryOrCommand is null)
            {

                return BadRequest();
            }

            var result = new CommandOrQueryResult<T>();

            if (ModelState.IsValid)
            {
                try
                {
                    result.Data = await _mediator.Send(queryOrCommand);
                    result.Success = true;
                }
                catch (Exception ex)
                {
                    result.Messages.Add(ex.Message);
                    throw;
                }
            }
            else
            {
                result.Messages = [.. ModelState.Values.SelectMany(m => m.Errors).Select(e => e.ErrorMessage)];
            }

            if (result.Success)
            {
                return Ok(result);
            }
            else
            {
                return BadRequest(result);
            }
        }


    }
}
