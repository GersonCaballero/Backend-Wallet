namespace ApiWallet.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class update1 : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.tipocuentas",
                c => new
                    {
                        TipoCuentaId = c.Int(nullable: false, identity: true),
                        Nombre = c.String(nullable: false, maxLength: 100),
                    })
                .PrimaryKey(t => t.TipoCuentaId);
            
            AddColumn("dbo.Cuentas", "TipoCuentaId", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Cuentas", "TipoCuentaId");
            DropTable("dbo.tipocuentas");
        }
    }
}
