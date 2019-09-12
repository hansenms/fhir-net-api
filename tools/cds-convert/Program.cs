using System;
using System.IO;
using System.Linq;
using Hl7.Fhir.Model;
using Hl7.Fhir.Serialization;
using Microsoft.Extensions.Configuration;

namespace cds_convert
{
    class Program
    {
        public static IConfiguration Configuration { get; private set; }

        static void Main(string[] args)
        {
            var builder = new ConfigurationBuilder()
                .AddEnvironmentVariables()
                .AddCommandLine(args);

            Configuration = builder.Build();

            var fileName = Configuration["FileName"];

            string resourceText = "";
            try
            {
                using (StreamReader sr = new StreamReader(fileName))
                {
                    resourceText = sr.ReadToEnd();
                }
            }
            catch (IOException e)
            {
                Console.WriteLine("The file could not be read:");
                Console.WriteLine(e.Message);
            }

            var parser = new FhirJsonParser();
            var serializer = new FhirJsonSerializer();

            try
            {
                var parsedResource = parser.Parse(resourceText);

                switch (parsedResource.TypeName)
                {
                    case "Patient":
                        CdsContact cdsContact = new CdsContact();
                        var patient = (Patient)parsedResource;
                        cdsContact.Lastname = patient.Name[0].Family;
                        cdsContact.Firstname = patient.Name[0].Given.First();
                        cdsContact.Fullname = $"{cdsContact.Firstname} {cdsContact.Lastname}";
                        var emails = patient.Telecom.Where(c => c.System == ContactPoint.ContactPointSystem.Email);
                        if (emails.Count() > 0)
                        {
                            cdsContact.Emailaddress1 = emails.First().Value;
                        }
                        Console.WriteLine(serializer.SerializeToString(cdsContact));
                        break;
                    default:
                        throw new NotImplementedException($"Conversion of resource type {parsedResource.TypeName}");
                }
            }
            catch (FormatException fe)
            {
                Console.WriteLine("Resource could not be parsed");
                Console.WriteLine(fe);
            }
        }
    }
}
