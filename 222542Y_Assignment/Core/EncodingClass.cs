using System;
using System.Text;

namespace _222542Y_Assignment.Core
{
    public class EncodingClass
    {
        public static string Base64Encode(string plainText)
        {
            byte[] plainTextBytes = Encoding.UTF8.GetBytes(plainText);
            return Convert.ToBase64String(plainTextBytes);
        }

        public static string Base64Decode(string encodedText)
        {
            byte[] encodedBytes = Convert.FromBase64String(encodedText);
            return Encoding.UTF8.GetString(encodedBytes);
        }
    }
}
