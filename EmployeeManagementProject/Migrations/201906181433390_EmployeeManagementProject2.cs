namespace EmployeeManagementProject.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class EmployeeManagementProject2 : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Skills", "Employee_Employee_id", "dbo.Employees");
            DropIndex("dbo.Skills", new[] { "Employee_Employee_id" });
            CreateTable(
                "dbo.SkillEmployees",
                c => new
                    {
                        Skill_Skill_id = c.Int(nullable: false),
                        Employee_Employee_id = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.Skill_Skill_id, t.Employee_Employee_id })
                .ForeignKey("dbo.Skills", t => t.Skill_Skill_id, cascadeDelete: true)
                .ForeignKey("dbo.Employees", t => t.Employee_Employee_id, cascadeDelete: true)
                .Index(t => t.Skill_Skill_id)
                .Index(t => t.Employee_Employee_id);
            
            DropColumn("dbo.Skills", "Employee_Employee_id");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Skills", "Employee_Employee_id", c => c.Int());
            DropForeignKey("dbo.SkillEmployees", "Employee_Employee_id", "dbo.Employees");
            DropForeignKey("dbo.SkillEmployees", "Skill_Skill_id", "dbo.Skills");
            DropIndex("dbo.SkillEmployees", new[] { "Employee_Employee_id" });
            DropIndex("dbo.SkillEmployees", new[] { "Skill_Skill_id" });
            DropTable("dbo.SkillEmployees");
            CreateIndex("dbo.Skills", "Employee_Employee_id");
            AddForeignKey("dbo.Skills", "Employee_Employee_id", "dbo.Employees", "Employee_id");
        }
    }
}
