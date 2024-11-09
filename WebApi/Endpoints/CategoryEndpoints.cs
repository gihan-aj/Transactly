using Application.Categories.Create;
using Application.Categories.Get;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace WebApi.Endpoints
{
    public static class CategoryEndpoints
    {
        public static void MapCategoryEndpoints(this IEndpointRouteBuilder app)
        {
            app.MapPost("api/categories", async (CreateCategoryRequest request, ISender sender) =>
            {
                var command = new CreateCategoryCommand(request.Name.ToLower(), request.Description);

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
        }
    }
}
