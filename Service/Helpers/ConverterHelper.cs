using System.Collections;

namespace Service;
public static class ConverterHelper
{
    public static Dictionary<string, string> ObjectToDictionary(IEnumerable<dynamic> records)
    {
        Dictionary<string, string> dictionary = new Dictionary<string, string>();



        foreach (var record in records) 
        {
            foreach (var r in record) 
            {
                var arr = new ArrayList();
                var properties = r.GetType().GetProperties();

                // Iterate through each property and add it to the dictionary
                foreach (var property in properties)
                {
                    arr.Add(property.GetValue(r));
                }
                dictionary.Add(arr[0].ToString(), arr[1].ToString());
            }
        }
        // Get all properties of the dynamic object using reflection
        

        return dictionary;
    }
}
