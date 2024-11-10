using Application.Categories.Activate;
using Application.Categories.Create;
using Application.Categories.Deactivate;
using Application.Categories.Delete;
using Application.Categories.Get;
using Application.Categories.GetById;
using Application.Categories.Update;
using Application.Common;
using Domain.Categories;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using System;

namespace WebApi.Endpoints
{
    public static class CategoryEndpoints
    {
        public static void MapCategoryEndpoints(this IEndpointRouteBuilder app)
        {
            app.MapPost("api/categories", async (CreateCategoryRequest request, ISender sender) =>
            {
                var command = new CreateCategoryCommand(request.Name.ToLower(), request.Description?.ToLower());

                await sender.Send(command);
                return Results.Ok();
            });

            app.MapGet("api/categories", async (
                string? searchTerm,
                string? sortColumn,
                string? sortOrder,
                int page,
                int pageSize,
                ISender sender) =>
            {
                var query = new GetCategoriesQuery(searchTerm, sortColumn, sortOrder, page, pageSize);
                var categories = await sender.Send(query);
                return Results.Ok(categories);
            });

            app.MapGet("api/categories/{id:guid}", async (Guid id, ISender sender) =>
            {
                try
                {
                    return Results.Ok(await sender.Send(new GetCategoryQuery(id)));
                }
                catch (CategoryNotFoundException e)
                {
                    return Results.NotFound(e.Message);
                }
            });
            
            app.MapPut("api/categories/{id:guid}", async (Guid id, [FromBody]UpdateCategoryRequest request, ISender sender) =>
            {
                try
                {
                    var command = new UpdateCategoryCommand(id, request.Name.ToLower(), request.Description?.ToLower()); 
                    await sender.Send(command);
                    return Results.NoContent();
                }
                catch (CategoryNotFoundException e)
                {
                    return Results.NotFound(e.Message);
                }
            });
            
            app.MapDelete("api/categories/{id:guid}", async (Guid id, ISender sender) =>
            {
                try
                {
                    await sender.Send(new DeleteCategoryCommand(id));
                    return Results.NoContent();
                }
                catch (CategoryNotFoundException e)
                {
                    return Results.NotFound(e.Message);
                }
            });
            
            app.MapPut("api/categories/activate", async ([FromBody]BulkRequest request, ISender sender) =>
            {
                try
                {
                    await sender.Send(new ActivateCategoriesCommand(request.Ids));
                    return Results.NoContent();
                }
                catch (CategoryNotFoundException e)
                {
                    return Results.NotFound(e.Message);
                }
            });
            
            app.MapPut("api/categories/deactivate", async ([FromBody]BulkRequest request, ISender sender) =>
            {
                try
                {
                    await sender.Send(new DeactivateCategoriesCommand(request.Ids));
                    return Results.NoContent();
                }
                catch (CategoryNotFoundException e)
                {
                    return Results.NotFound(e.Message);
                }
            });
            
            app.MapDelete("api/categories/delete", async ([FromBody]BulkRequest request, ISender sender) =>
            {
                try
                {
                    await sender.Send(new DeleteCategoriesCommand(request.Ids));
                    return Results.NoContent();
                }
                catch (CategoryNotFoundException e)
                {
                    return Results.NotFound(e.Message);
                }
            });
        }
    }
}
