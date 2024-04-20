using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace FitnessApp.API.Filters;

public class ResultFilter<T> : IAsyncResultFilter
{
    private readonly IMapper _mapper;

    public ResultFilter(IMapper mapper)
    {
        _mapper = mapper;
    }

    public async Task OnResultExecutionAsync(ResultExecutingContext context, ResultExecutionDelegate next)
    {
        var resultFromAction = context.Result as ObjectResult;
        if (resultFromAction?.Value is null ||
            resultFromAction.StatusCode < 200 ||
            resultFromAction.StatusCode >= 300)
        {
            await next();
            return;
        }

        resultFromAction.Value = Map(resultFromAction.Value);

        await next();
    }

    private T Map(object? value)
    {
        return _mapper.Map<T>(value);
    }
}
