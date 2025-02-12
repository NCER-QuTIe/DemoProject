using Contracts;
using Contracts.Logger;
using DataTransferObjects.TestResults;
using MimeKit;

namespace Service;

public class MyMailService(ILoggerManager logger, IExcelBuilder builder)
{
    private ContentToExcelService _contentToExcel = new ContentToExcelService(logger, builder);

    public async Task<bool> SendTestResponseMail(EmailContentDTO emailContent)
    {
        string excelFileAddress = "";
        try
        {
            logger.LogInfo($"Started generating excel for address: {emailContent.EmailToSend}, by: {emailContent.ResponseBundle!.StudentName}");
            excelFileAddress = await _contentToExcel.GenerateExcelFileAsync(emailContent.ResponseBundle!);
        }
        catch (Exception e)
        {
            logger.LogError($"Error while generating excel for address: {emailContent.EmailToSend}, by: {emailContent.ResponseBundle!.StudentName}. {e.Message}");
            if (File.Exists(excelFileAddress))
            {
                File.Delete(excelFileAddress);
            }
            return false;
        }

        logger.LogInfo($"Generated excel for address: {emailContent.EmailToSend}, by: {emailContent.ResponseBundle!.StudentName}, at: {excelFileAddress}");

        return await SendEmailWithAttachmentAsync(excelFileAddress, emailContent.EmailToSend!, emailContent);
    }

    private async Task<bool> SendEmailWithAttachmentAsync(string attachmentFilePath, string toAddress, EmailContentDTO content)
    {
        string fromAddress = Environment.GetEnvironmentVariable("EMAIL_TO_SEND_FROM")!;
        string password = Environment.GetEnvironmentVariable("EMAIL_PASSWORD")!;

        var message = new MimeMessage();
        message.From.Add(new MailboxAddress("NCER", fromAddress));
        message.To.Add(MailboxAddress.Parse(toAddress));
        message.Subject = $"ტესტის შედეგები - {content.ResponseBundle!.StudentName}";

        var bodyBuilder = new BodyBuilder();
        bodyBuilder.Attachments.Add(attachmentFilePath);
        message.Body = bodyBuilder.ToMessageBody();

        using var client = new MailKit.Net.Smtp.SmtpClient();
        client.Connect("smtp.gmail.com", 587, MailKit.Security.SecureSocketOptions.StartTls);

        client.Authenticate(fromAddress, password);

        try
        {
            logger.LogInfo($"Started sending mail to {toAddress}");
            //await client.SendAsync(message);
            await client.DisconnectAsync(true);
        }
        catch (Exception e)
        {
            logger.LogError($"Failed to send email to {toAddress}" + e.Message);
            return false;
        }
        finally
        {
            message.Dispose();
            if (File.Exists(attachmentFilePath))
            {
                //File.Delete(attachmentFilePath);
            }
        }

        return true;
    }
}
