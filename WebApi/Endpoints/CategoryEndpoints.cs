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
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using SharedKernel;
using System;
using System.Reflection;
using System.Threading.Tasks;
using WebApi.Extensions;

namespace WebApi.Endpoints
{
    public static class CategoryEndpoints
    {
        public static void MapCategoryEndpoints(this IEndpointRouteBuilder app)
        {
            app.MapPost("categories", Create);

            static async Task<IResult> Create(CreateCategoryRequest request, ISender sender)
            {
                var command = new CreateCategoryCommand(request.Name.ToLower(), request.Description?.ToLower());

                var result = await sender.Send(command);

                if (result.IsFailure)
                {
                    return HandleFailure(result);
                }

                return Results.Created($"categories/{result.Value}", result.Value);
            }


            app.MapGet("categories", Get);

            static async Task<IResult> Get(
                string? searchTerm,
                string? sortColumn,
                string? sortOrder,
                int page,
                int pageSize,
                ISender sender)
            {
                var query = new GetCategoriesQuery(searchTerm, sortColumn, sortOrder, page, pageSize);

                Result<PagedList<CategoryResponse>> result = await sender.Send(query);

                return Results.Ok(result.Value);
            }


            app.MapGet("categories/{id:guid}", GetById);

            static async Task<IResult> GetById(Guid id, ISender sender) {
                var query = new GetCategoryQuery(id);
                Result<CategoryResponse> response = await sender.Send(query);

                if (response.IsFailure)
                {
                    return HandleFailure(response);
                }

                return Results.Ok(response.Value);
            }

            
            app.MapPut("categories/{id:guid}", Update);

            static async Task<IResult> Update(Guid id, [FromBody]UpdateCategoryRequest request, ISender sender)
            {
                var command = new UpdateCategoryCommand(id, request.Name.ToLower(), request.Description?.ToLower());
                var result = await sender.Send(command);

                if(result.IsFailure)
                {
                    return HandleFailure(result);
                }

                return Results.NoContent();
            }

            
            app.MapPut("categories/activate", Activate);

            static async Task<IResult> Activate([FromBody] BulkRequest request, ISender sender)
            {
                var query = new ActivateCategoriesCommand(request.Ids);
                var result = await sender.Send(query);

                if(result.IsFailure)
                {
                    return HandleFailure(result);
                }

                return Results.NoContent();
            }

            
            app.MapPut("categories/deactivate", Deactivate);

            static async Task<IResult> Deactivate([FromBody] BulkRequest request, ISender sender)
            {
                var query = new DeactivateCategoriesCommand(request.Ids);
                var result = await sender.Send(query);

                if (result.IsFailure)
                {
                    return HandleFailure(result);
                }

                return Results.NoContent();
            }


            app.MapDelete("categories/delete", Delete);

            static async Task<IResult> Delete([FromBody] BulkRequest request, ISender sender)
            {
                var query = new DeleteCategoriesCommand(request.Ids);
                var result = await sender.Send(query);

                if (result.IsFailure)
                {
                    return HandleFailure(result);
                }

                return Results.NoContent();
            }
        }

        private static IResult HandleFailure(Result result) =>
            result switch
            {
                { IsSuccess: true } => throw new InvalidOperationException(),

                { Error: { Code: "Category.NotFound"} } =>
                Results.NotFound(ResultExtensions.CreateProblemDetails("Not found", StatusCodes.Status404NotFound, result.Error)),
                
                { Error: { Code: "Category.DuplicateName"} } =>
                Results.Problem(ResultExtensions.CreateProblemDetails("Not acceptable", StatusCodes.Status406NotAcceptable, result.Error)),
                
                IValidationResult validationResult =>
                Results.BadRequest(ResultExtensions.CreateProblemDetails("Validation error", StatusCodes.Status400BadRequest, result.Error, validationResult.Errors)),

                _ => Results.Problem(ResultExtensions.CreateProblemDetails("Internal server error", StatusCodes.Status500InternalServerError, result.Error))
            };
        

        
    }
}
