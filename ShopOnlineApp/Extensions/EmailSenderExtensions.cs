using System.Text.Encodings.Web;
using System.Threading.Tasks;
using ShopOnlineApp.Services;

namespace ShopOnlineApp.Extensions
{
    public static class EmailSenderExtensions
    {
        public static Task SendEmailConfirmationAsync(this IEmailSender emailSender, string email, string link)
        {
            return emailSender.SendEmailAsync(email, "Xác thực tài khoản",
                $"Xin hãy click vào link để truy cập vào hệ thống: <a href='{HtmlEncoder.Default.Encode(link)}'>link</a>");
        }
    }
}
