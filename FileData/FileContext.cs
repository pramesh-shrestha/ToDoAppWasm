using System.Text.Json;
using Domain.Models;

namespace FileData;
//This class is responsible for reading and writing the data from/to the file.
public class FileContext {
    private const string filePath = "data.json"; //defining the file path
    private DataContainer? dataContainer; // "?" simply means that we declared Data container as Nullable. This means this field can be null.

    public ICollection<Todo>  Todos {
        get {
            LoadData();
            return dataContainer!.Todos;
            // The ! operator after dataContainer is called a "null-forgiving" operator, and it tells the compiler to treat the dataContainer object as if it is null.
            // This operator is used here to access the Todos collection of the dataContainer object without having to first check if dataContainer is null.    
        }
    }

    public ICollection<User> Users {
        get {
            LoadData();
            return dataContainer!.Users;
        }
    }

    private void LoadData() {
        if (dataContainer != null) {
            return; //If this block is executed, then it simply exits the method and return control to the calling code without executing any further code withing the method
        }

        if (!File.Exists(filePath)) {
            //This code creates a new instance of the dataContainer class using the new keyword,
            //and then initializes its Todos and Users properties using new instances of List<ToDo> and List<User>, respectively.
            //The code below is similar to this:
            // dataContainer = new();
            // dataContainer.Todos = new List<Todo>();
            // dataContainer.Users = new List<User>();
            
            dataContainer = new() {
                Todos = new List<Todo>(),
                Users = new List<User>()
            };
            return;
        }

        //The File.ReadAllText() method in C# reads the entire contents of a text file and returns the contents as a string.
        string content = File.ReadAllText(filePath);
        dataContainer = JsonSerializer.Deserialize<DataContainer>(content);
    }

    //The purpose of this method is to take the content of the DataContainer field, and put into the file.
    public void SaveChanges() {
        string serialized = JsonSerializer.Serialize(dataContainer, new JsonSerializerOptions{
            WriteIndented = true
        });
        
        //The File.WriteAllText() method takes two parameters: a string that specifies the path of the file to be written to,
        //and a string that contains the content to be written to the file.
        File.WriteAllText(filePath, serialized);
        dataContainer = null;
    }
}