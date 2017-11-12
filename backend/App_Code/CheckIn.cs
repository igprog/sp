using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;
using System.IO;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using System.Security.Cryptography;
using Newtonsoft.Json;

/// <summary>
/// CheckIn
/// </summary>
[WebService(Namespace = "http://igprog.hr/")]
[WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
[System.Web.Script.Services.ScriptService]
public class CheckIn : System.Web.Services.WebService {
    SqlConnection connection = new SqlConnection(ConfigurationManager.ConnectionStrings["connectionString"].ConnectionString);
    int timeDiff = Convert.ToInt32(ConfigurationManager.AppSettings["ServerHostTimeDifference"]);

    public CheckIn() {
    }

    public class NewCheckin {
        public Int32? id { get; set; }
        public Int32? clientId { get; set; }
        public String firstName { get; set; }
        public String lastName { get; set; }
        public String service { get; set; }
        public DateTime? checkInTime { get; set; }
        public int isCheckOut { get; set; }
        public DateTime? checkOutTime { get; set; }
        public Int32? userId { get; set; }
        public int isDebtor { get; set; }
        public int debt { get; set; }
    }

    [WebMethod]
    public string Init() {
        NewCheckin xx = new NewCheckin();
        xx.id = null;
        xx.clientId = null;
        xx.firstName = null;
        xx.lastName = null;
        xx.service = "";
        xx.checkInTime = DateTime.UtcNow; // DateTime.Now;
        xx.isCheckOut = 0;
        xx.checkOutTime = null;
        xx.userId = null;
        xx.isDebtor = 0;
        xx.debt = 0;
        string json = JsonConvert.SerializeObject(xx, Formatting.Indented);
        return json;
    }

    [WebMethod]
    public string Load() {
        try {
            connection.Open();
            SqlCommand command = new SqlCommand(@"SELECT TOP 50 CheckIn.[Id], CheckIn.[ClientId], Clients.[FirstName], Clients.[LastName], CheckIn.[Service], CheckIn.[CheckInTime], CheckIn.[IsCheckOut], CheckIn.[CheckOutTime], CheckIn.[UserId] FROM CheckIn
                                                LEFT OUTER JOIN Clients
                                                ON CheckIn.[ClientId] = Clients.[ClientId]
                                                ORDER BY Id DESC", connection);
            List<NewCheckin> checkIns = new List<NewCheckin>();
            SqlDataReader reader = command.ExecuteReader();
            while (reader.Read()) {
                NewCheckin xx = new NewCheckin() {
                    id = reader.GetInt32(0),
                    clientId = reader.GetInt32(1),
                    firstName = reader.GetValue(2) == DBNull.Value ? "" : reader.GetString(2),
                    lastName = reader.GetValue(3) == DBNull.Value ? "" : reader.GetString(3),
                    service = reader.GetValue(4) == DBNull.Value ? "" : reader.GetString(4),
                    checkInTime = reader.GetValue(5) == DBNull.Value ? DateTime.UtcNow : reader.GetDateTime(5).AddHours(timeDiff),   //(timeDiff) = server host time difference,
                    isCheckOut = reader.GetInt32(6),
                    checkOutTime = reader.GetValue(7) == DBNull.Value ? DateTime.UtcNow : reader.GetDateTime(7).AddHours(timeDiff),
                    userId = reader.GetInt32(8)
                };
                checkIns.Add(xx);
            }
            connection.Close();
            string json = JsonConvert.SerializeObject(checkIns, Formatting.Indented);
            return json;
        } catch (Exception e) { return ("Error: " + e); }
    }

    [WebMethod]
    public string Save(NewCheckin checkIn) {
            try {
                connection.Open();
                string sql = @"INSERT INTO CheckIn (ClientId, Service, CheckInTime, IsCheckOut, UserId) VALUES  
                       (@ClientId, @Service, @CheckInTime, @IsCheckOut, @UserId)";
                SqlCommand command = new SqlCommand(sql, connection);
                command.Parameters.Add(new SqlParameter("ClientId", checkIn.clientId));
                command.Parameters.Add(new SqlParameter("Service", checkIn.service));
                command.Parameters.Add(new SqlParameter("CheckInTime", checkIn.checkInTime));
                command.Parameters.Add(new SqlParameter("IsCheckOut", checkIn.isCheckOut));
              //  command.Parameters.Add(new SqlParameter("CheckOutTime", checkIn.checkOutTime));
                command.Parameters.Add(new SqlParameter("UserId", checkIn.userId));
                command.ExecuteNonQuery();
                connection.Close();
            return "OK"; // Load();
        } catch (Exception e) { return ("Error: " + e); }
    }

    [WebMethod]
    public string CheckOut(NewCheckin x) {
        x.isCheckOut = 1;
        try {
            connection.Open();
            string sql = @"UPDATE CheckIn SET  
                        IsCheckOut = @IsCheckOut, CheckOutTime = @CheckOutTime
                        WHERE Id = @Id";
            SqlCommand command = new SqlCommand(sql, connection);
            command.Parameters.Add(new SqlParameter("Id", x.id));
            command.Parameters.Add(new SqlParameter("IsCheckOut", x.isCheckOut));
            command.Parameters.Add(new SqlParameter("CheckOutTime", x.checkOutTime));
            command.ExecuteNonQuery();
            connection.Close();
            return "OK"; // Load();
        } catch (Exception e) { return ("Error: " + e); }
    }

    [WebMethod]
    public string Delete(Int32 id) {
        try {
            connection.Open();
            SqlCommand command = new SqlCommand("DELETE FROM CheckIn WHERE Id = @Id", connection);
            command.Parameters.Add(new SqlParameter("Id", id));
            command.ExecuteNonQuery();
            connection.Close();
            return "OK";  // Load();
        } catch (Exception e) { return ("Error: " + e); }
    }

    [WebMethod]
    public string GetCheckInsByDate(DateTime date) {
        try {
            connection.Open();
            string sql = @"SELECT CheckIn.[Id], CheckIn.[ClientId], Clients.[FirstName], Clients.[LastName], CheckIn.[Service], CheckIn.[CheckInTime], CheckIn.[IsCheckOut], CheckIn.[CheckOutTime], CheckIn.[UserId], Count(ClientServices.[IsPaid]), SUM(CAST(ClientServices.[Price] AS INT)) FROM CheckIn
                        LEFT OUTER JOIN Clients
                        ON CheckIn.[ClientId] = Clients.[ClientId]
                        LEFT OUTER JOIN ClientServices
                        ON CheckIn.[ClientId] = ClientServices.[ClientId] AND ClientServices.[IsPaid] = 0
                        WHERE CheckIn.[CheckInTime] >= @Date AND CheckIn.[CheckInTime] < @Date + 1
                        GROUP BY CheckIn.[Id], CheckIn.[ClientId], Clients.[FirstName], Clients.[LastName], CheckIn.[Service], CheckIn.[CheckInTime], CheckIn.[IsCheckOut], CheckIn.[CheckOutTime], CheckIn.[UserId]
                        ORDER BY Id DESC";
            SqlCommand command = new SqlCommand(sql, connection);
            command.Parameters.Add(new SqlParameter("Date", date));
            List<NewCheckin> checkIns = new List<NewCheckin>();
            SqlDataReader reader = command.ExecuteReader();
            while (reader.Read()) {
                NewCheckin xx = new NewCheckin() {
                    id = reader.GetInt32(0),
                    clientId = reader.GetInt32(1),
                    firstName = reader.GetValue(2) == DBNull.Value ? "" : reader.GetString(2),
                    lastName = reader.GetValue(3) == DBNull.Value ? "" : reader.GetString(3),
                    service = reader.GetValue(4) == DBNull.Value ? "" : reader.GetString(4),
                    checkInTime = reader.GetValue(5) == DBNull.Value ? DateTime.UtcNow : reader.GetDateTime(5).AddHours(timeDiff),
                    isCheckOut = reader.GetInt32(6),
                    checkOutTime = reader.GetValue(7) == DBNull.Value ? DateTime.UtcNow : reader.GetDateTime(7).AddHours(timeDiff),
                    userId = reader.GetInt32(8),
                    isDebtor = reader.GetInt32(9) > 0 ? 1 : 0,
                    debt = reader.GetValue(10) == DBNull.Value ? 0 : reader.GetInt32(10)
                };
                checkIns.Add(xx);
            }
            connection.Close();
            string json = JsonConvert.SerializeObject(checkIns, Formatting.Indented);
            return json;
        } catch (Exception e) { return ("Error: " + e); }
    }

    [WebMethod]
    public string GetCheckInCount(Int32 clientId, String service) {
        try {
            connection.Open();
            SqlCommand command = new SqlCommand(@"SELECT COUNT(*) FROM CheckIn WHERE [Service] = @Service AND [ClientId] = @ClientId", connection);
            command.Parameters.Add(new SqlParameter("ClientId", clientId));
            command.Parameters.Add(new SqlParameter("Service", service));
            SqlDataReader reader = command.ExecuteReader();
            Int32 count = 0;
            while (reader.Read()) {
                count = reader.GetInt32(0);
            }
            connection.Close();
            string json = JsonConvert.SerializeObject(count, Formatting.Indented);
            return json;
        } catch (Exception e) { return ("Error: " + e); }
    }

}
