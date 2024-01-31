using System.Web;
using System.Xml.Linq;
using Contracts;

namespace Service;
public class CustomerService : ICustomerService
{
    
    public async Task GetSourceCustomers(string connectionString)
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
}


   