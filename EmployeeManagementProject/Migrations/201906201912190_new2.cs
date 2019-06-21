namespace EmployeeManagementProject.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class new2 : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Skills", "Skill_Creation_Date", c => c.DateTime());
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Skills", "Skill_Creation_Date", c => c.DateTime(nullable: false));
        }
    }
}
