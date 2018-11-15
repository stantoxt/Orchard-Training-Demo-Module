using System;
using Lombiq.TrainingDemo.Indexes;
using Lombiq.TrainingDemo.Models;
using OrchardCore.ContentManagement.Metadata;
using OrchardCore.ContentManagement.Metadata.Settings;
using OrchardCore.Data.Migration;

namespace Lombiq.TrainingDemo
{
    public class Migrations : DataMigration
    {
        IContentDefinitionManager _contentDefinitionManager;

        public Migrations(IContentDefinitionManager contentDefinitionManager)
        {
            _contentDefinitionManager = contentDefinitionManager;
        }

        public int Create()
        {
            _contentDefinitionManager.AlterTypeDefinition("Person", builder => builder
                .Creatable()
                .Listable()
                .WithPart(nameof(PersonPart))
            );

            SchemaBuilder.CreateMapIndexTable(nameof(PersonPartIndex), table => table
                .Column<DateTime>(nameof(PersonPartIndex.BirthDateUtc))
                .Column<string>(nameof(PersonPartIndex.ContentItemId), c => c.WithLength(26))
            ).AlterTable(nameof(PersonPartIndex), table => table
                .CreateIndex($"IDX_{nameof(PersonPartIndex)}_{nameof(PersonPartIndex.BirthDateUtc)}", nameof(PersonPartIndex.BirthDateUtc))
            );

            return 1;
        }
    }
}