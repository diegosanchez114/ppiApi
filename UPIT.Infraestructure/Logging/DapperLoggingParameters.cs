namespace UPIT.Infraestructure.Logging
{
    public class DapperLoggingParameters
    {
        public static string GetParametersValue(object obj)
        {
            var type = obj.GetType();
            var properties = type.GetProperties();

            string values = string.Empty;

            foreach (var property in properties)
            {
                var value = property.GetValue(obj);
                values += $"{property.Name}: {value}\n";                
            }
            return values;
        }
    }
}
