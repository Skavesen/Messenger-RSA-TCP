using System.Data.Entity;


namespace MessengerClient.DBMS
{
    //Класс контекст
    class ApplicationContext :DbContext
    {
        public DbSet<User> Users { get; set; }
        public ApplicationContext() : base("Default") { }
    }
}
