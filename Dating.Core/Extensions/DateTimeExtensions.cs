namespace Dating.Core.Extensions
{
    public static class DateTimeExtensions
    {
        public static int GetAge(this DateOnly dateOfBirth)
        {
            var today = DateOnly.FromDateTime(DateTime.UtcNow);
            var age = today.Year - dateOfBirth.Year;

            return dateOfBirth > today.AddYears(-age) ? age-- : age;
        }
    }
}
