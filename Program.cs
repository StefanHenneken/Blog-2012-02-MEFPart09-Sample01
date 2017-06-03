using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.ComponentModel.Composition.Hosting;
using System.ComponentModel.Composition.Primitives;

namespace Sample01
{   
class Program
{
    // [ImportMany(typeof(ICarContract))]
    private List<ICarContract> CarParts = new List<ICarContract>();
    private List<ICarContract> ExpensiveCarParts = new List<ICarContract>();

    static void Main(string[] args)
    {
        new Program().Run();
    }

    void Run()
    {
        var catalog = new AssemblyCatalog(System.Reflection.Assembly.GetExecutingAssembly()); 
        var container = new CompositionContainer(catalog); 
        // container.ComposeParts(this);

        foreach (ComposablePartDefinition partDefinition in container.Catalog.Parts)
        {
            ComposablePart part = partDefinition.CreatePart();               
            foreach (ExportDefinition exportDefinition in partDefinition.ExportDefinitions)
            {
                Console.WriteLine("Create new instance");
                ICarContract car = (ICarContract)part.GetExportedValue(exportDefinition);
                Console.WriteLine("Metadaten:");
                IDictionary<String, Object> metadata = exportDefinition.Metadata;
                Console.WriteLine("Name: {0}", metadata["Name"].ToString());
                Console.WriteLine("Price: {0}\n", metadata["Price"].ToString());
                if ((uint)metadata["Price"] > 50000)
                    ExpensiveCarParts.Add(car);
                else
                    CarParts.Add(car);
            }
        }

        Console.WriteLine("CarParts:");
        foreach (ICarContract carParts in CarParts)
            Console.WriteLine(carParts.GetName());

        Console.WriteLine("\nExpensiveCarParts:");
        foreach (ICarContract carParts in ExpensiveCarParts)
            Console.WriteLine(carParts.GetName());

        container.Dispose();
    }
}
    public interface ICarContract
    {
        string GetName();
    }

    [ExportMetadata("Name", "Mercedes")]
    [ExportMetadata("Price", (uint)48000)]
    [Export(typeof(ICarContract))]
    public class Mercedes : ICarContract
    {
        private static int counter;
        public Mercedes()
        {
            Console.WriteLine("constructor Mercedes: {0}", ++counter);
        }
        public string GetName()
        {
            return "Mercedes";
        }
    }

    [ExportMetadata("Name", "BMW")]
    [ExportMetadata("Price", (uint)55000)]
    [Export(typeof(ICarContract))]
    public class BMW : ICarContract
    {
        private static int counter;
        public BMW()
        {
            Console.WriteLine("constructor BMW: {0}", ++counter);
        }
        public string GetName()
        {
            return "BMW";
        }
    }

    [ExportMetadata("Name", "Audi")]
    [ExportMetadata("Price", (uint)53000)]
    [Export(typeof(ICarContract))]
    public class Audi : ICarContract
    {
        private static int counter;
        public Audi()
        {
            Console.WriteLine("constructor Audi: {0}", ++counter);
        }
        public string GetName()
        {
            return "Audi";
        }
    }
}