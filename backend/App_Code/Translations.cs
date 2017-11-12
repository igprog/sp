using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;
using System.Web.Script.Services;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;
//using Newtonsoft.Json;
//using System.Runtime.Serialization;
using System.IO;

/// <summary>
/// Language translations
/// </summary>
[WebService(Namespace = "http://rivierasplit.com/")]
[WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
[System.Web.Script.Services.ScriptService]
//[DataContract(IsReference = true)]
public class Translations : System.Web.Services.WebService {

    //public Translations () {
    //    //TranslationId = null;
    //    Title = null;
    //    Language1 = null;
    //    Language2 = null;
    //    Language3 = null;
    //    Language4 = null;
    //    Language5 = null;
    //    //Uncomment the following line if using designed components 
    //    //InitializeComponent(); 
    //}

   

    //[DataMember]
    //public Guid? TranslationId { get; set; }

    //[DataMember]
    //public String Title { get; set; }

    //[DataMember]
    //public String Language1 { get; set; }

    //[DataMember]
    //public String Language2 { get; set; }

    //[DataMember]
    //public String Language3 { get; set; }

    //[DataMember]
    //public String Language4 { get; set; }

    //[DataMember]
    //public String Language5 { get; set; }

    //[WebMethod]
    //public String OldGetAllTranslation() {
    //    SqlConnection connection = new SqlConnection(ConfigurationManager.ConnectionStrings["connectionString"].ConnectionString);
    //    connection.Open();
    //    SqlCommand command = new SqlCommand("SELECT TranslationId, Title, Language1, Language2, Language3, Language4, Language5 FROM Translations", connection);

    //    SqlDataReader reader = command.ExecuteReader();
    //    List<Translations> translations = new List<Translations>();
    //    List<String> titleList = new List<String>();

    //    while (reader.Read()){
    //        Translations xx = new Translations() {
    //          //  TranslationId = reader.GetGuid(0),
    //            Title = reader.GetValue(1) == DBNull.Value ? "" : reader.GetString(1),
    //            Language1 = reader.GetValue(2) == DBNull.Value ? "" : reader.GetString(2),
    //            Language2 = reader.GetValue(3) == DBNull.Value ? "" : reader.GetString(3),
    //            Language3 = reader.GetValue(4) == DBNull.Value ? "" : reader.GetString(4),
    //            Language4 = reader.GetValue(5) == DBNull.Value ? "" : reader.GetString(5),
    //            Language5 = reader.GetValue(6) == DBNull.Value ? "" : reader.GetString(6)
    //        };
    //        translations.Add(xx);
    //        //titleList.Add(
          
    //        //(reader.GetValue(1) == DBNull.Value ? "" : reader.GetString(1)).ToString()
    //        //+ ":" + 
    //        //(reader.GetValue(2) == DBNull.Value ? "" : reader.GetString(2)).ToString()
          
    //        //    );

    //      //  titleList.Add(
            
    //        //string[] names = new string[] {"Matt", "Joanne", "Robert"};
    //        //List<int> termsList = new List<int>();
    //        // termsList.Add(value);
    //        //string[] titleList = new string[] {

    //        //};



    //    }
    //    connection.Close();

    //    string json = JsonConvert.SerializeObject(translations, Formatting.Indented);
    //    return json;
    //}

    [WebMethod]
    public void GetAllTranslations() {
        string translations = readLanguages(2);
        CreateFolder("~/json/translations/hr/");
        WriteFile("~/json/translations/hr/main.json", translations);
        translations = readLanguages(3);
        CreateFolder("~/json/translations/en/");
        WriteFile("~/json/translations/en/main.json", translations);
        translations = readLanguages(4);
        CreateFolder("~/json/translations/de/");
        WriteFile("~/json/translations/de/main.json", translations);
    }

    public void CreateFolder(string path) {
        if (!Directory.Exists(Server.MapPath(path))) {
            Directory.CreateDirectory(Server.MapPath(path));
        }
    }

    public void WriteFile(string path, string value) {
        File.WriteAllText(Server.MapPath(path), value);
    }

    public string readLanguages(int col){
        SqlConnection connection = new SqlConnection(ConfigurationManager.ConnectionStrings["connectionString"].ConnectionString);
        connection.Open();
        SqlCommand command = new SqlCommand("SELECT TranslationId, Title, Language1, Language2, Language3, Language4, Language5 FROM Translations", connection);
        SqlDataReader reader = command.ExecuteReader();
        string json = "";
        string comma = "";
        while (reader.Read()) {
            if (json == "") { comma = ""; } else { comma = ","; }
            json = json + comma + "'" +
                (reader.GetValue(1) == DBNull.Value ? "" : reader.GetString(1)).ToString()
                + "':'" +
                (reader.GetValue(col) == DBNull.Value ? "" : reader.GetString(col)).ToString()
                + "'";
        }
        json = ("{" + json + "}").Replace("'", "\"");
        connection.Close();

        return json;
    }

}
