namespace ApiWallet.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class update : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Cuentas",
                c => new
                    {
                        CuentaId = c.Int(nullable: false, identity: true),
                        UserId = c.Int(nullable: false),
                        Nombre = c.String(nullable: false, maxLength: 100),
                        Monto = c.Double(nullable: false),
                    })
                .PrimaryKey(t => t.CuentaId);
            
            CreateTable(
                "dbo.Egresoes",
                c => new
                    {
                        EgresoId = c.Int(nullable: false, identity: true),
                        CuentaId = c.Int(nullable: false),
                        Descripcion = c.String(nullable: false, maxLength: 100),
                        Monto = c.Double(nullable: false),
                        MontoActual = c.Double(nullable: false),
                        FechaEgreso = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.EgresoId);
            
            CreateTable(
                "dbo.Ingresoes",
                c => new
                    {
                        IngresoId = c.Int(nullable: false, identity: true),
                        CuentaId = c.Int(nullable: false),
                        Descripcion = c.String(nullable: false, maxLength: 100),
                        Monto = c.Double(nullable: false),
                        MontoActual = c.Double(nullable: false),
                        FechaIngreso = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.IngresoId);
            
            CreateTable(
                "dbo.Trasladoes",
                c => new
                    {
                        TrasladoId = c.Int(nullable: false, identity: true),
                        CuentaId = c.Int(nullable: false),
                        Descripcion = c.String(nullable: false, maxLength: 100),
                        Monto = c.Double(nullable: false),
                        MontoActual = c.Double(nullable: false),
                        FechaIngreso = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.TrasladoId);
            
            CreateTable(
                "dbo.Users",
                c => new
                    {
                        UserId = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false, maxLength: 100),
                        Email = c.String(),
                        Password = c.String(),
                    })
                .PrimaryKey(t => t.UserId);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.Users");
            DropTable("dbo.Trasladoes");
            DropTable("dbo.Ingresoes");
            DropTable("dbo.Egresoes");
            DropTable("dbo.Cuentas");
        }
    }
}
