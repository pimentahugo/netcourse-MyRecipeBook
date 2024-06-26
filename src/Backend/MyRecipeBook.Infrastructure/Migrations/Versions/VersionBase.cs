using FluentMigrator;

namespace MyRecipeBook.Infrastructure.Migrations.Versions;
public abstract class VersionBase : ForwardOnlyMigration
{
	protected FluentMigrator.Builders.Create.Table.ICreateTableColumnOptionOrWithColumnSyntax CreateTable(string table)
	{
		return Create.Table(table)
		.WithColumn("Id").AsInt64().PrimaryKey().Identity()
		.WithColumn("CreatedOn").AsDateTime().NotNullable()
		.WithColumn("Active").AsBoolean().NotNullable();
	}
}

