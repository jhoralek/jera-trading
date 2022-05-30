using Microsoft.EntityFrameworkCore.Migrations;

namespace SA.EntityFramework.Migrations
{
    public partial class IndexesOnRecords : Migration
    {
        private static string UserActivationTableName = "UserActivations";
        private static string CustomerTableName => "Customers";
        private static string RecordTableName => "Records";

        private static string ColumnCreated => "Created";
        private static string ColumnId => "Id";
        private static string ColumnUserId => "UserId";


        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // Records
            migrationBuilder.CreateIndex($"IX_{RecordTableName}_{ColumnCreated}", RecordTableName, ColumnCreated);
            migrationBuilder.CreateIndex($"IX_{RecordTableName}_{ColumnId}", RecordTableName, ColumnId);

            // Customers
            migrationBuilder.CreateIndex($"IX_{CustomerTableName}_{ColumnId}", CustomerTableName, ColumnId);

            // UserActivations
            migrationBuilder.CreateIndex($"IX_{UserActivationTableName}_{ColumnUserId}", UserActivationTableName, ColumnUserId);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            // Records
            migrationBuilder.DropIndex($"IX_{RecordTableName}_{ColumnCreated}", RecordTableName);
            migrationBuilder.DropIndex($"IX_{RecordTableName}_{ColumnId}", RecordTableName);

            // Customers
            migrationBuilder.DropIndex($"IX_{CustomerTableName}_{ColumnId}", CustomerTableName);

            // UserActivations
            migrationBuilder.DropIndex($"IX_{UserActivationTableName}_{ColumnUserId}", UserActivationTableName);
        }
    }
}
