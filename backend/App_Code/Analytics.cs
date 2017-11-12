using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;
using System.Data.SqlClient;
using System.Configuration;
using Newtonsoft.Json;

/// <summary>
/// Analytics
/// </summary>
[WebService(Namespace = "http://igprog.hr/")]
[WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
[System.Web.Script.Services.ScriptService]
public class Analytics : System.Web.Services.WebService {
    SqlConnection connection = new SqlConnection(ConfigurationManager.ConnectionStrings["connectionString"].ConnectionString);
    int timeDiff = Convert.ToInt32(ConfigurationManager.AppSettings["ServerHostTimeDifference"]);

    public Analytics() {
    }

    #region Services
    private class Service {
        public String service { get; set; }
        public String option { get; set; }
        public Int32 count { get; set; }
        public Decimal price { get; set; }
    }

    [WebMethod]
    public string InitServices() {
        Service x = new Service();
        x.service = null;
        x.option = null;
        x.count = 0;
        x.price = 0;
        string json = JsonConvert.SerializeObject(x, Formatting.Indented);
        return json;
    }

    [WebMethod]
    public string GetClientServicesCount() {
        try {
            connection.Open();
            SqlCommand command = new SqlCommand(@"SELECT [Service],[Option],COUNT([Service]),SUM(CONVERT(DECIMAL,[Price]))
                                                FROM ClientServices
                                                WHERE [IsPaid] = 1
                                                GROUP BY[Service],[Option]", connection);
            SqlDataReader reader = command.ExecuteReader();
            List<Service> services = new List<Service>();
            while (reader.Read()) {
                Service x = new Service();
                x.service = reader.GetString(0);
                x.option = reader.GetString(1);
                x.count = reader.GetInt32(2);
                x.price = reader.GetDecimal(3);
                services.Add(x);
            }
            connection.Close();
            string json = JsonConvert.SerializeObject(services, Formatting.Indented);
            return json;
        }
        catch (Exception e) { return ("Error: " + e); }
    }

    [WebMethod]
    public string GetActiveClientServicesCountByDate(DateTime from, DateTime to, int isPaid) {
        try {
            from = from.AddHours(-timeDiff).AddMinutes(0).AddSeconds(0);
            to = to.AddHours(-timeDiff).AddMinutes(0).AddSeconds(0);

            connection.Open();
            SqlCommand command = new SqlCommand(@"SELECT [Service],[Option],COUNT([Service]),SUM(CONVERT(DECIMAL,[Price]))
                                                FROM ClientServices
                                                WHERE ([IsPaid] = @IsPaid AND [ActivationDate] BETWEEN @From AND @To) OR ([IsPaid] = @IsPaid AND [ExpirationDate] BETWEEN @From AND @To)
                                                GROUP BY[Service],[Option]", connection);
            command.Parameters.Add(new SqlParameter("From", from));
            command.Parameters.Add(new SqlParameter("To", to));
            command.Parameters.Add(new SqlParameter("IsPaid", isPaid));
            SqlDataReader reader = command.ExecuteReader();
            List<Service> services = new List<Service>();
            while (reader.Read()) {
                Service x = new Service();
                x.service = reader.GetString(0);
                x.option = reader.GetString(1);
                x.count = reader.GetInt32(2);
                x.price = reader.GetDecimal(3);
                services.Add(x);
            }
            connection.Close();
            string json = JsonConvert.SerializeObject(services, Formatting.Indented);
            return json;
        } catch (Exception e) { return ("Error: " + e); }
    }

    [WebMethod]
    public string GetPayedClientServicesCountByDate(DateTime from, DateTime to, int isPaid) {
        try {
            from = from.AddHours(-timeDiff).AddMinutes(0).AddSeconds(0);
            to = to.AddHours(-timeDiff).AddMinutes(0).AddSeconds(0);

            connection.Open();
            SqlCommand command = new SqlCommand(@"SELECT [Service],[Option],COUNT([Service]),SUM(CONVERT(DECIMAL,[Price]))
                                                FROM ClientServices
                                                WHERE ([IsPaid] = @IsPaid AND [ActivationDate] BETWEEN @From AND @To)
                                                GROUP BY[Service],[Option]", connection);
            command.Parameters.Add(new SqlParameter("From", from));
            command.Parameters.Add(new SqlParameter("To", to));
            command.Parameters.Add(new SqlParameter("IsPaid", isPaid));
            SqlDataReader reader = command.ExecuteReader();
            List<Service> services = new List<Service>();
            while (reader.Read()) {
                Service x = new Service();
                x.service = reader.GetString(0);
                x.option = reader.GetString(1);
                x.count = reader.GetInt32(2);
                x.price = reader.GetDecimal(3);
                services.Add(x);
            }
            connection.Close();
            string json = JsonConvert.SerializeObject(services, Formatting.Indented);
            return json;
        } catch (Exception e) { return ("Error: " + e); }
    }
    #endregion

    #region Clients
    private class Clients {
        public Int32 currentClients { get; set; }
        public Int32 activeClients { get; set; }
        public Int32 inactiveClients { get; set; }
        public Int32 totalClients { get; set; }
    }

    [WebMethod]
    public string InitClients() {
        Clients x = new Clients();
        x.totalClients = 0;
        x.currentClients = 0;
        x.activeClients = 0;
        x.inactiveClients = 0;
       
        string json = JsonConvert.SerializeObject(x, Formatting.Indented);
        return json;
    }

    [WebMethod]
    public string GetClientsCount() {
        Clients x = new Clients();
        x.totalClients = GetTotalClientsCount();
        x.currentClients = GetCurrentClientsCount();
        x.activeClients = GetActiveClientsCount();
        x.inactiveClients = x.totalClients - x.activeClients;

        string json = JsonConvert.SerializeObject(x, Formatting.Indented);
        return json;
    }

    private int GetTotalClientsCount() {
        Int32 count = 0;
        try {
            connection.Open();
            SqlCommand command = new SqlCommand(@"SELECT COUNT(DISTINCT[ClientId]) FROM ClientServices", connection);
            SqlDataReader reader = command.ExecuteReader();
            while (reader.Read())
            {
                count = reader.GetInt32(0);
            }
            connection.Close();
            return count;
        } catch (Exception e) { return count; }
    }

    private int GetCurrentClientsCount() {
        DateTime today = DateTime.Today.Date.AddHours(-timeDiff).AddMinutes(0).AddSeconds(0);
        Int32 count = 0;
        try {
            connection.Open();
            SqlCommand command = new SqlCommand(@"SELECT COUNT([ClientId]) FROM CheckIn WHERE CheckInTime <= @Today + 1 AND CheckInTime > @Today AND IsCheckOut = 0", connection);
            command.Parameters.Add(new SqlParameter("Today", today));
            SqlDataReader reader = command.ExecuteReader();
            while (reader.Read()) {
                count = reader.GetInt32(0);
            }
            connection.Close();
            return count;
        } catch (Exception e) { return count; }
    }

    private int GetActiveClientsCount() {
        DateTime today = DateTime.Today.Date.AddHours(0).AddMinutes(0).AddSeconds(0);
        Int32 count = 0;
        try {
            connection.Open();
            SqlCommand command = new SqlCommand(@"SELECT COUNT(DISTINCT[ClientId]) FROM ClientServices WHERE [ExpirationDate] > @Today", connection);
            command.Parameters.Add(new SqlParameter("Today", today));
            SqlDataReader reader = command.ExecuteReader();
            while (reader.Read()) {
                count = reader.GetInt32(0);
            }
            connection.Close();
            return count;
        } catch (Exception e) { return count; }
    }

    //TODO

    private class CheckIn {
        public string date { get; set; }
        public int count { get; set; }
    }

    [WebMethod]
    public string GetCheckInCountByDate(DateTime from, DateTime to) {
        try {
            from = from.AddHours(-timeDiff).AddMinutes(0).AddSeconds(0);
            to = to.AddHours(-timeDiff).AddMinutes(0).AddSeconds(0);

            connection.Open();
            SqlCommand command = new SqlCommand(@"SELECT DISTINCT CAST(CheckInTime AS DATE), COUNT(Id) FROM CheckIn
                                                WHERE CAST(CheckInTime AS DATE) BETWEEN @From AND @To
                                                GROUP BY CAST(CheckInTime AS DATE)", connection);
            command.Parameters.Add(new SqlParameter("From", from));
            command.Parameters.Add(new SqlParameter("To", to));
            SqlDataReader reader = command.ExecuteReader();
            List<CheckIn> xx = new List<CheckIn>();
            while (reader.Read()) {
                CheckIn x = new CheckIn();
                x.date =  reader.GetDateTime(0).ToShortDateString();
                x.count = reader.GetInt32(1);
                xx.Add(x);
            }
            connection.Close();
            string json = JsonConvert.SerializeObject(xx, Formatting.Indented);
            return json;
        } catch (Exception e) { return ("Error: " + e); }
    }
    #endregion

    #region MailMessages
    [WebMethod]
    public Int32 GetMailMessagesCount() {
        try {
            Int32 count = 0;
            connection.Open();
            SqlCommand command = new SqlCommand(@"SELECT COUNT([Id])
                                                FROM MailMessages", connection);
            SqlDataReader reader = command.ExecuteReader();
            while (reader.Read()) {
                count = reader.GetInt32(0);
            }
            connection.Close();
            return count;
        } catch (Exception e) { return 0; }
    }
    #endregion


}
