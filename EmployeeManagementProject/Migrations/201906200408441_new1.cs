namespace EmployeeManagementProject.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class new1 : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Employees", "Hiring_Date", c => c.DateTime());
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Employees", "Hiring_Date", c => c.DateTime(nullable: false));
        }
    }
}
