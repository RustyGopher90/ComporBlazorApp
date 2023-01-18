namespace Compor_App.Data
{
    public class NewComporModel
    {
        public string EmployeeID { get; set; } = "";
        public string ComporID { get; set; } = "";
        public string FirstName { get; set; } = "";
        public string LastName { get; set; } = "";
        public string ComporLevel { get; set; } = "";
        public decimal PercentageOverage { get; set; } = 0;
        public decimal DollarOverage { get; set; } = 0;
        public string? Department { get; set; } = "";
        public string? Enabled { get; set; } = "";

        public decimal TurnStringToDecimal(string value)
        {
            bool didYeParse = Decimal.TryParse(value, out decimal result);
            if (didYeParse)
            {
                return result;
            }
            else
            {
                return 0;
            }
        }

    }
}
