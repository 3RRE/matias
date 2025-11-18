using S3k.Utilitario.Models;
using System.Configuration;

namespace S3k.Utilitario
{
    public class UrlUtil
    {
        public static string GetNewUrlWithToken(UserClaim userClaim, string redirectUrl = null)
        {
            string frontend = ConfigurationManager.AppSettings["NewIAS_Frontend"].ToString();

            string token = JwtUtil.GenerateToken(userClaim);

            string accessUrl = string.Format("{0}/login?access_token={1}", frontend, token);

            if (!string.IsNullOrEmpty(redirectUrl))
            {
                accessUrl = string.Format("{0}&redirect_url={1}", accessUrl, redirectUrl);
            }

            return accessUrl;
        }
    }
}
