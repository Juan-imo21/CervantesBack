using SendGrid;
using SendGrid.Helpers.Mail;
using System;
using System.Threading.Tasks;

namespace CervantesBack.Helpers
{
    public class SendGridEmailHelper
    {
        // API Key de SendGrid (usá una variable de entorno en producción)
        private readonly string apiKey = Environment.GetEnvironmentVariable("SENDGRID_API_KEY");


        public async Task EnviarCodigoAsync(string destino, string codigo)
        {
            var client = new SendGridClient(apiKey);

            var from = new EmailAddress("daddysystem00@gmail.com", "Daddy"); // Remitente
            var to = new EmailAddress(destino); // Destinatario
            var subject = "Your DaddySystem recovery code";

            var plainTextContent = $"Your verification code is: {codigo}";
            var htmlContent = $@"
                <div style='font-family: sans-serif; padding: 10px;'>
                    <h2 style='color: #333;'>🔐 Recovery Code</h2>
                    <p>Your verification code is:</p>
                    <p style='font-size: 20px; font-weight: bold; color: #007BFF;'>{codigo}</p>
                    <p>This code will expire in 10 minutes.</p>
                    <hr />
                    <small>If you didn't request this code, you can ignore this email.</small>
                    <br/><br/>
                    <strong>– The DaddySystem Team</strong>
                </div>";

            var msg = MailHelper.CreateSingleEmail(from, to, subject, plainTextContent, htmlContent);

            var response = await client.SendEmailAsync(msg);

            Console.WriteLine($"[SendGrid] Status Code: {response.StatusCode}");

            if (!response.IsSuccessStatusCode)
            {
                var errorBody = await response.Body.ReadAsStringAsync();
                Console.WriteLine($"[SendGrid] Error sending email: {errorBody}");
            }
        }
    }
}