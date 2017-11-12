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
using Newtonsoft.Json;

/// <summary>
/// ClientServices
/// </summary>
[WebService(Namespace = "http://igprog.hr/")]
[WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
[System.Web.Script.Services.ScriptService]
public class ClientServices : System.Web.Services.WebService {
    SqlConnection connection = new SqlConnection(ConfigurationManager.ConnectionStrings["connectionString"].ConnectionString);

    public ClientServices() {
    }

    public class NewService {
        public Int32? id { get; set; }
        public Int32? clientId { get; set; }
        public String service { get; set; }
        public String option { get; set; }
        public Int32? quantity { get; set; }
        public String unit { get; set; }
        public String price { get; set; }
        public DateTime? activationDate { get; set; }
        public DateTime? expirationDate { get; set; }
        public Int32? quantityLeft { get; set; }
        public bool isPaid { get; set; }
        public bool isFreezed { get; set; }
    }

    [WebMethod]
    public string Init() {
        NewService xx = new NewService();
        xx.id = null;
        xx.clientId = null;
        xx.service = null;
        xx.option = null;
        xx.quantity = null;
        xx.unit = null;
        xx.price = null;
        xx.activationDate = DateTime.UtcNow; // DateTime.Now;
        xx.expirationDate = DateTime.UtcNow.AddDays(31);
        xx.quantityLeft = null;
        xx.isPaid = false;
        xx.isFreezed = false;
        string json = JsonConvert.SerializeObject(xx, Formatting.Indented);
        return json;
    }

    [WebMethod]
    public string Load() {
     try {
        connection.Open();
        SqlCommand command = new SqlCommand(@"SELECT [Id], [ClientId], [Service], [Option], [Quantity], [Unit], [Price], [ActivationDate], [ExpirationDate], [QuantityLeft], [IsPaid], [IsFreezed] FROM ClientServices
                                            ORDER BY Id DESC", connection);
        SqlDataReader reader = command.ExecuteReader();
        List<NewService> services = new List<NewService>();
        while (reader.Read()) {
            NewService xx = new NewService() {
                id = reader.GetInt32(0),
                clientId = reader.GetInt32(1),
                service = reader.GetValue(2) == DBNull.Value ? "" : reader.GetString(2),
                option = reader.GetValue(3) == DBNull.Value ? "" : reader.GetString(3),
                quantity = reader.GetValue(4) == DBNull.Value ? 0 : reader.GetInt32(4),
                unit = reader.GetValue(5) == DBNull.Value ? "" : reader.GetString(5),
                price = reader.GetValue(6) == DBNull.Value ? "" : reader.GetString(6),
                activationDate = reader.GetValue(7) == DBNull.Value ? DateTime.UtcNow : reader.GetDateTime(7),
                expirationDate = reader.GetValue(8) == DBNull.Value ? DateTime.UtcNow : reader.GetDateTime(8),
                quantityLeft = reader.GetValue(9) == DBNull.Value ? 0 : reader.GetInt32(9),
                isPaid = Convert.ToBoolean(reader.GetInt32(10)),
                isFreezed = Convert.ToBoolean(reader.GetInt32(11))
            };
            services.Add(xx);
        }
            connection.Close();
            string json = JsonConvert.SerializeObject(services, Formatting.Indented);
            return json;
            } catch (Exception e) { return ("Error: " + e); }
    }

    [WebMethod]
    public string Save(NewService clientService) {
        try {
            connection.Open();
            string sql = @"INSERT INTO ClientServices VALUES  
                       (@ClientId, @Service, @Option, @Quantity, @Unit, @Price, @ActivationDate, @ExpirationDate, @QuantityLeft, @IsPaid, @IsFreezed)";
            SqlCommand command = new SqlCommand(sql, connection);
            command.Parameters.Add(new SqlParameter("ClientId", clientService.clientId));
            command.Parameters.Add(new SqlParameter("Service", clientService.service));
            command.Parameters.Add(new SqlParameter("Option", clientService.option));
            command.Parameters.Add(new SqlParameter("Quantity", clientService.quantity));
            command.Parameters.Add(new SqlParameter("Unit", clientService.unit));
            command.Parameters.Add(new SqlParameter("Price", clientService.price));
            command.Parameters.Add(new SqlParameter("ActivationDate", clientService.activationDate));
            command.Parameters.Add(new SqlParameter("ExpirationDate", clientService.expirationDate));
            command.Parameters.Add(new SqlParameter("QuantityLeft", clientService.quantityLeft));
            command.Parameters.Add(new SqlParameter("IsPaid", clientService.isPaid));
            command.Parameters.Add(new SqlParameter("IsFreezed", clientService.isFreezed));
            command.ExecuteNonQuery();
            connection.Close();
            return "OK"; // GetClientServices(clientService.clientId);
        } catch (Exception e) { return ("Error: " + e); }
    }

    [WebMethod]
    public string Update(NewService clientService) {
        try {
            connection.Open();
            string sql = @"UPDATE ClientServices SET  
                        [Service] = @Service, [Option] = @Option, [Quantity] = @Quantity, [Unit] = @Unit, [Price] = @Price, [ActivationDate] = @ActivationDate, [ExpirationDate] = @ExpirationDate, [QuantityLeft] = @QuantityLeft, [IsPaid] = @IsPaid, [IsFreezed] = @IsFreezed
                        WHERE [ClientId] = @ClientId AND [Id] = @Id";

            SqlCommand command = new SqlCommand(sql, connection);
            command.Parameters.Add(new SqlParameter("Id", clientService.id));
            command.Parameters.Add(new SqlParameter("ClientId", clientService.clientId));
            command.Parameters.Add(new SqlParameter("Service", clientService.service));
            command.Parameters.Add(new SqlParameter("Option", clientService.option));
            command.Parameters.Add(new SqlParameter("Quantity", clientService.quantity));
            command.Parameters.Add(new SqlParameter("Unit", clientService.unit));
            command.Parameters.Add(new SqlParameter("Price", clientService.price));
            command.Parameters.Add(new SqlParameter("ActivationDate", clientService.activationDate));
            command.Parameters.Add(new SqlParameter("ExpirationDate", clientService.expirationDate));
            command.Parameters.Add(new SqlParameter("QuantityLeft", clientService.quantityLeft));
            command.Parameters.Add(new SqlParameter("IsPaid", clientService.isPaid));
            command.Parameters.Add(new SqlParameter("IsFreezed", clientService.isFreezed));
            command.ExecuteNonQuery();
            connection.Close();
            return "OK";  // Load();
        } catch (Exception e) { return ("Update failed! (Error: )" + e); }
    }

    [WebMethod]
    public string Delete(Int32 id) {
        try {
            connection.Open();
            SqlCommand command = new SqlCommand("DELETE FROM ClientServices WHERE Id = @Id", connection);
            command.Parameters.Add(new SqlParameter("Id", id));
            command.ExecuteNonQuery();
            connection.Close();
            return "OK"; // Load();
        } catch (Exception e) { return ("Error: " + e); }
    }

    [WebMethod]
    public string GetClientServices(Int32? clientId) {
    try {
        connection.Open();
        SqlCommand command = new SqlCommand(@"SELECT [Id], [ClientId], [Service], [Option], [Quantity], [Unit], [Price], [ActivationDate], [ExpirationDate], [QuantityLeft], [IsPaid], [IsFreezed] FROM ClientServices
                                            WHERE ClientId = @ClientId
                                            ORDER BY [Id] DESC", connection);
        command.Parameters.Add(new SqlParameter("ClientId", clientId));
        SqlDataReader reader = command.ExecuteReader();
        List<NewService> clientServices = new List<NewService>();
        while (reader.Read()) {
                NewService xx = new NewService() {
                    id = reader.GetInt32(0),
                    clientId = reader.GetInt32(1),
                    service = reader.GetValue(2) == DBNull.Value ? "" : reader.GetString(2),
                    option = reader.GetValue(3) == DBNull.Value ? "" : reader.GetString(3),
                    quantity = reader.GetValue(4) == DBNull.Value ? 0 : reader.GetInt32(4),
                    unit = reader.GetValue(5) == DBNull.Value ? "" : reader.GetString(5),
                    price = reader.GetValue(6) == DBNull.Value ? "" : reader.GetString(6),
                    activationDate = reader.GetValue(7) == DBNull.Value ? DateTime.UtcNow : reader.GetDateTime(7),
                    expirationDate = reader.GetValue(8) == DBNull.Value ? DateTime.UtcNow.AddDays(31) : reader.GetDateTime(8),
                    quantityLeft = reader.GetValue(9) == DBNull.Value ? 0 : reader.GetInt32(9),
                    isPaid = Convert.ToBoolean(reader.GetInt32(10)),
                    isFreezed = Convert.ToBoolean(reader.GetInt32(11))
                };
            clientServices.Add(xx);
        }
            connection.Close();
            string json = JsonConvert.SerializeObject(clientServices, Formatting.Indented);
            return json;
        } catch (Exception e) { return ("Error: " + e); }
    }

    [WebMethod]
    public string GetActiveClientServices(Int32 clientId) {
        try {
            connection.Open();
            SqlCommand command = new SqlCommand(@"SELECT DISTINCT [Id], [ClientId], [Service], [Option], [Quantity], [Unit], [Price], [ActivationDate], [ExpirationDate], [QuantityLeft], [IsPaid], [IsFreezed] FROM ClientServices
                                            WHERE ClientId = @ClientId AND [ActivationDate] <= GETDATE() AND [ExpirationDate] >= GETDATE() AND QuantityLeft > 0
                                            GROUP BY [Id], [ClientId], [Service], [Option], [Quantity], [Unit], [Price], [ActivationDate], [ExpirationDate], [QuantityLeft], [IsPaid], [IsFreezed]
                                            ORDER BY [Service] ASC", connection);
            command.Parameters.Add(new SqlParameter("ClientId", clientId));
            SqlDataReader reader = command.ExecuteReader();
            List<NewService> clientServices = new List<NewService>();
            while (reader.Read()) {
                NewService xx = new NewService() {
                    id = reader.GetInt32(0),
                    clientId = reader.GetInt32(1),
                    service = reader.GetValue(2) == DBNull.Value ? "" : reader.GetString(2),
                    option = reader.GetValue(3) == DBNull.Value ? "" : reader.GetString(3),
                    quantity = reader.GetValue(4) == DBNull.Value ? 0 : reader.GetInt32(4),
                    unit = reader.GetValue(5) == DBNull.Value ? "" : reader.GetString(5),
                    price = reader.GetValue(6) == DBNull.Value ? "" : reader.GetString(6),
                    activationDate = reader.GetValue(7) == DBNull.Value ? DateTime.UtcNow : reader.GetDateTime(7),
                    expirationDate = reader.GetValue(8) == DBNull.Value ? DateTime.UtcNow.AddDays(31) : reader.GetDateTime(8),
                    quantityLeft = reader.GetValue(9) == DBNull.Value ? 0 : reader.GetInt32(9),
                    isPaid = Convert.ToBoolean(reader.GetInt32(10)),
                    isFreezed = Convert.ToBoolean(reader.GetInt32(11))
                };
                clientServices.Add(xx);
            }
            connection.Close();
            string json = JsonConvert.SerializeObject(clientServices, Formatting.Indented);
            return json;
        } catch (Exception e) { return ("Error: " + e); }
    }

}
