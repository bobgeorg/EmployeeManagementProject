namespace EmployeeManagementProject.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class EmployeeManagementProject11 : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.EmployeeSkills", "User_Id", "dbo.AspNetUsers");
            DropIndex("dbo.EmployeeSkills", new[] { "User_Id" });
            AddColumn("dbo.Employees", "LastDate", c => c.DateTime(nullable: false));
            AddColumn("dbo.Skills", "Employee_Employee_id", c => c.Int());
            CreateIndex("dbo.Skills", "Employee_Employee_id");
            AddForeignKey("dbo.Skills", "Employee_Employee_id", "dbo.Employees", "Employee_id");
            DropTable("dbo.EmployeeSkills");
        }
        
        public override void Down()
        {
            CreateTable(
                "dbo.EmployeeSkills",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Employee_id = c.Int(nullable: false),
                        Skill_id = c.Int(nullable: false),
                        User_Id = c.String(maxLength: 128),
                    })
                .PrimaryKey(t => t.Id);
            
            DropForeignKey("dbo.Skills", "Employee_Employee_id", "dbo.Employees");
            DropIndex("dbo.Skills", new[] { "Employee_Employee_id" });
            DropColumn("dbo.Skills", "Employee_Employee_id");
            DropColumn("dbo.Employees", "LastDate");
            CreateIndex("dbo.EmployeeSkills", "User_Id");
            AddForeignKey("dbo.EmployeeSkills", "User_Id", "dbo.AspNetUsers", "Id");
        }
    }
}
