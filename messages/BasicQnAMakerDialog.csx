using System;
using Microsoft.Bot.Builder.CognitiveServices.QnAMaker;
using System.Threading.Tasks;
using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Connector;
using System.Linq;
using Microsoft.Bot.Builder.Azure;
using System.Data.SqlClient;


[Serializable]
public class BasicQnAMakerDialog : QnAMakerDialog
{
  public BasicQnAMakerDialog() : base(new QnAMakerService(new QnAMakerAttribute(Utils.GetAppSetting("QnASubscriptionKey"), Utils.GetAppSetting("QnAKnowledgebaseId"))))
   {

   }
   // Override to also include the knowledgebase question with the answer on confident matches
   protected override async Task RespondFromQnAMakerResultAsync(IDialogContext context, IMessageActivity message, QnAMakerResult results)
        {
          
                var response = "Here is the match from FAQ:  \r\n  a: " + results.Answer;
                using (SqlConnection conn = new SqlConnection("Server=tcp:starwarsneu.database.windows.net,1433;Initial Catalog=starwars;Persist Security Info=False;User ID=adminlogin;Password=P@ssw0rd;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;"))
                {
                    conn.Open();
        
                    var text = "INSERT INTO [dbo].[DroneCommand]([DroneID],[DroneCommand])VALUES(1,'"+
                    results.Answer +"')";

                    using (SqlCommand cmd = new SqlCommand(text, conn))
                    {
                    // Execute the command and log the # rows affected.
                    var rows = cmd.ExecuteNonQuery();
                    }
                }
                await context.PostAsync(response);
        }
}
