﻿using Domain.DTOs.Requests;
using Domain.DTOs.Responses;
using Domain.Entities;
using Domain.Repositories;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories;

public class TicketRepository(AppDbContext appDbContext)
    : GenericRepository<Ticket>(appDbContext), ITicketRepository
{
    #region Private fields declaration

    private readonly AppDbContext m_AppDbContext = appDbContext;

    #endregion

    #region Public methods declaration

    /// <inheritdoc />
    public List<Ticket> GetTickets(TicketRequest? request)
    {
        IQueryable<Ticket> query = m_AppDbContext.Set<Ticket>()
            .Include(x => x.Category)
            .Include(x => x.Priority)
            .Include(x => x.Product)
            .Include(x => x.User)
            .Include(x => x.AssignedTo);

        if (request is null) return query.ToList();

        if (!string.IsNullOrWhiteSpace(request.Summary))
            query = query.Where(x => EF.Functions.Like(x.Summary, $"%{request.Summary}%"));

        if (request is { ProductId: not null, ProductId.Length: > 0 })
            query = query.Where(x => request.ProductId.Contains(x.ProductId));

        if (request is { CategoryId: not null, CategoryId.Length: > 0 })
            query = query.Where(x => request.CategoryId.Contains(x.CategoryId));

        if (request is { PriorityId: not null, PriorityId.Length: > 0 })
            query = query.Where(x => request.PriorityId.Contains(x.PriorityId));

        if (request is { Status: not null, Status.Length: > 0 })
            query = query.Where(x => request.Status.Contains(x.Status));

        if (request is { RaisedBy: not null, RaisedBy.Length: > 0 })
            query = query.Where(x => request.RaisedBy.Contains(x.RaisedBy));

        return query.OrderByDescending(x => x.RaisedDate).ToList();
    }

    /// <inheritdoc />
    public List<ChartResponse> Last12MonthTickets()
    {
        var startMonth = DateTime.Now.AddMonths(-11);

        var query = appDbContext.Set<Ticket>()
            .Where(x => x.RaisedDate >= startMonth)
            .GroupBy(x => new { x.RaisedDate.Month, x.RaisedDate.Year })
            .Select(g => new
            {
                g.Key.Month,
                g.Key.Year,
                Count = g.Count(),
            })
            .OrderBy(x => x.Year).ThenBy(x => x.Month)
            .ToList();

        return query.Select(x => new ChartResponse
        {
            Label = new DateTime(x.Year, x.Month, 1).ToString("MMM yyyy"),
            Value = x.Count
        }).ToList();
    }

    /// <inheritdoc />
    public List<ChartResponse> ChartByCategory(string category)
    {
        IQueryable<IGrouping<string, Ticket>> data;

        category = category.ToLower();
        switch (category)
        {
            case "category":
                data = appDbContext.Set<Ticket>().Include(x => x.Category).GroupBy(x => x.Category.Name);
                break;
            case "product":
                data = appDbContext.Set<Ticket>().Include(x => x.Product).GroupBy(x => x.Product.Name);
                break;
            case "priority":
                data = appDbContext.Set<Ticket>().Include(x => x.Priority).GroupBy(x => x.Priority.Name);
                break;
            default:
                return null;
        }

        return data.Select(x => new ChartResponse
        {
            Label = x.Key,
            Value = x.Count()
        }).ToList();
    }

    /// <inheritdoc />
    public List<ChartResponse> GetSummary()
    {
        return appDbContext.Set<Ticket>()
            .GroupBy(x => x.Status)
            .Select(g => new ChartResponse
            {
                Label = g.Key,
                Value = g.Count()
            }).ToList();
    }

    #endregion
}