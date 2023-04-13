using Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace EfcDataAccess; 

public class ToDoContext : DbContext{

    //We interact with the DbSet in a similar way to how we used the Collection of the FileContext.
    //We interact with this DbSet to add, get, update, remove Todos/Users from the database.
    public DbSet<User> Users { get; set; }
    public DbSet<Todo> Todos { get; set; }
    
    // public ToDoContext(DbContextOptions<ToDoContext> options)
    //     : base(options)
    // {
    // }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder) {
        // The program is started from WebAPI component. So, the path to the SQLite file should be relative to that component
        optionsBuilder.UseSqlite("Data Source = ../EfcDataAccess/Todo.db");
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder) {
        //In this way, we say that the entity "To-do" has a key, and the lambda expression defines which property to use as the key.
        modelBuilder.Entity<Todo>().HasKey(todo => todo.Id);
        modelBuilder.Entity<User>().HasKey(user => user.Id);
        
        //You can also do some of the constraints in OnModelCreating(..). Here is an example of limiting the To-do::Title to 50 characters:
        modelBuilder.Entity<Todo>().Property(todo => todo.Title).HasMaxLength(50);
    }
}