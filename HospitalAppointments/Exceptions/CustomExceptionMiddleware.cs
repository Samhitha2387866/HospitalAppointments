using Microsoft.AspNetCore.Http;

using Newtonsoft.Json;

using System;

using System.Net;

using System.Threading.Tasks;

namespace NewHospitalManagementSystem.Exceptions

{

    public class CustomExceptionMiddleware

    {

        private readonly RequestDelegate _next;

        public CustomExceptionMiddleware(RequestDelegate next)

        {

            _next = next;

        }

        public async Task Invoke(HttpContext context)

        {

            try

            {

                await _next(context);

            }

            catch (Exception ex)

            {

                await HandleExceptionAsync(context, ex);

            }

        }

        private static Task HandleExceptionAsync(HttpContext context, Exception exception)

        {

            HttpStatusCode statusCode;

            string errorMessage = exception.Message;

            switch (exception)

            {

                case NotFoundException _:

                    statusCode = HttpStatusCode.NotFound; // 404

                    break;

                case BadRequestException _:

                    statusCode = HttpStatusCode.BadRequest; // 400

                    break;

                case DatabaseException _:

                    statusCode = HttpStatusCode.InternalServerError; // 500

                    errorMessage = "A database error occurred.";

                    break;

                default:

                    statusCode = HttpStatusCode.InternalServerError; // 500

                    errorMessage = "An unexpected error occurred.";

                    break;

            }

            var response = new { message = errorMessage };

            context.Response.ContentType = "application/json";

            context.Response.StatusCode = (int)statusCode;

            return context.Response.WriteAsync(JsonConvert.SerializeObject(response));

        }

    }

}
