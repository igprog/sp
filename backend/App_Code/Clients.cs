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
/// Clients
/// </summary>
[WebService(Namespace = "http://igprog.hr/")]
[WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
[System.Web.Script.Services.ScriptService]
public class Clients : System.Web.Services.WebService {
    SqlConnection connection = new SqlConnection(ConfigurationManager.ConnectionStrings["connectionString"].ConnectionString);
    public Clients() {
    }
    public class NewClient {
        public Int32? clientId { get; set; }
        public String firstName { get; set; }
        public String lastName { get; set; }
        public String email { get; set; }
        public String phone { get; set; }
        public DateTime? activationDate { get; set; }
        public int isActive { get; set; }
        public int isDebtor { get; set; }
        public DateTime? expirationService { get; set; }
    }

    [WebMethod]
    public string Init() {
        NewClient client = new NewClient();
        client.clientId = null;
        client.firstName = "";
        client.lastName = "";
        client.email = "";
        client.phone = "";
        client.activationDate = DateTime.UtcNow;
        client.isActive = 1;
        client.isDebtor = 0;
        client.expirationService = DateTime.UtcNow;
        string json = JsonConvert.SerializeObject(client, Formatting.Indented);
        return json;
    }

    [WebMethod]
    public string Load() {
        try {
            connection.Open();
            string sql = @"SELECT DISTINCT c.ClientId, c.FirstName, c.LastName, c.Email, c.Phone, c.ActivationDate, c.IsActive, Count(cs.IsPaid), cs1.ExpirationDate  FROM Clients AS c
                        LEFT OUTER JOIN ClientServices AS cs
                        ON c.ClientId = cs.ClientId AND cs.IsPaid = 0
                        LEFT OUTER JOIN ClientServices AS cs1
                        ON c.ClientId = cs1.ClientId AND cs1.ExpirationDate >= GETDATE()
                        GROUP BY c.ClientId, c.FirstName, c.LastName, c.Email, c.Phone, c.ActivationDate, c.IsActive, cs1.ExpirationDate
                        ORDER BY cs1.ExpirationDate DESC";
            SqlCommand command = new SqlCommand(sql, connection);
            SqlDataReader reader = command.ExecuteReader();
            List<NewClient> xx = new List<NewClient>();
            while (reader.Read()) {
                NewClient x = new NewClient();
                x.clientId = reader.GetInt32(0);
                x.firstName = reader.GetValue(1) == DBNull.Value ? "" : reader.GetString(1);
                x.lastName = reader.GetValue(2) == DBNull.Value ? "" : reader.GetString(2);
                x.email = reader.GetValue(3) == DBNull.Value ? "" : reader.GetString(3);
                x.phone = reader.GetValue(4) == DBNull.Value ? "" : reader.GetString(4);
                x.activationDate = reader.GetDateTime(5);
                x.isActive = reader.GetValue(6) == DBNull.Value ? 1 : reader.GetInt32(6);
                x.isDebtor = reader.GetInt32(7) > 0 ? 1 : 0;
                x.expirationService = reader.GetValue(8) == DBNull.Value ? DateTime.UtcNow : reader.GetDateTime(8);
                xx.Add(x);
            }
            connection.Close();
            var distinctUsers = xx.GroupBy(x => x.clientId).Select(x => x.First()).ToList();
            string json = JsonConvert.SerializeObject(distinctUsers, Formatting.Indented);
            return json;
        } catch (Exception e) { return ("Error: " + e); }
    }

    [WebMethod]
    public string Save(NewClient client) {
        if (CheckClient(client) == false){
            return ("Član je već registriran.");
        }
        else {
            try {
                connection.Open();
                string sql = @"INSERT INTO Clients VALUES  
                       (@FirstName, @LastName, @Email, @Phone, @ActivationDate, @IsActive)";
                SqlCommand command = new SqlCommand(sql, connection);
                command.Parameters.Add(new SqlParameter("FirstName", client.firstName));
                command.Parameters.Add(new SqlParameter("LastName", client.lastName));
                command.Parameters.Add(new SqlParameter("Email", client.email));
                command.Parameters.Add(new SqlParameter("Phone", client.phone));
                command.Parameters.Add(new SqlParameter("ActivationDate", client.activationDate));
                command.Parameters.Add(new SqlParameter("IsActive", client.isActive));
                command.ExecuteNonQuery();
                connection.Close();
                return ("Registracija uspješna.");
            } catch (Exception e) { return ("Registracija nije uspjela! (Error: )" + e); }
        }
    }

    [WebMethod]
    public string Update(NewClient client) {
        try {
            connection.Open();
            string sql = @"UPDATE Clients SET  
                        FirstName = @FirstName, LastName = @LastName, Email = @Email, Phone = @Phone, ActivationDate = @ActivationDate, IsActive = @IsActive
                        WHERE ClientId = @ClientId";
            SqlCommand command = new SqlCommand(sql, connection);
            command.Parameters.Add(new SqlParameter("ClientId", client.clientId));
            command.Parameters.Add(new SqlParameter("FirstName", client.firstName));
            command.Parameters.Add(new SqlParameter("LastName", client.lastName));
            command.Parameters.Add(new SqlParameter("Email", client.email));
            command.Parameters.Add(new SqlParameter("Phone", client.phone));
            command.Parameters.Add(new SqlParameter("ActivationDate", client.activationDate));
            command.Parameters.Add(new SqlParameter("IsActive", client.isActive));
            command.ExecuteNonQuery();
            connection.Close();
            return ("Spremljeno.");
        } catch (Exception e) { return ("Error: " + e); }
    }

    [WebMethod]
    public string Delete(NewClient x) {
        try {
            connection.Open();
            string sql = @"BEGIN
                        DELETE FROM Clients WHERE ClientId = @ClientId;
                        DELETE FROM ClientServices WHERE ClientId = @ClientId;
                        END";
            SqlCommand command = new SqlCommand(sql, connection);
            command.Parameters.Add(new SqlParameter("ClientId", x.clientId));
            command.ExecuteNonQuery();
            connection.Close();
            return "OK";
        } catch (Exception e) { return ("Error: " + e); }
    }

    [WebMethod]
    public string GetClient(Int32 clientId) {
        try {
        connection.Open();
        SqlCommand command = new SqlCommand("SELECT ClientId, FirstName, LastName, Email, Phone, ActivationDate, IsActive FROM Clients WHERE ClientId = @ClientId", connection);
        command.Parameters.Add(new SqlParameter("ClientId", clientId));
        NewClient xx = new NewClient();
        SqlDataReader reader = command.ExecuteReader();
        while (reader.Read()) {
            xx.clientId = reader.GetInt32(0);
            xx.firstName = reader.GetValue(1) == DBNull.Value ? "" : reader.GetString(1);
            xx.lastName = reader.GetValue(2) == DBNull.Value ? "" : reader.GetString(2);
            xx.email = reader.GetValue(3) == DBNull.Value ? "" : reader.GetString(3);
            xx.phone = reader.GetValue(4) == DBNull.Value ? "" : reader.GetString(4);
            xx.activationDate = reader.GetDateTime(5);
            xx.isActive = reader.GetValue(6) == DBNull.Value ? 1 : reader.GetInt32(6);
        }
        connection.Close();
        string json = JsonConvert.SerializeObject(xx, Formatting.Indented);
        return json;
        } catch (Exception e) { return ("Error: " + e); }
    }

    protected bool CheckClient(NewClient client) {
        try {
        string firstName = "";
        string lastName = "";
        SqlConnection connection = new SqlConnection(ConfigurationManager.ConnectionStrings["connectionString"].ConnectionString);
        connection.Open();
        SqlCommand command = new SqlCommand(
            "SELECT FirstName, LastName FROM Clients WHERE FirstName = @FirstName AND LastName = @LastName ", connection);

        command.Parameters.Add(new SqlParameter("FirstName", client.firstName));
        command.Parameters.Add(new SqlParameter("LastName", client.lastName));
        SqlDataReader reader = command.ExecuteReader();
        while (reader.Read()) {
            firstName = reader.GetString(0);
            lastName = reader.GetString(1);
        }
        connection.Close();
            if (client.firstName == firstName && client.lastName == lastName) {
                return false;
            }
            return true;
        } catch (Exception e) { return false; }
    }

    public string test { get; set; }


}
