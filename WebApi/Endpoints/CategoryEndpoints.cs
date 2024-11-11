using Application.Categories.Activate;
using Application.Categories.Create;
using Application.Categories.Deactivate;
using Application.Categories.Delete;
using Application.Categories.Get;
using Application.Categories.GetById;
using Application.Categories.Update;
using Application.Common;
using Asp.Versioning;
using Asp.Versioning.Builder;
using Domain.Categories;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using SharedKernel;
using System;

namespace WebApi.Endpoints
{
    public static class CategoryEndpoints
    {
        public static void MapCategoryEndpoints(this IEndpointRouteBuilder app)
        {
            app.MapPost("categories", async (CreateCategoryRequest request, ISender sender) =>
            {
                var command = new CreateCategoryCommand(request.Name.ToLower(), request.Description?.ToLower());

                var result = await sender.Send(command);
                return result.IsSuccess ? Results.Created() : Results.BadRequest(result.Error);
            });

            app.MapGet("categories", async (
                string? searchTerm,
                string? sortColumn,
                string? sortOrder,
                int page,
                int pageSize,
                ISender sender) =>
            {
                var query = new GetCategoriesQuery(searchTerm, sortColumn, sortOrder, page, pageSize);
                Result<PagedList<CategoryResponse>> result = await sender.Send(query);

                return Results.Ok(result.Value);
            });

            app.MapGet("categories/{id:guid}", async (Guid id, ISender sender) =>
            {
                var query = new GetCategoryQuery(id);
                Result<CategoryResponse> response = await sender.Send(query);

                return response.IsSuccess ? Results.Ok(response.Value) : Results.NotFound(response.Error);
            });
            
            app.MapPut("categories/{id:guid}", async (Guid id, [FromBody]UpdateCategoryRequest request, ISender sender) =>
            {
                var command = new UpdateCategoryCommand(id, request.Name.ToLower(), request.Description?.ToLower());
                var result = await sender.Send(command); 

                if (result.IsSuccess)
                {
                    return Results.NoContent();
                }

                if (result.Error == CategoryErrors.DuplicateName(request.Name))
                {
                    return Results.BadRequest(result.Error);
                }

                return Results.NotFound(result.Error);
            });
            
            app.MapPut("categories/activate", async ([FromBody]BulkRequest request, ISender sender) =>
            {
                var query = new ActivateCategoriesCommand(request.Ids);
                var result = await sender.Send(query);

                return result.IsSuccess ? Results.NoContent() : Results.NotFound(result.Error);
            });
            
            app.MapPut("categories/deactivate", async ([FromBody]BulkRequest request, ISender sender) =>
            {
                var query = new DeactivateCategoriesCommand(request.Ids);
                var result = await sender.Send(query);

                return result.IsSuccess ? Results.NoContent() : Results.NotFound(result.Error);
            });

            app.MapDelete("categories/delete", async ([FromBody] BulkRequest request, ISender sender) =>
            {
                var query = new DeleteCategoriesCommand(request.Ids);
                var result = await sender.Send(query);

                return result.IsSuccess ? Results.NoContent() : Results.NotFound(result.Error);
            });
        }
    }
}
