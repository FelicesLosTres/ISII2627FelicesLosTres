namespace AppForSEII.API.DTOs.AlquilerDTOs
{
    public class AlquilerForCreateDTO
    {
        protected bool CompareDate(DateTime date1, DateTime date2)
            {
                return (date1.Subtract(date2) < new TimeSpan(0, 1, 0));
            }
    }
}