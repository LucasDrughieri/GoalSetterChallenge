namespace Core.Utils
{
    public static class Constants
    {
        //GSE = GoalSetterError
        //GSS = GoalSetterSuccess
        //GSW = GoalSetterWarning

        public const string GENERAL_ERROR = "GSE_9999";

        public const string START_DATE_INVALID = "GSE_1000";
        public const string END_DATE_INVALID = "GSE_1001";
        public const string START_DATE_GREATER_THAN_END_DATE = "GSE_1002";
        public const string VEHICLE_NOT_AVAILABLE = "GSE_1003";
        public const string VEHICLE_NOT_FOUND = "GSE_1004";
        public const string CLIENT_NOT_FOUND = "GSE_1005";
        public const string CLIENTID_EMPTY = "GSE_1006";
        public const string VEHICLEID_EMPTY = "GSE_1007";
        public const string RENTAL_NOT_FOUND = "GSE_1008";
        public const string NAME_EMPTY = "GSE_1009";
        public const string PHONE_EMPTY = "GSE_1010";
        public const string BRAND_EMPTY = "GSE_1011";
        public const string PRICE_PER_DAY_INVALID = "GSE_1012";

        public const string VEHICLE_SAVED = "GSS_2000";
        public const string VEHICLE_DELETED = "GSS_2001";
        public const string RENTAL_SAVED = "GSS_2002";
        public const string RENTAL_CANCELLED = "GSS_2003";
        public const string CLIENT_SAVED = "GSS_2004";
        public const string CLIENT_DELETED = "GSS_2005";

        public const string NO_VEHICLE_AVAILABLES = "GSW_3000";
    }
}
