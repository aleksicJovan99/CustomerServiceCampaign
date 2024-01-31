using System.Globalization;
using System.Web;
using System.Xml.Linq;
using Contracts;
using Entities;
using MySql.Data.MySqlClient;

namespace Service;
public class CustomerService : ICustomerService
{

    private readonly IRepositoryManager _repository;

    public CustomerService(IRepositoryManager repository)
    {
        _repository = repository;
    }

    public async Task ImportSourceCustomers(string connectionString)
    {
        IEnumerable<dynamic> customers = null;
        string result = string.Empty;
        int id = 1;
        
        using (var client = new HttpClient())
        {
            while(true)
            {
                var uri = new Uri("https://www.crcind.com/csp/samples/SOAP.Demo.cls?soap_method=FindPerson&id=1");
                var uriBuilder = new UriBuilder(uri);
                var uriQuery = HttpUtility.ParseQueryString(uriBuilder.Query);

                uriQuery["id"] = id.ToString();
                uriBuilder.Query = uriQuery.ToString();

                var url = uriBuilder.ToString();
                var response = client.GetAsync(url);

                if (!response.Result.IsSuccessStatusCode)
                {
                    throw new Exception("Cannot get API table: " + response.Result.StatusCode);
                }

                var xmlString = response.Result.Content.ReadAsStringAsync().Result;
                // Parse the XML string into an XElement
                XDocument doc = XDocument.Parse(xmlString);

                // Define namespaces
                XNamespace soapEnv = "http://schemas.xmlsoap.org/soap/envelope/";
                XNamespace tempuri = "http://tempuri.org";

                // Get FindPersonResult element
                var findPersonResult = doc.Descendants(tempuri + "FindPersonResult").FirstOrDefault();

                if (findPersonResult == null) break;
            
                // Create a dictionary to store values
                Dictionary<string, string> values = new Dictionary<string, string>();
                values["Id"] = Guid.NewGuid().ToString();
                values["SourceId"] = id.ToString();

                // Iterate through child elements and store their names and values in the dictionary
                foreach (var element in findPersonResult.Elements())
                {
                    // Check if the element has child elements
                    if (element.HasElements)
                    {
                        foreach (var childElement in element.Elements())
                        {
                            values[$"{childElement.Parent.Name.LocalName}{childElement.Name.LocalName}"] = childElement.Value;                         
                        }
                    }
                    else
                    {
                        // If it doesn't have child elements, store its value directly
                        values[element.Name.LocalName] = element.Value;
                    }
                }
                values["ImportDate"] = DateTime.Now.ToString("yyyy'-'MM'-'dd");

                SqlHelper.InsertData(values, connectionString, "SourceCustomers");

                id++;
            }
            

        }
        
    }

    public async Task<bool> UpdateCustomersTable(string connectionString)
    {

        var newDataCheck = false;
        using (MySqlConnection connection = new MySqlConnection(connectionString))
        {
            Customer? customer = null;
            CustomerForCreateDto? customerDto = null;

            connection.Open();
                
            string query = 
                "SELECT Id, Name, SSN, DOB, HomeStreet, HomeCity, HomeState, HomeZip, OfficeStreet, OfficeCity, OfficeState, OfficeZip, Title, Salary, ImportDate  FROM SourceCustomers";
                
            using (MySqlCommand command = new MySqlCommand(query, connection))
            {
                using (MySqlDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        Guid customerId = Guid.TryParse(SqlHelper.
                            GetNullableString(reader, "Id"), out Guid result) ?  result : Guid.NewGuid();
                        
                        var checkCustomer = await _repository.Customer.GetCustomerByIdAsync(customerId);

                        if(checkCustomer != null) continue;

                        DateTime? birthDate = DateTime.
                            TryParseExact(SqlHelper.GetNullableString(reader, "DOB"), "yyyy-MM-dd", CultureInfo.InvariantCulture, DateTimeStyles.AdjustToUniversal, out DateTime birth)
                                ? birth : null; 

                        DateTime importDate = DateTime.
                            TryParseExact(SqlHelper.GetNullableString(reader, "ImportDate"), "yyyy-MM-dd", CultureInfo.InvariantCulture, DateTimeStyles.AdjustToUniversal, out DateTime import)
                                ? import : DateTime.Now;

                        customer = new Customer
                        {
                            Id = customerId,
                            Name = SqlHelper.GetNullableString(reader, "Name"),
                            SSN = SqlHelper.GetNullableString(reader, "SSN"),
                            Birthdate = birthDate,
                            HomeStreet = SqlHelper.GetNullableString(reader, "HomeStreet"),
                            HomeCity = SqlHelper.GetNullableString(reader, "HomeCity"),
                            HomeState = SqlHelper.GetNullableString(reader, "HomeState"),
                            HomeZip = SqlHelper.GetNullableString(reader, "HomeZip"),
                            OfficeStreet = SqlHelper.GetNullableString(reader, "OfficeStreet"),
                            OfficeCity = SqlHelper.GetNullableString(reader, "OfficeCity"),
                            OfficeState = SqlHelper.GetNullableString(reader, "OfficeState"),
                            OfficeZip = SqlHelper.GetNullableString(reader, "OfficeZip"),
                            Title = SqlHelper.GetNullableString(reader, "Title"),
                            Salary = SqlHelper.GetNullableString(reader, "Salary"),
                            DateImported = importDate
                        };
                        
                        _repository.Customer.CreateCustomer(customer);
                        await _repository.SaveAsync();
                        newDataCheck = true;

                    }
                }
            }
        
        }
        return newDataCheck;

    }
}


   