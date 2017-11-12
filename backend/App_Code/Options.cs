using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;
using System.Web.Script.Services;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using Newtonsoft.Json;
using System.Runtime.Serialization;
using System.IO;

/// <summary>
/// Summary description for Options
/// </summary>
[WebService(Namespace = "http://igprog.hr/")]
[WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
[System.Web.Script.Services.ScriptService]
[DataContract(IsReference = true)]
public class Options : System.Web.Services.WebService {

    public Options () {
        Id = null;
        Cod = null;
        OptionCod = null;
        Title = null; 
    }

    [DataMember]
    public Guid? Id { get; set; }

    [DataMember]
    public String Cod { get; set; }

    [DataMember]
    public String OptionCod { get; set; }

    [DataMember]
    public String Title { get; set; }


    [WebMethod]
    public void GetAllOptions() {
        SqlConnection connection = new SqlConnection(ConfigurationManager.ConnectionStrings["connectionString"].ConnectionString);
        connection.Open();
        SqlCommand command = new SqlCommand("SELECT Id, Cod, OptionCod, Title FROM Options", connection);
        SqlDataReader reader = command.ExecuteReader();
        List<Options> options = new List<Options>();
        while (reader.Read()) {
            Options xx = new Options() {
                Id = reader.GetGuid(0),
                Cod = reader.GetValue(1) == DBNull.Value ? "" : reader.GetString(1),
                OptionCod = reader.GetValue(2) == DBNull.Value ? "" : reader.GetString(2),
                Title = reader.GetValue(3) == DBNull.Value ? "" : reader.GetString(3),
            };
            options.Add(xx);
        }
        connection.Close();

        string json = JsonConvert.SerializeObject(options, Formatting.Indented);
        CreateFolder("~/json/");
        WriteFile("~/json/alloptions.json", json);
    }

    [WebMethod]
    public void GetOptionsByCod(String cod) {
        SqlConnection connection = new SqlConnection(ConfigurationManager.ConnectionStrings["connectionString"].ConnectionString);
        connection.Open();
        SqlCommand command = new SqlCommand("SELECT Id, Cod, OptionCod, Title FROM Options WHERE @Cod = Cod", connection);
        command.Parameters.Add(new SqlParameter("Cod", cod));

        SqlDataReader reader = command.ExecuteReader();
        List<Options> options = new List<Options>();
        while (reader.Read()) {
            Options xx = new Options() {
                Id = reader.GetGuid(0),
                Cod = reader.GetValue(1) == DBNull.Value ? "" : reader.GetString(1),
                OptionCod = reader.GetValue(2) == DBNull.Value ? "" : reader.GetString(2),
                Title = reader.GetValue(3) == DBNull.Value ? "" : reader.GetString(3),
            };
            options.Add(xx);
        }
        connection.Close();

        string json = JsonConvert.SerializeObject(options, Formatting.Indented);
        CreateFolder("~/json/");
        WriteFile("~/json/" + cod + ".json", json);

    }

    public void CreateFolder(string path) {
        if (!Directory.Exists(Server.MapPath(path))) {
            Directory.CreateDirectory(Server.MapPath(path));
        }
    }

    public void WriteFile(string path, string value) {
        File.WriteAllText(Server.MapPath(path), value);
    }

    
}
