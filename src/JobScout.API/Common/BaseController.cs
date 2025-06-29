using System;
using Microsoft.AspNetCore.Mvc;
using MediatR;
using JobScout.App.Common;
using AutoMapper;
using JobScout.Core.Exceptions;

namespace JobScout.API.Common
{
    [Route("api/[controller]")]
    public class BaseController(IMediator mediator, IMapper mapper) : Controller
    {
        protected readonly IMediator _mediator = mediator;
        protected readonly IMapper _mapper = mapper;
        protected async Task<IActionResult> Handle<TDto, TRequest, TResponse>(Guid? id, TDto dto)
            where TRequest : IRequest<TResponse>
        {
            var queryOrCommand = _mapper.Map<TRequest>(dto);

            if (id.HasValue)
            {
                var idProp = typeof(TRequest).GetProperty("Id");
                if (idProp is not null && idProp.CanWrite)
                {
                    idProp.SetValue(queryOrCommand, id.Value);
                }
            }

            return await Handle(queryOrCommand);
        }

        protected async Task<IActionResult> Handle<TRequest, TResponse>(Guid id)
        where TRequest : IRequest<TResponse>, new()
        {
            var command = new TRequest();

            var idProp = typeof(TRequest).GetProperty("Id");
            if (idProp is not null && idProp.CanWrite)
            {
                idProp.SetValue(command, id);
            }

            return await Handle(command);
        }

        protected async Task<IActionResult> Handle<T>(IRequest<T> queryOrCommand)
        {
            var result = new CommandOrQueryResult<T>();
            try
            {
                if (queryOrCommand is null)
                {
                    return BadRequest();
                }

                if (!ModelState.IsValid)
                {
                    result.Messages = [..ModelState.Values
                        .SelectMany(m => m.Errors)
                        .Select(e => e.ErrorMessage)];

                    return BadRequest(result);
                }

                result.Data = await _mediator.Send(queryOrCommand);
                result.Success = true;
                return Ok(result);
            }
            catch (ConflictException ex)
            {
                result.Messages.Add(ex.Message);
                result.Success = false;
                return Conflict(result); //409
            }
        }
    }
}
