using Core.Models;
using System;

namespace Core.Utils
{
    public static class DateUtils
    {
        public static void ValidateRangeDates(Response response, DateTime? startDate, DateTime? endDate)
        {
            var yesterday = DateTime.Now.AddDays(-1);

            if (!startDate.HasValue || startDate.Value <= yesterday) 
                response.AddError(Constants.START_DATE_INVALID, "The field StartDate is empty or before today");

            if (!endDate.HasValue || endDate.Value <= yesterday) 
                response.AddError(Constants.END_DATE_INVALID, "The field EndDate is empty or before today");

            if (startDate.HasValue && endDate.HasValue && startDate.Value > endDate.Value) 
                response.AddError(Constants.START_DATE_GREATER_THAN_END_DATE, "The field EndDate cannot be greater than StartDate");
        }
    }
}
