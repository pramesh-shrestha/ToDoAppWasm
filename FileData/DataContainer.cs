//This is clas that holds the data. Having all data in one class makes it easier to write it to a file.

using Domain.Models;

namespace FileData; 
// The point is, we will read data from the file and load into these two collections. 
// The collections are essentially our database tables. If we were to need more model 
// classes in the future, e.g. Category, Project, or something else, we would add more collections.

// We could use IList, List or other types of collections, but the Collection will behave similar 
// to how we can interact with the database later on, using Entity Framework Core. 
// So we use ICollection to practice.
public class DataContainer {
    public ICollection<User> Users { get; set; }
    public ICollection<Todo> Todos { get; set; }
    
}