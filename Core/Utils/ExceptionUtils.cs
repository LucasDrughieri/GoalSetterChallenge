using Core.Models;
using Microsoft.Extensions.Logging;
using System;

namespace Core.Utils
{
    public static class ExceptionUtils
    {
        public static void HandleGeneralError(Response response, ILogger logger, Exception e)
        {
            const string msg = "An general error has ocurred";
            response.AddError(msg);
            logger.LogError($"{msg}. Exception: {e.StackTrace}");
        }
    }
}
