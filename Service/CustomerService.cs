﻿using System.Collections;
using System.Globalization;
using System.IdentityModel.Tokens.Jwt;
using System.Web;
using System.Xml.Linq;
using AutoMapper;
using Contracts;
using CsvHelper;
using Entities;
using MySql.Data.MySqlClient;

namespace Service;
public class CustomerService : ICustomerService
{

    private readonly IRepositoryManager _repository;
    private readonly IMapper _mapper;


    public CustomerService(IRepositoryManager repository, IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }

    //add Customer to Loyalty Table
    public async Task<LoyaltyCustomer> CreateLoyaltyCustomer(LoyaltyCustomerForCreate loyaltyCustomer, string token)
    {
        var customer = await _repository.Customer.GetCustomerBySsnAsync(loyaltyCustomer.Ssn);

        if (customer == null) return null;

        var tokenHandler = new JwtSecurityTokenHandler();
        var jwtToken = tokenHandler.ReadJwtToken(token);
        var ssn = jwtToken.Claims.FirstOrDefault().Value;

        var agent = await _repository.Agent.GetAgentBySsnAsync(ssn);
        
        if (agent == null) {return null;}

        var loyaltyList =  await _repository.LoyaltyCustomers.GetCustomersAsync();

        if (loyaltyList.Any(l => l.CustomerId == customer.Id) || 
            (loyaltyList.Where(l => l.AgentId == agent.Id && l.DateAdded.Date == DateTime.Now.Date).Count() >= 5)) return null;
                
        var forCreate = new LoyaltyCustomer 
        {
            CustomerId = customer.Id,
            AgentId = agent.Id,
            DateAdded = DateTime.Now
        }; 

        _repository.LoyaltyCustomers.CreateCustomer(forCreate);
        await _repository.SaveAsync();
        
        return forCreate;
        
    }

    public async Task<CustomerDto> GetCustomerById(Guid customerId)
    {
         var customer = await _repository.Customer.GetCustomerByIdAsync(customerId);

        if (customer == null) return null;

        var result = _mapper.Map<CustomerDto>(customer);

        return result;
    }

    public async Task<CustomerDto> GetCustomerBySsn(string customerSsn)
    {
        var customer = await _repository.Customer.GetCustomerBySsnAsync(customerSsn);

        if (customer == null) return null;

        var result = _mapper.Map<CustomerDto>(customer);

        return result;
    }

    public async Task<IEnumerable<CustomerDto>> GetCustomersList()
    {
        var customers = await _repository.Customer.GetCustomersAsync();

        if (customers == null) { return null; }

        var result = _mapper.Map<IEnumerable<CustomerDto>>(customers);

        return result;
    }

    // Imports customers from csv file
    public async Task ImportCsvCustomers(Stream file, string connectionString)
    {
        var reader = new StreamReader(file);
        var csv = new CsvReader(reader, CultureInfo.InvariantCulture);

        var records = csv.GetRecords<dynamic>();


        foreach (var record in records) 
        {
            var dictionary = new Dictionary<string, string>();

            foreach (var r in record) 
            {
                var arr = new ArrayList();
                var properties = r.GetType().GetProperties();

                foreach (var property in properties)
                {
                    arr.Add(property.GetValue(r));
                }
                dictionary.Add(arr[0].ToString(), arr[1].ToString());
            }

            dictionary["ImportDate"] = DateTime.Now.ToString("yyyy'-'MM'-'dd");
            SqlHelper.InsertData(dictionary, connectionString, "CsvCustomers");
        }
        
    }

    // Imports customers from a remote source
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
                XDocument doc = XDocument.Parse(xmlString);

                XNamespace soapEnv = "http://schemas.xmlsoap.org/soap/envelope/";
                XNamespace tempuri = "http://tempuri.org";

                var findPersonResult = doc.Descendants(tempuri + "FindPersonResult").FirstOrDefault();

                if (findPersonResult == null) break;
            
                Dictionary<string, string> values = new Dictionary<string, string>();
                values["Id"] = Guid.NewGuid().ToString();
                values["SourceId"] = id.ToString();

                foreach (var element in findPersonResult.Elements())
                {
                    if (element.HasElements)
                    {
                        foreach (var childElement in element.Elements())
                        {
                            values[$"{childElement.Parent.Name.LocalName}{childElement.Name.LocalName}"] = childElement.Value;                         
                        }
                    }
                    else
                    {
                        values[element.Name.LocalName] = element.Value;
                    }
                }
                values["ImportDate"] = DateTime.Now.ToString("yyyy'-'MM'-'dd");

                SqlHelper.InsertData(values, connectionString, "SourceCustomers");

                id++;
            }
            

        }
        
    }

    // Updates customers table based on imported data
    public async Task<bool> UpdateCustomersTable(string connectionString, string updateFrom)
    {

        string tableName = updateFrom.Equals("source") ? "SourceCustomers" : "CsvCustomers";

        var newDataCheck = false;
        using (MySqlConnection connection = new MySqlConnection(connectionString))
        {
            Customer? customer = null;
            CustomerForCreateDto? customerDto = null;

            connection.Open();
                
            string query = 
                "SELECT Id, Name, SSN, DOB, HomeStreet, HomeCity, HomeState, HomeZip, OfficeStreet, OfficeCity, OfficeState, OfficeZip, Title, Salary, ImportDate  FROM " + tableName;
                
            using (MySqlCommand command = new MySqlCommand(query, connection))
            {
                using (MySqlDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        var ssn = SqlHelper.GetNullableString(reader, "SSN");
                        if (ssn == null) continue;

                        ssn = ssn.Replace("-", "");

                        Guid customerId = Guid.TryParse(SqlHelper.
                            GetNullableString(reader, "Id"), out Guid result) ?  result : Guid.NewGuid();

                        
                        var checkCustomer = await _repository.Customer.GetCustomersAsync();

                        if(checkCustomer.Where(c => c.Id == customerId || c.SSN == ssn).Any()) continue;



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
                            SSN = ssn,
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


   