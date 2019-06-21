namespace EmployeeManagementProject.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class _new : DbMigration
    {
        public override void Up()
        {
            DropColumn("dbo.Employees", "LastDate");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Employees", "LastDate", c => c.DateTime(nullable: false));
        }
    }
}
