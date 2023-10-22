using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Web.Common.Atributes;
using Web.Common.Data.DataBase.Abstract;
using WebUtilities.Interfaces;
using WebUtilities.Model;

namespace Web.Common.Controllers;

[ApiController]
[GenericRestControllerNameConvention]
[Authorize]
[Route("[controller]/crud")]
public class CommonController<T>: ControllerBase where T: BaseObject
{
    private readonly IQueryService<T> _queryService;
    private readonly ICrudService<T> _crudService;
    private readonly ITransactionContextFactory _transactionContextFactory;
    public CommonController(IQueryService<T> queryService, ITransactionContextFactory transactionContextFactory, ICrudService<T> crudService)
    {
        _queryService = queryService;
        _transactionContextFactory = transactionContextFactory;
        _crudService = crudService;
    }

    [HttpDelete("{id}")]
    public async Task Delete(string id)
    {
        var context = _transactionContextFactory.Create();
        var entity = await _queryService.GetAll(context).Where(x => x.Id == id).FirstOrDefaultAsync();
        if (entity?.Id is not null)
        {
            await _crudService.DeleteAsync(context, entity);
        }

        await context.CommitAsync();
    }

    [HttpPatch]
    public async Task Patch([FromBody] T entity)
    {
        using var context = _transactionContextFactory.Create();
        await _crudService.UpdateAsync(context, entity);
        await context.CommitAsync();
    }

    [HttpPost]
    public async Task<string> Post([FromBody] T entity)
    {
        using var context = _transactionContextFactory.Create();
        var id = await _crudService.CreateAsync(context, entity);
        await context.CommitAsync();
        return id;
    }

    [HttpGet("{id}")]
    public async Task<T?> Get(string id)
    {
        using var qContext = _transactionContextFactory.CreateTransactionLess();
        return await _queryService.GetAll(qContext).FirstOrDefaultAsync(x => x.Id == id);
    }
}