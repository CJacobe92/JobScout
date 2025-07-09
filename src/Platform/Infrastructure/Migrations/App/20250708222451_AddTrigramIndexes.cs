using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations.App
{
    /// <inheritdoc />
    public partial class AddTrigramIndexes : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("CREATE EXTENSION IF NOT EXISTS pg_trgm;");
            migrationBuilder.Sql("CREATE INDEX idx_tenants_name_trgm ON \"Tenants\" USING gin (\"Name\" gin_trgm_ops);");
            migrationBuilder.Sql("CREATE INDEX idx_tenants_license_trgm ON \"Tenants\" USING gin (\"License\" gin_trgm_ops);");
            migrationBuilder.Sql("CREATE INDEX idx_tenants_registeredto_trgm ON \"Tenants\" USING gin (\"RegisteredTo\" gin_trgm_ops);");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("DROP INDEX IF EXISTS idx_tenants_name_trgm;");
            migrationBuilder.Sql("DROP INDEX IF EXISTS idx_tenants_license_trgm;");
            migrationBuilder.Sql("DROP INDEX IF EXISTS idx_tenants_registeredto_trgm;");
        }

    }
}
