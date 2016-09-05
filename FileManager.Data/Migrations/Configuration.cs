namespace FileManager.Data.Migrations
{
    using System.Data.Entity.Migrations;

    internal sealed class Configuration : DbMigrationsConfiguration<AppDbContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
            ContextKey = "AppDbContext";
        }

        protected override void Seed(AppDbContext context)
        {
            base.Seed(context);
        }
    }
}
