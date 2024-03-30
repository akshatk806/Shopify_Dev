using System.ComponentModel.DataAnnotations;

namespace Product_Management.Validation
{
    public class PhoneNumberAttribute : ValidationAttribute
    {
        public override bool IsValid(object value)
        {
            if (value == null)
                return false;

            string phone = value.ToString();

            long phoneNumber = long.Parse(phone);

            if(phoneNumber > 6000000000) 
                return true;

            return false;
        }
    }
}
