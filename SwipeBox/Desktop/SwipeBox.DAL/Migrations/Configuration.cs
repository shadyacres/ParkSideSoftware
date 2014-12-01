using SwipeBox.DAL.Context;
using System.Data.Entity.Migrations;

namespace SwipeBox.DAL.Migrations
{
    class Configuration : DbMigrationsConfiguration<SwipeBoxContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = true;
            ContextKey = "SwipeBox.DAL.Context.SwipeBoxContext";

            // Remove for production
            AutomaticMigrationDataLossAllowed = true;
        }

        protected override void Seed(SwipeBoxContext context)
        {
            base.Seed(context);
        }
    }
}
