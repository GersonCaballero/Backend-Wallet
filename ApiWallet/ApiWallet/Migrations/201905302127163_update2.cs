namespace ApiWallet.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class update2 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Trasladoes", "FechaTraslado", c => c.DateTime(nullable: false));
            DropColumn("dbo.Trasladoes", "FechaIngreso");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Trasladoes", "FechaIngreso", c => c.DateTime(nullable: false));
            DropColumn("dbo.Trasladoes", "FechaTraslado");
        }
    }
}
