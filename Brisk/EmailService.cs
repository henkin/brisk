namespace Brisk
{
    public class EmailMessage
    {
        
    }
    public class EmailService : Service
    {
         public void SendEmail<TMEssage>(string email, TMEssage message) where TMEssage : EmailMessage
         {
             
         }
    }
}