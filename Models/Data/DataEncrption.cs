using Microsoft.AspNetCore.Hosting.Server;
using System.Security.Cryptography;
using System.Text;

namespace api.Models.Data
{
    public class DataEncrption
    {
        public string? data { get; set; }
        public DataEncrption(string? value)
        {
            if (string.IsNullOrEmpty(value)) return;
            byte[] iv = new byte[16];
            byte[] array;
            using (Aes aes = Aes.Create())
            {
                aes.Key = Encoding.UTF8.GetBytes(Models.Config.EncrptionKey);
                aes.IV = iv;

                ICryptoTransform encryptor = aes.CreateEncryptor(aes.Key, aes.IV);

                using (MemoryStream memoryStream = new MemoryStream())
                {
                    using (CryptoStream cryptoStream = new CryptoStream((Stream)memoryStream, encryptor, CryptoStreamMode.Write))
                    {
                        using (StreamWriter streamWriter = new StreamWriter((Stream)cryptoStream))
                        {
                            streamWriter.Write(value.Trim());
                        }

                        array = memoryStream.ToArray();
                    }
                }
            }
            data = Convert.ToBase64String(array);
        }
    }
}
