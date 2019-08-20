using System.Collections.Generic;
using System.Net;
using Microsoft.AspNetCore.Mvc;
using AccountAPI.Models;
using Microsoft.EntityFrameworkCore;
using AccountAPI.Repositories;

namespace AccountAPI.Services
{
    public interface IResponse
    {
        string Message {get;set;}
        bool DidError {get;set;}
    }

    public interface ISingleResponse<TModel> : IResponse
    {
        TModel Model {get;set;}
    }

    public interface IListResponse<TModel> : IResponse
    {
        IEnumerable<TModel> Model {get;set;}
    }

    public class Response : IResponse
    {
        public string Message {get;set;}
        public bool DidError {get;set;}
    }

    public class SingleResponse<TModel> : ISingleResponse<TModel>
    {
        public string Message {get;set;}
        public bool DidError {get;set;}
        public TModel Model {get;set;}
    }

    public class ListResponse<TModel> : IListResponse<TModel>
    {
        public string Message {get;set;}
        public bool DidError {get;set;}
        public IEnumerable<TModel> Model {get;set;}
    }

    public static class ResponseExtensions
    {
        public static IActionResult ToHttpResponse(this IResponse response)
        {
            var status = HttpStatusCode.OK;

            return new ObjectResult(response)
            {
                StatusCode = (int)status
            };
        }

        public static IActionResult ToHttpResponse<TModel>(this ISingleResponse<TModel> response)
        {
            var status = HttpStatusCode.OK;

            return new ObjectResult(response)
            {
                StatusCode = (int)status
            };
        }

        public static IActionResult ToHttpResponse<TModel>(this IListResponse<TModel> response)
        {
            var status = HttpStatusCode.OK;

            return new ObjectResult(response)
            {
                StatusCode = (int)status
            };
        }
    }
}