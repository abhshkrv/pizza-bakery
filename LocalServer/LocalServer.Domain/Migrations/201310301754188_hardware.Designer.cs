// <auto-generated />
namespace LocalServer.Domain.Migrations
{
    using System.Data.Entity.Migrations;
    using System.Data.Entity.Migrations.Infrastructure;
    using System.Resources;
    
    public sealed partial class hardware : IMigrationMetadata
    {
        private readonly ResourceManager Resources = new ResourceManager(typeof(hardware));
        
        string IMigrationMetadata.Id
        {
            get { return "201310301754188_hardware"; }
        }
        
        string IMigrationMetadata.Source
        {
            get { return null; }
        }
        
        string IMigrationMetadata.Target
        {
            get { return Resources.GetString("Target"); }
        }
    }
}
