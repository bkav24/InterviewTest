using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml.Linq;
using System.Xml.Serialization;

namespace Convert_CSV_To_JSON
{
    class Program
    {
        [STAThread]
        static void Main(string[] args)
        {
            Console.WriteLine("What would you like to convert your csv file to?\n \n1: Convert to XML\n2: Convert to JSON ");
            var resp = Console.ReadLine();
            if (resp == "1" || resp == "2")
            {
                OpenFileDialog fbd = new OpenFileDialog();
                fbd.Filter = "CSV files (*.csv)|*.csv";
                if (fbd.ShowDialog() == DialogResult.OK)
                {
                    var file = fbd.FileName;

                    if (File.Exists(file) && file.EndsWith(".csv"))
                    {
                        Console.WriteLine("File: " + file + " selected");

                        if (resp == "2")
                        {
                            Convert_CSV_To_JSON(file);
                        }

                        else
                        {
                            Convert_CSV_To_XML(file);
                        }

                    }

                    Console.WriteLine("Press enter to exit the app");
                    Console.ReadLine();

                }

            }
            else
            {
                Console.WriteLine("Invalid Selection");
                Console.WriteLine("\nThe application will now close");
                Console.ReadLine();
                return;
            }

        }

        private static void Convert_CSV_To_XML(string file)
        {
            try
            {
                var lines = File.ReadAllLines(file);

                string[] headers = lines[0].Split(',');


                XElement xElement = new XElement("TopElement",
                                                       lines.Where((line, index) => index > 0)
                                                       .Select(line => new XElement("Item",
                                                          line.Split(',')
                                                          .Select((column, index) => new XElement(headers[index], column)))));
                var xml = xElement;
                var path = @"C:\xmlout.xml";

                xml.Save(path);
                Console.WriteLine("File has been converted\nYou can view the converted file here: " + path);
            }
            catch (Exception ex)
            {

                Console.WriteLine("There was an error converting your file. \nWith the following exception: " + ex);
            }

        }

        public static void Convert_CSV_To_JSON(string path)
        {

            string textFilePath = path;
            const Int32 BufferSize = 128;

            try
            {
                using (var fileStream = File.OpenRead(textFilePath))
                using (var streamReader = new StreamReader(fileStream, Encoding.UTF8, true, BufferSize))
                {
                    String line;
                    Dictionary<string, string> jsonRow = new Dictionary<string, string>();

                    while ((line = streamReader.ReadLine()) != null)
                    {

                        string[] parts = line.Split(',');

                        string key_ = parts[0];
                        string value = parts[1];


                        if (!jsonRow.Keys.Contains(key_))
                        {
                            jsonRow.Add(key_, value);
                        }

                    }
                    var json = JsonConvert.SerializeObject(jsonRow);
                    string path_ = @"C:\Test.csv";
                    File.WriteAllText(path_, json);
                    Console.WriteLine("File has been converted\nYou can view the converted file here: " + path);
                }
            }
            catch (Exception ex)
            {

                Console.WriteLine("There was an error converting your file. \nWith the following exception: " + ex);

            }

        }

    }
}
