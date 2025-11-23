using Microsoft.AspNetCore.Hosting.Server;
using System.Security.Cryptography;
using System.Text;

namespace api.Models.Data
{
    public class DataDecryption
    {
        public string? data { get; set; }

        public DataDecryption(string? value)
        {
            if (string.IsNullOrEmpty(value)) return;
            byte[] iv = new byte[16];
            byte[] buffer = Convert.FromBase64String(value.Trim());
            using (Aes aes = Aes.Create())
            {
                aes.Key = Encoding.UTF8.GetBytes(Models.Config.EncrptionKey);
                aes.IV = iv;
                ICryptoTransform decryptor = aes.CreateDecryptor(aes.Key, aes.IV);

                using (MemoryStream memoryStream = new MemoryStream(buffer))
                {
                    using (CryptoStream cryptoStream = new CryptoStream((Stream)memoryStream, decryptor, CryptoStreamMode.Read))
                    {
                        using (StreamReader streamReader = new StreamReader((Stream)cryptoStream))
                        {
                            data = streamReader.ReadToEnd();
                        }
                    }
                }
            }
        }
    }
}
