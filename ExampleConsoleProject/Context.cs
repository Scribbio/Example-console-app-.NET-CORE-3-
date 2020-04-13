using Microsoft.EntityFrameworkCore;

public partial class ExampleContext : DbContext
{
    // Connection via connection string passed as param 
    public ExampleContext(string connString) : base()
    {
    }

    public ExampleContext(DbContextOptions<ExampleContext> options) : base(options) 
    { 
    }

}
