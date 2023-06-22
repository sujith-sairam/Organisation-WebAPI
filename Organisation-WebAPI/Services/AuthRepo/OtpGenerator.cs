using System.Security.Cryptography;

namespace Organisation_WebAPI.Services.AuthRepo
{
    public class OtpGenerator
    {
        private const int OtpLength = 6;

        public string GenerateOtp()
        {
            using (var rng = new RNGCryptoServiceProvider())
            {
                var buffer = new byte[OtpLength];
                rng.GetBytes(buffer);

                var otp = BitConverter.ToInt32(buffer, 0) % (int)Math.Pow(10, OtpLength);
                return otp.ToString("D" + OtpLength);
            }
        }
    }
}
